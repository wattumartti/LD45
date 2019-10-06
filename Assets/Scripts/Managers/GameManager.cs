using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

    [System.Serializable]
    public struct LevelInfo
    {
        public Vector2 startingPosition;
        public int levelId;
        public string sceneName;
    }

    public LevelInfo[] levelInfoArray;
    public int currentLevelId = 0;

    public PowerupCostList powerUpCosts = null;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        playerController = Instantiate(playerControllerPrefab, levelInfoArray[0].startingPosition, Quaternion.identity);

        // TODO: Cool entrance
        playerController.playerSprite.transform.DOScale(1, 0.5f);
        playerController.body.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ChangeLevel(string sceneToOpen)
    {
        if (string.IsNullOrEmpty(sceneToOpen))
        {
            return;
        }

        SceneManager.LoadScene(sceneToOpen);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    public void OnLevelLoaded(Scene scene, LoadSceneMode loadMode)
    {
        foreach (LevelInfo info in levelInfoArray)
        {
            if (info.sceneName == scene.name)
            {
                currentLevelId = info.levelId;
            }
        }

        StartGame();
    }
}
