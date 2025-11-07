using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController controller;
    AnimationHandler animationHandler;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animationHandler = GetComponent<AnimationHandler>();
    }
}
