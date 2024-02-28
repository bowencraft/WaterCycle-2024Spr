using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropManager : SystemsManager
{
    [SerializeField] private RaindropController controller;
    [SerializeField] private RaindropAppearance appearance;

    [Space]
    [SerializeField] private float spawnSize = 0f;

    public override float CurrentSize { get { return appearance.dropSize; } }

    public override void Activate(Vector3 position, Vector3 velocity)
    {
        SoundManager.PlayMusic("Town");
        controller.rb.position = position;
        controller.rb.velocity = velocity;
        controller.ResetSpring();

        appearance.dropSize = spawnSize;

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

    public override Vector3 GetPosition()
    {
        return controller.rb.position;
    }
}
