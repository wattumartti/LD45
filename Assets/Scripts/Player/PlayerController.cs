using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _instance = null;

    public static PlayerController Instance
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
    public System.Action onGroundedAction = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetButtonUp("Vertical"))
        {
            useJump = true;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            PlayerInventory.Instance.UnlockPowerup(PowerupBase.PowerupType.DOUBLE_JUMP);
            PlayerInventory.Instance.PurchasePowerup(PowerupBase.PowerupType.DOUBLE_JUMP);
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
        
        if (CanJump())
        {
            DoJump();
        }
        else if (useJump && HasPowerup(PowerupBase.PowerupType.DOUBLE_JUMP) && !isGrounded)
        {
            if (!PlayerInventory.Instance.playerPowerups[PowerupBase.PowerupType.DOUBLE_JUMP].ActivatePowerup())
            {
                useJump = false;
            }
        }
    }

    private void UpdateGrounded()
    {
        if (Physics2D.Raycast(playerSprite.transform.position - new Vector3(0, playerSprite.bounds.extents.y, 0), -transform.up, groundRayCastDistance, groundLayer))
        {
            if (!isGrounded)
            {
                onGroundedAction?.Invoke();
            }

            isGrounded = true; 
        }
        else
        {
            isGrounded = false;
        }
    }

    public void DoJump()
    {
        Vector3 jumpMovement = new Vector3(0.0f, jumpForce, 0.0f);
        body.AddForce(jumpMovement, ForceMode2D.Impulse);
        useJump = false;
    }

    private bool HasPowerup(PowerupBase.PowerupType type)
    {
        return PlayerInventory.Instance.playerPowerups.ContainsKey(type);
    }

    private bool CanJump()
    {
        return useJump && isGrounded;
    }
}
