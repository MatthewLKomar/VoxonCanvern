using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagTrigger : MonoBehaviour
{
    [SerializeField] GameObject artTag;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlashLightT"))
        {
            artTag.SetActive(true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FlashLightT"))
        {
            artTag.SetActive(false);
        }
    }
}
