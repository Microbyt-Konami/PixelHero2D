using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemType : MonoBehaviour
{
    // Fields
    [SerializeField] private int orderToUnlock;
    [SerializeField] private int countItemsToUnlock;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool canDash;
    [SerializeField] private bool canEnterBallMode;
    [SerializeField] private bool canDropBombs;

    // Variables
    private int _itemsPendingToUnlock;

    public int OrderToUnlock { get => orderToUnlock; }
    public int CountItemsToUnlock1 { get; set; }
    public bool CanDoubleJump { get => canDoubleJump; }
    public bool CanDash { get => canDash; }
    public bool CanEnterBallMode { get => canEnterBallMode; }
    public bool CanDropBombs { get => canDropBombs; }

    public int ItemsPendingToUnlock => _itemsPendingToUnlock;

    public void ResetItemsPendingToUnlock()
    {
        _itemsPendingToUnlock = countItemsToUnlock;
    }

    public void CatchIt()
    {
        if (_itemsPendingToUnlock > 0)
            _itemsPendingToUnlock--;
    }
}
