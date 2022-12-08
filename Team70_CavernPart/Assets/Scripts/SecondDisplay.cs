using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using TMPro;

public class SecondDisplay : MonoBehaviour
{
    public static SecondDisplay instance { private set; get; }
    
    [SerializeField] int displayIndex;
    
    [SerializeField] private TextMeshProUGUI passwordUI;
    [SerializeField] private TextMeshProUGUI itemUI;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[displayIndex].Activate(2560, 1440, 60);
        }
    }
    

    public void SetPasswordUI(string newInput)
    {
        passwordUI.text = newInput;
    }


    public string GetPasswordUI()
    {
        return passwordUI.text;
    }


    public void SetItemUI(string newInput)
    {
        itemUI.text = newInput;
    }
    
}
