using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using System.Linq;
using Coskunerov.Managers;
using DG.Tweening;

public class PlayerController : GameSingleActor<PlayerController>
{
    [Header("Companents")]
    public Animator anim;
    public Rigidbody rb;
    public InputManager inputManager;
    [Header("Refs")]
    public float movementSpeed;
    public float rotateSpeed;
    public bool isMovement = true;
    public List<string> currentWords;
    public IceGroupCarrier currentIceGroup;
    private bool inputChecking;
    private float inputCheckingTimer;
    public  List<IceChar> allcollectedChars;
    private bool IsFailLetter;
    [Header("Particles")]
    public ParticleSystem allTakeIceChars;
 
    public override void ActorAwake()
    {
        Letter.onDownLetterButton = (string letter) =>
          {
              currentWords.Add(letter);
              inputChecking = true;
              inputCheckingTimer = Time.time + 1.15f;
             
          };
     }
    public Vector3 LastPos => transform.position;
   
    public override void ActorStart()
    {
        anim.SetBool("run", true);
    }
    public override void ActorUpdate()
    {
       if(inputChecking)
        {
            if(inputCheckingTimer<Time.time)
            {
                inputChecking = false;
                IsThereWord();
            }
        }
    }
    public override void ActorFixedUpdate()
    {
        if (!isMovement) return;
        Movement();
    }
    public bool AnimStatus =>isMovement;
    public void Movement()
    {
      
        rb.velocity = new Vector3(inputManager.Result * rotateSpeed, rb.velocity.y, movementSpeed);
        float rot_y = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
        rot_y = Mathf.Clamp(rot_y, -25, 25);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.localEulerAngles.x, rot_y, transform.localEulerAngles.x), 0.5f);
    }
    private void StopOrContinue(bool movementStatus)
    {
        if(movementStatus)
        {
            isMovement = false;
            rb.velocity = Vector3.zero;
        }
        else
        {
            isMovement = true;
            
        }
    }

    public void OnTouchedIceGroupCollider(IceGroupCarrier touchedGroup)
    {
        currentIceGroup = touchedGroup;
       
      
    }
    public void MovementSpeedController(bool IsitClose)
    {
       
        if(IsitClose)
        {
            movementSpeed /= 2.5f;

        }
        else
        {
            movementSpeed *= 2.5f;
        }
    }
    public bool IsThereWord()
    {
        if (IsFailLetter)
        {
            return false;
        }
        bool result = false;
        IceGroup  findedgroup=currentIceGroup.groups.Find(x => x.iceProfiles.Count == currentWords.Count);
        int count = 0;
        if (findedgroup)
        {
            for (int i = 0; i < findedgroup.iceProfiles.Count; i++)
            {
                if (findedgroup.iceProfiles[i].iceName.ToString() == currentWords[i])
                {
                    count++;
                    continue;

                }
                else
                {
                    break;

                }
            }
            if (count == findedgroup.iceProfiles.Count)
            {
                Debug.Log("WIN");

                result = true;
                StartCoroutine(TrueAnswer());
                return true;
            }
        }
      
          Debug.Log("FAIL");
          FalseAnswer();
            result =false;
        
      
        return result;
    
    }
    public IEnumerator TrueAnswer()
    {
       
        IceGroup findedgroup = currentIceGroup.groups.Find(x => x.iceProfiles.Count == currentWords.Count);
        StartCoroutine(UIActor.Instance.ClearTypedLetter(true));
        Destroy(currentIceGroup.failCollider);
        for (int i = 0; i < findedgroup.iceProfiles.Count; i++)
        {
            var item = findedgroup.iceProfiles[i];
            allcollectedChars.Add(item.ice.iceChar);

            System.Action action = null;
            if (i == findedgroup.iceProfiles.Count - 1) action = RefreshCamera;
            item.ice.GetComponent<Ice>().BreakIce(action);
            yield return new WaitForSeconds(0.1f);
            Destroy(item.ice.gameObject);
            yield return new WaitForSeconds(0.25f);

        }
        MovementSpeedController(false);
        


        yield return new WaitForSeconds(0.15f);
        currentIceGroup.groups.ForEach(X => Destroy(X.gameObject));
           yield return new WaitForSeconds(0.25f);
           StopOrContinue(false);
           currentWords.Clear();

     
          
    }
    private void RefreshCamera()
    {

        CameraActor.Instance.firstFollowCamera.Follow = allcollectedChars[allcollectedChars.Count - 1].transform;
        allTakeIceChars.Play();
    }
    public void FalseAnswer()
    {
           StartCoroutine(UIActor.Instance.ClearTypedLetter(false));
            currentWords.Clear();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITriggerListener triggerListener))
        {
            triggerListener.OnTouched(this);
        }
    }
    private void Fail()
    {
       
        isMovement = false;
        rb.velocity = Vector3.zero;
        GameManager.Instance.FinishLevel(false);
        UIActor.Instance.ShowHideLetterPanel(false);
        anim.SetTrigger("fail");
    }
    public void OnTouchedFailCollider(FailColliderController fail)
    {
        Fail();
    }
    public void OnTouchedLastInput(LastInputController lastInputController)
    {
        IsFailLetter = true;
    }
    public void OnTouchedFinish(FinishController finish)
    {
        UIActor.Instance.ShowHideLetterPanel(false);
        FinishController.FinishProfile profile=finish.finishProfiles.Find(x => x.mincollectedCharacterCount<=allcollectedChars.Count && x.maxcollectedCharacterCount>= allcollectedChars.Count);
        float dist = Vector3.Distance(transform.position, profile.targetPoint.position);
        Debug.Log(dist);
        rb.velocity = Vector3.zero;
        isMovement = false;
        dist /= 3;
        transform.DOMove(profile.targetPoint.position, dist / finish.finishProfiles.Count).SetEase(Ease.Flash).OnComplete(()=> 
        {
            CameraActor.Instance.firstFollowCamera.Follow = null;
            foreach (var item in allcollectedChars)
            {
               
                item.transform.SetParent(null);
                item.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f).OnComplete(() => 
                {
                    item.OnFinish();
                });
            }

           transform.DOLocalRotate(new Vector3(0, 180, 0), 0.5f);
            anim.SetTrigger("dance");
           

        });

    }
    public void  OnTouchedFinishXCollider(FinishXController finishX)
    {
       
         anim.SetTrigger("punch");
        foreach (var item in finishX.ownedPolices)
        {
           
            StartCoroutine(item.Throw());
        }
    }

}
  