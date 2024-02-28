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

    [SerializeField] private float playerSpeed = 0;
    
    private Vector3 lastPosition;
    void Start()
    {
        lastPosition = ballRigidBody.transform.position;
    }

    void FixedUpdate()
    {
        float distanceMoved = Vector3.Distance(ballRigidBody.transform.position, lastPosition);
        
        playerSpeed = distanceMoved / Time.deltaTime;
        
        lastPosition = ballRigidBody.transform.position;
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public PlayerForm GetPlayerForm()
    {
        return playerForm;
    }

    public void ChangePlayerForm(PlayerForm targetForm)
    {
        playerForm = targetForm;
        string export = "Change Form To: ";
        switch (targetForm)
        {
            case PlayerForm.Cloud:
                export += "Cloud";
                break;
            case PlayerForm.Drop:
                export += "Drop";
                break;
            case PlayerForm.Ice:
                export += "Ice";
                break;
            case PlayerForm.Wave:
                export += "Wave";
                break;
        }
        print(export);
    }
}
