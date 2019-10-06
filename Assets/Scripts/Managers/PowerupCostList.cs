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
}
