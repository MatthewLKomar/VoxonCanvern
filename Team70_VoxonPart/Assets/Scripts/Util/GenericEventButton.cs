using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericEventTrigger)), CanEditMultipleObjects]
public class GenericEventButton : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenericEventTrigger script = (GenericEventTrigger)target;
        if (GUILayout.Button("Transmit"))
        {
            for(int i = 0; i < script.Triggers.Count; i++)
            {
                script.Transmit(i);
            }
        }


    }
}
