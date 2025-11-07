using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public AnimationHandler animationHandler;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        animationHandler = GetComponent<AnimationHandler>();
        CharacterManager.Instance.Player = this;
    }
}
