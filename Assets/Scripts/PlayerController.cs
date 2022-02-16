using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using System.Linq;

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
    public override void ActorAwake()
    {
        Letter.onDownLetterButton = (string letter) =>
          {
              currentWords.Add(letter);
              inputChecking = true;
              inputCheckingTimer = Time.time + 1.5f;
             
          };
     }
    public  Vector3 LastPos => transform.localPosition;
   
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
    public bool IsThereWord()
    {
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
     
        foreach (var item in findedgroup.iceProfiles)
            {
           
              allcollectedChars.Add(item.ice.iceChar);
              //yield return new WaitForSeconds(0.15f);
              item.ice.GetComponent<Ice>().BreakIce();
               yield return new WaitForSeconds(0.55f);
                Destroy(item.ice.gameObject);
                yield return new WaitForSeconds(0.1f);
          
            }
        CameraActor.Instance.firstFollowCamera.Follow = allcollectedChars[allcollectedChars.Count - 1].transform;


        yield return new WaitForSeconds(0.15f);
        currentIceGroup.groups.ForEach(X => Destroy(X.gameObject));
           yield return new WaitForSeconds(0.25f);
           StopOrContinue(false);
           currentWords.Clear();
           anim.SetBool("run", AnimStatus);
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

}
