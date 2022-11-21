using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { private set; get; }

    [HideInInspector]
    public bool isStart, isEnd = false;
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

    /* Mkomar says... 
     *  Start experiecne and EndExperience will be triggered via
     *  UnityEvents in the EventManager 
     */
    public void StartExperience()
    {
        isStart = true;
    }

    public void EndExperience()
    {
        isEnd = true;
    }



    public event Action<string> onCheckPassword;
    public void EveCheckPassword(string password)
    {
        if(onCheckPassword != null)
        {
            onCheckPassword(password);
        }
    }
}
