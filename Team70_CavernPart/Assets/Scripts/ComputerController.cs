using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerController : MonoBehaviour
{
    public static ComputerController instance { private set; get; }

    private bool isStart = false;

    public bool findMatch = false;

    [SerializeField] string currInput = "";
    [SerializeField] TextMeshPro inputUI;

    private char[] decodeKey = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();            // Use this to find which key is pressed and whether is a letter.


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

        if (Input.GetKeyDown(KeyCode.Return) || currInput.Length > 3)
        {
            GameEvents.instance.EveCheckPassword(currInput);

            currInput = "";

            if (findMatch)
            {
                inputUI.text = "Correct Password";
                AudioManager.instance.PlayInputSound(1);
            }
            else
            {
                inputUI.text = "Wrong Password: ";
                AudioManager.instance.PlayInputSound(0);
            }
            findMatch = false;
        }
        else
        {
            foreach (char charK in decodeKey)               
            {
                KeyCode newkeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), charK.ToString());
                if (Input.GetKeyDown(newkeyCode))         // If it is a letter or a number.
                {
                    currInput += charK.ToString();              // Add it to the bottom.

                    if(inputUI.text[0] == 'E')
                    {
                        inputUI.text += charK;
                    }
                    else
                    {
                        inputUI.text = "Enter Password: " + charK;
                    }
                    break;
                }
            }
        }
    }

    public void SetStartPC(bool newStatus)
    {
        isStart = newStatus;

        if (newStatus)
        {
            inputUI.text = "Enter Password: ";
        }
    }

}
