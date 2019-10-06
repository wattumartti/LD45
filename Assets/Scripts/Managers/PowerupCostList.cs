using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerups", menuName = "ScriptableObjects/PowerupCostList", order = 1)]
public class PowerupCostList : ScriptableObject
{
    [System.Serializable]
    public struct PowerUpCostPair
    {
        public PowerupBase.PowerupType type;
        public int cost;
        public Sprite powerupSprite;
        public string titleText;
        public string descriptionText;
    }

    public List<PowerUpCostPair> powerUpCostList = new List<PowerUpCostPair>();

    public int GetCostForPowerup(PowerupBase.PowerupType type)
    {
        foreach (PowerUpCostPair pair in powerUpCostList)
        {
            if (pair.type == type)
            {
                return pair.cost;
            }
        }

        return int.MaxValue;
    }

    public Sprite GetSpriteForPowerup(PowerupBase.PowerupType type)
    {
        foreach (PowerUpCostPair pair in powerUpCostList)
        {
            if (pair.type == type)
            {
                return pair.powerupSprite;
            }
        }

        return null;
    }

    public string GetTitleText(PowerupBase.PowerupType type)
    {
        foreach (PowerUpCostPair pair in powerUpCostList)
        {
            if (pair.type == type)
            {
                return pair.titleText;
            }
        }

        return null;
    }

    public string GetDescriptionText(PowerupBase.PowerupType type)
    {
        foreach (PowerUpCostPair pair in powerUpCostList)
        {
            if (pair.type == type)
            {
                return pair.descriptionText;
            }
        }

        return null;
    }

    public PowerUpCostPair GetCostPair(PowerupBase.PowerupType type)
    {
        foreach (PowerUpCostPair pair in powerUpCostList)
        {
            if (pair.type == type)
            {
                return pair;
            }
        }

        return new PowerUpCostPair();
    }
}
