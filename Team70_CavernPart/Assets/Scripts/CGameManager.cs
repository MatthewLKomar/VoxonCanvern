using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CGameManager : MonoBehaviour
{
    public static CGameManager instance { private set; get; }

    private int gameStage = 0;          // 0 = before game, 1 = in game, 2 = end game.
    private bool isInStage = false;

    private float timer = 300f;
    private int nItemCollected = 0;

    [SerializeField] TextMeshPro timerUI;           // TODO: Will need move all the UI stuff to a specific script.
    [SerializeField] GameObject counterUI;


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

    
    void Update()
    {
        UpdateStatus();
        UpdateTimer();
    }


    private void UpdateStatus()              // Update the status of the game.
    {
        if (isInStage)
        {
            return;
        }

        switch (gameStage)
        {
            case 1:
                return;
            case 2:
                // TODO: END WORK.
                break;
            case 0:
                StartCoroutine(GameStage0());
                isInStage = true;
                break;
        }
    }


    // Start the pre game part.
    private IEnumerator GameStage0()
    {
        while (!Input.GetKey(KeyCode.RightShift))
        {
            yield return null;              // Wait until the player enters a button.
        }

        ComputerController.instance.SetStartPC(true);           // Enable the input computer.

        gameStage = 1;
        isInStage = false;
    }


    private void UpdateTimer()              // Update the timer of the game.
    {
        if(gameStage != 1)
        {
            return;
        }

        if (timer <= 0)
        {
            gameStage = 2;
            isInStage = false;
            return;
        }

        timer -= 1 * Time.deltaTime;                // Update the timer.
        timerUI.text = Math.Floor(timer).ToString();        // Update the timer UI.
    }


    public void IncreItemCollected()            // Call by other classes to increment the number of artworks collected.
    {
        nItemCollected++;
        counterUI.GetComponent<TextMeshPro>().text = "Number of item collected: " + nItemCollected.ToString();
    }
}
