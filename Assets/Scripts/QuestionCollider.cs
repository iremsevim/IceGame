using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCollider : MonoBehaviour,ITriggerListener
{
    public IceGroupCarrier touchediceGroupCarrier;

    public void OnTouched(MonoBehaviour touched)
    {
      if(touched is PlayerController player)
        {
            player.OnTouchedIceGroupCollider(touchediceGroupCarrier);
        }
    }
}
