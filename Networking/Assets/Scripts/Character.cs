using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int id;



    void Start()
    {
        GameEvents.instance.onUpdatePos += UpdatePos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePos(int newid, Vector3 newPos)
    {
        if (id == newid)
        {
            transform.position = newPos;
        }
    }
}
