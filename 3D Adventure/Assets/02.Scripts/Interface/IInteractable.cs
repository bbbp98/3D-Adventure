using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public ItemData GetItemData();
    public void OnInteract();
}
