using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float waitForExplode;
    [SerializeField] private float waitForDestroy;
    private Animator animator;
    private bool isActive;
    private int idIsActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        idIsActive = Animator.StringToHash("isActive");
    }

    private void Update()
    {
        waitForExplode -= Time.deltaTime;
        waitForDestroy -= Time.deltaTime;
        if (waitForExplode <= 0 && !isActive)
        {
            isActive = true;
            animator.SetBool(idIsActive, isActive);
        }
        if (waitForDestroy <= 0)
            Destroy(gameObject);
    }
}
