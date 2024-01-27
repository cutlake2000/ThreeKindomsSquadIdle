using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class SpriteManager : MonoBehaviour
    {
        public static SpriteManager Instance;
        
        [Header("장비 스프라이트")]
        [Tooltip("무기 - 워리어")] public List<Sprite> warriorWeaponSprite;
        [Tooltip("무기 - 아처")] public List<Sprite> archerWeaponSprite;
        [Tooltip("무기 - 위자드")] public List<Sprite> wizardWeaponSprite;
        [Tooltip("방어구 - 헬멧")] public List<Sprite> helmetGearSprite;
        [Tooltip("방어구 - 갑옷")] public List<Sprite> armorGearSprite;
        [Tooltip("방어구 - 장갑")] public List<Sprite> gauntletGearSprite;
        
        [Space(5)]
        [Header("캐릭터 스프라이트")]
        [Tooltip("일러스트 - 워리어")] public List<Sprite> warriorSprite;
        [Tooltip("일러스트 - 아처")] public List<Sprite> archerSprite;
        [Tooltip("일러스트 - 위자드")] public List<Sprite> wizardSprite;
        
        [Space(5)]
        [Header("스킬 스프라이트")]
        [Tooltip("스킬 - 워리어")] public List<Sprite> warriorSkillSprite;
        [Tooltip("스킬 - 아처")] public List<Sprite> archerSkillSprite;
        [Tooltip("스킬 - 위자드")] public List<Sprite> wizardSkillSprite;

        private void Awake()
        {
            Instance = this;
        }
    }
}