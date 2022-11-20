using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    
    public static LightManager instance { private set; get; }

    [SerializeField] List<GameObject> museumLights;

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

    
    void Update()
    {
        
    }


    public void TurnOnLights()
    {
        foreach (GameObject ml in museumLights)
        {
            ml.GetComponent<Animator>().SetTrigger("TurnOn");
        }
    }


    public void TurnOnLight(int index)
    {
        museumLights[index].GetComponent<Animator>().SetTrigger("TurnOn");
    }


    public void ToggleFlashLight(bool input)
    {
        foreach (GameObject ml in museumLights)
        {
            if (input)
            {
                ml.GetComponent<Light>().color = new Color(1, 0, 0);
                ml.GetComponent<Animator>().SetBool("isFlash",true);
            }
            else
            {
                ml.GetComponent<Light>().color = new Color(1, 1, 1);
                ml.GetComponent<Animator>().SetBool("isFlash", false);
            }
        }
    }


}
