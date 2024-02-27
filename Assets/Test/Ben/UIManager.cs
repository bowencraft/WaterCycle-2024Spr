using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject cloudControls;
    [SerializeField] private GameObject raindropControls;
    [SerializeField] private GameObject waveControls;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        cloudControls.SetActive(false);
        raindropControls.SetActive(false);
        waveControls.SetActive(false);

        switch (PhaseManager.CurrentPhase)
        {
            case PhaseManager.Phase.Cloud:
                cloudControls.SetActive(true);
                break;
            case PhaseManager.Phase.Raindrop:
                raindropControls.SetActive(true);
                break;
            case PhaseManager.Phase.Wave:
                waveControls.SetActive(true);
                break;
        }
    }
}
