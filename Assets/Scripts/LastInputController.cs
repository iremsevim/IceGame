using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastInputController : MonoBehaviour, ITriggerListener
{
    public void OnTouched(MonoBehaviour touched)
    {
       if(touched is PlayerController player)
        {
            player.OnTouchedLastInput(this);
        }
    }
}
