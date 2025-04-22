using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private CameraRotate _cameraRotate;
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    private void LateUpdate()
    {
        PersonViewRotatePlayer();
        QuarterViewRotatePlayer();
    }
    private void PersonViewRotatePlayer()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.FPS 
        || CameraTypeManager.Instance.CameraType == CameraType.TPS)
        {
            Quaternion targetRotation = Quaternion.Euler(_cameraRotate.RotationValue.y, _cameraRotate.RotationValue.x, 0);

            _target.rotation = Quaternion.Lerp(_target.rotation, targetRotation, Time.deltaTime * _playerData.RotationSmoothness);
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

            _cameraRotate.SetRotationValueX(_target.eulerAngles.y);
        }
    }
}
