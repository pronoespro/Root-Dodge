using PronoesPro.Scripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PronoesPro.Inventory
{

    public enum handHoldStyles
    {
        none,
        aim,
        holdInHand,
        holdOnBack,
        holdOnHead,
        onShoulder,
        twoHandAim
    }

    public enum heldUseStyles
    {
        none,
        pullBack,
        holdUp,
        legBack,
        channeling
    }

    public enum useStyles
    {
        none,
        swing,
        stab,
        shoot,
        holdOut,
        summon,
        drinkEat
    }

    [System.Serializable]
    public class ItemUses
    {
        public string name;

        [Space(10)]
        public bool canCancel;
        public bool hasAirUse;

        [Space(10),Header("Ground/General version")]
        public string groundUpponUse;
        public string groundHoldDownUse;
        public float groundChargeTimer;
        public string groundChargeFinishUse;
        public string groundCancelChargeUse;

        [Space(10),Header("Air version")]
        public string airUpponUse;
        public string airHoldDownUse;
        public float airChargeTimer;
        public string airChargeFinishUse;
        public string airCancelChargeUse;
    }

    [System.Serializable]
    public class SpecialItemUse
    {
        public string name;

        public ItemUses use;
        public Condition[] requirements;

    }

    [System.Serializable]
    public class ItemUseWithSpecial
    {

        public bool autoReuse;
        public bool canHoldDown;
        public float useTime;
        public float useDelay;

        public SpecialItemUse[] specialUses;
        public ItemUses[] normalUses;
    }

    [CreateAssetMenu(fileName = "new Item", menuName = "Items/basicItem")]
    public class ItemBase : ScriptableObject
    {

        public new string name;
        [TextArea(3, 15)]
        public string description;
        public bool canBeEquiped;

        [Space(15)]
        public Vector2 scale;
        public Sprite sprite;

        [Space(15),Header("General")]
        public handHoldStyles onHandStyle;
        public useStyles useStyle;
        public heldUseStyles holdUseStyle;

        [Space(15), Header("Item uses")]

        public ItemUseWithSpecial primaryUse;
        public ItemUseWithSpecial secondaryUse;
        [Space(15)]
        public SpecialItemUse[] firstSpecialUse;
        public SpecialItemUse[] secondSpecialUse;
        public SpecialItemUse[] finalSpecialUse;

        
    }
}