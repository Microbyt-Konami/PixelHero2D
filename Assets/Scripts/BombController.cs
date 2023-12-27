using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    // Fields
    [SerializeField] private float waitForExplode;
    [SerializeField] private float waitForDestroy;
    [SerializeField] private float expansiveWaveRange;
    [SerializeField] private LayerMask isDestroyable;

    // Compoments
    private Animator animator;
    private Transform transformBomb;

    // Flags
    private bool isActive;

    // Id Parameters Animator
    private int idIsActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        transformBomb = GetComponent<Transform>();
        idIsActive = Animator.StringToHash("isActive");
    }

    private void Update()
    {
        waitForExplode -= Time.deltaTime;
        waitForDestroy -= Time.deltaTime;
        if (waitForExplode <= 0 && !isActive)
            ActivatedBomb();
        if (waitForDestroy <= 0)
            Destroy(gameObject);
    }

    private void ActivatedBomb()
    {
        isActive = true;
        animator.SetBool(idIsActive, isActive);

        Collider2D[] destroyedObjects = Physics2D.OverlapCircleAll(transformBomb.position, expansiveWaveRange, isDestroyable);

        if (destroyedObjects.Length > 0)
            foreach (Collider2D coll in destroyedObjects)
                Destroy(coll.gameObject);
    }
}
