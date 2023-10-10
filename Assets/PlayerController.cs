using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic; // Add this at the top of your script


public class PlayerController : MonoBehaviour
{
    // Player Stats
    [Header("Player Stats")]
    public float speed = 10f;
    public float jumpForce = 5f;
    public float doubleJumpForce = 4f;
    public float wallSlideSpeed = 3f;
    public float wallJumpForce = 10f;
    public float jumpCooldown = 0.2f;

    // Fall sound parameters
    [Header("Fall sound parameters")]
    public float airTimeThreshold = 0.5f;
    private float timeInAir = 0f;

    // Death parameters
    [Header("Death parameters")]
    public float deathFallSpeed = 1.0f;
    public float deathJumpForce = 5f;

    // Environment Checks
    [Header("Environment Checks")]
    public float unstuckForce = 1f;
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float directionChangeCooldown = 0.1f;
    public float takeoffAnimationLength = 0.2f;
    public bool IsDead { get; private set; }
    public GameObject tapToPlayText;
    public bool isGameActive = false;
    public GameObject StartButton;
    public GameObject RetryButton;
    public GameObject RetryImage;
    public GameObject ScoreMenue;
    public GameObject ScoreMask;
    public GameObject WhiteScreen;
    public GameObject BlackScreen;
    public GameObject TimerMask;
    public GameObject CoinsPlayMask;
    public GameObject ButtonSlots;
    public GameObject ShopButton;
    public GameObject ImageSlots;
    public Animator SettingMenue;
    public AudioSource JumpSound;
    public AudioSource DoubleJumpSound;
    public AudioSource WallJumpSound;
    public AudioSource DeathSound;
    public AudioSource HitWallSound;
    public AudioSource LandSound;
    public AudioSource ScreamSound;
    public AudioSource PushSound;
    public AudioSource TurnSound;
    public BackgroundMusicController musicController;
    public ParticleSystem Dust;
    public ParticleSystem DeathPart;
    public UIManager uiManager;
    public float checkDistance = 0.1f;
    public Animator pusherAnimator;
    public Animator ButtonSlotsAnim;
    public Animator ShopButtonAnimation;
    public Rigidbody2D pusherRigidbody;
    public Animator UpperMenueAnimation;
    public InterstitialAdController interstitialAdController;
    public float gameStartDelay = 2.0f;
    public PowerUpController powerupcontroller;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool wallJumping;
    private bool doubleJumped;
    private bool isIdle;
    private int direction = 1;
    private float lastJumpTime = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float lastDirectionChangeTime = 0f;
    private Vector3 startPosition;
    private bool hasHitWallSoundPlayed = false;
    private float fallStartY;
    private RaycastHit2D groundHit;
    private RaycastHit2D wallHit;
    private float initialMaxFallSpeed = -16f;
    private float currentMaxFallSpeed;
    private bool tripleJumpEnabled = false; // Add this to your variables
    private int jumpCount = 0; // Add this to your variables


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.isKinematic = true;
        tapToPlayText.SetActive(true);

        StartButton.SetActive(true);  // Start button is visible initially
        RetryButton.SetActive(false);  // Retry button is invisible initially
        RetryImage.SetActive(false);  // Retry image is invisible initially
        ScoreMenue.SetActive(false);
        ScoreMask.SetActive(false);
        WhiteScreen.SetActive(true);
        TimerMask.SetActive(false);
        CoinsPlayMask.SetActive(false);
        BlackScreen.SetActive(false);
        ButtonSlots.SetActive(false);

        currentMaxFallSpeed = initialMaxFallSpeed;

    }



    private void Update()
    {
        if (IsDead) return;

        CheckEnvironment();
        UpdateAnimations();

        if (isGameActive)
        {
            if (Input.touchCount > 0)
            {
                HandleTouchInput();
            }
            HandleKeyboardInput();
        }

        if (powerupcontroller.IsMagnetActive())
        {
            AttractCoins();
        }

        if (powerupcontroller.IsMagnetTimerActive())
        {
            AttractTimer();
        }

        HandleMovement();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastJumpTime >= jumpCooldown)
            {
                if (isGrounded || (!doubleJumped && !isWallSliding))
                {
                    DoubleJump();
                    lastJumpTime = Time.time;
                }
                else if (isWallSliding && !wallJumping)
                {
                    WallJump();
                    lastJumpTime = Time.time;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeDirection();
        }
    }



    private void CheckEnvironment()
    {
        bool wasGrounded = isGrounded;

        groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, whatIsGround);
        isGrounded = groundHit.collider != null;

        wallHit = Physics2D.Raycast(wallCheck.position, Vector2.right * direction, checkDistance, whatIsWall);
        isTouchingWall = wallHit.collider != null;

        isWallSliding = isTouchingWall && !isGrounded;

        if (!isGrounded)
        {
            timeInAir += Time.deltaTime;
        }

        if (!wasGrounded && isGrounded && timeInAir >= airTimeThreshold)
        {
            LandSound.Play();
            timeInAir = 0f;
            CreateDust();
        }
        else if (isGrounded)
        {
            timeInAir = 0f;
        }

        // Inside CheckEnvironment()
        if (isGrounded || isTouchingWall)
        {
            jumpCount = 0;
        }

        if (isTouchingWall && isGrounded && !hasHitWallSoundPlayed)
        {
            HitWallSound.Play();
            hasHitWallSoundPlayed = true;
            ChangeDirection(true, false); // immediateChange is true, playSound is false
            CreateDust();
        }

        else if (!isTouchingWall || !isGrounded)
        {
            hasHitWallSoundPlayed = false;
        }

    }


    private void HandleTouchInput()
    {
        // Return if game has not started
        if (!isGameActive)
            return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2)
                {
                    ChangeDirection();
                }
                else
                {
                    if (Time.time - lastJumpTime >= jumpCooldown)
                    {
                        if (isGrounded || (!doubleJumped && !isWallSliding))
                        {
                            DoubleJump();
                            lastJumpTime = Time.time;
                        }
                        else if (isWallSliding && !wallJumping)
                        {
                            WallJump();
                            lastJumpTime = Time.time;
                        }
                    }
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (!isGameActive || IsDead)
        {
            return;
        }
        if (isWallSliding)
        {
            spriteRenderer.flipX = direction == -1 ? true : false;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            spriteRenderer.flipX = direction == -1 ? false : true;
        }

        // Apply the horizontal speed
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);

        // Modify the vertical speed if falling too fast
        if (rb.velocity.y < currentMaxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentMaxFallSpeed);
        }


    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        JumpSound.Play();
    }

    private void DoubleJump()
    {
        if (isGrounded)
        {
            Jump();
            jumpCount = 0; // Reset jump count to 1 after landing on the ground and jumping again
        }
        else if (jumpCount < 1 || (tripleJumpEnabled && jumpCount < 2)) // Modify this line
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            jumpCount++; // Increase jump count
            DoubleJumpSound.Play();
        }
    }


    private void WallJump()
    {
        wallJumping = true;
        direction *= -1;
        rb.velocity = new Vector2(speed * direction, wallJumpForce);
        StartCoroutine(DisableMovement(0.1f));
        WallJumpSound.Play();


    }

    private void ChangeDirection(bool immediateChange = false, bool playSound = true)
    {
        if (immediateChange || Time.time - lastDirectionChangeTime >= directionChangeCooldown)
        {
            direction *= -1;
            spriteRenderer.flipX = direction == 1;
            lastDirectionChangeTime = Time.time;

            // Play the turn sound only if playSound is true
            if (playSound)
            {
                TurnSound.Play();
            }
        }
    }


    private IEnumerator DisableMovement(float time)
    {
        yield return new WaitForSeconds(time);
        wallJumping = false;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("speed", isGrounded && Mathf.Abs(rb.velocity.x) > 0.1f);
        animator.SetBool("isJumping", rb.velocity.y > 0 && !isWallSliding && !isTouchingWall);
        animator.SetBool("isFalling", rb.velocity.y < 0);
        animator.SetBool("isSliding", isWallSliding && isTouchingWall);
        animator.SetBool("IsIdle", isGrounded && Mathf.Abs(rb.velocity.x) < 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pusher"))
        {
            PushSound.Play();
            // This code executes only when player collides with the pusher
            // Push the player
            rb.AddForce(new Vector2(3f, 0f), ForceMode2D.Impulse);

            // Call ActivateGame method after a delay
            Invoke("ActivateGame", gameStartDelay);

            // Stop the pusher
            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.isKinematic = true; // Make pusher static

            // Get pusher's animator and set "Run" to false
            Animator pusherAnimator = collision.gameObject.GetComponent<Animator>();
            if (pusherAnimator != null)
            {
                pusherAnimator.SetBool("Run", false);
            }
        }
    }

    void ActivateGame()
    {
        isGameActive = true;
    }

    public void Die()
    {
        IsDead = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        tapToPlayText.SetActive(false);
        RetryButton.SetActive(true);  // Show the Retry button
        RetryImage.SetActive(true);  // Show the Retry image
        StartButton.SetActive(false);  // Make sure the Start button is hidden
        isGameActive = false;
        ScoreMask.SetActive(false);
        WhiteScreen.SetActive(false);
        GameManager1.instance.EndGame();
        TimerMask.SetActive(false);
        ScoreMenue.SetActive(false);
        DeathPart.Play();
        DeathSound.Play();
        Invoke("ShowScoreMenu", 0.5f);
        CoinsPlayMask.SetActive(false);
        BlackScreen.SetActive(true);
    }

    void ShowScoreMenu()
    {
        ScoreMenue.SetActive(true);
        uiManager.ShowScoreMenu();
        musicController.ChangeToDeathVolume();

        if (UnityEngine.Random.value < 0.1f)
        {
            interstitialAdController.ShowAdIfReady();
        }
    }

    public void StartGame()
    {
        musicController.ChangeToGameplayVolume();
        ScreamSound.Play();
        IsDead = false;

        // Reset player position and physics
        rb.isKinematic = false;
        boxCollider.enabled = true;

        // Hide Retry UI and show Start UI
        RetryButton.SetActive(false);
        RetryImage.SetActive(false);
        StartButton.SetActive(false);
        tapToPlayText.SetActive(false);
        ScoreMenue.SetActive(false);
        ScoreMask.SetActive(true);
        WhiteScreen.SetActive(false);
        TimerMask.SetActive(true);
        CoinsPlayMask.SetActive(true);
        GameManager1.instance.StartGame();
        UpperMenueAnimation.SetTrigger("Go");
        SettingMenue.SetBool("Open", false);
        BlackScreen.SetActive(false);
        ButtonSlots.SetActive(true);
        Invoke("TriggerGoAnimation", 5f);
        ShopButtonAnimation.SetTrigger("Go");
        ImageSlots.SetActive(false);

        // Set the pusher to start running
        pusherAnimator.SetBool("Run", true);
        pusherRigidbody.velocity = new Vector2(6f, 0);

    }

    public void OnRetryButtonClicked()
    {
        RetryGame();
    }

    public void RetryGame()
    {
        GameManager1.instance.ReloadScene();
    }

    void CreateDust()
    {
        Dust.Play();
    }

    private void TriggerGoAnimation()
    {
        ButtonSlotsAnim.SetTrigger("Go");
    }

    public void AdjustFallingSpeed(float multiplier)
    {
        currentMaxFallSpeed = initialMaxFallSpeed * multiplier;
    }

    public void ResetFallingSpeed()
    {
        currentMaxFallSpeed = initialMaxFallSpeed;
    }

    public void AttractCoins()
    {
        // Find all coins within the magnet range
        Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(transform.position, powerupcontroller.magnetRange, LayerMask.GetMask("Coin"));
        foreach (var coin in coinsInRange)
        {
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb)
            {
                Vector2 direction = (transform.position - coin.transform.position).normalized;
                coinRb.velocity = direction * powerupcontroller.magnetForce;
            }
        }
    }

    public void AttractTimer()
    {
        // Find all coins within the magnet range
        Collider2D[] coinsInRange = Physics2D.OverlapCircleAll(transform.position, powerupcontroller.magnetRangeTimer, LayerMask.GetMask("Timer"));
        foreach (var coin in coinsInRange)
        {
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb)
            {
                Vector2 direction = (transform.position - coin.transform.position).normalized;
                coinRb.velocity = direction * powerupcontroller.magnetForceTimer;
            }
        }
    }

    public void EnableTripleJump()
    {
        tripleJumpEnabled = true;
    }

    public void DisableTripleJump()
    {
        tripleJumpEnabled = false;
    }

}




















































































