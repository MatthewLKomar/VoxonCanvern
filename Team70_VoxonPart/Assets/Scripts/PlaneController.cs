using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public static PlaneController instance { private set; get; }

    [SerializeField] List<GameObject> planeList;
    private int currIndex = 0;

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
        planeList[currIndex].transform.position = new Vector3(0,-10f,0);        // Move the old plane away, replace it with a new one.
        planeList[index].transform.position = new Vector3(0, -1.5f, 0);

        currIndex = index;
    }

}
