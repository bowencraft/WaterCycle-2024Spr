using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Invector.vCamera;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = System.Random;

public class PlayerControllerManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private vThirdPersonCamera vCamera;
    
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
    [SerializeField] private float initialScale = 1f; // 新的Scale值
    [SerializeField] private float minScale = 0.5f; // 最小Scale值
    [SerializeField] private float maxScale = 3f; // 最大Scale值

    private GameObject currentController;
    
    [Header("vThirdPersonController")]
    [SerializeField] private vShooterMeleeInput vInput;

    public Rigidbody getCurrentControllerRigidbody()
    {
        return currentController.GetComponent<Rigidbody>();
    }
    
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
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //     ChangePlayerForm(PlayerController.PlayerForm.Drop);
        // } else if (Input.GetKeyDown(KeyCode.O))
        // {
        //     ChangePlayerForm(PlayerController.PlayerForm.Cloud);
        // }  
        if (Input.GetKeyDown(KeyCode.L)) // 按下"["
        {
            print("Size down");
            AdjustScale(-scaleIncrement); // 减小scale
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) // 按下"]"
        {
            print("Size up");
            AdjustScale(scaleIncrement); // 增加scale
        }

        if (currentController.transform.localScale.x <= minScale)
        {
            currentController.transform.localScale = new Vector3(initialScale, initialScale, initialScale); // 应用新的scale
            switch (playerForm)
            {
                case PlayerController.PlayerForm.Cloud:
                    ChangePlayerForm(PlayerController.PlayerForm.Drop);
                    break;
                case PlayerController.PlayerForm.Ice:
                    ChangePlayerForm(PlayerController.PlayerForm.Drop);
                    break;
            }
            
        }
    }


    public void ChangePlayerForm(PlayerController.PlayerForm form)
    {

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
            {
                newController = iceController;
                break;
            }
            case PlayerController.PlayerForm.Wave:
                newController = waveController;
                break;
            case PlayerController.PlayerForm.Cloud:
                newController = cloudController;
                // CameraController.Instance.distance = CameraController.Instance.distanceMinMax.y;
                StartCoroutine(SwitchBackToWaterForm());
                break;
        }

        if(newController != null)
        {
            // Activate the new controller
            // Deactivate all controllers
            newController.transform.position = previousController.transform.position;
            if (form == PlayerController.PlayerForm.Drop)
            {
                newController.transform.parent.position = previousController.transform.position;
                newController.transform.localPosition = new Vector3(0, 0.551f, 0f);
                vInput.ChangeCameraState("Strafing", true);
            } else if (form == PlayerController.PlayerForm.Ice)
            {
                newController.transform.position = previousController.transform.position + new Vector3(0, 1f, 0f);
                vInput.ChangeCameraState("Strafing", true);
            } else if (form == PlayerController.PlayerForm.Cloud)
            {
                newController.transform.position = previousController.transform.position + new Vector3(0, 10f, 0f);
                vInput.ChangeCameraState("Cloud", true);
                
            }
            print(vCamera.currentState.Name);
            
            
            iceController.SetActive(false);
            dropController.SetActive(false);
            waveController.SetActive(false);
            cloudController.SetActive(false);
            
            newController.SetActive(true);
            vCamera.SetMainTarget(newController.transform);
            // newController.GetComponent<Rigidbody>().AddForce(new Vector3(0, 500, 0));


            currentController = newController;

            // If there is a previous active controller, set the new one's position to match it
            // This is just a placeholder, adjust according to your game's logic
            // e.g., newController.transform.position = previousController.transform.position;

            // Update the player form
            playerForm = form;
            PlayerController.i.ChangePlayerForm(form);

            // Set cameraFreeLook's follow target to the new controller's transform
            // cameraFollower.target = newController.transform;
            
        }
    }

    public void AdjustScale(float increment, float growingSizeMax = -1)
    {
        if (growingSizeMax != -1)
        {
            maxScale = growingSizeMax;
        }
        
        GameObject activeController = GetActiveController();
        if(activeController != null)
        {
            if(activeController.transform.localScale.x > maxScale) return;
            
            Vector3 currentScale = activeController.transform.localScale;
            float newScale = Mathf.Clamp(currentScale.x + increment, minScale, maxScale); // 计算新的scale值，确保它在限定范围内
            activeController.transform.localScale = new Vector3(newScale, newScale, newScale); // 应用新的scale
        }
    }
    
    public float duration = 10f;
    IEnumerator SwitchBackToWaterForm()
    {
        yield return new WaitForSeconds(duration);
        PlayerControllerManager.Instance.ChangePlayerForm(PlayerController.PlayerForm.Drop);
    }

    private GameObject GetActiveController()
    {
        // 根据当前的 playerForm 返回相应的controller
        // switch(playerForm)
        // {
        //     // case PlayerController.PlayerForm.Drop:
        //     // {
        //     //     Vector3 dropPos = dropController.transform.position + new Vector3(0, 2f, 0f);
        //     //     dropController.transform.position = dropPos;
        //     //     
        //     //     return dropController.transform.gameObject;
        //     // }
        //     
        //     case PlayerController.PlayerForm.Drop: return dropController;
        //     case PlayerController.PlayerForm.Ice: return iceController;
        //     case PlayerController.PlayerForm.Wave: return waveController;
        //     case PlayerController.PlayerForm.Cloud: return cloudController;
        //     default: return null;
        // }
        return currentController;
    }
    
    public void ChangeCurrentControllerActive(bool active)
    {
        GameObject currentController = GetActiveController();
        if (currentController != null)
        {
            if (GetComponent<GeneralController>() != null) GetComponent<GeneralController>().enabled = active;
            if (GetComponent<CloudController>() != null) GetComponent<CloudController>().enabled = active;
            if (GetComponent<WaterCharacterController>() != null) GetComponent<WaterCharacterController>().enabled = active;
        }
    }
}
