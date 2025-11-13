using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffManager : Singleton<UIBuffManager>
{
    [SerializeField] private Transform buffPanel;
    [SerializeField] private GameObject buffSlotPrefab;

    private readonly List<UIBuffSlot> activeBuffs = new List<UIBuffSlot>();


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            if (activeBuffs[i].UpdateTimer(Time.deltaTime))
            {
                Destroy(activeBuffs[i].gameObject);
                activeBuffs.RemoveAt(i);
            }
        }
    }

    public void AddBuffIcon(Sprite iconSprite, float duration)
    {
        GameObject slotObj = Instantiate(buffSlotPrefab, buffPanel);
        UIBuffSlot slot = slotObj.GetComponent<UIBuffSlot>();

        slot.Initialize(iconSprite, duration);
        activeBuffs.Add(slot);
    }
}
