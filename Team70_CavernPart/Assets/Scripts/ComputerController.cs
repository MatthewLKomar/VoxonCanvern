using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public static ComputerController instance { private set; get; }

    private bool isStart = false;

    private string currInput = "";
    private char[] decodeKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();            // Use this to find which key is pressed and whether is a letter.


    private void Awake()
    {
        if (instance != null && instance || this)
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
        
    }

    
    void Update()
    {
        UpdateInput();
    }


    public void UpdateInput()
    {
        if (!isStart)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || currInput.Length > 5)
        {
            // TODO: Compare password with current one.
        }
        else
        {
            foreach (char charK in decodeKey)               
            {
                if (Input.GetKeyDown(charK.ToString()))         // If it is a letter or a number.
                {
                    currInput += charK.ToString();              // Add it to the bottom.
                    break;
                }
            }
        }
    }

    public void SetStartPC(bool newStatus)
    {
        isStart = newStatus;
    }

}
