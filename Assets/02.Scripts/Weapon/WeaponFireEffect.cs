using UnityEngine;

public class WeaponFireEffect : MonoBehaviour
{
    [SerializeField] private WeaponStatData _weaponStatData;
    [SerializeField] private PlayerFire _weaponFire;
    [SerializeField] private Animator _weaponAni;
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private WFX_LightFlicker _fireLight;
    private const float _lightDelayValue = 0.5f;

    private void Start()
    {
        _fireLight.gameObject.SetActive(false);
        _fireLight.time = _weaponFire.DelayTime * _lightDelayValue;
    }
    private void Update()
    {
        FireEffect(_weaponFire.BulletCount, _weaponFire.IsReloading);
        SetDelayTime();
    }
    private void SetDelayTime()
    {
        _fireLight.time = _weaponFire.DelayTime * _lightDelayValue;
        _weaponAni.SetFloat("Speed", 1 / _weaponStatData.DelayValue);
    }
    private void FireEffect(int bulletCount, bool isReloading)
    {
        if(Input.GetMouseButtonDown(0) && bulletCount > 0 && !isReloading)
        {
            _weaponAni.SetBool("IsFire", true);
        }
        else if(Input.GetMouseButtonUp(0) || bulletCount <= 0 || isReloading)
        {
            _weaponAni.SetBool("IsFire", false);
            _fireLight.gameObject.SetActive(false);
        }
    }
    private void FireEffectPlay()
    {
        _weaponFire.Fire();
        _fireEffect.Play();
        _fireLight.gameObject.SetActive(true);
    }
}
