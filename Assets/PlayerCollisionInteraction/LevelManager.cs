using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public static LevelManager i
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }
            return instance;
        }
    }

    public int collectiblesGained = 0;
    public List<Level> allLevels = new List<Level>();

    public UnityEvent<int> OnCollectiblesAmountChange = new UnityEvent<int>();

    private void Awake()
    {
        foreach (var VARIABLE in allLevels)
        {
            VARIABLE.SetUp();
        }
    }

    public void ObtainCollectible()
    {
        collectiblesGained++;
        OnCollectiblesAmountChange.Invoke(collectiblesGained);
    }

}
