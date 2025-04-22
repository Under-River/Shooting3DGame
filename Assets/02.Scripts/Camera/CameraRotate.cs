using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Vector3 _quarterViewDirectionOffset = new Vector3(60f, 0f, 0f);
    [SerializeField] private float _minMouseY = -90f;
    [SerializeField] private float _maxMouseY = 90f;
    [SerializeField] private float _recoilPower= 10f;
    [SerializeField] private float _recoilXAngle= 5f;
    public Vector2 _rotationValue;
    public Vector2 _totalRotation;
    public Vector2 RotationValue => _rotationValue;
    private void LateUpdate()
    {
        PersonViewRotateCamera();
        QuarterViewRotateCamera();
        LerpRecoil();
    }
    public void SetRotationValueX(float x)
    {
        _rotationValue.x = x;
    }
    private void PersonViewRotateCamera()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.FPS 
        || CameraTypeManager.Instance.CameraType == CameraType.TPS)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _rotationValue.x += mouseX * _playerData.RotationSpeed * Time.deltaTime;
            _rotationValue.y -= mouseY * _playerData.RotationSpeed * Time.deltaTime;
            _rotationValue.y = Mathf.Clamp(_rotationValue.y, _minMouseY, _maxMouseY);

            transform.eulerAngles = new Vector3(_rotationValue.y, _rotationValue.x, 0);
        }
    }
    private void QuarterViewRotateCamera()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.QuarterView)
        {
            transform.eulerAngles = _quarterViewDirectionOffset;
        }
    }
    public void AddRecoil()
    {
        float x = Random.Range(-_recoilXAngle, _recoilXAngle);
        _totalRotation = new Vector3(x, -_recoilPower);
    }
    public void ResetRecoil()
    {
        _totalRotation = Vector2.zero;
    }
    void LerpRecoil()
    {
        if(_totalRotation.magnitude > 0.01f )
        {
            _rotationValue = Vector2.Lerp(_rotationValue, _rotationValue + _totalRotation, Time.deltaTime);
        }
    }
}