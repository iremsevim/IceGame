using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishXController : MonoBehaviour, ITriggerListener
{
    public List<FinishPolice> ownedPolices;
    public void OnTouched(MonoBehaviour touched)
    {
       if(touched is PlayerController player)
        {
            player.OnTouchedFinishXCollider(this);
            Destroy(gameObject);
        }
    }
}
