using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private static PhaseManager instance;

    [Header("References")]
    [SerializeField] private UIManager uiManager;

    public enum Phase { Cloud, Raindrop, Wave }

    [Header("Phases")]
    public Phase currentPhase;
    public SystemsManager currentManager;


    [Space]
    [SerializeField] private CloudManager cloud;
    [SerializeField] private RaindropManager raindrop;
    [SerializeField] private WaveManager wave;

    public static Phase CurrentPhase { get { return instance.currentPhase; } }
    public static float CurrentSize { get { return instance.currentManager.CurrentSize; } }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switch (currentPhase)
        {
            case Phase.Cloud:
                SoundManager.PlayMusic("Town");
                break;
            case Phase.Raindrop:
                SoundManager.PlayMusic("Town");
                break;
            case Phase.Wave:
                SoundManager.PlayMusic("Ocean");
                break;
        }
    }

    private void Update()
    {
        if (CurrentPhase != Phase.Wave)
            wave.SetPosition(currentManager.GetPosition());
    }

    public static void UpdatePhase(Phase newPhase, Vector3 position, Vector3 velocity)
    {
        Vector2 camRotation = instance.currentManager.GetCamRotation();

        instance.currentPhase = newPhase;
        instance.currentManager.Deactivate();

        switch (newPhase)
        {
            case Phase.Cloud:
                SoundManager.PlayMusic("Town");
                instance.currentManager = Instantiate(instance.cloud);
                instance.currentManager.Activate(position, velocity);
                break;

            case Phase.Raindrop:
                SoundManager.PlayMusic("Town");
                instance.currentManager = Instantiate(instance.raindrop);
                instance.currentManager.Activate(position, velocity);
                break;

            case Phase.Wave:
                SoundManager.PlayMusic("Ocean");
                instance.currentManager = instance.wave;
                instance.wave.Activate(position, velocity);
                break;
        }

        instance.currentManager.SetCamRotation(camRotation);

        instance.uiManager.UpdateUI();
    }
}
