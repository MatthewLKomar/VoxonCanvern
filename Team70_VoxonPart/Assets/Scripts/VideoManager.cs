using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager instance { private set; get; }

    [SerializeField] GameObject videoPlane;
    [SerializeField] List<VideoClip> videoClips;
    
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
    

    public void PlayVideo(int index, bool isLoop)
    {
        if(videoPlane.activeSelf == false)
        {
            videoPlane.SetActive(true);
        }
        VideoPlayer vp = videoPlane.GetComponent<VideoPlayer>();
        vp.targetTexture.Release();
        if (vp.isPlaying)
        {
            vp.Stop();
        }
        vp.clip = videoClips[index];
        vp.isLooping = isLoop;
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
