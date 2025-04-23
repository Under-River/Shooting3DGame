using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private EffectPool _bulletEffect;
    [SerializeField] private GameObject _warningSphere;
    [SerializeField] [Range(0.05f, 0.5f)] private float _perDelayTime = 0.1f;
    [SerializeField] [Range(0.5f, 5f)]private float _cycleValue = 1f;
    [SerializeField] [Range(0.5f, 100f)] private float RAY_DISTANCE = 100f;
    private float _delayTime;
    private RaycastHit _camRayHit;
    private int _layerMask;

    public float DelayTime => _delayTime;
    public float CycleValue => _cycleValue;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        _delayTime = _perDelayTime * _cycleValue;
    }
    private void Start()
    {
        _warningSphere.SetActive(false);
    }
    private void Update()
    {
        _delayTime = _perDelayTime * _cycleValue;
    }
    private void LateUpdate()
    {
        CamRayCast();
        MuzzleRaycast();
        FireStop();
    }
    private void FireStop()
    {
        if(Input.GetMouseButtonUp(0))
        {
            _weaponRecoil.ResetRecoil();
        }
    }
    private void Fire()
    {
        if (UpdateRaycast(_muzzle.position, _muzzle.forward, out RaycastHit hit))
        {
            _bulletEffect.SpawnEffect(hit.point, hit.normal);
            _weaponRecoil.AddRecoil();
        }
    }
    private void CamRayCast()
    {
        if (UpdateRaycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
        {
            _camRayHit = hit;
            _muzzle.LookAt(_camRayHit.point);
            _hand.LookAt(_camRayHit.point);
            Debug.DrawRay(_muzzle.position, _muzzle.forward * RAY_DISTANCE, Color.green, 1f);
        }
    }
    private void MuzzleRaycast()
    {
        if (UpdateRaycast(_muzzle.position, _muzzle.forward, out RaycastHit hit))
        {
            bool isTooNear = Vector3.Distance(_camRayHit.point, hit.point) > 0.1f;
            _warningSphere.SetActive(isTooNear);
            if (isTooNear)
            {
                _warningSphere.transform.position = hit.point;
            }
        }
    }
    private bool UpdateRaycast(Vector3 origin, Vector3 direction, out RaycastHit hit)
    {
        Ray ray = new Ray(origin, direction);
        return Physics.Raycast(ray, out hit, RAY_DISTANCE, _layerMask);
    }

}
