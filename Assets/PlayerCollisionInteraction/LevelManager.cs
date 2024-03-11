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
    public List<MonoBehaviour> allISkillsMonobehaviour = new List<MonoBehaviour>();
    private List<ISkill> allSkills = new List<ISkill>();
    private int skillAmount = 0;

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

        foreach (var VARIABLE in allISkillsMonobehaviour)
        {
            ISkill testISkill = (ISkill)VARIABLE;
            if (testISkill == null)
            {
                Debug.LogError("NO ISkill in assigned monobehaviour");
            }
            else
            {
                allSkills.Add((ISkill)VARIABLE);
            }
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

    /// <summary>
    /// Collectibles unlocks new levels
    /// </summary>
    private void ObtainCollectible()
    {
        collectiblesGained++;
        UI_PlayerStats.i.UpdateCollectibleCount(collectiblesGained,(int)totalCollectibles);
        OnCollectiblesAmountChange.Invoke(collectiblesGained);
    }

    /// <summary>
    /// Interaction points unlock new skills
    /// </summary>
    private void ObtainInteractionPoint()
    {
        interactionPoint++;
        if (interactionPoint > ipNeedToGetSkill) //Get New Skill
        {
            interactionPoint = 0;
            if (allSkills.Count > skillAmount)
            {
                allSkills[skillAmount].UnlockSkill();
                skillAmount++;
            }
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
