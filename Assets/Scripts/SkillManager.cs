using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    static SkillManager instance;
    public static SkillManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SkillManager>();
            }
            return instance;
        }
    }

    [SerializeField] private List<MonoBehaviour> allSkillsList = null;

    public void UnlockSkill(int targetIndex)
    {
        if (targetIndex > allSkillsList.Count)
        {
            Debug.LogError("BRO YOU FUCKED UP, skill index too large");
        }
        else if(((ISkill)allSkillsList[targetIndex]).IsSkillUnlocked())
        {
            Debug.LogError("skill index of " + targetIndex + " is already unlocked" );
        }
        else 
        {
            ((ISkill)allSkillsList[targetIndex]).UnlockSkill();
        }
    }

    private void Awake()
    {
        foreach (var element in allSkillsList)
        {
            if ((ISkill)element == null)
            {
                Debug.LogError("BRO YOU FUCKED UP");
            }
        }
    }

    public bool GetSkillUnlocked(int targetIndex)
    {
        if (targetIndex > allSkillsList.Count)
        {
            return false;
        }
        else 
        {
            return ((ISkill)allSkillsList[targetIndex]).IsSkillUnlocked() ;
        }
    }
}
