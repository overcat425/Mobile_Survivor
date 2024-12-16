using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="ScriptableObject/ItemData")]

public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }
    [Header("Information")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemInfo;
    public Sprite itemIcon;

    [Header("Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damageUp;
    public int[] countUp;

    [Header("Weapon")]
    public GameObject projectile;
}
