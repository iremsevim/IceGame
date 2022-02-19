using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using System.Linq;
using Coskunerov.Managers;
using DG.Tweening;
using Coskunerov.Resources;
using Coskunerov.Utilities;

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
    [SerializeField] private bool inputChecking;
    [SerializeField] private float inputCheckingTimer;
    public  List<IceChar> allcollectedChars;
    [SerializeField] private bool IsFailLetter;
    [Header("Particles")]
    public ParticleSystem allTakeIceChars;
 
    public override void ActorAwake()
    {
        Letter.onDownLetterButton = (string letter) =>
          {
              currentWords.Add(letter);
              inputChecking = true;
              inputCheckingTimer = Time.timeSinceLevelLoad + 1.15f;
             
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
            if(inputCheckingTimer<Time.timeSinceLevelLoad)
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

      
        yield return new WaitForSeconds(0.15f);
        currentIceGroup.groups.ForEach(X => Destroy(X.gameObject));
           yield return new WaitForSeconds(0.25f);
           StopOrContinue(false);
           currentWords.Clear();
           IsFailLetter = false;
        Destroy(currentIceGroup.gameObject);
        if (currentIceGroup.nexttouchediceGroupCarrier)
        {
          
            currentIceGroup = currentIceGroup.nexttouchediceGroupCarrier;
            currentWords.Clear();

        }

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
    private IEnumerator Fail()
    {
        if(allcollectedChars.Count>0)
        {
            foreach (var item in allcollectedChars)
            {
                GameObject icecharRagdoll = Instantiate(item.ragdoll.gameObject, item.transform.position, item.transform.rotation);
                Rigidbody rb = icecharRagdoll.GetComponent<Ragdoll>().pelvis;
                rb.AddForce(item.transform.up * 100 +item. transform.right * Random.Range(-1, 1) * 50, ForceMode.Impulse);
                yield return new WaitForSeconds(0.1f);
                Destroy(item.gameObject);
            }
        }
       
        isMovement = false;
        rb.velocity = Vector3.zero;
        GameManager.Instance.FinishLevel(false);
        UIActor.Instance.ShowHideLetterPanel(false);
        anim.SetTrigger("fail");
    }
    public IEnumerator Win()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.FinishLevel(true);
    }
    public void OnTouchedFailCollider(FailColliderController fail)
    {
       StartCoroutine(Fail());
    }
    public void OnTouchedLastInput(LastInputController lastInputController)
    {
        IsFailLetter = true;
    }
    public void OnTouchedFinish(FinishController finish)
    {
        rb.isKinematic = true;
        RefreshCamera();

        GameObject fakeTarget = new GameObject("FakeTarget");
        fakeTarget.transform.SetParent(transform.parent);
        fakeTarget.transform.position = allcollectedChars[allcollectedChars.Count - 1].transform.position;
        fakeTarget.transform.position = new Vector3(transform.position.x, fakeTarget.transform.position.y, fakeTarget.transform.position.z);
         CameraActor.Instance.firstFollowCamera.Follow = fakeTarget.transform;

        CameraActor.Instance.SwitchCameraUpdateMode(Cinemachine.CinemachineBrain.UpdateMethod.LateUpdate);
        UIActor.Instance.ShowHideLetterPanel(false);
        FinishController.FinishProfile profile=finish.finishProfiles.Find(x => x.mincollectedCharacterCount<=allcollectedChars.Count && x.maxcollectedCharacterCount>= allcollectedChars.Count);
        float dist = Vector3.Distance(transform.position, profile.targetPoint.position);
        Debug.Log(dist);
        rb.velocity = Vector3.zero;
        isMovement = false;
        dist /= 3;
       
        transform.DOMove(profile.targetPoint.position, dist / finish.finishProfiles.Count).SetEase(Ease.Linear).OnComplete(()=> 
        {
            fakeTarget.transform.position = transform.position;
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
      
            ParticleFXDisplayer confetti = new ParticleFXDisplayer() { destroyTime = 2f, particleID = "confetti", position = transform.localPosition+transform.up*8f };
            confetti.Display();
            StartCoroutine(Win());
           
        }).OnUpdate(()=> 
        {
            fakeTarget.transform.position = allcollectedChars[allcollectedChars.Count - 1].transform.position;
            fakeTarget.transform.position = new Vector3(transform.position.x, fakeTarget.transform.position.y, fakeTarget.transform.position.z);

        });

    }
    public void LevelLoaded()
    {
        currentWords.Clear();
        allcollectedChars.ForEach(x => Destroy(x.gameObject));
        allcollectedChars.Clear();

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
  