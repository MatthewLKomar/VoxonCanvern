using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    VXCamera newCamera;

    // Start is called before the first frame update
    void Start()
    {
        newCamera = GetComponent<VXCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        UpdateMove();
    }


    public void UpdateMove()
    {
        //Vector3 worldRot = Voxon.VXProcess.Instance.EulerAngles;
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

        /*if (Voxon.Input.GetKey("Up"))
        {
            worldPos.z -= 0.05f;
        }
        if (Voxon.Input.GetKey("Down"))
        {
            worldPos.z += 0.05f;
        }

        if (Voxon.Input.GetKey("Left"))
        {
            worldPos.x += 0.05f;
        }
        if (Voxon.Input.GetKey("Right"))
        {
            worldPos.x -= 0.05f;
        }
        */

        transform.eulerAngles = worldRot;
        transform.position = worldPos;
    }
}
