using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CloudForm : MonoBehaviour, ISkill
{
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool canUse = false;
    public float coolDownTime = 10f;
    public float duration = 10f;
    
    public int enlargeCount = 30;

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
        PlayerControllerManager.Instance.AdjustScale(enlargeCount);
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
        float count = enlargeCount;
        
        for (float timer = duration; timer >= 0; timer -= Time.deltaTime)
        {
            if (timer < count * (duration / (float)enlargeCount))
            {
                PlayerControllerManager.Instance.AdjustScale(-1);
                print("reduce 1" );
                count--;
            }
            // PlayerControllerManager.Instance.AdjustScale(- ((enlargeCount + 0.2f) / duration) * Time.deltaTime);
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
