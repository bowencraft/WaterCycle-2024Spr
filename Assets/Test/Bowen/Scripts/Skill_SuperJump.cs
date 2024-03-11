using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SuperJump : MonoBehaviour
{
    private bool isUnlocked = true;

    private bool canUse = true;
    // Update is called once per frame
    void Update()
    {
        if (isUnlocked && canUse && Input.GetKeyDown(KeyCode.P))
        {
            PlayerControllerManager.Instance.getCurrentControllerRigidbody().AddForce(Vector3.up * 1000f);
        }
    }
}
