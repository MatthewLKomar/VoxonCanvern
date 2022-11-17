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
        GameEvents.instance.onCheckPassword += UnloadArtwork;
    }


    public string GetPassword()
    {
        return password;
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
            return;
        }

        CGameManager.instance.IncreItemCollected();

        Debug.LogWarning(id + " is unlocked");

        isLocked = false;

        // TODO: Add unlock events.
        Vector3 newPos = transform.position;
        newPos.y -= 5.5f;

        transform.position = newPos;
    }
}
