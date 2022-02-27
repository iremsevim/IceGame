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
    public GameObject targetLastObject;



    public override void ActorAwake()
    {
      
        targetLastObject = new GameObject();
        targetLastObject.transform.SetParent(transform);
        targetLastObject.transform.localPosition = Vector3.zero;

        Letter.onDownLetterButton = (string letter) =>
          {
              currentWords.Add(letter);
              IsThereWord();
              inputChecking = true;
              inputCheckingTimer = Time.timeSinceLevelLoad + 1.15f;
             
          };
        Delete.OnTouched = () =>
          {
              if (currentWords.Count <= 0) return;
              UIActor.Instance.DeleteText(currentWords);
          };
        Info.onClosedPanel = () =>
          {
              StopOrMovement(true);
              PoliceMan.Instance.isStop = false;
              PoliceMan.Instance.animator.SetBool("run", isMovement);
            //CameraActor.Instance.SwitchCamera(CameraType.PoliceChase);


          };
     }
    public Vector3 LastPos => transform.position;
   
    public override void ActorStart()
    {
        StopOrMovement(Info.displayed);
    }
    public override void ActorUpdate()
    {
       //if(inputChecking)
       // {
       //     if(inputCheckingTimer<Time.timeSinceLevelLoad)
       //     {
       //         inputChecking = false;
       //         IsThereWord();
       //     }
       // }
    }
    public override void ActorFixedUpdate()
    {
        if (!isMovement) return;
        Movement();
    }
    public void StopOrMovement(bool status)
    {
        isMovement = status;
        rb.velocity = Vector3.zero;
        anim.SetBool("run", status);
       
    }
   
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

   
    public void IsThereWord()
    {
        if (IsFailLetter||currentWords.Count<2)
        {
            return;
        }

        IceGroup matchGroup = null;
        foreach (var item in currentIceGroup.groups)
        {
            
            bool isMatch = true;
            for (int i = 0; i < item.iceProfiles.Count; i++)
            {
                if (i > currentWords.Count-1)
                {
                    break;
                }
                if (item.iceProfiles[i].iceName.ToString() != currentWords[i])
                {
                    isMatch = false;
                    break;
                }
            }
            if (isMatch) 
            {
                matchGroup = item;
                break;
            }
        }
        if (matchGroup!=null)
        {
            if (currentWords.Count == matchGroup.iceProfiles.Count) 
            {
                Debug.Log("WIN");
                StartCoroutine(TrueAnswer());
            }
        }
        else
        {
            Debug.Log("FAIL");
            FalseAnswer();
        }

       
    
    }  
    public bool IsThereWord2()
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
          

            System.Action action = null;
            if (i == findedgroup.iceProfiles.Count - 1) action = RefreshCamera;
            allcollectedChars.Add(item.ice.GetComponent<Ice>().BreakIce(action, i * 17));
            yield return new WaitForSeconds(0.01f);
            Destroy(item.ice.gameObject);
           //yield return new WaitForSeconds(0.1f);

        }

      
      //  yield return new WaitForSeconds(0.15f);
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
        Vector3 pos = allcollectedChars[allcollectedChars.Count - 1].transform.localPosition;
        pos.x = 0;
        targetLastObject.transform.localPosition = pos;
        CameraActor.Instance.firstFollowCamera.Follow =targetLastObject.transform;
       
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
                icecharRagdoll.transform.SetParent(CustomLevelActor.Instance.transform);
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
        if(profile==null)
        {
            profile = finish.finishProfiles[finish.finishProfiles.Count - 1];
        }
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
  