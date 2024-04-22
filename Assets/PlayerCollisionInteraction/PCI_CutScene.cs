using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCamera;

public class PCI_CutScene : PlayerCollisionInteraction
{
    [Header("Cut Scene Related")]
    [SerializeField]private Transform targetTransformToLook;

    private Transform holdingTransform = null;
    protected override void PlayEffect()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        holdingTransform = vThirdPersonCamera.instance.mainTarget;
        vThirdPersonCamera.instance.SetMainTarget(targetTransformToLook);
        yield return new WaitForSeconds(1);
        vThirdPersonCamera.instance.FreezeCamera();
        yield return new WaitForSeconds(3);
        vThirdPersonCamera.instance.UnFreezeCamera();
        yield return new WaitForSeconds(1);
        vThirdPersonCamera.instance.SetMainTarget(holdingTransform);
    }
}
