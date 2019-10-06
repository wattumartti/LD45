using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance = null;

    [SerializeField] private Canvas mainCanvas = null;
    [SerializeField] private PowerupShop shopPrefab = null;

    private PowerupShop shopInstance = null;

    public static MenuManager Instance
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

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
#if UNITY_EDITOR
        Debug.Log("start game");
#endif
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("quit game");
#endif
        Application.Quit();
    }

    public void TweenOut(GameObject objectToTween)
    {
        CanvasGroup canvas = objectToTween.GetComponent<CanvasGroup>();

        canvas.DOFade(0, 1).OnComplete(() => 
        {
            objectToTween.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }

    public void OpenShop()
    {
        if (shopPrefab == null || mainCanvas == null)
        {
            return;
        }

        if (shopInstance != null)
        {
            shopInstance.CloseShop();
            return;
        }

        Time.timeScale = 0;

        shopInstance = Instantiate(shopPrefab, mainCanvas.transform);
    }
}
