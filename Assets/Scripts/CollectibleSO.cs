using UnityEngine;

public abstract class CollectibleSO : ScriptableObject
{
    public string itemName;

    public abstract void Collect(Mage player);
}
