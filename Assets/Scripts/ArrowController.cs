using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;
    [SerializeField] private GameObject arrowImpact;
    private Rigidbody2D arrowRB;
    private Vector2 _arrowDirection;
    private Transform transformArrow;

    public Vector2 ArrowDirection { get => _arrowDirection; set => _arrowDirection = value; }

    void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        transformArrow = GetComponent<Transform>();
    }

    void Update()
    {
        arrowRB.velocity = ArrowDirection * arrowSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Instantiate(arrowImpact, transformArrow.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
