using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CloudManager : SystemsManager
{
    [SerializeField] private CloudController controller;
    [SerializeField] private CloudAppearance appearance;

    [Space]
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private int spawnSize = 30;

    public override float CurrentSize { get { return appearance.elementCount; } }

    public override void Activate(Vector3 position, Vector3 velocity)
    {
        SoundManager.PlayMusic("Town");
        position.y = spawnHeight;
        controller.rb.position = position;

        appearance.elementCount = spawnSize;

        cam.Priority = 2;
        controller.enabled = true;
        appearance.enabled = true;
    }

    public override void Deactivate()
    {
        cam.Priority = -1;
        gameObject.SetActive(false);

        Destroy(gameObject, 5f);
    }
}
