using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SuperJump : MonoBehaviour, ISkill
{
    [SerializeField]
    private bool isUnlocked = false;
    [SerializeField]
    private bool canUse = false;
    public float coolDownTime = 10f;
    public LayerMask groundLayer; // 用于射线检测的地面层

    private void Start()
    {
        // if (groundLayer == 0) groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        if (isUnlocked && canUse && Input.GetKeyDown(KeyCode.P))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0,1,0), -Vector3.up, out hit, Mathf.Infinity, groundLayer))
            {
                float heightAboveGround = hit.distance;
                if (heightAboveGround < 2f)
                {
                    UseSkill();
                }
            }
        }
    }

    public void UseSkill()
    {
        PlayerControllerManager.Instance.getCurrentControllerRigidbody().AddForce(Vector3.up * 1000f, ForceMode.Impulse);
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
