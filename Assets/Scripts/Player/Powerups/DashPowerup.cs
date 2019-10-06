using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowerup : PowerupBase
{
    public DashPowerup() : base()
    {
        type = PowerupType.DASH;
        cooldownSeconds = 5;
    }

    public override bool ActivatePowerup()
    {
        if (!base.ActivatePowerup())
        {
            Debug.LogError("Powerup not ready!");
            return false;
        }

        PlayerController.Instance.StartCoroutine(PlayerController.Instance.DoDash());
        
        return true;
    }
}
