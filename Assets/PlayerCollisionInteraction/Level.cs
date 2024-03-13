using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class Level
{
    // Serialized Private Variables
    [SerializeField] private int levelID = -1;
    [SerializeField] private int amountInteractionPointToUnlockNext = 10;
    [SerializeField] private PlayableDirector playableDirectorToQue = null;
    [SerializeField] private GameObject levelMapGameObject = null;

    // Private Variables
    private Level nextLevel;
    private bool levelUnlocked = false;
    private int amountInteractionPointGained = 0;
    
    public void SetUp(Level nextLevel)
    {
        this.nextLevel = nextLevel;
    }

    public void UponInteractionPointGained(int newInteractionPoint)
    {
        amountInteractionPointGained += newInteractionPoint;
        if (amountInteractionPointGained >= amountInteractionPointToUnlockNext)
        {
            levelUnlocked = true;
            if(playableDirectorToQue != null) playableDirectorToQue.Play();
            if(levelMapGameObject != null) levelMapGameObject.SetActive(true);
        }
    }
    
}
