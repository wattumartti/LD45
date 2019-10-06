using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupShopItem : MonoBehaviour
{
    [SerializeField] private Image powerupImage = null;

    private PowerupShop parentShop = null;
    private PowerupCostList.PowerUpCostPair powerUpInfo;

    public void InitializeItem(PowerupCostList.PowerUpCostPair costPair, PowerupShop shop)
    {
        parentShop = shop;
        powerUpInfo = costPair;

        if (powerupImage == null)
        {
            return;
        }

        powerupImage.sprite = costPair.powerupSprite;

        if (!PlayerInventory.Instance.unlockedPowerUps.Contains(powerUpInfo.type))
        {
            // TODO: Show locked
        }
        else
        {
            if (PlayerInventory.Instance.playerPowerups.ContainsKey(powerUpInfo.type))
            {
                // TODO: Show owned
            }
            else
            {
                // TODO: Show purchasable
            }
        }
    }

    public void InitializeItem(PowerupBase.PowerupType type, PowerupShop shop)
    {
        parentShop = shop;
        powerUpInfo = GameManager.Instance.powerUpCosts.GetCostPair(type);

        if (powerupImage == null)
        {
            return;
        }

        powerupImage.sprite = powerUpInfo.powerupSprite;

        if (!PlayerInventory.Instance.unlockedPowerUps.Contains(powerUpInfo.type))
        {
            // TODO: Show locked
        }
        else
        {
            if (PlayerInventory.Instance.playerPowerups.ContainsKey(powerUpInfo.type))
            {
                // TODO: Show owned
            }
            else
            {
                // TODO: Show purchasable
            }
        }
    }

    public void OnItemClicked()
    {
        if (parentShop == null)
        {
            return;
        }

        parentShop.selectedItem = powerUpInfo.type;
    }
}
