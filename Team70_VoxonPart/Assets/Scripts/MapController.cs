using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance { private set; get; }

    public int hackStatus = 0;

    private float timer = 300f;

    [SerializeField] Transform magicCube;                  // The actual cube object
    [SerializeField] VoxonTextController textCon;
    [SerializeField] Voxon.VXTextComponent timerUI;
    


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
        GameEvents.instance.onSetActiveCube += SetActiveCube;       // Add the function to the event.
    }


    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        UpdateMove();

        UpdateTimer();
    }


    public void UpdateMove()
    {
        if (hackStatus != 1)
        {
            return;
        }

        if (Voxon.Input.GetKey("rotate_Left_x"))          // Rotate the cube along x.
        {
            magicCube.Rotate(-Vector3.left * 1.5f);
        }
        if (Voxon.Input.GetKey("rotate_Right_x"))
        {
            magicCube.Rotate(Vector3.left * 1.5f);
        }

        if (Voxon.Input.GetKey("rotate_Left_y"))          // Rotate the cube along z.
        {
            magicCube.Rotate(Vector3.forward * 1.5f);
        }
        if (Voxon.Input.GetKey("rotate_Right_y"))
        {
            magicCube.Rotate(-Vector3.forward * 1.5f);
        }

        if (Voxon.Input.GetKey("rotate_Left_z"))          // Rotate the cube along y.
        {
            magicCube.Rotate(Vector3.up * 1.5f);
        }
        if (Voxon.Input.GetKey("rotate_Right_z"))
        {
            magicCube.Rotate(-Vector3.up * 1.5f);
        }

        UpdatePlane();
    }


    public void UpdateTimer()
    {
        if(timer <= 0)
        {
            SetActiveCube(false);
            timerUI.text = "";
            return;
        }


        if (hackStatus == 1)
        {
            timer -= 1 * Time.deltaTime;
            if (timer >= 0)
            {
                timerUI.text = Mathf.Floor(timer).ToString();
            }
        }
    }


    public void SetActiveCube(bool newStatus)
    {
        if (newStatus)
        {
            magicCube.position = new Vector3(0, 0.4f, 0);            // Move it to the center.
            AudioManager.instance.PlayActivateSound(0);
            instance.hackStatus = 1;
            // TODO: Pass data to Cavern notifying the timer starts counting down.
        }
        else
        {
            magicCube.position = new Vector3(0, 5.4f, 0);            // Move it to the center.
            PlaneController.instance.ChangePlaneList(0);
            instance.hackStatus = 2;
            // TODO: Add audio about ending.
        }
    }


    public void UpdatePlane()
    {
        Vector3 degree = magicCube.rotation.eulerAngles;

        float x_mod = degree.x % 360f;
        float z_mod = degree.z % 360f;

        bool findMatch = false;

        if (x_mod < 20 || x_mod > 340)
        {
            if (z_mod < 20 || z_mod > 340)
            {
                // White
                PlaneController.instance.ChangePlaneList(3);
                findMatch = true;
            }
            else if (z_mod < 200 && z_mod > 160)
            {
                // Green
                PlaneController.instance.ChangePlaneList(2);
                findMatch = true;
            }
            else if (z_mod < 110 && z_mod > 70)
            {
                // Blue
                PlaneController.instance.ChangePlaneList(4);
                findMatch = true;
            }
            else if (z_mod < 290 && z_mod > 250)
            {
                // Red
                PlaneController.instance.ChangePlaneList(1);
                findMatch = true;
            }
        }
        else if (z_mod < 20 || z_mod > 340)
        {
            if (x_mod < 200 && x_mod > 160)
            {
                // Green
                PlaneController.instance.ChangePlaneList(2);
                findMatch = true;
            }
            else if (x_mod < 110 && x_mod > 70)
            {
                // Orange
                PlaneController.instance.ChangePlaneList(5);
                findMatch = true;
            }
            else if (x_mod < 290 && x_mod > 250)
            {
                // Yellow
                PlaneController.instance.ChangePlaneList(6);
                findMatch = true;
            }
        }

        if (!findMatch)
        {
            PlaneController.instance.ChangePlaneList(0);
        }
    }
}
