using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public static PlaneController instance { private set; get; }

    [SerializeField] List<GameObject> planeList;
    private int currIndex = -1;

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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangePlaneList(int index)
    {
        if(currIndex != -1)
        {
            planeList[currIndex].SetActive(false);
            currIndex = -1;
        }

        if(index == -1)
        {
            return;
        }

        planeList[index].SetActive(true);

        currIndex = index;
    }

}
