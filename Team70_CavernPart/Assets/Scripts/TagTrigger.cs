using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagTrigger : MonoBehaviour
{
    [SerializeField] GameObject artTag;
    [SerializeField] private int id;
    

    void Start()
    {
        
        PuzzleVisibility.instance.AddClue(id);
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashLightT"))            // If triggered by the flashlight.
        {
            artTag.SetActive(true);
            PuzzleVisibility.instance.ToggleClue(id,true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashLightT"))            // If the flashlight left.
        {
            artTag.SetActive(false);
            PuzzleVisibility.instance.ToggleClue(id,false);
        }
    }
}
