using UnityEngine;

public class GrenadeFire : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private UI_Player _uiPlayer;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _throwPowerMin = 10f;
    [SerializeField] private float _throwPowerMax = 20f;
    [SerializeField] private float _chargeSpeed = 3f;
    [SerializeField] private float _throwAngle = 45f;
    [SerializeField] private float _throwPower;
    [SerializeField] private LineRenderer _trajectoryLine;
    [SerializeField] private int _trajectoryPoints = 30;
    [SerializeField] private float _trajectoryTimeStep = 0.1f;
    private int _grenadeCount;

    void Start()
    {
        _grenadeCount = _playerData.GrenadeCountMax;
        _uiPlayer.UpdateGrenadeCountUI(_grenadeCount);
    }

    private void LateUpdate()
    {
        if(_grenadeCount > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _throwPower = _throwPowerMin;
                _trajectoryLine.positionCount = _trajectoryPoints;
            }
            else if (Input.GetMouseButton(1))
            {
                ChargePower();
                UpdateTrajectory();
            }
            if (Input.GetMouseButtonUp(1))
            {
                FireInTheHole();
                _trajectoryLine.positionCount = 0;
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
        Vector3[] points = new Vector3[_trajectoryPoints];

        Vector3 startPosition = _firePosition.position;
        Vector3 startVelocity = _firePosition.forward * _throwPower;

        for (int i = 0; i < _trajectoryPoints; i++)
        {
            float time = i * _trajectoryTimeStep;
            points[i] = startPosition + startVelocity * time + 0.5f * Physics.gravity * time * time;
        }

        _trajectoryLine.SetPositions(points);
    }
}
