using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition Health;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
