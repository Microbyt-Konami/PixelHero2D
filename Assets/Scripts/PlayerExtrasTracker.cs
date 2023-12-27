using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtrasTracker : MonoBehaviour
{
    [SerializeField] private bool _canDoubleJump, _canDash, _CanEnterBallMode, _CanDropBombs;

    public bool CanDoubleJump { get => _canDoubleJump; set => _canDoubleJump = value; }
    public bool CanDash { get => _canDash; set => _canDash = value; }
    public bool CanEnterBallMode { get => _CanEnterBallMode; set => _CanEnterBallMode = value; }
    public bool CanDropBombs { get => _CanDropBombs; set => _CanDropBombs = value; }
}
