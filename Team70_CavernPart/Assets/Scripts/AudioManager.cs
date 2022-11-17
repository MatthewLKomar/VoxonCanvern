using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }

    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource inputSource;
    [SerializeField] AudioSource alertSource;

    [SerializeField] List<AudioClip> voeList;

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

    
    void Update()
    {
        
    }


    public void PlayBGMSound()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        bgmSource.Play();
    }


    public void StopBGMSound()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void PlayInputSound(int index)
    {
        if (inputSource.isPlaying)
        {
            inputSource.Stop();
        }

        inputSource.PlayOneShot(voeList[index]);
    }


    public void StopInputSound()
    {
        if (inputSource.isPlaying)
        {
            inputSource.Stop();
        }
    }


    public void PlayAlertSound()
    {
        if (alertSource.isPlaying)
        {
            alertSource.Stop();
        }
        alertSource.Play();
    }


    public void StopAlertSound()
    {
        if (alertSource.isPlaying)
        {
            alertSource.Stop();
        }
    }
}
