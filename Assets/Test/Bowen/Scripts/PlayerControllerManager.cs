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
    [SerializeField] private float scaleIncrement = 0.1f; // Scale变化的增量
    [SerializeField] private float minScale = 0.5f; // 最小Scale值
    [SerializeField] private float maxScale = 3f; // 最大Scale值

    
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
        if (Input.GetKeyDown(KeyCode.LeftBracket)) // 按下"["
        {
            AdjustScale(-scaleIncrement); // 减小scale
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) // 按下"]"
        {
            AdjustScale(scaleIncrement); // 增加scale
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
    
    private void AdjustScale(float increment)
    {
        GameObject activeController = GetActiveController();
        if(activeController != null)
        {
            Vector3 currentScale = activeController.transform.localScale;
            float newScale = Mathf.Clamp(currentScale.x + increment, minScale, maxScale); // 计算新的scale值，确保它在限定范围内
            activeController.transform.localScale = new Vector3(newScale, newScale, newScale); // 应用新的scale
        }
    }

    private GameObject GetActiveController()
    {
        // 根据当前的 playerForm 返回相应的controller
        switch(playerForm)
        {
            case PlayerController.PlayerForm.Drop:
            {
                Vector3 dropPos = dropController.transform.position + new Vector3(0, 2f, 0f);
                dropController.transform.position = dropPos;
                return dropController.transform.parent.gameObject;
            }
            case PlayerController.PlayerForm.Ice: return iceController;
            case PlayerController.PlayerForm.Wave: return waveController;
            case PlayerController.PlayerForm.Cloud: return cloudController;
            default: return null;
        }
    }
}
