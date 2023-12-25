using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [SerializeField] private int target = -1;

    private void Awake()
    {
        //Application.targetFrameRate = -1; => A lo que el juego
        Application.targetFrameRate = target;
    }
}
