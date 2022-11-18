using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { private set; get; }

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


    public event Action<int, Vector3> onUpdatePos;

    public void EveUpdatePos(int id, Vector3 newPos)
    {
        if(onUpdatePos != null)
        {
            onUpdatePos(id, newPos);
        }
    }
}
