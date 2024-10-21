using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private static AchievementManager instance;
    public static AchievementManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<AchievementManager>();
            return instance;
        }
    }

    private int currentThresholdIndex;

    [SerializeField] private AchievementSO[] achievements;
    [SerializeField] private AchievementView achievementView;

    private void Awake()
    {
        instance = this;
        achievements = achievements.OrderBy(a => a.threshold).ToArray();
    }

    private void Start()
    {
        achievementView.CreateAchievementSlots(achievements);  // UI 생성
        RocketMovementC.OnHighScoreChanged += CheckAchievement;
    }
    
    private void CheckAchievement(float height)
    {
        while (currentThresholdIndex < achievements.Length && height >= achievements[currentThresholdIndex].threshold)
        {
            if (!achievements[currentThresholdIndex].isUnlocked)
            {
                achievements[currentThresholdIndex].isUnlocked = true;
                Debug.Log($"Achievement unlocked: Reach {achievements[currentThresholdIndex].threshold}M!");
                achievementView.UnlockAchievement(achievements[currentThresholdIndex].threshold);
            }
            currentThresholdIndex++;  // 다음 업적 임계값으로 이동
        }
    }
}