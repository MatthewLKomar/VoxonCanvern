using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] Transform tracker;

    private Vector3 posOffset = new Vector3(0,0,0);
    private Vector3 initPos;
    private Vector3 angleOffset = new Vector3(0,0,0);


    void Start()
    {
        initPos = transform.position;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            CalibrateTracker();
            Debug.Log(tracker.position);
        }

        transform.position = tracker.position + posOffset + initPos;
        Vector3 newAngle = new Vector3(tracker.eulerAngles.x + angleOffset.x, tracker.eulerAngles.y + angleOffset.y, 0);
        transform.rotation = Quaternion.Euler(newAngle);
        //transform.Rotate(angleOffset);
    }


    public void CalibrateTracker()
    {
        posOffset = -tracker.position;
        Vector3 euAngle = tracker.rotation.eulerAngles;
        angleOffset = new Vector3(-euAngle.x, -euAngle.y, -euAngle.z);
        //TODO: Add pos and angle;
    }
}
