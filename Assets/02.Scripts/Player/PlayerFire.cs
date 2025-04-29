using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Gun,
    Knife,
}
public class PlayerFire : MonoBehaviour
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private PlayerWeaponData _playerWeaponData;
    [SerializeField] private WeaponStatData _weaponStatData;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private ParticleSystem _fireEffect;
    [SerializeField] private WFX_LightFlicker _fireLight;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private Collider _swardCollider;
    [SerializeField] [Range(0.5f, 100f)]  private float _rayDistance = 100f;
    private PoolingSystem _bulletPool;
    private int _bulletCount;
    private int _fireCount = 0;
    private float _delayTime;
    private RaycastHit _camRayHit;
    private int _layerMask;
    private const float _lightDelayValue = 0.5f;
    public WeaponStatData WeaponStatData => _weaponStatData;
    public int BulletCount => _bulletCount;
    public WeaponType WeaponType => _weaponType;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _bulletCount = _playerWeaponData.BulletCountMax;
        _delayTime = _weaponStatData.PerDelayTime * _weaponStatData.DelayValue;
        _layerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }
    private void Start()
    {
        _bulletPool = PoolSelector.instance.BulletPool;
        _fireLight.time = _delayTime * _lightDelayValue;
    }
    private void Update()
    {
        _delayTime = _weaponStatData.PerDelayTime * _weaponStatData.DelayValue;
        FireStop();
    }
    private void LateUpdate()
    {
        CamRayCast();
    }
    public void SetWeaponType(WeaponType weaponType)
    {
        _weaponType = weaponType;
    }
    private void FireStop()
    {
        if(Input.GetMouseButtonUp(0) || _bulletCount <= 0 || _playerAnimator.PlayerAniType == PlayerAniType.Reload)
        {
            _fireCount = 0;
            _weaponRecoil.ResetRecoil();
            _fireLight.gameObject.SetActive(false);
        }
    }
    public void Fire()
    {
        if(_bulletCount > 0)
        {
            _bulletCount--;
            _fireEffect.Play();
            _fireLight.gameObject.SetActive(true);

            _bulletPool.SpawnObject(_muzzle.position, _muzzle.forward);
            
            GameObject bullet = _bulletPool.Target;

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(_muzzle.forward * _weaponStatData.ShotPower, ForceMode.Impulse);
            bulletRigidbody.AddTorque(Vector3.one);

            _fireCount++;
            _weaponRecoil.AddRecoil(_fireCount);
        }
    }
    public void Reload()
    {
        _bulletCount = _playerWeaponData.BulletCountMax;
    }
    private void CamRayCast()
    {
        if(_playerAnimator.PlayerAniType == PlayerAniType.Idle && _weaponType == WeaponType.Gun)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _layerMask))
            {
                _camRayHit = hit;
                _hand.LookAt(_camRayHit.point);
            }
            else
            {
                _hand.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            }
        }
    }
    
    public void Swap()
    {
        foreach(GameObject weapon in _weapons)
        {
            if(weapon == null) continue;
            weapon.SetActive(false);
        }
        switch(_weaponType)
        {
            case WeaponType.Gun:
                _weapons[0].SetActive(true);
                break;
            case WeaponType.Knife:
                _weapons[2].SetActive(true);
                break;
        }
    }
    private void ActiveSwing(bool b)
    {
        _swardCollider.enabled = b;
    }
}
