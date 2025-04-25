using System.Collections;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private PlayerMoveData _playerData;
    [SerializeField] private Vector3 _quarterViewDirectionOffset = new Vector3(60f, 0f, 0f);
    [SerializeField] private float _minMouseY = -90f;
    [SerializeField] private float _maxMouseY = 90f;
    private Vector3 _rotationValue;
    private bool _isShaking = false;
    public Vector3 RotationValue => _rotationValue;
    private void FixedUpdate()
    {
        PersonViewRotateCamera();
        QuarterViewRotateCamera();
    }
    private void Update()
    {
        ShakeCamera();
    }
    public void SetRotationValue(Vector3 vec)
    {
        _rotationValue = vec;
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

            transform.eulerAngles = new Vector3(_rotationValue.y, _rotationValue.x, transform.eulerAngles.z);
        }
    }
    private void QuarterViewRotateCamera()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.QuarterView)
        {
            transform.eulerAngles = _quarterViewDirectionOffset;
        }
    }
    private void ShakeCamera()
    {
        if(!_isShaking) return;

        float z = Random.Range(-0.3f, 0.3f);
        transform.eulerAngles = transform.eulerAngles + Vector3.forward * z;
    }
    public void ActiveShakeCamera(bool isActive)
    {
        _isShaking = isActive;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
