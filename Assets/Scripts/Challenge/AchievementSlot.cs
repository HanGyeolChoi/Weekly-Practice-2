using TMPro;
using UnityEngine;

public class AchievementSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descTxt;
    [SerializeField] private GameObject checkMark;
    public bool isUnlocked = false;

    public void Init(AchievementSO data)
    {
        data.isUnlocked = false;
        titleTxt.text = data.displayName;
        descTxt.text = data.displayDesc;
        checkMark.SetActive(false);
    }

    public void MarkAsChecked()
    {
        checkMark.SetActive(true);
        isUnlocked = true;
    }
}