using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance { private set; get; }

    [SerializeField] GameObject videoplayer0;
    [SerializeField] GameObject videoplayer1;
    
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
    

    public void PlayVideo(int index)
    {
        if (index == 0)
        {
            StopVideo(1);
            videoplayer0.transform.position = new Vector3(0, 0, 0);
            videoplayer0.GetComponent<VideoPlayer>().Play();
        }
        else
        {
            StopVideo(0);
            videoplayer1.transform.position = new Vector3(0, 0, 0);
            videoplayer1.GetComponent<VideoPlayer>().Play();
        }
    }


    public void StopVideo(int index)
    {
        transform.position = new Vector3(0, -10f, 0);
        
        if (index == 0)
        {
            videoplayer0.transform.position = new Vector3(0, -10f, 0);
            videoplayer0.GetComponent<VideoPlayer>().Stop();
        }
        else
        {
            videoplayer1.transform.position = new Vector3(0, -10f, 0);
            videoplayer1.GetComponent<VideoPlayer>().Stop();
        }
    }
}
