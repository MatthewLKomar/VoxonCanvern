using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { private set; get; }

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

    public event Action<string> onCheckPassword;

    public void EveCheckPassword(string password)
    {
        if(onCheckPassword != null)
        {
            onCheckPassword(password);
        }
    }
}
