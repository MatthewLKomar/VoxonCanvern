using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDisplay : MonoBehaviour
{
    [SerializeField] int displayIndex;
    
    void Start()
    {
        if (Display.displays.Length > 1)
        {
            Display.displays[displayIndex].Activate(2560, 1440, 60);
        }
    }
    
}
