using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public BreakableWindow breakableWindow;
    public ParticleSystem smoke;
    public void BreakIce()
    {
       // breakableWindow.breakWindow();
        smoke.Play();
    }
}
