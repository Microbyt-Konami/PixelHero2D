using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExtras : MonoBehaviour
{
    // Fields
    [SerializeField] private bool canDoubleJump, canDash, canEnterBallMode, canDropBombs;

    // Compoments
    private GameObject player;
    private PlayerExtrasTracker playerExtrasTracker;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerExtrasTracker = player.GetComponent<PlayerExtrasTracker>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SetTracker();
        Destroy(gameObject);
    }

    private void SetTracker()
    {
        if (canDoubleJump)
            playerExtrasTracker.CanDoubleJump = true;
        if (canDash)
            playerExtrasTracker.CanDash = true;
        if (canEnterBallMode)
            playerExtrasTracker.CanEnterBallMode = true;
        if (canDropBombs)
            playerExtrasTracker.CanDropBombs = true;
    }
}
