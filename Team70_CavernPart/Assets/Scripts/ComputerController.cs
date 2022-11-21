using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerController : MonoBehaviour
{
    public static ComputerController instance { private set; get; }

    private bool isStart = false;                           // Is the game in stage one?

    public bool findMatch = false;                          // If there is a match each round.

    [SerializeField] string currInput = "";
    [SerializeField] TextMeshPro inputUI;

    private Dictionary<KeyCode, int> keyDict = new Dictionary<KeyCode, int>()
    {
        {KeyCode.Keypad0, 0},
        {KeyCode.Keypad1, 1},
        {KeyCode.Keypad2, 2},
        {KeyCode.Keypad3, 3},
        {KeyCode.Keypad4, 4},
        {KeyCode.Keypad5, 5},
        {KeyCode.Keypad6, 6},
        {KeyCode.Keypad7, 7},
        {KeyCode.Keypad8, 8},
        {KeyCode.Keypad9, 9},
    };


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
            foreach (KeyValuePair<KeyCode, int> keyp in keyDict)
            {
                if (Input.GetKeyDown(keyp.Key))         // If it is a letter or a number.
                {
                    currInput += keyp.Value.ToString();              // Add it to the bottom.

                    if (inputUI.text[0] == 'E')
                    {
                        inputUI.text += keyp.Value.ToString();
                    }
                    else
                    {
                        inputUI.text = "Enter Password: " + keyp.Value.ToString();
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
        else
        {
            inputUI.text = "";
        }
    }

}
