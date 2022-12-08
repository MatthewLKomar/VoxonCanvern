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
    //[SerializeField] TextMeshPro counterUI;
    //[SerializeField] TextMeshPro titleUI;
    private float ftimer = 300f;

    
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

        UpdatePuzzVisib();
        
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


    public void StartGame()
    {            
        ComputerController.instance.SetStartPC(true);           // Enable the input computer.

        SecondDisplay.instance.SetItemUI("Number of locks opened: 0");

        LightManager.instance.TurnOnLights();

        gameStage = 1;
        isInStage = false;
    }
    
    
    // Start the pre game part.
    private IEnumerator GameStage0()
    {
        LightManager.instance.TurnOnLight(0);
        
        while (!Input.GetKey(KeyCode.KeypadMinus) /*|| !GameEvents.instance.isStart*/) //Wait until the Voxon tells the game starts, or manually press the right shift button.
        {
            if (gameStage != 0) yield break; 
            yield return null;      
        }

        StartGame();
        // Wait until the player enters a button.
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


    private void UpdatePuzzVisib()
    {
        if (gameStage != 1)
        {
            return;
        }

        string transmitStr = PuzzleVisibility.instance.outputString;

        int numberCount = (int)char.GetNumericValue(transmitStr[0]);
        int numberOne = -1;
        int numberTwo = -1;

        if (numberCount == 1)
        {
            numberOne =  (int)char.GetNumericValue(transmitStr[1]);
        }
        else if (numberCount == 2)
        {
            numberTwo = (int)char.GetNumericValue(transmitStr[2]);
        }
        
        
        if (Mathf.Floor(timer) != ftimer)
        {
            //NetworkManager.current.Send( ObjectManager.current.BuildBufferPuzzleVisibile(2, 1, 1));
            NetworkManager.current.Send( ObjectManager.current.BuildBufferPuzzleVisibile(numberCount, numberOne, numberTwo));
            ftimer = Mathf.Floor(timer);
        }
    }


    public void IncreItemCollected()            // Call by other classes to increment the number of artworks collected.
    {
        nItemCollected++;
       // counterUI.text = "Number of item stole: " + nItemCollected.ToString();
       SecondDisplay.instance.SetItemUI("Number of locks opened: " + nItemCollected.ToString());
    }
}
