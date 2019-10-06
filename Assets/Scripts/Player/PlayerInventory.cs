using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory _instance = null;

    public static PlayerInventory Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            if (_instance != null && _instance != value)
            {
                Destroy(_instance);
            }

            _instance = value;
        }
    }

    public Dictionary<PowerupBase.PowerupType, PowerupBase> playerPowerups = new Dictionary<PowerupBase.PowerupType, PowerupBase>();
    public List<PowerupBase.PowerupType> unlockedPowerUps = new List<PowerupBase.PowerupType>();

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockPowerup(PowerupBase.PowerupType unlockedType)
    {
        if (!unlockedPowerUps.Contains(unlockedType))
        {
            unlockedPowerUps.Add(unlockedType);
        }
    }

    public void PurchasePowerup(PowerupBase.PowerupType purchasedType)
    {
        if (playerPowerups.ContainsKey(purchasedType))
        {
            Debug.LogError("Player already owns this powerup");
            return;
        }

        // TODO: Add all other types
        switch (purchasedType)
        {
            case PowerupBase.PowerupType.DOUBLE_JUMP:
                playerPowerups.Add(purchasedType, new DoubleJumpPowerup());
                playerPowerups[purchasedType].OnPurchased();
                break;
            case PowerupBase.PowerupType.DASH:
                playerPowerups.Add(purchasedType, new DashPowerup());
                playerPowerups[purchasedType].OnPurchased();
                break;
        }
    }
}
