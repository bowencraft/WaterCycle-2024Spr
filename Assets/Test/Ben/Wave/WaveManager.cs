using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaveManager : SystemsManager
{
    [SerializeField] private WaveController controller;
    [SerializeField] private WaveAppearance appearance;

    [Space]
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnSize = 0f;

    public override float CurrentSize { get { return appearance.waveSize; } }

    public override void Activate(Vector3 position, Vector3 velocity)
    {
        SoundManager.PlayMusic("Ocean");
        position.y = spawnHeight;
        controller.rb.position = position;

        appearance.waveSize = spawnSize;

        cam.Priority = 2;
        controller.active = true;
    }

    public override void Deactivate()
    {
        cam.Priority = -1;
        controller.active = false;
    }

    public override void SetPosition(Vector3 position)
    {
        position.y = spawnHeight;
        controller.rb.position = position;
    }
}
