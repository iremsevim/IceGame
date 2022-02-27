using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceChar : MonoBehaviour
{
    public Animator anim;
    public bool isMovement;
    private static float direcitonAmount=-1;
    public Ragdoll ragdoll;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
   
    private static PlayerController player => PlayerController.Instance;
   

    public IEnumerator Fall(System.Action onMovementDone=null,int zdelay=0)
    {
        transform.SetParent(null);
        anim.SetTrigger("fall");
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y;
        pos.z -= zdelay;
        transform.position = pos;
        
        Vector3 charPos =player.LastPos - (player.allcollectedChars.Count * (Vector3.forward * 3f)) + Vector3.up * 0.5f+(Vector3.right *1.25f)*direcitonAmount;
        yield return new WaitForSeconds(0.00001f);
        direcitonAmount *= -1;
        transform.localScale *= 2f;

        float dist= Vector3.Distance(transform.position, charPos);
        float multiper = dist / 30f;
        transform.SetParent(player.transform);
        Vector3 targetPos= player.transform.InverseTransformPoint(charPos);
        anim.SetBool("run", true);
        transform.DOLocalMove(targetPos, multiper*0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.localEulerAngles = Vector3.zero;
            isMovement = true;
            onMovementDone?.Invoke();
          
        }); 


    }
    public void OnFinish()
    {
        anim.SetTrigger("dance");

    }
}
