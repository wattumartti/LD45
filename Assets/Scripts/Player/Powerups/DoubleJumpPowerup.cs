using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerup : PowerupBase
{
    public override bool ActivatePowerup()
    {
        if (!base.ActivatePowerup())
        {
            Debug.LogError("asdasdasd");
            return false;
        }

        PlayerController.Instance.DoJump();

        SetPowerupEnabled(!isEnabled);
        return true;
    }

    public override void OnPurchased()
    {
        base.OnPurchased();

        PlayerController.Instance.onGroundedAction += OnGrounded;
    }

    private void OnGrounded()
    {
        SetPowerupEnabled(true);
    }
}
