using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSO", menuName = "AchievementData", order = 0)]
public class AchievementSO : ScriptableObject
{
    public int threshold;
    public string displayName;
    public string displayDesc;
    public bool isUnlocked;
}