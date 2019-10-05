using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private SpriteRenderer playerSprite = null;

    [Header("Ground Checking")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundRayCastDistance;

    private float horizontalMovement = 0;
    private bool useJump = false;

    public bool isGrounded = false;

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetButtonUp("Vertical"))
        {
            useJump = true;
        }
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        if (horizontalMovement != 0)
        {
            Vector3 movement = new Vector3(horizontalMovement, 0.0f, 0.0f);
            body.AddForce(movement * speed);
        }
        
        if (useJump && isGrounded)
        {
            Vector3 jumpMovement = new Vector3(0.0f, jumpForce, 0.0f);
            body.AddForce(jumpMovement, ForceMode2D.Impulse);
            useJump = false;
        }
    }

    private void UpdateGrounded()
    {
        if (Physics2D.Raycast(playerSprite.transform.position - new Vector3(0, playerSprite.bounds.extents.y, 0), -transform.up, groundRayCastDistance, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
