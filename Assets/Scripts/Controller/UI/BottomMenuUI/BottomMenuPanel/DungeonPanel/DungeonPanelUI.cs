using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel
{
    public class DungeonPanelUI : MonoBehaviour
    {
        public DungeonItemUI[] dungeonItems;
        public GameObject[] dungeonLockItems;
        public ScrollRect scrollBar;

        public void InitializeEventListener()
        {
            foreach (var t in dungeonLockItems)
            {
                t.GetComponent<LockButtonUI>().InitializeEventListener();
            }
            
            foreach (var t in dungeonItems)
            {
                t.clearDungeonButton.GetComponent<LockButtonUI>().InitializeEventListener();
            }
        }

        public void UpdateLockItemUI(int index)
        {
            dungeonItems[index].gameObject.SetActive(true);
            dungeonLockItems[index].gameObject.SetActive(false);
        }
        
        public void SetScrollViewVerticalPosition(float position)
        {
            scrollBar.verticalNormalizedPosition = Mathf.Clamp01(position);
        }
    }
}