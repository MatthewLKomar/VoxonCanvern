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
    private int nItemCollected = 0;             // The number of item collected.

    [SerializeField] float timer = 300f;        // The total time of this game after started.
    [SerializeField] TextMeshPro counterUI;


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
            case 1:                         // If it's in game.
                return;
            case 2:                         // If after the game.
                StartCoroutine(GameStage2());
                isInStage = true;
                break;
            case 0:                        // If before the game.
                StartCoroutine(GameStage0());
                isInStage = true;
                break;
        }
    }


    // Start the pre game part.
    private IEnumerator GameStage0()
    {
        LightManager.instance.TurnOnLight(0);

        while (!Input.GetKey(KeyCode.RightShift) || GameEvents.instance.isStart) //Wait until the Voxon tells the game starts, or manually press the right shift button.
        {
            yield return null;              // Wait until the player enters a button.
        }

        ComputerController.instance.SetStartPC(true);           // Enable the input computer.

        counterUI.text = "Number of item collected: 0";         // Set the UI.

        LightManager.instance.TurnOnLights();

        gameStage = 1;
        isInStage = false;
    }


    private IEnumerator GameStage2()
    {
        LightManager.instance.ToggleFlashLight(true);
        
        AudioManager.instance.StopBGMSound();
        AudioManager.instance.PlayAlertSound();

        yield return null;
    }


    private void UpdateTimer()              // Update the timer of the game.
    {
        if(gameStage != 1)
        {
            return;
        }

        if (timer <= 0 || GameEvents.instance.isEnd)
        {
            gameStage = 2;
            isInStage = false;
            return;
        }

        timer -= 1 * Time.deltaTime;                    // Update the timer.
    }


    public void IncreItemCollected()            // Call by other classes to increment the number of artworks collected.
    {
        nItemCollected++;
        counterUI.text = "Number of item collected: " + nItemCollected.ToString();
    }
}
