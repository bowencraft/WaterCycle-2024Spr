using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerControllerManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private CinemachineFreeLook cameraFreeLook;
    
    [Header("Controllers")]
    [SerializeField]
    private GameObject iceController;
    [SerializeField]
    private GameObject dropController;
    [SerializeField]
    private GameObject waveController;
    [SerializeField]
    private GameObject cloudController;

    [Header("State & Size")]
    public int size;
    
    [SerializeField] private PlayerController.PlayerForm playerForm = PlayerController.PlayerForm.Drop;

    static PlayerControllerManager instance;
    public static PlayerControllerManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PlayerControllerManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        
        ChangePlayerForm(playerForm);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangePlayerForm(PlayerController.PlayerForm.Drop);
        } else if (Input.GetKeyDown(KeyCode.I))
        {
            ChangePlayerForm(PlayerController.PlayerForm.Ice);
        } else if (Input.GetKeyDown(KeyCode.O))
        {
            ChangePlayerForm(PlayerController.PlayerForm.Cloud);
        }  
    }


    public void ChangePlayerForm(PlayerController.PlayerForm form)
    {
        // Deactivate all controllers
        iceController.SetActive(false);
        dropController.SetActive(false);
        waveController.SetActive(false);
        cloudController.SetActive(false);

        // Variable to hold the new active controller
        GameObject newController = null;
        GameObject previousController = null;

        // Activate the new controller based on the form argument
        
        switch(playerForm)
        {
            case PlayerController.PlayerForm.Drop:
                previousController = dropController;
                break;
            case PlayerController.PlayerForm.Ice:
                previousController = iceController;
                break;
            case PlayerController.PlayerForm.Wave:
                previousController = waveController;
                break;
            case PlayerController.PlayerForm.Cloud:
                previousController = cloudController;
                break;
        }
        
        switch(form)
        {
            case PlayerController.PlayerForm.Drop:
                newController = dropController;
                break;
            case PlayerController.PlayerForm.Ice:
                newController = iceController;
                break;
            case PlayerController.PlayerForm.Wave:
                newController = waveController;
                break;
            case PlayerController.PlayerForm.Cloud:
                newController = cloudController;
                break;
        }

        if(newController != null)
        {
            // Activate the new controller
            newController.SetActive(true);

            newController.transform.position = previousController.transform.position;
            // If there is a previous active controller, set the new one's position to match it
            // This is just a placeholder, adjust according to your game's logic
            // e.g., newController.transform.position = previousController.transform.position;

            // Update the player form
            playerForm = form;

            // Set cameraFreeLook's follow target to the new controller's transform
            cameraFreeLook.Follow = newController.transform;
            cameraFreeLook.LookAt = newController.transform;
        }
    }
}
