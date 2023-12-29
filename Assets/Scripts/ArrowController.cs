using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;
    [SerializeField] private GameObject arrowImpact;

    //Components
    private Rigidbody2D arrowRB;
    private Vector2 _arrowDirection;
    private Transform transformArrow;

    // Variables
    private int idEnemy;

    public Vector2 ArrowDirection { get => _arrowDirection; set => _arrowDirection = value; }

    void Awake()
    {
        arrowRB = GetComponent<Rigidbody2D>();
        transformArrow = GetComponent<Transform>();
    }

    private void Start()
    {
        idEnemy = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        arrowRB.velocity = ArrowDirection * arrowSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == idEnemy)
            col.GetComponent<EnemyController>()?.Explode();
        else
            Impact();
    }

    private void Impact()
    {
        Instantiate(arrowImpact, transformArrow.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
