using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            if (_instance != null && _instance != value)
            {
                Destroy(_instance.gameObject);
            }

            _instance = value;
        }
    }

    public PlayerController playerController = null;
    [SerializeField] private PlayerController playerControllerPrefab = null;

    public Vector2[] levelStartingPositions;
    public int currentLevelId = 0;

    public PowerupCostList powerUpCosts = null;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        playerController = Instantiate(playerControllerPrefab, levelStartingPositions[0], Quaternion.identity);

        // TODO: Cool entrance
        playerController.playerSprite.transform.DOScale(1, 0.5f);
        playerController.body.bodyType = RigidbodyType2D.Dynamic;
    }
}
