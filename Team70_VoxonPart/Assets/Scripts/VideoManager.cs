using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance { private set; get; }

    [SerializeField] GameObject videoPlane;
    [SerializeField] List<VideoClip> videoClips;
    //[SerializeField] GameObject videoplayer0;
    //[SerializeField] GameObject videoplayer1;
    
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
        if(videoPlane.activeSelf == false)
        {
            videoPlane.SetActive(true);
        }

        VideoPlayer vp = videoPlane.GetComponent<VideoPlayer>();
        if (vp.isPlaying)
        {
            vp.Stop();
        }
        vp.clip = videoClips[index];
        vp.Play();
    }


    public void StopVideo()
    {
        VideoPlayer vp = videoPlane.GetComponent<VideoPlayer>();
        if(vp == null)
        {
            return;
        }

        if (vp.isPlaying)
        {
            vp.Stop();
        }

        videoPlane.SetActive(false);
    }
}
