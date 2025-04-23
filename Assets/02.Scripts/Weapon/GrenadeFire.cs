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
            }
            else if (Input.GetMouseButton(1))
            {
                ChargePower();
            }
            if (Input.GetMouseButtonUp(1))
            {
                FireInTheHole();
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
}
