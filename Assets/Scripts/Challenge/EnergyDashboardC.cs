using UnityEngine;
using UnityEngine.UI;

public class EnergyDashboardC : MonoBehaviour
{
    [SerializeField] private EnergySystemC energySystem;
    [SerializeField] private Image fillBar;
    private void Start()
    {
        energySystem.OnEnergyChanged +=  ((amount) => fillBar.fillAmount = amount / energySystem.MaxFuel);
    }

}