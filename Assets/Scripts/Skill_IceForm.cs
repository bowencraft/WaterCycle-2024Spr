using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IceForm : MonoBehaviour, ISkill
{
    private bool isLocked = false;
    private bool canUse = false;
    public float coolDownTime = 10f;

    private void Update()
    {
        if (!isLocked && canUse && Input.GetKeyDown(KeyCode.I))
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        PlayerControllerManager.Instance.ChangePlayerForm(PlayerController.PlayerForm.Ice);
        canUse = false;
        StartCoroutine(StartCoolDown());
    }

    IEnumerator StartCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canUse = true;
    }

    public void UnlockSkill()
    {
        isLocked = true;
        canUse = true;
    }
    
    public bool IsSkillCanUse()
    {
        return canUse;
    }
    
    public bool IsSkillUnlocked()
    {
        return isLocked;
    }
}
