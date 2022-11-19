using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class GenericEventButton : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenericEventTrigger script = (GenericEventTrigger)target;
        if (GUILayout.Button("Transmit"))
        {
            script.Transmit();
        }


    }
}
