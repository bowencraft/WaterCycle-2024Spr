using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_Collectible : PlayerCollisionInteraction
{
    [SerializeField] private Transform collectibleGameObject;
    //[SerializeField] private MonoBehaviour ISkillMonobehaviour = null;
    [SerializeField] private int skillIndex = -1;
    
    private void Awake()
    {
        if (skillIndex == -1)
        {
            Debug.LogError("Bro you forgot to assign a valid skill index at gameobject: "+ gameObject.name);
        }
        /*
        if (ISkillMonobehaviour == null)
        {
            Debug.LogError("Bro you forgot to assign skill for collectible at gameobject: "+ gameObject.name);
        }
        else if ((ISkill)ISkillMonobehaviour == null)
        {
            Debug.LogError("Bro the monobehaviour you assigned at gameobject: "+ gameObject.name + " does not have a ISkill interface");
        }*/
    }

    protected override void PlayEffect()
    {
        collectibleGameObject.gameObject.SetActive(false);
        //((ISkill)ISkillMonobehaviour).UnlockSkill();
        base.PlayEffect();
    }
}
