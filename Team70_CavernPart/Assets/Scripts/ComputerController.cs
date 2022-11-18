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

    private Dictionary<KeyCode, int> keyDict = new Dictionary<KeyCode, int>()
    {
        {KeyCode.Alpha0, 0},
        {KeyCode.Alpha1, 1},
        {KeyCode.Alpha2, 2},
        {KeyCode.Alpha3, 3},
        {KeyCode.Alpha4, 4},
        {KeyCode.Alpha5, 5},
        {KeyCode.Alpha6, 6},
        {KeyCode.Alpha7, 7},
        {KeyCode.Alpha8, 8},
        {KeyCode.Alpha9, 9},
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
    }

}
