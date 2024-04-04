using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IcCloudrm : MonoBehaviour, ISkill
{
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool canUse = false;
    public float coolDownTime = 10f;
    public float duration = 10f;

    private void Update()
    {
        if (isUnlocked && canUse && Input.GetKeyDown(KeyCode.Alpha2)) // press 1 to use
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        PlayerControllerManager.Instance.ChangePlayerForm(PlayerController.PlayerForm.Cloud);
        canUse = false;
        StartCoroutine(StartCoolDown());
        // StartCoroutine(SwitchBackToWaterForm());
    }

    IEnumerator StartCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canUse = true;
    }
    
    IEnumerator SwitchBackToWaterForm()
    {
        yield return new WaitForSeconds(duration);
        PlayerControllerManager.Instance.ChangePlayerForm(PlayerController.PlayerForm.Drop);
    }


    public void UnlockSkill()
    {
        isUnlocked = true;
        canUse = true;
    }
    
    public bool IsSkillCanUse()
    {
        return canUse;
    }
    
    public bool IsSkillUnlocked()
    {
        return isUnlocked;
    }
}
