using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceChar : MonoBehaviour
{
    public Animator anim;
    public bool isMovement;
    private static float direcitonAmount=-1;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private static PlayerController player => PlayerController.Instance;
    private void Update()
    {
        if(isMovement)
        {
            anim.SetBool("run", player.AnimStatus);
        }
       
    }
    public IEnumerator Fall()
    {
        transform.SetParent(null);
        anim.SetTrigger("fall");
      
        Vector3 charPos =player.LastPos - (player.allcollectedChars.Count * (Vector3.forward * 3f)) + Vector3.up * 0.5f+(Vector3.right *1.25f)*direcitonAmount;
        yield return new WaitForSeconds(0.5f);
        direcitonAmount *= -1;
        transform.localScale *= 2.85f;
        transform.DOMove(charPos, 0.5f).OnComplete(() =>
        {
            transform.localEulerAngles = Vector3.zero;
            isMovement = true;
            transform.SetParent(player.transform);
     
        }); 


    }
   
}
