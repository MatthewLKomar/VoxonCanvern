using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artworks : MonoBehaviour
{
    [SerializeField] int id;                // The id of the artwork.
    [SerializeField] string password;       // The password to open 

    private bool isLocked = true;           // Check if the artwork is stole.

    void Start()
    {
        GameEvents.instance.onCheckPassword += UnloadArtwork;
    }


    public string GetPassword()
    {
        return password;                    // Get the password of that artwork.
    }


    public void UnloadArtwork(string newPassword)
    {
        if(password != newPassword)             // If password does not match, return.
        {
            return;
        }

        ComputerController.instance.findMatch = true;       // Notify the computer that there's a match.

        if (!isLocked)
        {
            return;                                     // If it's already stole.
        }

        CGameManager.instance.IncreItemCollected();             // Increment the number of item collected.

        isLocked = false;

        // TODO: Add unlock events.
        Vector3 newPos = transform.position;
        newPos.y -= 5.5f;

        transform.position = newPos;
    }
}
