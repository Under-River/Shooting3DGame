using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private PlayerRotate _playerRotate;
    [SerializeField] private Vector3 _quarterViewDirectionOffset = new Vector3(60f, 0f, 0f);
    private void FixedUpdate()
    {
        CameraRotateUpdate();
    }
    void CameraRotateUpdate()
    {
        PersonViewRotateCamera();
        QuarterViewRotateCamera();
    }
    void PersonViewRotateCamera()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.FPS 
        || CameraTypeManager.Instance.CameraType == CameraType.TPS)
        {
            transform.eulerAngles = new Vector3(_playerRotate.RotationValue.y, _playerRotate.RotationValue.x, 0);
        }
    }
    void QuarterViewRotateCamera()
    {
        if(CameraTypeManager.Instance.CameraType == CameraType.QuarterView)
        {
            transform.eulerAngles = _quarterViewDirectionOffset;
        }
    }
}
