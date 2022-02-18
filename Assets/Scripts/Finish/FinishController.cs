using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour, ITriggerListener
{
    public List<FinishProfile> finishProfiles;
    public void OnTouched(MonoBehaviour touched)
    {
       if(touched is PlayerController player)
        {
            player.OnTouchedFinish(this);
            Destroy(gameObject);
        }
    }
    [System.Serializable]
    public class FinishProfile
    {
        public int mincollectedCharacterCount;
        public int maxcollectedCharacterCount;
        public Transform targetPoint;

       
    }
}
