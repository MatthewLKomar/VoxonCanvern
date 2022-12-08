using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static PuzzleVisibility instance { private set; get; }

    private Dictionary<int, bool> clueDict = new Dictionary<int, bool>();

    public string outputString = "";

    private void Awake()
    {
        if(instance != null && instance != this)
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

    // Update is called once per frame
    void Update()
    {
        FindOutput();
    }


    private void FindOutput()
    {
        int numShowing = 0;
        outputString = "";
        
        foreach (KeyValuePair<int,bool> clue in clueDict)
        {
            if (clue.Value == true)
            {
                outputString += clue.Key.ToString();
                numShowing++;
            }
        }

        outputString = numShowing.ToString() + outputString;
    }
    

    public void AddClue(int id)
    {
        clueDict.Add(id,false);
    }


    public void ToggleClue(int id, bool newInput)
    {
        if (!clueDict.ContainsKey(id))
        {
            Debug.LogWarning("Does not contain that clue key.");
            return;
        }

        clueDict[id] = newInput;
    }
}
