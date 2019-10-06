using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerup : PowerupBase
{
    public DoubleJumpPowerup() : base()
    {
        type = PowerupType.DOUBLE_JUMP;
    }

    public override bool ActivatePowerup()
    {
        if (!base.ActivatePowerup())
        {
            Debug.LogError("Powerup not ready!");
            return false;
        }

        if (PlayerController.Instance.isGrounded)
        {
            Debug.LogError("Player is on ground!");
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
