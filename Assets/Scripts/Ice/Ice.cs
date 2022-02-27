using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
   public BreakableWindow breakableWindow;
    public IceChar iceChar;

    private void Awake()
    { 
        if(iceChar)
     iceChar.gameObject.SetActive(false);
    }

    public IceChar BreakIce(System.Action onMovementDone=null,int zdelay=0)
    {
        IceChar result = null;

        if (breakableWindow == null) return result;
           breakableWindow.breakWindow();

        GameObject createdChar= Instantiate(GameData.Instance.gameDataSO.GetIceChar.gameObject,
         iceChar.transform.position,iceChar.transform.rotation);
        createdChar.transform.localScale /= 2.2f;
        if(createdChar.TryGetComponent(out IceChar iceCharComp)) 
        {
            StartCoroutine(iceCharComp.Fall(onMovementDone, zdelay));
            result = iceCharComp;
        }
        return result;
        //iceChar.gameObject.SetActive(true);
        

    }
}
