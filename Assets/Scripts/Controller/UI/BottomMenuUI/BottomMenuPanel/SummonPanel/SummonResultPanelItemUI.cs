using System.Collections.Generic;
using Data;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel
{
    public class SummonResultPanelItemUI : MonoBehaviour
    {
        [Header("티어")] public TMP_Text itemTier;
        [Header("아이템 이미지")] public Image itemImage;
        [Header("등급")] public int itemRarity;
        [Header("아이템 배경 효과")] public Image itemBackgroundEffect;
        [Header("소환된 개수")] public TMP_Text itemCount;
        [Header("소환 이펙트 애니메이션")] public Animator summonEffectsAnimator;
        [Header("소환 이펙트 파티클")] public List<ParticleSystem> summonEffectsParticle;

        public void UpdateSummonResultPanelItemUI(int tier, Sprite image, int grade, int count)
        {
            itemTier.text = $"{tier} 티어";
            itemImage.sprite = image;
            itemBackgroundEffect.sprite = SpriteManager.Instance.GetEquipmentBackgroundEffect(grade);
            itemCount.text = $"{count}";
            itemRarity = grade;
        }

        public void StartSummonEffect()
        {
            summonEffectsAnimator.SetTrigger("Summon");
            summonEffectsParticle[itemRarity].Play(true);
        }
    }
}