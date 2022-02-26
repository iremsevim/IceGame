using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
   public BreakableWindow breakableWindow;
    public IceChar iceChar;

    private void Awake()
    {
        iceChar.gameObject.SetActive(false);
    }

    public void BreakIce(System.Action onMovementDone=null)
    {
        if (breakableWindow == null) return;
           breakableWindow.breakWindow();

        iceChar.gameObject.SetActive(true);
         StartCoroutine(iceChar.Fall(onMovementDone));

    }
}
