using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Transform _target;
    [SerializeField] private float _minMouseY = -90f;
    [SerializeField] private float _maxMouseY = 90f;
    private Vector2 _rotationValue;
    public Transform Target => _target;
    public Vector2 RotationValue => _rotationValue;

    private void FixedUpdate()
    {
        PersonViewRotatePlayer();
        QuarterViewRotatePlayer();
    }
    private void PersonViewRotatePlayer()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.FPS 
        || CameraTypeManager.Instance.CameraType == CameraType.TPS)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _rotationValue.x += mouseX * _playerData.RotationSpeed * Time.deltaTime;
            _rotationValue.y -= mouseY * _playerData.RotationSpeed * Time.deltaTime;
            _rotationValue.y = Mathf.Clamp(_rotationValue.y, _minMouseY, _maxMouseY);

            _target.eulerAngles = new Vector3(_rotationValue.y, _rotationValue.x, 0);
        }
    }
    private void QuarterViewRotatePlayer()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.QuarterView)
        {
            if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Vector3 _moveDir = new Vector3(h, 0, v).normalized;

                Quaternion rotation = Quaternion.LookRotation(_moveDir);
                _target.rotation = rotation;
            }

            _rotationValue.x = _target.eulerAngles.y;
        }
    }
}
