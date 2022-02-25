using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coskunerov.Actors;

public class Delete : MonoBehaviour
{
    private Button selfButton;
    public static System.Action OnTouched;
    public string letter;
    private void Awake()
    {
        
        selfButton = GetComponent<Button>();
    }
    private void Start()
    {
        selfButton.onClick.AddListener(() => PressKey());
    }
    public void PressKey()
    {
        OnTouched.Invoke();
      
    }

}
