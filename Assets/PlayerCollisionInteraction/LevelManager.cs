using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

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
    public float totalCollectibles = 0;
    public UnityEvent<int> OnCollectiblesAmountChange = new UnityEvent<int>();

    public int interactionPoint = 0;
    public int ipNeedToGetSkill = 10;

    public List<Level> allLevels = new List<Level>();


    public enum RewardType
    {
        InteractionPoint,
        Collectible
    }

    private void Awake()
    {
        foreach (var VARIABLE in allLevels)
        {
            VARIABLE.SetUp();
        }
    }

    private void Start()
    {
        int totalCollectiblesCount = 0;
        foreach (var VARIABLE in FindObjectsOfType<PlayerCollisionInteraction>())
        {
            if (VARIABLE.interactionReward == RewardType.Collectible)
            {
                totalCollectiblesCount++;
            }
        }

        totalCollectibles = totalCollectiblesCount;
        UI_PlayerStats.i.UpdateCollectibleCount(0,(int)totalCollectibles);
        
        UI_PlayerStats.i.UpdateExperienceDisplay(((float)interactionPoint)/((float)ipNeedToGetSkill));
    }

    private void ObtainCollectible()
    {
        collectiblesGained++;
        UI_PlayerStats.i.UpdateCollectibleCount(collectiblesGained,(int)totalCollectibles);
        OnCollectiblesAmountChange.Invoke(collectiblesGained);
    }

    private void ObtainInteractionPoint()
    {
        interactionPoint++;
        if (interactionPoint > ipNeedToGetSkill)
        {
            interactionPoint = 0;
        }
        UI_PlayerStats.i.UpdateExperienceDisplay(((float)interactionPoint)/((float)ipNeedToGetSkill));
    }

    public void GainReward(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.InteractionPoint:
                ObtainInteractionPoint();
                break;
            case RewardType.Collectible:
                ObtainCollectible();
                break;
        }
    }

}
