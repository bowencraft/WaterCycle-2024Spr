using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_IceForm : MonoBehaviour, ISkill
{
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool canUse = false;
    public float coolDownTime = 10f;
    public float duration = 10f;
    public float enlargeScale = 0.5f;

    private void Update()
    {
        if (isUnlocked && canUse && Input.GetKeyDown(KeyCode.Alpha1)) // press 1 to use
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        PlayerControllerManager.Instance.ChangePlayerForm(PlayerController.PlayerForm.Ice);
        PlayerControllerManager.Instance.AdjustScale(enlargeScale);
        canUse = false;
        StartCoroutine(StartCoolDown());
        StartCoroutine(SwitchBackToWaterForm());
    }

    IEnumerator StartCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canUse = true;
    }
    
    IEnumerator SwitchBackToWaterForm()
    {
        // yield return new WaitForSeconds(duration);
        
        for (float timer = duration; timer >= 0; timer -= Time.deltaTime)
        {
            PlayerControllerManager.Instance.AdjustScale(- ((enlargeScale + 0.2f) / duration) * Time.deltaTime);
            yield return null;
        }
        
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
