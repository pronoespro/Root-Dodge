 using PronoesPro.Player.Movement;
using PronoesPro.Scripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PronoesPro.Inventory
{
    public class InventoryManager : ActionReader
    {

        [Space(15)]
        public bool invertedScroll;

        public ItemBase[] items;
        public Transform[] itemSlots;
        [SerializeField] private bool resizeOnStart;

        [SerializeField] private Sprite selectedSprite, unselectedSprite;

        [SerializeField]private Transform heldItem;

        private PlayerMovement move;
        private int selectedSlot;
        private float itemUseTime;

        private int autoUseType;
        private int curCombo;

        protected override void Start()
        {
            base.Start();
            if (resizeOnStart)
            {
                ResizeInventory();
            }
            ApplyHoldItem();
            move = GetComponent<PlayerMovement>();
        }

        public void ResizeInventory()
        {
            ItemBase[] oldInv = items;
            items = new ItemBase[itemSlots.Length];
            for(int i = 0; i < oldInv.Length && i<items.Length; i++)
            {
                items[i] = oldInv[i];
            }
        }

        public override void Update()
        {
            base.Update();

            UpdateUI();
            itemUseTime -= Time.deltaTime;
        }

        private void UpdateUI()
        {
            if (items.Length == 0)
            {
                return;
            }

            if (items[selectedSlot]!=null && autoUseType > 0)
            {
                if ((autoUseType == 1 && items[selectedSlot].primaryUse.autoReuse) || (autoUseType == 2 && items[selectedSlot].secondaryUse.autoReuse)){
                    if (!CanUseItem(autoUseType == 1 ? "_l" : "_r"))
                    {
                        autoUseType = 0;
                    }
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (itemSlots.Length > i)
                {
                    Image selImage = itemSlots[i].Find("Icon").GetComponent<Image>();

                    if (selImage != null)
                    {
                        if (items[i] != null)
                        {
                            selImage.sprite = items[i].sprite;
                            selImage.color = Color.white;
                        }
                        else
                        {
                            selImage.sprite = null;
                            selImage.color = Color.clear;
                        }
                    }

                    selImage = itemSlots[i].Find("Frame").GetComponent<Image>();

                    if (selImage != null)
                    {
                        if (i == selectedSlot)
                        {
                            selImage.sprite = selectedSprite;
                        }
                        else
                        {
                            selImage.sprite = unselectedSprite;
                        }
                    }
                }

            }
        }

        public void NavigateHotbar(string direction)
        {
            if (itemUseTime >= 0)
            {
                return;
            }
            float dir;
            if (float.TryParse(direction, out dir))
            {
                dir *= (invertedScroll) ? -1 : 1;
                if (items.Length > 0)
                {
                    selectedSlot = (selectedSlot + (int)dir / 120) % items.Length;
                }
                if (selectedSlot < 0)
                {
                    selectedSlot = items.Length - 1;
                }
            }
            ApplyHoldItem();
        }

        public void MoveToItem(int item)
        {
            selectedSlot = Mathf.Clamp(item, 0, items.Length-1);
            ApplyHoldItem();
        }

        public void MoveToItem(string item)
        {
            int itemToUse;
            if (int.TryParse(item, out itemToUse))
            {
                selectedSlot = Mathf.Clamp(itemToUse, 0, items.Length - 1);
                ApplyHoldItem();
            }
        }

        public bool CanUseItem(string useStyle)
        {

            if (items.Length == 0){
                return false;
            }

            if (itemUseTime>=0 ||items.Length==0|| items[selectedSlot] == null){
                return false;
            }

            //Special uses (first because they have priority)
            if (useStyle.ToLower().Contains("special1") ||useStyle.ToLower().Contains("s1"))
            {
                DoSpecialUse(items[selectedSlot].firstSpecialUse);
            }else if (useStyle.ToLower().Contains("special2") || useStyle.ToLower().Contains("s2"))
            {
                DoSpecialUse(items[selectedSlot].secondSpecialUse);
            }
            else if (useStyle.ToLower().Contains("special3") || useStyle.ToLower().Contains("s3"))
            {
                DoSpecialUse(items[selectedSlot].finalSpecialUse);
            }

            //Normal Uses (last because they don't have priority)
            else if (useStyle.ToLower().Contains("right") || useStyle.Contains("_r")) {

                if (!DoItemUse(items[selectedSlot].secondaryUse))
                {
                    return false;
                }

            }else{
                if (DoItemUse(items[selectedSlot].primaryUse))
                {
                    return false;
                }
            }

            if (autoUseType == 1)
            {
                if (items[selectedSlot].primaryUse.normalUses.Length > 0)
                {
                    curCombo = (curCombo + 1) % items[selectedSlot].primaryUse.normalUses.Length ;
                }
            }else{
                if (items[selectedSlot].secondaryUse.normalUses.Length > 0){
                    curCombo = (curCombo + 1) % items[selectedSlot].secondaryUse.normalUses.Length;
                }
            }
            return true;
        }

        public bool DoItemUse(ItemUseWithSpecial use)
        {
            if (items.Length == 0) { 
                return false;
            }
            if (use.normalUses.Length == 0)
            {
                return false;
            }

            int specialToUse = GetSpecialUse(use.specialUses);
            if (specialToUse >= 0 && DoEffectBasedOnGrounded(use.specialUses[specialToUse].use, move.GetGrounded()))
            {
                StartCoroutine(UseItem(use.specialUses[specialToUse].use, move.GetGrounded()));
            }
            else
            {
                if (use.normalUses.Length > 0 && !DoEffectBasedOnGrounded(use.normalUses[curCombo], move.GetGrounded()))
                {
                    return false;
                }

                autoUseType = 2;
                itemUseTime = use.useTime + use.useDelay;

                if (use.canHoldDown)
                {
                    StartCoroutine(UseItem(use.normalUses[curCombo], move.GetGrounded()));
                }
            }
            return true;
        }

        public bool DoSpecialUse(SpecialItemUse[] uses)
        {
            if (items.Length == 0) { 
                return false;
            }
            int specialToUse = GetSpecialUse(items[selectedSlot].firstSpecialUse);
            if (specialToUse >= 0 && DoEffectBasedOnGrounded(uses[specialToUse].use, move.GetGrounded()))
            {
                StartCoroutine(UseItem(uses[specialToUse].use, move.GetGrounded()));
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator UseItem(ItemUses use,bool grounded)
        {
            bool g= (use.hasAirUse) ? grounded : true;
            float _useTimer = 0;
            bool interrupted = false;
            while (!interrupted && _useTimer < ((g) ?use.groundChargeTimer:use.airChargeTimer)){
                _useTimer += Time.deltaTime;

                if (!DoEffect((g) ?use.groundHoldDownUse:use.airHoldDownUse) || (autoUseType==0 && use.canCancel)){
                    Debug.Log("interrupted");
                    DoEffect((g) ?use.groundCancelChargeUse:use.airCancelChargeUse);
                    interrupted = true;
                }

                yield return null;
            }

            if (!interrupted)
            {
                DoEffect((g) ?use.groundChargeFinishUse:use.airChargeFinishUse);
                
                yield return null;
            }
        }

        public bool DoEffectBasedOnGrounded(ItemUses use, bool grounded)
        {
            bool g = (use.hasAirUse) ? grounded : true;
            return DoEffect((g) ? use.groundUpponUse : use.airUpponUse);
        }

        [Tooltip("You can use this to get what special to use, returns -1 if none.")]
        private int GetSpecialUse(SpecialItemUse[] uses)
        {

            if (items.Length == 0) { 
                return -1;
            }

            int specialToUse = -1;
            for (int i = 0; i < uses.Length; i++)
            {
                bool useSpecial = true;
                for (int r = 0; r < uses[i].requirements.Length; r++)
                {
                    if (!ConditionCheck(uses[i].requirements[r].conditionScript,uses[i].requirements[r].useTargetInstead)){
                        useSpecial = false;
                    }
                }
                if (useSpecial)
                {
                    return i;
                }
            }
            return specialToUse;
        }

        public void CancelUseItem()
        {
            autoUseType = 0;
        }

        public void ApplyHoldItem()
        {
            if (items.Length == 0) { 
                return;
            }
            if (heldItem == null || items.Length==0)
            {
                return;
            }
            heldItem.SendMessage("ChangePosition", "none");
            //SendMessage("SetAimingToMouse", false);
            if (items[selectedSlot] == null)
            {
                heldItem.SendMessage("RemoveSprite");
                return;
            }

            heldItem.SendMessage("SwitchSprite", items[selectedSlot].sprite);
            heldItem.SendMessage("ChangeSize", items[selectedSlot].scale); 

            switch (items[selectedSlot].onHandStyle)
            {
                default:
                    break;
                case handHoldStyles.aim:
                    heldItem.SendMessage("ChangePosition", "aim");

                    SendMessage("SetAimingToMouse", true);
                    SendMessage("ChangeAimState", "normal");
                    break;
                case handHoldStyles.twoHandAim:
                    heldItem.SendMessage("ChangePosition", "doubleAim");

                    SendMessage("SetAimingToMouse", true);
                    SendMessage("ChangeAimState", "twohands");
                    break;
                case handHoldStyles.holdInHand:
                    heldItem.SendMessage("ChangePosition", "holdHand");
                    break;
            }
        }

        public override Transform SpecialTransform()
        {
            if (heldItem != null)
            {
                return heldItem;
            }
            else
            {
                return transform;
            }
        }

    }
}