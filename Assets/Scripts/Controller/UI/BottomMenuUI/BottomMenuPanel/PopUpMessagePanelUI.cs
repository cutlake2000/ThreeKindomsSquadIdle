using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Module;
using UnityEngine;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel
{
    public class PopUpMessagePanelUI : MonoBehaviour
    {
        private ObjectPool objectPool;
        [SerializeField] public bool[] activePopUpMessagePanelItems;

        public void InitializeEventListener()
        {
            objectPool = GetComponent<ObjectPool>();

            activePopUpMessagePanelItems = new bool[Enums.lockButtonTypes.Length];

            for (var index = 0; index < activePopUpMessagePanelItems.Length; index++)
            {
                activePopUpMessagePanelItems[index] = false;
            }
        }

        public void UpdatePopUpMessagePanelUI(Enums.LockButtonType type)
        {
            if (activePopUpMessagePanelItems[(int)type]) return;
            
            var message = objectPool.SpawnFromPool(Enums.PoolType.PopUpMessage);
            activePopUpMessagePanelItems[(int)type] = true;
            message.GetComponent<PopUpMessagePanelItemUI>().SetMessage(PopUpMessageString(type));
            message.SetActive(true);
            message.GetComponent<PopUpMessagePanelItemUI>().StartCoroutine(type);
        }

        private string PopUpMessageString(Enums.LockButtonType type)
        {
            return type switch
            {
                Enums.LockButtonType.WillBeUpdate => "업데이트 예정입니다",
                Enums.LockButtonType.TalentPanel => "메인 퀘스트 7 도달 시 해금",
                Enums.LockButtonType.SummonPanel => "메인 퀘스트 9 도달 시 해금",
                Enums.LockButtonType.SummonWeaponPanel => "메인 퀘스트 9 도달 시 해금",
                Enums.LockButtonType.InventoryPanel => "메인 퀘스트 10 도달 시 해금",
                Enums.LockButtonType.SummonGearPanel => "메인 퀘스트 14 도달 시 해금",
                Enums.LockButtonType.SummonCharacterPanel => "메인 퀘스트 18 도달 시 해금",
                Enums.LockButtonType.SquadConfigurePanel => "메인 퀘스트 19 도달 시 해금",
                Enums.LockButtonType.DungeonPanel => "메인 퀘스트 23 도달 시 해금",
                Enums.LockButtonType.GoldDungeonPanel => "메인 퀘스트 23 도달 시 해금",
                Enums.LockButtonType.SquadEnhanceStoneDungeonPanel => "메인 퀘스트 37 도달 시 해금",
                Enums.LockButtonType.NotEnoughSquadEnhanceStone => "강화석이 부족합니다",
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