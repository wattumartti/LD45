using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance = null;

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

        canvas.DOFade(0, 2).OnComplete(() => 
        {
            objectToTween.SetActive(false);
            GameManager.Instance.StartGame();
        });
    }
}
