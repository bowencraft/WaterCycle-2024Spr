using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCI_FormChange : PlayerCollisionInteraction
{
    public PlayerController.PlayerForm changeFormTo = PlayerController.PlayerForm.Drop;
    
    protected override void PlayEffect()
    {
        PlayerController.i.ChangePlayerForm(changeFormTo);
    }
}
