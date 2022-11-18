using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public void Awake()
    {
        //makes this game object a publicly visible object 
        if (current == null)
        {
            current = this;
        }
        else Destroy(gameObject);
    }
    public void NetworkerPrinter(string text)
    {
        print(text);
    }
}
