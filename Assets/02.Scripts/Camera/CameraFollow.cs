using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] [Range(0f, 3f)] private float _fpsHeightOffset = 2f;
    [SerializeField] private Vector3 _tpsOffset = new Vector3(0f, 2f, -5f);
    [SerializeField] private Vector3 _quarterViewOffset = new Vector3(0f, 15f, -10f);

    private void LateUpdate()
    {
        FollowCamera();
    }
    private void FollowCamera()
    {
        switch (CameraTypeManager.Instance.CameraType)
        {
            case CameraType.FPS:
                FPSFollow();
                break;  
            case CameraType.TPS:
                TPSFollow();
                break;
            case CameraType.QuarterView:
                QuarterViewFollow();
                break;
        }
    }
    private void FPSFollow()
    {
        transform.position = _target.position + Vector3.up * _fpsHeightOffset;
    }
    private void TPSFollow()
    {
        transform.position = _target.position + transform.rotation * _tpsOffset;
    }
    private void QuarterViewFollow()
    {
        transform.position = _target.position + _quarterViewOffset;
    }
}
