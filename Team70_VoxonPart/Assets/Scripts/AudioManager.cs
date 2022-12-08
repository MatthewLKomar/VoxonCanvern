using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { private set; get; }

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioList;

    private void Awake()
    {
        if(instance != null && instance != this)
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

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayActivateSound(int index)            // Play the audio in the list based on the index.
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.PlayOneShot(audioList[index]);
    }


    public bool isPlaying()
    {
        return audioSource.isPlaying;
    }
}
