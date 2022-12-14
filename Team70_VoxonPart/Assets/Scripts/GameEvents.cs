using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericEventTrigger))]
public class GameEvents : MonoBehaviour
{
    public static GameEvents instance { private set; get; }
    private GenericEventTrigger genericEventTrigger;
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
        genericEventTrigger = GetComponent<GenericEventTrigger>();
    }


    //Mkomar says
    // probably a better way of making these more generic/designer accessible but thats ok for now
    public void StartExperience()
    {
        genericEventTrigger.Transmit(0);
    }

    public void EndExperience()
    {
        genericEventTrigger.Transmit(1);
    }
}
