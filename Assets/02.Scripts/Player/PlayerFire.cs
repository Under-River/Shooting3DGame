using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private PlayerWeaponData _playerWeaponData;
    [SerializeField] private WeaponStatData _weaponStatData;
    [SerializeField] private UI_Player _uiPlayer;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _muzzle;
    [SerializeField] [Range(0.5f, 100f)]  private float _rayDistance = 100f;
    private PoolingSystem _bulletPool;
    private int _bulletCount;
    private int _fireCount = 0;
    private float _delayTime;
    private RaycastHit _camRayHit;
    private int _layerMask;
    private Coroutine _reloadCoroutine;
    private bool _isReloading = false;

    public WeaponStatData WeaponStatData => _weaponStatData;
    public int BulletCount => _bulletCount;
    public bool IsReloading => _isReloading;
    public float DelayTime => _delayTime;
    public RaycastHit CamRayHit => _camRayHit;

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
        _uiPlayer.ReloadImageActive(false);
    }
    private void Update()
    {
        _delayTime = _weaponStatData.PerDelayTime * _weaponStatData.DelayValue;
    }
    private void LateUpdate()
    {
        CamRayCast();
        FireStop();
        Reload();
        CancelReload();
    }
    private void FireStop()
    {
        if(Input.GetMouseButtonUp(0) || _isReloading || _bulletCount <= 0)
        {
            _fireCount = 0;
            _weaponRecoil.ResetRecoil();
        }
    }
    public void Fire()
    {
        if(_bulletCount > 0)
        {
            _bulletCount--;
            _uiPlayer.UpdateBulletCountUI(_bulletCount);

            _bulletPool.SpawnObject(_muzzle.position, _muzzle.forward);
            
            GameObject bullet = _bulletPool.Target;

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(_muzzle.forward * _weaponStatData.ShotPower, ForceMode.Impulse);
            bulletRigidbody.AddTorque(Vector3.one);

            _fireCount++;
            _weaponRecoil.AddRecoil(_fireCount);
        }
    }
    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }
    }
    private void CancelReload()
    {
        if(Input.GetMouseButtonDown(0) && _isReloading)
        {
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);
                _reloadCoroutine = null;
            }
            _isReloading = false;
            _uiPlayer.ReloadImageActive(false);
            _uiPlayer.UpdateReloadUI(0, _weaponStatData.ReloadTime);
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        float time = 0f;
        _uiPlayer.UpdateReloadUI(time, _weaponStatData.ReloadTime);
        _uiPlayer.ReloadImageActive(true);
        _isReloading = true;

        while(time < _weaponStatData.ReloadTime)
        {
            time += Time.deltaTime;
            _uiPlayer.UpdateReloadUI(time, _weaponStatData.ReloadTime);
            yield return null;
        }
        _uiPlayer.ReloadImageActive(false);
        _isReloading = false;

        _bulletCount = _playerWeaponData.BulletCountMax;
        _uiPlayer.UpdateBulletCountUI(_bulletCount);
    }
    private void CamRayCast()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _layerMask))
        {
            _camRayHit = hit;
            _hand.LookAt(_camRayHit.point);
            Debug.DrawRay(_muzzle.position, _muzzle.forward * _rayDistance, Color.green, 1f);
        }
    }
}
