using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
   public BreakableWindow breakableWindow;
    public IceChar iceChar;

    public void BreakIce()
    {
        if (breakableWindow == null) return;
       breakableWindow.breakWindow();
         StartCoroutine(iceChar.Fall());

     
    }
}
