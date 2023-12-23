using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask selectLayerMask;
    [SerializeField] private ArrowController arrowController;

    private Rigidbody2D playerRB;
    private Animator animator;
    private Transform checkGroundPoint;
    private Transform transformArrowPoint;

    private bool isGrounded;
    private bool isFlipedInX;
    private int idSpeed;
    private int idIsGrounded;
    private int idShootArrow;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        animator = GameObject.Find("StandingPlayer").GetComponent<Animator>();
        idSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idShootArrow = Animator.StringToHash("shootArrow");
    }

    void Update()
    {
        Move();
        Jump();
        CheckAndSetDirection();
        ShootArrow();
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
        if (Input.GetButtonDown("Jump") && isGrounded)
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        animator.SetBool(idIsGrounded, isGrounded);
    }

    private bool CheckAndSetDirection()
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

        return isFlipedInX;
    }
}
