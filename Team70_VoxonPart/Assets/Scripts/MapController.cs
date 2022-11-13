using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    VXCamera newCamera;
    [SerializeField] GameObject magicCube;                  // The actual cube object

    void Start()
    {
        newCamera = GetComponent<VXCamera>();

        GameEvents.instance.onSetActiveCube += SetActiveCube;       // Add the function to the event.
    }


    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        UpdateMove();
    }


    public void UpdateMove()
    {
        Vector3 worldRot = transform.eulerAngles;
        Vector3 worldPos = transform.position;
        
        newCamera = GetComponent<VXCamera>();

        if (Voxon.Input.GetKey("rotate_Left"))          // Rotate the cube horizontally.
        {
            worldRot.y += 0.5f;
        }
        if (Voxon.Input.GetKey("rotate_Right"))
        {
            worldRot.y -= 0.5f;
        }

        if (Voxon.Input.GetKey("rotate_Up"))            // Rotate the cube vertically.
        {
            worldRot.x += 0.5f;
        }
        if (Voxon.Input.GetKey("rotate_Down"))
        {
            worldRot.x -= 0.5f;
        }

        if (Voxon.Input.GetKey("zoom_In"))          // Zoom in/out the cube.
        {
            newCamera.BaseScale += 0.5f;
        } 

        if (Voxon.Input.GetKey("zoom_Out"))
        {
            newCamera.BaseScale -= 0.5f;
        }

        transform.eulerAngles = worldRot;
        transform.position = worldPos;
    }


    public void SetActiveCube()
    {
        magicCube.transform.position = new Vector3(0, 0, 0);            // Move it to the center.
    }
}
