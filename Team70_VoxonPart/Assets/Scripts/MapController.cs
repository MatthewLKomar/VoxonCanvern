using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class MapController : MonoBehaviour
{
    public static MapController instance { private set; get; }

    public int hackStatus = -1;              // The status of the game: 0 = not hacked in; 1 = is hacked in; 2 = reach the time limit.                          

    [SerializeField] float timer;

    [SerializeField] GameObject magicCube;                  // The actual cube transform
    [SerializeField] VoxonTextController textCon;
    [SerializeField] TextMeshProUGUI timerUI;


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
        StartCoroutine(PlayVideo());
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

        if (Input.GetKey(KeyCode.A))          // Rotate the cube along x.
        {
            magicCube.transform.Rotate(-Vector3.left * 1.5f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            magicCube.transform.Rotate(Vector3.left * 1.5f);
        }

        if (Input.GetKey(KeyCode.Q))          // Rotate the cube along z.
        {
            magicCube.transform.Rotate(Vector3.forward * 1.5f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            magicCube.transform.Rotate(-Vector3.forward * 1.5f);
        }

        if (Input.GetKey(KeyCode.W))          // Rotate the cube along y.
        {
            magicCube.transform.Rotate(Vector3.up * 1.5f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            magicCube.transform.Rotate(-Vector3.up * 1.5f);
        }

        UpdatePlane();
    }


    public void UpdateTimer()
    {
        if(timer <= 0 && hackStatus == 1)                      // If reaches the time limit.
        {
            SetActiveCube(false);
            timerUI.text = "";
            hackStatus = 2;
            return;
        }

        if (hackStatus == 1)                // If is in the counting.
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
            magicCube.SetActive(true);
            AudioManager.instance.PlayActivateSound(0);
            instance.hackStatus = 1;
            //GameEvents.instance.StartExperience();                          // Pass data to Cavern notifying the timer starts counting down.
        }
        else
        {
            magicCube.SetActive(false);
            PlaneController.instance.ChangePlaneList(0);
            instance.hackStatus = 2;

            //GameEvents.instance.EndExperience();
            // TODO: Add audio about ending.
            
            VideoManager.instance.PlayVideo(1);  
        }
    }


    public void UpdatePlane()
    {
        Vector3 degree = magicCube.transform.rotation.eulerAngles;

        float x_mod = degree.x % 360f;
        float z_mod = degree.z % 360f;

        bool findMatch = false;

        if (x_mod < 30 || x_mod > 330)
        {
            if (z_mod < 30 || z_mod > 330)
            {
                // White
                PlaneController.instance.ChangePlaneList(2);
                findMatch = true;
            }
            else if (z_mod < 210 && z_mod > 150)
            {
                // Green
                PlaneController.instance.ChangePlaneList(1);
                findMatch = true;
            }
            else if (z_mod < 120 && z_mod > 60)
            {
                // Blue
                PlaneController.instance.ChangePlaneList(3);
                findMatch = true;
            }
            else if (z_mod < 300 && z_mod > 240)
            {
                // Red
                PlaneController.instance.ChangePlaneList(0);
                findMatch = true;
            }
        }
        else if (x_mod < 120 && x_mod > 60)
        {
            // Orange
            PlaneController.instance.ChangePlaneList(4);
            findMatch = true;
        }
        else if (x_mod < 300 && x_mod > 240)
        {
            // Yellow
            PlaneController.instance.ChangePlaneList(5);
            findMatch = true;
        }
        else if (z_mod < 30 || z_mod > 330)
        {
            if (x_mod < 210 && x_mod > 150)
            {
                // Green
                PlaneController.instance.ChangePlaneList(1);
                findMatch = true;
            }
        }

        if (!findMatch)
        {
            PlaneController.instance.ChangePlaneList(-1);
        }
    }


    private IEnumerator PlayVideo()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        VoxonTextController.instance.SetText("");
        
        VideoManager.instance.PlayVideo(0);

        yield return new WaitForSeconds(17);
        
        VideoManager.instance.StopVideo();

        VoxonTextController.instance.SetText("Password: ");
        hackStatus = 0;
    }
}
