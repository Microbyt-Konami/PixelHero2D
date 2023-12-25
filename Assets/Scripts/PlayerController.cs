using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask selectLayerMask;
    [Header("Player Shoot")]
    [SerializeField] private ArrowController arrowController;
    [Header("Player Dust")]
    [SerializeField] private GameObject dustJump;
    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [Header("Player Dash After Image")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer afterImageSR;
    [SerializeField] private float afterImageLifetime;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageTimeBetween;
    [SerializeField] private float afterImageCounter;

    // Variables
    private float dashCounter;

    // Compoments
    private Rigidbody2D playerRB;
    private Animator animator;
    private Transform checkGroundPoint, transformArrowPoint, transformDustPoint, transformPlayer;

    // Flags
    private bool isGrounded, isFlipedInX, isIdle, canDoubleJump;

    // Id Parameters Animator
    private int idSpeed, idIsGrounded, idShootArrow, idCanDoubleJump;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        transformPlayer = GetComponent<Transform>();
    }

    private void Start()
    {
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
        animator = GameObject.Find("StandingPlayer").GetComponent<Animator>();
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idShootArrow = Animator.StringToHash("shootArrow");
        idCanDoubleJump = Animator.StringToHash("canDoubleJump");
    }

    void Update()
    {
        Dash();
        Jump();
        CheckAndSetDirection();
        ShootArrow();
        PlayDust();
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            dashCounter = dashTime;
            ShowAfterImage();
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            playerRB.velocity = new Vector2(dashSpeed * transformPlayer.localScale.x, playerRB.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
                ShowAfterImage();
        }
        else
            Move();
    }

    private void ShootArrow()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ArrowController tempArrowController = Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);

            if (isFlipedInX)
            {
                tempArrowController.ArrowDirection = new Vector2(-1, 0);
                tempArrowController.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
                tempArrowController.ArrowDirection = new Vector2(1, 0);

            animator.SetTrigger(idShootArrow);
        }
    }

    private void Move()
    {
        // En juegos de plataforma mejor usar GetAxisRaw que da un valor entre 0 y 1 para no hacer acceleraciones
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        playerRB.velocity = new Vector2(inputX, playerRB.velocity.y);
        animator.SetFloat(idSpeed, Mathf.Abs(playerRB.velocity.x));
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, 0.2f, selectLayerMask);
        if (Input.GetButtonDown("Jump") && (isGrounded || canDoubleJump))
        {
            if (isGrounded)
            {
                canDoubleJump = true;
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }
            else
            {
                canDoubleJump = false;
                animator.SetTrigger(idCanDoubleJump);
            }
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
        animator.SetBool(idIsGrounded, isGrounded);
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
        SpriteRenderer afterImage = Instantiate(afterImageSR, transformPlayer.position, transformPlayer.rotation);

        afterImage.sprite = playerSR.sprite;
        afterImage.transform.localScale = transformPlayer.localScale;
        afterImage.color = afterImageColor;
        Destroy(afterImage.gameObject, afterImageLifetime);
        afterImageCounter = afterImageTimeBetween;
    }
}
