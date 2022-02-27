using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;

public class PoliceMan : GameSingleActor<PoliceMan>
{

    public Vector3 offset;
    public float chaseTimer = 2f;
    public Animator animator;
    public bool isStop;
    public override void ActorAwake()
    {
        if (!Info.displayed) 
        {
            isStop = true;
           animator.SetBool("run", false);
        }
        else
        {
            animator.SetBool("run", true);
        }
        
    }
    public override void ActorUpdate()
    {
        if (isStop) return;
        chaseTimer -= Time.deltaTime;
        if(chaseTimer<=0)
        {
           StartCoroutine(StopChase());
            return;
        }
        ChasePlayer();
    }
    private void ChasePlayer()
    {
        transform.position = PlayerController.Instance.transform.position + offset;
           
    }
    private IEnumerator StopChase()
    {
      
        CameraActor.Instance.SwitchCamera(CameraType.AfterChase);
        yield return new WaitForSeconds(0.1f);
        CameraActor.Instance.SwitchCamera(CameraType.CharacterFollow);
         CameraActor.Instance.firstFollowCamera.Follow = PlayerController.Instance.transform;
        isStop = true;
        yield return new WaitForSeconds(1f);
        UIActor.Instance.ShowHideLetterPanel(true);
        yield return new WaitForSeconds(2F);
        animator.SetBool("idle", true);

    }

}
