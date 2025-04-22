using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;
    public void UpdateStaminaUI(float stamina, float staminaMax)
    {
        _staminaSlider.value = stamina / staminaMax;
    }
}
