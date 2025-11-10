using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition Health;
    public Condition Stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
