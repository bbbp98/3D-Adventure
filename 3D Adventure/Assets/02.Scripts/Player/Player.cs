using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public AnimationHandler animationHandler;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CharacterManager.Instance.Player = this;
    }
}
