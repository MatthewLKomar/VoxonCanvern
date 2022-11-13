using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artworks : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] string password;

    private bool isLocked = true;

    void Start()
    {
        
    }


    public string GetPassword()
    {
        return password;
    }


    public void UnloadArtwork()
    {
        isLocked = false;

        // TODO: Add unlock events.
    }
}
