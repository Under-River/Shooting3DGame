using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private PlayerMoveData _playerData;
    [SerializeField] private CameraRotate _cameraRotate;
    [SerializeField] private Transform _targetX;
    [SerializeField] private Transform _targetY;
    public Transform TargetX => _targetX;
    public Transform TargetY => _targetY;

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
            Quaternion targetRotationX = Quaternion.Euler(0, _cameraRotate.RotationValue.x, 0);
            Quaternion targetRotationY = Quaternion.Euler(_cameraRotate.RotationValue.y, 0, 0);

            _targetX.rotation = Quaternion.Lerp(_targetX.rotation, targetRotationX, Time.deltaTime * _playerData.RotationSmoothness);
            _targetY.localRotation = Quaternion.Lerp(_targetY.localRotation, targetRotationY, Time.deltaTime * _playerData.RotationSmoothness);
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
                _targetX.rotation = rotation;
            }

            _cameraRotate.SetRotationValue(new Vector3(_targetX.eulerAngles.y, 0, 0));
        }
    }
}
