using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public static class SpriteManager
    {
        [Header("장비 스프라이트")]
        [Tooltip("무기 - 워리어")] public static List<Sprite> WarriorWeaponSprite;
        [Tooltip("무기 - 아처")] public static List<Sprite> ArcherWeaponSprite;
        [Tooltip("무기 - 위자드")] public static List<Sprite> WizardWeaponSprite;
        [Tooltip("방어구 - 헬멧")] public static List<Sprite> HelmetGearSprite;
        [Tooltip("방어구 - 갑옷")] public static List<Sprite> ArmorGearSprite;
        [Tooltip("방어구 - 장갑")] public static List<Sprite> GauntletGearSprite;
        
        [Space(5)]
        [Header("캐릭터 스프라이트")]
        [Tooltip("일러스트 - 워리어")] public static List<Sprite> WarriorSprite;
        [Tooltip("일러스트 - 아처")] public static List<Sprite> ArcherSprite;
        [Tooltip("일러스트 - 위자드")] public static List<Sprite> WizardSprite;

        [Space(5)]
        [Header("스킬 스프라이트")]
        [Tooltip("스킬 - 워리어")] public static List<Sprite> WarriorSkillSprite;
        [Tooltip("스킬 - 아처")] public static List<Sprite> ArcherSkillSprite;
        [Tooltip("스킬 - 위자드")] public static List<Sprite> WizardSkillSprite;
    }
}