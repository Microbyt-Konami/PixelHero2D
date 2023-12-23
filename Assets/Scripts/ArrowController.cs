using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;
    private Rigidbody2D arrowRB;
    private Vector2 arrowDirection;

    public Vector2 ArrowDirection { get => arrowDirection; set => arrowDirection = value; }

    void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        arrowRB.velocity = ArrowDirection * arrowSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
