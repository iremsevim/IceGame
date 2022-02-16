using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailColliderController : MonoBehaviour, ITriggerListener
{
    public void OnTouched(MonoBehaviour touched)
    {
       if(touched is PlayerController player)
        {
            player.OnTouchedFailCollider(this);
            Destroy(gameObject);
        }
    }
}
