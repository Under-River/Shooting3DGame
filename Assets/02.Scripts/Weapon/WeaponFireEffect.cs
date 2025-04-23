using UnityEngine;

public class WeaponFireEffect : MonoBehaviour
{
    [SerializeField] private WeaponFire _weaponFire;
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
        FireEffect();
        SetDelayTime();
    }
    private void SetDelayTime()
    {
        _fireLight.time = _weaponFire.DelayTime * _lightDelayValue;
        _weaponAni.SetFloat("Speed", 1 / _weaponFire.CycleValue);
    }
    private void FireEffectPlay()
    {
        _fireEffect.Play();
        _fireLight.gameObject.SetActive(true);
    }
    private void FireEffect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _weaponAni.SetBool("IsFire", true);
            _fireLight.gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _weaponAni.SetBool("IsFire", false);
            _fireLight.gameObject.SetActive(false);
        }
    }
}
