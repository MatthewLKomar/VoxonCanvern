using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleVisib : MonoBehaviour
{
    public static PuzzleVisib instance { private set; get; }
    [SerializeField] List<GameObject> BrightPuzzle;
    [SerializeField] List<GameObject> DarkPuzzle;

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


    public void TogglePuzzle(bool isBright)
    {
        for(int i = 0; i < BrightPuzzle.Count; i++)
        {
            if (!isBright)
            {
                BrightPuzzle[i].SetActive(false);
                DarkPuzzle[i].SetActive(true);
            }
            else
            {
                BrightPuzzle[i].SetActive(true);
                DarkPuzzle[i].SetActive(false);
            }
        }
    }


    public void TogglePuzzle(int id, bool isBright)
    {
        if (!isBright)
        {
            BrightPuzzle[id].SetActive(false);
            DarkPuzzle[id].SetActive(true);
        }
        else
        {
            BrightPuzzle[id].SetActive(true);
            DarkPuzzle[id].SetActive(false);
        }
    }


    public void TurnOffPuzzle()
    {
        for (int i = 0; i < BrightPuzzle.Count; i++)
        {
            BrightPuzzle[i].SetActive(false);
            DarkPuzzle[i].SetActive(false);
        }
    }
}
