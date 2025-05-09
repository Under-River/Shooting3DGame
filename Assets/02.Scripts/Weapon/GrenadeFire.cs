using UnityEngine;

public class GrenadeFire : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerWeaponData _playerWeaponData;
    [SerializeField] private UI_Player _uiPlayer;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _firePosition;

    [Header("Throw Stat")]
    [SerializeField] private float _throwPowerMin = 10f;
    [SerializeField] private float _throwPowerMax = 20f;
    [SerializeField] private float _chargeSpeed = 3f;
    [SerializeField] private float _throwAngle = 45f;

    [Header("LineRenderer")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _pointCount = 30;
    [SerializeField] private float _timeStep = 0.1f;

    private float _throwPower;
    private int _grenadeCount;

    void Start()
    {
        _grenadeCount = _playerWeaponData.GrenadeCountMax;
        _uiPlayer.UpdateGrenadeCountUI(_grenadeCount);
    }

    private void LateUpdate()
    {
        if(_grenadeCount > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _throwPower = _throwPowerMin;
                _line.positionCount = _pointCount;
            }
            else if (Input.GetMouseButton(1))
            {
                ChargePower();
                UpdateTrajectory();
            }
            if (Input.GetMouseButtonUp(1))
            {
                FireInTheHole();
                _line.positionCount = 0;
            }
        }
    }

    private void FireInTheHole()
    {
        _grenadeCount--;
        _uiPlayer.UpdateGrenadeCountUI(_grenadeCount);

        GameObject bomb = Instantiate(_bombPrefab);
        bomb.transform.position = _firePosition.position;
        
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        _firePosition.localEulerAngles = new Vector3(-_throwAngle, 0, 0);
        bombRigidbody.AddForce(_firePosition.forward * _throwPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
    }

    private void ChargePower()
    {
        _throwPower += Time.deltaTime * _chargeSpeed;
        _throwPower = Mathf.Clamp(_throwPower, _throwPowerMin, _throwPowerMax);
    }

    private void UpdateTrajectory()
    {
        Vector3[] points = new Vector3[_pointCount];

        Vector3 startPosition = _firePosition.position;
        Vector3 startVelocity = _firePosition.forward * _throwPower;

        for (int i = 0; i < _pointCount; i++)
        {
            float time = i * _timeStep;
            points[i] = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
        }

        _line.SetPositions(points);
    }
}
