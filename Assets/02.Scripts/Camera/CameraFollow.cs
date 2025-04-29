using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _fpsTarget;
    [SerializeField] private Transform _tpsTarget;
    [SerializeField] private Transform _quarterViewTarget;

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
        transform.position = _fpsTarget.position;
    }
    private void TPSFollow()
    {
        transform.position = _tpsTarget.position;
    }
    private void QuarterViewFollow()
    {
        transform.position = _quarterViewTarget.position;
    }
}
