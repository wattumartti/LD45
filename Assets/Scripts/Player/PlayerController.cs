using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public Rigidbody2D body = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public SpriteRenderer playerSprite = null;

    [Header("Ground Checking")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundRayCastDistance;

    internal float horizontalMovement = 0;
    private bool useJump = false;
    private bool _enableMovementInput = true;
    private bool enableMovementInput
    {
        get
        {
            return _enableMovementInput;
        }
        set
        {
            if (value)
            {
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            _enableMovementInput = value;
        }
    }

    private bool isLookingRight = true;

    public bool isGrounded = false;
    public System.Action onGroundedAction = null;

    [Header("PowerUps")]
    [SerializeField] private float dashForce = 300f;

    [Header("Health")]
    [SerializeField] private ParticleSystem DeathVfx = null;

    private List<PowerupBase.PowerupType> usePowerUps = new List<PowerupBase.PowerupType>();

    #region Health

    public int maxHp = 1;
    public int currentHp = 1;

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            DoDeath();
        }
    }

    public void DoDeath()
    {
        body.velocity = Vector2.zero;
        body.bodyType = RigidbodyType2D.Kinematic;
        enableMovementInput = false;
        playerSprite.transform.DOScale(0, 0.2f).OnComplete(() =>
        {
            StartCoroutine(PlayDeathVfx());
        });
    }

    public IEnumerator PlayDeathVfx()
    {
        if (DeathVfx == null)
        {
            yield break;
        }

        DeathVfx.Play();

        yield return new WaitForSeconds(DeathVfx.main.startLifetime.constantMax);

        body.transform.localPosition = Vector3.zero;

        playerSprite.transform.DOScale(1, 0.4f).OnComplete(() =>
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            enableMovementInput = true;
        });
    }

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * speed;

        if (Input.GetButtonDown("Vertical"))
        {
            if (HasPowerup(PowerupBase.PowerupType.DOUBLE_JUMP))
            {
                usePowerUps.Add(PowerupBase.PowerupType.DOUBLE_JUMP);
            }
            useJump = true;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerInventory.Instance.UnlockPowerup(PowerupBase.PowerupType.DOUBLE_JUMP);
            PlayerInventory.Instance.PurchasePowerup(PowerupBase.PowerupType.DOUBLE_JUMP);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerInventory.Instance.UnlockPowerup(PowerupBase.PowerupType.DASH);
            PlayerInventory.Instance.PurchasePowerup(PowerupBase.PowerupType.DASH);
        }

        if (Input.GetKeyDown(KeyCode.E) && HasPowerup(PowerupBase.PowerupType.DASH))
        {
            usePowerUps.Add(PowerupBase.PowerupType.DASH);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(1);
        }
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        if (horizontalMovement != 0)
        {
            isLookingRight = horizontalMovement > 0;
            playerSprite.flipX = !isLookingRight;

            Vector2 bodyVelocity = body.velocity;

            if (CanUpdateXMovement(bodyVelocity.x))
            {
                bodyVelocity.x = horizontalMovement;
                body.velocity = bodyVelocity;
            }
        }
        
        if (CanJump())
        {
            DoJump();
        }

        foreach (PowerupBase.PowerupType powerUpType in usePowerUps)
        {
            PlayerInventory.Instance.playerPowerups[powerUpType].ActivatePowerup();
        }

        usePowerUps.Clear();

        useJump = false;      
    }

    private bool CanUpdateXMovement(float xVelocity)
    {
        if (!isGrounded || !enableMovementInput)
        {
            return false;
        }

        return xVelocity <= 0 && (xVelocity >= horizontalMovement || horizontalMovement > 0) 
            || xVelocity >= 0 && (xVelocity <= horizontalMovement || horizontalMovement < 0);
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
        enableMovementInput = true;
        body.velocity = new Vector2(body.velocity.x, 0);

        if ((isLookingRight && body.velocity.x < 0) || (!isLookingRight && body.velocity.x > 0))
        {
            body.velocity = new Vector2(-body.velocity.x, 0);
        }

        Vector3 jumpMovement = new Vector3(0.0f, jumpForce, 0.0f);
        body.AddForce(jumpMovement, ForceMode2D.Impulse);
    }

    private bool HasPowerup(PowerupBase.PowerupType type)
    {
        return PlayerInventory.Instance.playerPowerups.ContainsKey(type);
    }

    private bool CanJump()
    {
        return useJump && isGrounded;
    }

    public IEnumerator DoDash()
    {
        enableMovementInput = false;
        body.velocity = Vector2.zero;
        body.constraints = body.constraints | RigidbodyConstraints2D.FreezePositionY;
        body.AddForce(new Vector2(isLookingRight ? dashForce : -dashForce, 0), ForceMode2D.Impulse);

        float timeRemaining = 0.5f;

        while (timeRemaining > 0 && Mathf.Abs(body.velocity.x) > 0)
        {
            if (enableMovementInput)
            {
                yield break;
            }

            timeRemaining -= Time.deltaTime;

            yield return null;
        }

        enableMovementInput = true;
    }
}
