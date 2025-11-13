using System.Collections;
using UnityEngine;

public class BuffManager : Singleton<BuffManager>
{
    private PlayerController controller;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    public void ApplyBuff(BuffItemData buffData)
    {
        // stat
        controller.ApplyBuff(buffData);

        // ui
        Sprite buffIcon = buffData.buffIcon;
        UIBuffManager.Instance.AddBuffIcon(buffIcon, buffData.duration);

        StartCoroutine(RemoveBuff(buffData));
    }


    private IEnumerator RemoveBuff(BuffItemData buffData)
    {
        yield return new WaitForSeconds(buffData.duration);
        controller.RemoveBuff(buffData);
    }
}
