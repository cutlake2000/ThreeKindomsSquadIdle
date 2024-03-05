using Data;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Module;
using UnityEngine;

namespace Controller.UI.BottomMenuUI.PopUpUI
{
    public class PopUpMessagePanelUI : MonoBehaviour
    {
        private ObjectPool _objectPool;
        [SerializeField] public bool[] activePopUpMessagePanelItems;

        public void InitializeEventListener()
        {
            _objectPool = GetComponent<ObjectPool>();

            activePopUpMessagePanelItems = new bool[Enums.LockButtonTypes.Length];

            for (var index = 0; index < activePopUpMessagePanelItems.Length; index++)
            {
                activePopUpMessagePanelItems[index] = false;
            }
        }

        public void UpdateLockButtonPopUpMessagePanelUI(Enums.LockButtonType type)
        {
            if (activePopUpMessagePanelItems[(int)type]) return;
            
            var message = _objectPool.SpawnFromPool(Enums.PoolType.LockButtonPopUpMessage);
            activePopUpMessagePanelItems[(int)type] = true;
            message.GetComponent<PopUpMessagePanelItemUI>().SetMessage(LockButtonPopUpMessageString(type));
            message.GetComponent<PopUpMessagePanelItemUI>().StartLockButtonCoroutine(type);
        }

        public void UpdateTotalCombatPowerPopUpMessagePanelUI(BigInteger currentTotalCombatPower, BigInteger variationValue, bool plusMark)
        {
            var message = _objectPool.SpawnFromPool(Enums.PoolType.TotalCombatPowerPopUpMessage);

            var variationMark = plusMark
                ? $"<color=#FF0000><sprite={(int)Enums.IconType.Icon_Arrow_Up} color=#FF0000>{variationValue.ChangeMoney()}</color>"
                : $"<color=#0000FF><sprite={(int)Enums.IconType.Icon_Arrow_Down} color=#0000FF>{BigInteger.Abs(variationValue).ChangeMoney()}</color>";
            var totalCombatPowerText = $"전투력 {currentTotalCombatPower.ChangeMoney()} ( {variationMark} )"; // plusMark
            
            message.GetComponent<PopUpMessagePanelItemUI>().SetMessage(totalCombatPowerText);
            message.GetComponent<PopUpMessagePanelItemUI>().StartTotalCombatPowerCoroutine();
        }

        private static string LockButtonPopUpMessageString(Enums.LockButtonType type)
        {
            return type switch
            {
                Enums.LockButtonType.WillBeUpdate => "업데이트 예정입니다",
                
                Enums.LockButtonType.TalentPanel => "메인 퀘스트 2 도달 시 해금",
                Enums.LockButtonType.SummonPanel => "메인 퀘스트 11 도달 시 해금",
                Enums.LockButtonType.SummonWeaponPanel => "메인 퀘스트 11 도달 시 해금",
                Enums.LockButtonType.InventoryPanel => "메인 퀘스트 12 도달 시 해금",
                Enums.LockButtonType.SummonGearPanel => "메인 퀘스트 15 도달 시 해금",
                Enums.LockButtonType.SummonCharacterPanel => "메인 퀘스트 19 도달 시 해금",
                Enums.LockButtonType.SquadConfigurePanel => "메인 퀘스트 20 도달 시 해금",
                Enums.LockButtonType.DungeonPanel => "메인 퀘스트 25 도달 시 해금",
                Enums.LockButtonType.GoldDungeonPanel => "메인 퀘스트 25 도달 시 해금",
                Enums.LockButtonType.EquipmentEnhanceStoneDungeonPanel => "메인 퀘스트 32 도달 시 해금",
                Enums.LockButtonType.SquadEnhanceStoneDungeonPanel => "메인 퀘스트 41 도달 시 해금",
                
                Enums.LockButtonType.NotEnoughSquadEnhanceStone => "영웅 강화석이 부족합니다",
                Enums.LockButtonType.NotEnoughEquipmentEnhanceStone => "장비 강화석이 부족합니다",
                Enums.LockButtonType.NotEnoughDungeonTicket => "입장권이 부족합니다",
                Enums.LockButtonType.NotEnoughGold => "골드가 부족합니다",
                Enums.LockButtonType.NotEnoughExp => "경험치가 부족합니다",
                Enums.LockButtonType.NotEnoughStatPoint => "스탯 포인트가 부족합니다",
                Enums.LockButtonType.AlreadyAutoEquip => "이미 최고 등급 장비가 장착된 상태입니다",
                Enums.LockButtonType.AlreadyAllComposite => "합성할 장비가 부족합니다",
                _ => null
            };
        }
    }
}