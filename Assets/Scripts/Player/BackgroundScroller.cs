using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private bool scrollVertical = false;

    private Vector2 currentScroll = new Vector2();

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (backgroundImage == null || playerController == null)
        {
            return;
        }

        float x = playerController.body.velocity.x * speed;
        float y = (playerController.body.transform.position.y - GameManager.Instance.levelInfoArray[GameManager.Instance.currentLevelId].startingPosition.y) * speed;

        currentScroll = new Vector2(currentScroll.x + x * Time.deltaTime, scrollVertical ? y : 0);

        backgroundImage.material.mainTextureOffset = currentScroll;
    }

    [ContextMenu("Reset Material")]
    public void ResetMaterial()
    {
        backgroundImage.material.mainTextureOffset = new Vector2(0, 0);
    }
}
