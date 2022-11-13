using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    public static CGameManager instance { private set; get; }

    private int gameStage = 0;          // 0 = before game, 1 = in game, 2 = end game.
    private bool isInStage = false;

    private float timer = 300f;


    private void Awake()
    {
        if(instance != null && instance || this)
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
        UpdateStaus();
        UpdateTimer();
    }


    private void UpdateStaus()              // Update the status of the game.
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
                // TODO: START GAME.
                break;
        }
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

        timer -= 1 * Time.deltaTime;
    }
}
