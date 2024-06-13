using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ISerializable
{
    // Fields
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float waitForBallMode;
    [SerializeField] private float isGroundedRange = 0.2f;
    [SerializeField] private LayerMask selectLayerMask;
    [SerializeField] private Transform checkGroundPoint;
    [Header("Player Shoot")]
    [SerializeField] private ArrowController arrowController;
    [SerializeField] private GameObject prefabBomb;
    [Header("Player Dust")]
    [SerializeField] private GameObject dustJump;
    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float waitForDust;
    [Header("Player Dash After Image")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer afterImageSR;
    [SerializeField] private float afterImageLifetime;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageTimeBetween;

    // Variables
    private Vector3 positionInitial;
    private float dashCounter;
    private float afterImageCounter;
    private float afterDashCounter;
    private float ballModeCounter;

    // Compoments
    private PlayerInput playerInput;
    private Rigidbody2D playerRB;
    private Animator animatorStandingPlayer;
    private Animator animatorBallPlayer;
    private Transform transformArrowPoint, transformDustPoint, transformBombPoint, transformPlayerController;
    private PlayerExtrasTracker playerExtrasTracker;

    // Player Sprites
    private GameObject standingPlayer;
    private GameObject ballPlayer;

    // Flags
    private bool isGrounded, isFlipedInX, isIdle, canDoubleJump;

    // Id Parameters Animator
    private int idSpeed, idIsGrounded, idShootArrow, idCanDoubleJump;

    // Bits actions
    private bool isJump;
    private bool isAttack;

    // Actions
    private InputActionMap playerNormalMap;
    private InputAction hAxisPlayerNormalAction;
    private InputAction vAxisPlayerNormalAction;
    private InputAction jumpPlayerNormalAction;
    private InputAction attackPlayerNormalAction;
    private InputAction dashPlayerNormalAction;
    private InputAction hAxisAction;
    private InputAction vAxisAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction dashAction;

    private class Prefs
    {
        public Vector3 positionInitial, positionEnd;
        public bool isFlipedInX;

        public Prefs(PlayerController playerController)
        {
            positionInitial = playerController.positionInitial;
            positionEnd = playerController.transformPlayerController.position;
            isFlipedInX = playerController.isFlipedInX;
        }

        public void Restore(PlayerController playerController)
        {
            playerController.positionInitial = positionInitial;
            playerController.transformPlayerController.position = positionEnd;
            playerController.SetUpFlipedInX(isFlipedInX);
        }
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRB = GetComponent<Rigidbody2D>();
        transformPlayerController = GetComponent<Transform>();
        playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
    }

    void OnEnable()
    {
        playerNormalMap = playerInput.actions.FindActionMap("PlayerNormal");
        hAxisPlayerNormalAction = playerNormalMap.FindAction("Horizontal Axis");
        vAxisPlayerNormalAction = playerNormalMap.FindAction("Vertical Axis");
        jumpPlayerNormalAction = playerNormalMap.FindAction("Jump");
        jumpPlayerNormalAction.performed += JumpExample;
        jumpPlayerNormalAction.canceled += StopJumpExample;
        attackPlayerNormalAction = playerNormalMap.FindAction("Attack");
        attackPlayerNormalAction.performed += AttackExample;
        attackPlayerNormalAction.canceled += StopAttackExample;
        dashPlayerNormalAction = playerNormalMap.FindAction("Dash");

        ActivatePlayerNormal();
    }

    void OnDisable()
    {
        jumpPlayerNormalAction.performed -= JumpExample;
        jumpPlayerNormalAction.canceled -= StopJumpExample;
        attackPlayerNormalAction.performed -= AttackExample;
        attackPlayerNormalAction.canceled -= StopAttackExample;
    }

    void JumpExample(InputAction.CallbackContext context)
    {
        Debug.Log("JumpExample");
        isJump = true;
    }

    void StopJumpExample(InputAction.CallbackContext context)
    {
        Debug.Log("StopJumpExample");
        isJump = false;
    }

    void AttackExample(InputAction.CallbackContext context)
    {
        isAttack = true;
    }

    void StopAttackExample(InputAction.CallbackContext context)
    {
        isAttack = false;
    }

    void ActivatePlayerNormal()
    {
        hAxisAction = hAxisPlayerNormalAction;
        vAxisAction = vAxisPlayerNormalAction;
        jumpAction = jumpPlayerNormalAction;
        attackAction = attackPlayerNormalAction;
        dashAction = dashPlayerNormalAction;

        playerInput.SwitchCurrentActionMap("PlayerNormal");
        Debug.Log("Cambio a Basic Map");
    }

    private void Start()
    {
        standingPlayer = GameObject.Find("StandingPlayer");
        ballPlayer = GameObject.Find("BallPlayer");
        ballPlayer.SetActive(false);
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
        transformBombPoint = GameObject.Find("BombPoint").GetComponent<Transform>();
        animatorStandingPlayer = standingPlayer.GetComponent<Animator>();
        animatorBallPlayer = ballPlayer.GetComponent<Animator>();
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idShootArrow = Animator.StringToHash("shootArrow");
        idCanDoubleJump = Animator.StringToHash("canDoubleJump");
        positionInitial = transformPlayerController.position;
        FindAnyObjectByType<SaveDataGame>().ObjectsToSerialize.Add(this);
    }

    void Update()
    {
        Dash();
        Jump();
        CheckAndSetDirection();
        Shoot();
        PlayDust();
        BallMode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(checkGroundPoint.position, isGroundedRange);
    }

    private void Dash()
    {
        if (afterDashCounter > 0)
            afterDashCounter -= Time.deltaTime;
        else
        {
            if (dashAction.WasPressedThisFrame() && standingPlayer.activeSelf && playerExtrasTracker.CanDash)
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }

        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            playerRB.velocity = new Vector2(dashSpeed * transformPlayerController.localScale.x, playerRB.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
                ShowAfterImage();
            afterDashCounter = waitForDust;
        }
        else
            Move();
    }

    private void Shoot()
    {
        //if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
        if (attackAction.WasPressedThisFrame() && standingPlayer.activeSelf)
        {
            ArrowController tempArrowController = Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);

            if (isFlipedInX)
            {
                tempArrowController.ArrowDirection = new Vector2(-1, 0);
                tempArrowController.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
                tempArrowController.ArrowDirection = new Vector2(1, 0);

            animatorStandingPlayer.SetTrigger(idShootArrow);
        }

        if (attackAction.WasPressedThisFrame() && ballPlayer.activeSelf && playerExtrasTracker.CanDropBombs)
            Instantiate(prefabBomb, transformBombPoint.position, Quaternion.identity);
    }

    private void Move()
    {
        float inputX = hAxisAction.ReadValue<float>() * moveSpeed;

        playerRB.velocity = new Vector2(inputX, playerRB.velocity.y);
        if (standingPlayer.activeSelf)
            animatorStandingPlayer.SetFloat(idSpeed, Mathf.Abs(playerRB.velocity.x));
        else if (ballPlayer.activeSelf)
            animatorBallPlayer.SetFloat(idSpeed, Mathf.Abs(playerRB.velocity.x));
    }

    private void Jump()
    {
        // Se puede resolver por OverlapCircle o por Raycast
        //isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, isGroundedRange, selectLayerMask);
        isGrounded = Physics2D.Raycast(checkGroundPoint.position, Vector2.down, isGroundedRange, selectLayerMask);
        if (jumpAction.WasPressedThisFrame() && (isGrounded || (canDoubleJump && playerExtrasTracker.CanDoubleJump)))
        {
            if (isGrounded)
            {
                canDoubleJump = true;
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }
            else
            {
                canDoubleJump = false;
                animatorStandingPlayer.SetTrigger(idCanDoubleJump);
            }
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
        animatorStandingPlayer.SetBool(idIsGrounded, isGrounded);
    }

    private void SetUpFlipedInX(bool value)
    {
        transform.localScale = value ? new Vector3(-1, 1, 1) : Vector3.one;
        isFlipedInX = value;
    }

    private void CheckAndSetDirection()
    {
        if (playerRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFlipedInX = true;
        }
        else if (playerRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
            isFlipedInX = false;
        }
    }

    private void PlayDust()
    {
        if ((playerRB.velocity.x != 0) && isIdle)
        {
            isIdle = false;
            if (isGrounded)
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
        }
        if (playerRB.velocity.x == 0)
            isIdle = true;
    }

    private void ShowAfterImage()
    {
        SpriteRenderer afterImage = Instantiate(afterImageSR, transformPlayerController.position, transformPlayerController.rotation);

        afterImage.sprite = playerSR.sprite;
        afterImage.transform.localScale = transformPlayerController.localScale;
        afterImage.color = afterImageColor;
        Destroy(afterImage.gameObject, afterImageLifetime);
        afterImageCounter = afterImageTimeBetween;
    }

    private void BallMode()
    {
        float inputVertical = vAxisAction.ReadValue<float>();

        if (((inputVertical <= -.9f && !ballPlayer.activeSelf) || (inputVertical >= .9f && ballPlayer.activeSelf)) && playerExtrasTracker.CanEnterBallMode)
        {
            ballModeCounter -= Time.deltaTime;
            if (ballModeCounter < 0)
            {
                ballPlayer.SetActive(!ballPlayer.activeSelf);
                standingPlayer.SetActive(!standingPlayer.activeSelf);

            }
        }
        else
            ballModeCounter = waitForBallMode;
    }

    public JObject Serialize()
    {
        var prefs = new Prefs(this);
        string jsonString = JsonUtility.ToJson(prefs);
        JObject returnObject = JObject.Parse(jsonString);

        return returnObject;
    }

    public void DeSerialized(string jsonString)
    {
        var prefs = JsonUtility.FromJson<Prefs>(jsonString);

        prefs.Restore(this);
    }

    public string GetJsonKey() => "Player";
}
