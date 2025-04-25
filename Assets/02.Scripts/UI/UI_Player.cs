using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    [SerializeField] private PlayerWeaponData _playerWeaponData;
    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private TextMeshProUGUI _grenadeCountText;
    [SerializeField] private TextMeshProUGUI _bulletCountText;
    [SerializeField] private Image _reloadImage;
    public void UpdateStaminaUI(float stamina, float staminaMax)
    {
        _staminaSlider.value = stamina / staminaMax;
    }
    public void UpdateGrenadeCountUI(int grenadeCount)
    {
        _grenadeCountText.text = $"수류탄 : {grenadeCount} / {_playerWeaponData.GrenadeCountMax}";
    }
    public void UpdateBulletCountUI(int bulletCount)
    {
        _bulletCountText.text = $"총알 : {bulletCount} / {_playerWeaponData.BulletCountMax}";
    }
    public void UpdateReloadUI(float time, float reloadTime)
    {
        _reloadImage.fillAmount = time / reloadTime;
    }
    public void ReloadImageActive(bool isActive)
    {
        _reloadImage.gameObject.SetActive(isActive);
    }
}
