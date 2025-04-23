using System.Collections;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private UI_Player _uiPlayer;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _muzzle;
    [SerializeField] [Range(50f, 200f)]private float _shotPower = 50f;
    [SerializeField] [Range(0.05f, 0.5f)] private float _perDelayTime = 0.1f;
    [SerializeField] [Range(0.5f, 5f)]private float _cycleValue = 1f;
    [SerializeField] [Range(0.5f, 100f)] private float RAY_DISTANCE = 100f;
    [SerializeField] [Range(0.5f, 4f)]private float _reloadTime = 2f;
    private PoolingSystem _bulletObjectPool;
    private int _bulletCount;
    private float _delayTime;
    private RaycastHit _camRayHit;
    private int _layerMask;
    private Coroutine _reloadCoroutine;
    private bool _isReloading = false;

    public int BulletCount => _bulletCount;
    public bool IsReloading => _isReloading;
    public float CycleValue => _cycleValue;
    public float DelayTime => _delayTime;
    public RaycastHit CamRayHit => _camRayHit;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _bulletCount = _playerData.BulletCountMax;
        _delayTime = _perDelayTime * _cycleValue;
        _layerMask = ~(1 << LayerMask.NameToLayer("Player"));
    }
    private void Start()
    {
        _bulletObjectPool = PoolSelector.instance.BulletObjectPool;
        _uiPlayer.ReloadImageActive(false);
    }
    private void Update()
    {
        _delayTime = _perDelayTime * _cycleValue;
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
            _weaponRecoil.ResetRecoil();
        }
    }
    public void Fire()
    {
        if(_bulletCount > 0)
        {
            _bulletCount--;
            _uiPlayer.UpdateBulletCountUI(_bulletCount);

            _bulletObjectPool.SpawnEffect( _muzzle.position, _muzzle.forward);
            
            GameObject bullet = _bulletObjectPool.Target;

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(_muzzle.forward * _shotPower, ForceMode.Impulse);
            bulletRigidbody.AddTorque(Vector3.one);

            _weaponRecoil.AddRecoil();
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
            _uiPlayer.UpdateReloadUI(0, _reloadTime);
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        float time = 0f;
        _uiPlayer.UpdateReloadUI(time, _reloadTime);
        _uiPlayer.ReloadImageActive(true);
        _isReloading = true;

        while(time < _reloadTime)
        {
            time += Time.deltaTime;
            _uiPlayer.UpdateReloadUI(time, _reloadTime);
            yield return null;
        }
        _uiPlayer.ReloadImageActive(false);
        _isReloading = false;

        _bulletCount = _playerData.BulletCountMax;
        _uiPlayer.UpdateBulletCountUI(_bulletCount);
    }
    private void CamRayCast()
    {
        if (UpdateRaycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
        {
            _camRayHit = hit;
            _hand.LookAt(_camRayHit.point);
            Debug.DrawRay(_muzzle.position, _muzzle.forward * RAY_DISTANCE, Color.green, 1f);
        }
    }
    private bool UpdateRaycast(Vector3 origin, Vector3 direction, out RaycastHit hit)
    {
        Ray ray = new Ray(origin, direction);
        return Physics.Raycast(ray, out hit, RAY_DISTANCE, _layerMask);
    }

}
