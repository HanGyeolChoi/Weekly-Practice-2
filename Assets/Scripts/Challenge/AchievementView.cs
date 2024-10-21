using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementView : MonoBehaviour
{
    [SerializeField] private GameObject achievementSlotPrefab;  // 업적 슬롯 프리팹
    private Dictionary<int, AchievementSlot> achievementSlots = new();

    public void CreateAchievementSlots(AchievementSO[] achievements)
    {
        foreach (var achievement in achievements)
        {
            var slotInstance = Instantiate(achievementSlotPrefab, transform);
            var slot = slotInstance.GetComponent<AchievementSlot>();
            slot.Init(achievement);
            achievementSlots.Add(achievement.threshold, slot);
        }
    }

    public void UnlockAchievement(int threshold)
    {
        if (achievementSlots.ContainsKey(threshold))
        {
            achievementSlots[threshold].MarkAsChecked();
        }
    }
}