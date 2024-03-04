using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class Level
{
    [SerializeField] private int requiredCollectiblesAmount = 10;
    [SerializeField] private PlayableDirector playableDirectorToQue = null;
    [SerializeField] private GameObject levelMapGameObject = null;

    private bool levelUnlocked = false;
        
    public void SetUp()
    {
        LevelManager.i.OnCollectiblesAmountChange.AddListener(UponCollectiblesChange);
    }

    public void UponCollectiblesChange(int newCollectiblesAmount)
    {
        if (newCollectiblesAmount >= requiredCollectiblesAmount)
        {
            levelUnlocked = true;
            LevelManager.i.OnCollectiblesAmountChange.RemoveListener(UponCollectiblesChange);
            if(playableDirectorToQue != null) playableDirectorToQue.Play();
            if(levelMapGameObject != null) levelMapGameObject.SetActive(true);
        }
    }
    
}
