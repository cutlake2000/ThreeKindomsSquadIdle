using System;
using System.Collections.Generic;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Data;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class PushRewardUI : MonoBehaviour
    {
        [SerializeField] private Button confirmBtn;
        [SerializeField] private RewardItemUI[] rewardSlots;
        
        private List<Reward> pushRewards;
        
        public void InitializeEventListener()
        {
            confirmBtn.onClick.AddListener(GiveReward);
        }

        public void SetUI(List<Reward> targetRewards)
        {
            gameObject.SetActive(true);
            
            SetReward(targetRewards);
            SetSlots();
        }

        private void SetReward(List<Reward> targetRewards)
        {
            pushRewards = targetRewards;
        }
        
        private void SetSlots()
        {
            for (var i = 0; i < pushRewards.Count; i++)
            {
                var reward = pushRewards[i];
                
                var targetAmount = reward.amount;
                string targetTypeString;
                
                switch (reward.rewardType)
                {
                    case Enums.RewardType.Dia:
                        targetTypeString = "다이아";
                        rewardSlots[i].UpdateRewardItemUI(reward.rewardType, SpriteManager.Instance.GetCurrencySprite(Enums.CurrencyType.Dia), targetTypeString, targetAmount.ChangeMoney(), false);
                        break;
                    case Enums.RewardType.Rare_5_Sword:
                        targetTypeString = "모험자의 검 5";
                        rewardSlots[i].UpdateRewardItemUI(reward.rewardType, SpriteManager.Instance.GetEquipmentSprite(Enums.EquipmentType.Sword, 15), targetTypeString, BigInteger.ToInt32(targetAmount).ToString(), true);
                        break;
                }
                
                rewardSlots[i].gameObject.SetActive(true);
            }
        }
        
        private void GiveReward()
        {
            foreach (var reward in pushRewards)
            {
                switch (reward.rewardType)
                {
                    case Enums.RewardType.Dia:
                        AccountManager.Instance.AddCurrency((Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), reward.rewardType.ToString()), reward.amount);
                        break;
                    case Enums.RewardType.Rare_5_Sword:
                        var targetEquipment = InventoryManager.Instance.SwordsDictionary["Rare_5_Sword"];

                        if (targetEquipment.isPossessed == false)
                        {
                            targetEquipment.isPossessed = true;
                            
                            UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemSwords[15].GetComponent<InventoryPanelItemUI>().UpdateInventoryPanelItemPossessMark(targetEquipment.isPossessed);
                        }
                        
                        targetEquipment.equipmentQuantity++;
                        
                        UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemSwords[15].GetComponent<InventoryPanelItemUI>().UpdateInventoryPanelItemQuantityUI(targetEquipment.equipmentQuantity);
                        InventoryManager.Instance.SaveAllEquipmentInfo();
                        break;
                }
            }
            
            foreach (var slot in rewardSlots)
            {
                slot.gameObject.SetActive(false);
            }
            
            gameObject.SetActive(false);
        }
    }
}