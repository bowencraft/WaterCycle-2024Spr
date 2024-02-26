using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController instance;
    public static PlayerController i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }
    
    [SerializeField] private Rigidbody ballRigidBody;

    public enum PlayerForm{Drop, Ice, Wave, Cloud}
    [SerializeField] private PlayerForm playerForm = PlayerForm.Drop;

    public float playerSpeedDisplay = 0;
    
    private void Update()
    {
        playerSpeedDisplay = GetPlayerSpeed();
    }

    public float GetPlayerSpeed()
    {
        return ballRigidBody.velocity.magnitude;
    }

    public PlayerForm GetPlayerForm()
    {
        return playerForm;
    }
}
