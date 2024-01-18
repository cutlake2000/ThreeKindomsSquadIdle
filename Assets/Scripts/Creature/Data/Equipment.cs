using Data;
using Function;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Creature.Data
{
    public class Equipment : MonoBehaviour
    {
        public IObjectPool<Equipment> ManagedPool;
        
        public string id;       // 장비의 이름
        public int quantity;         // 장비의 개수
        public int tier;
        public bool isEquipped;
        public Enum.EquipmentType type;   // 장비의 타입 (예: 무기, 방어구 등)
        [FormerlySerializedAs("rarity")] public Enum.EquipmentRarity equipmentRarity;        // 장비의 희귀도
        public int level; // 강화 상태 (예: 0, 1, 2, ...)
        public int basicEquippedEffect;
        public int basicOwnedEffect;
        [FormerlySerializedAs("basicBackgroundEffect")] public Image basicEquipmentBackground;
        public Image basicEquipmentImage;
        
        public Sprite equipmentBackground;
        public Sprite equipmentImage;
        public TMP_Text tierText;
        public TMP_Text levelText;
        public Slider quantityBar;
        public TMP_Text quantityText;
        public TMP_Text summonCountText;
        
        public int equippedEffect;  // 장착효과
        public int ownedEffect;     // 보유효과

        public int summonCount; // 소환된 개수
        
        public int maxQuantity = 5;
        public int maxLevel = 250;
        
        [SerializeField] private ParticleSystem[] summonEffects;

        public Equipment(string id, int quantity, int tier, bool isEquipped, Enum.EquipmentType type, Enum.EquipmentRarity equipmentRarity,
            int level, int basicEquippedEffect, int basicOwnedEffect)
        {
            this.id = id;
            this.quantity = quantity;
            this.tier = tier;
            this.isEquipped = isEquipped;
            this.type = type;
            this.equipmentRarity = equipmentRarity;
            this.level = level;
            this.basicEquippedEffect = basicEquippedEffect;
            this.basicOwnedEffect = basicOwnedEffect;

            equippedEffect = this.basicEquippedEffect;
            ownedEffect = this.basicOwnedEffect;
        }
        
        public void SetManagedPool(IObjectPool<Equipment> pool)
        {
            ManagedPool = pool;
        }
        
        // 강화 메서드
        public void Enhance()
        {
            // 강화 로직...
            equippedEffect += basicEquippedEffect;
            ownedEffect += basicOwnedEffect;

            tier++;
            // 강화효과 업데이트...
        }

        // 강화할 때 필요한 강화석 return 시키는 메서드
        public BigInteger GetEnhanceStone()
        {
            Debug.Log($"{ownedEffect}  {basicOwnedEffect}");
            var requiredEnhanceStone = equippedEffect - basicOwnedEffect;

            return requiredEnhanceStone;
        }

        // 개수 체크하는 메서드
        public bool CheckQuantity()
        {
            if (quantity >= 4)
            {
                return true;
            }

            SetQuantityText();
            return false;
        }

        // 장비 데이터를 ES3 파일에 저장
        public void SaveEquipmentAllInfo()
        {
            ES3.Save("id_" + id, id);
            ES3.Save("quantity_" + id, quantity);
            ES3.Save("grade_" + id, tier);
            ES3.Save("onEquipped_" + id, isEquipped);
            ES3.Save("type_" + id, type);
            ES3.Save("rarity_" + id, equipmentRarity);
            ES3.Save("level_"+ id, level);
            ES3.Save("basicEquippedEffect_" + id, basicEquippedEffect);
            ES3.Save("basicOwnedEffect_" + id, basicOwnedEffect);

            ES3.Save("equippedEffect_" + id, equippedEffect);
            ES3.Save("ownedEffect_" + id, ownedEffect);
        }
        
        public void SaveEquipmentAllInfo(string equipmentID)
        {
            ES3.Save("id_" + equipmentID, id);
            ES3.Save("quantity_" + equipmentID, quantity);
            ES3.Save("grade_" + equipmentID, tier);
            ES3.Save("onEquipped_" + equipmentID, isEquipped);
            ES3.Save("type_" + equipmentID, type);
            ES3.Save("rarity_" + equipmentID, equipmentRarity);
            ES3.Save("level_"+ equipmentID, level);
            ES3.Save("basicEquippedEffect_" + equipmentID, basicEquippedEffect);
            ES3.Save("basicOwnedEffect_" + equipmentID, basicOwnedEffect);
            ES3.Save("equippedEffect_" + equipmentID, equippedEffect);
            ES3.Save("ownedEffect_" + equipmentID, ownedEffect);
        }
    
        public void SaveEquipmentEachInfo(string equipmentID, Enum.EquipmentProperty property)
        {
            switch (property)
            {
                case Enum.EquipmentProperty.Name:
                    ES3.Save("id_" + equipmentID, id);
                    break;
                case Enum.EquipmentProperty.Quantity:
                    ES3.Save("quantity_" + equipmentID, quantity);
                    break;
                case Enum.EquipmentProperty.Level:
                    ES3.Save("grade_" + equipmentID, tier);
                    break;
                case Enum.EquipmentProperty.Equipped:
                    ES3.Save("onEquipped_" + equipmentID, isEquipped);
                    break;
                case Enum.EquipmentProperty.Type:
                    ES3.Save("type_" + equipmentID, type);
                    break;
                case Enum.EquipmentProperty.Rarity:
                    ES3.Save("rarity_" + equipmentID, equipmentRarity);
                    break;
                case Enum.EquipmentProperty.EnhancementLevel:
                    ES3.Save("level_"+ equipmentID, level);
                    break;
                case Enum.EquipmentProperty.BasicEquippedEffect:
                    ES3.Save("basicEquippedEffect_" + equipmentID, basicEquippedEffect);
                    break;
                case Enum.EquipmentProperty.BasicOwnedEffect:
                    ES3.Save("basicOwnedEffect_" + equipmentID, basicOwnedEffect);
                    break;
                case Enum.EquipmentProperty.EquippedEffect:
                    ES3.Save("equippedEffect_" + equipmentID, equippedEffect);
                    break;
                case Enum.EquipmentProperty.OwnedEffect:
                    ES3.Save("ownedEffect_" + equipmentID, ownedEffect);
                    break;
            }
        }

        // 장비 데이터를 ES3 파일에서 불러오기
        public void LoadEquipment()
        {
            if (!ES3.KeyExists("id_" + id)) return;

            id = ES3.Load<string>("id_" + id);
            quantity = ES3.Load<int>("quantity_" + id);
            tier = ES3.Load<int>("grade_" + id);
            isEquipped = ES3.Load<bool>("onEquipped_" + id);
            type = ES3.Load<Enum.EquipmentType>("type_" + id);
            equipmentRarity = ES3.Load<Enum.EquipmentRarity>("rarity_" + id);
            level = ES3.Load<int>("level_" + id);
            basicEquippedEffect = ES3.Load<int>("basicEquippedEffect_" + id);
            basicOwnedEffect = ES3.Load<int>("basicOwnedEffect_" + id);

            equippedEffect = ES3.Load<int>("equippedEffect_" + id);
            ownedEffect = ES3.Load<int>("ownedEffect_" + id);

        }
        public void LoadEquipment(string equipmentID)
        {
            if (!ES3.KeyExists("id_" + equipmentID)) return;
            
            id = ES3.Load<string>("id_" + equipmentID);
            quantity = ES3.Load<int>("quantity_" + equipmentID);
            tier = ES3.Load<int>("grade_" + equipmentID);
            isEquipped = ES3.Load<bool>("onEquipped_" + equipmentID);
            type = ES3.Load<Enum.EquipmentType>("type_" + equipmentID);
            equipmentRarity = ES3.Load<Enum.EquipmentRarity>("rarity_" + equipmentID);
            level = ES3.Load<int>("level_" + equipmentID);
            basicEquippedEffect = ES3.Load<int>("basicEquippedEffect_" + equipmentID);
            basicOwnedEffect = ES3.Load<int>("basicOwnedEffect_" + equipmentID);

            equippedEffect = ES3.Load<int>("equippedEffect_" + equipmentID);
            ownedEffect = ES3.Load<int>("ownedEffect_" + equipmentID);
        }

        // 매개변수로 받은 WeaponInfo 의 정보 복사
        public void SetEquipmentInfo(Equipment equipment)
        {
            id = equipment.id;
            quantity = equipment.quantity;
            tier = equipment.tier;
            isEquipped = equipment.isEquipped;
            type = equipment.type;
            equipmentRarity = equipment.equipmentRarity;
            level = equipment.level;
            basicEquippedEffect = equipment.basicEquippedEffect;
            basicOwnedEffect = equipment.basicOwnedEffect;
            equipmentBackground = equipment.equipmentBackground;
            equipmentImage = equipment.equipmentImage;

            equippedEffect = this.basicEquippedEffect;
            ownedEffect = this.basicOwnedEffect;
        }
        
        public void SetEquipmentInfo(string id, int quantity,int tier, bool isEquipped, Enum.EquipmentType type, Enum.EquipmentRarity equipmentRarity, int level, int basicEquippedEffect, int basicOwnedEffect, Sprite backgroundEffect, Sprite equipmentImage)
        {
            this.id = id;
            this.quantity = quantity;
            this.tier = tier;
            this.isEquipped = isEquipped;
            this.type = type;
            this.equipmentRarity = equipmentRarity;
            this.level = level;
            this.basicEquippedEffect = basicEquippedEffect;
            this.basicOwnedEffect = basicOwnedEffect;
            this.equipmentBackground = backgroundEffect;
            this.equipmentImage = equipmentImage;

            equippedEffect = this.basicEquippedEffect;
            ownedEffect = this.basicOwnedEffect;

            SetUI();
        }
    
        public void SetUI()
        {
            SetEquipmentImage();
            SetBackgroundEffectText();
            SetTierText();
            SetQuantityText();
            SetLevelText();
            SetCountText();
        }

        private void SetCountText()
        {
            if (summonCountText == null) return;
            summonCountText.text = $"{summonCount}";
        }

        // 장비 개수 보여주는 UI 업데이트 하는 메서드
        public void SetQuantityText()
        {
            if (quantityBar == null) return;
            quantityBar.value = quantity;
            quantityText.text = $"{quantity}/{maxQuantity}";
        }

        // 배경색 바꾸는 메서드 (Sprite로 변경해야함)
        public void SetBackgroundEffectText()
        {
            basicEquipmentBackground.sprite = equipmentBackground;
        }
        
        public void SetEquipmentImage()
        {
            basicEquipmentImage.sprite = equipmentImage;
        }

        // 레벨 보여주는 UI 업데이트 하는 메서드
        public void SetTierText()
        {
            tierText.text = $"{tier} 티어";
        }
        
        public void SetLevelText()
        {
            if (levelText == null) return;
            levelText.text = $"Lv.{level}/{maxLevel}";
        }

        public void SetSummonUI()
        {
            tierText.text = $"{tier} 티어";
            basicEquipmentBackground.sprite = equipmentBackground;
            basicEquipmentImage.sprite = equipmentImage;
            summonCountText.text = $"{summonCount}";
            
            summonEffects[(int) equipmentRarity].Play();
        }

        public void ResetEquipment()
        {
            basicEquipmentBackground.sprite = null;
            basicEquipmentImage.sprite = null;
            tierText.text = "";
            summonCountText.text = "";
        }
    }
}