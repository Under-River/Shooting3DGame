using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] private CameraRotate _cameraRotate;
    [SerializeField] private float _recoilStrengthX = 0.1f;
    [SerializeField] private float _recoilStrengthY = 1f;
    [SerializeField] private float _recoilMultiplier = 1f;
    [SerializeField] private float _recoilYDuration = 0.1f;
    [SerializeField] private AnimationCurve _recoilCurve;
    private Vector3 _totalRotation;
    private Vector3 _targetRotation;

    void Update()
    {
        LerpRecoil();
    }

    public void AddRecoil()
    {
        float x = Random.Range(-_recoilStrengthX, _recoilStrengthX) * _recoilMultiplier;
        _totalRotation = new Vector3(x, -_recoilStrengthY * _recoilMultiplier, 0);
        _targetRotation = _cameraRotate.RotationValue + _totalRotation;
    }

    public void ResetRecoil()
    {
        _totalRotation = Vector2.zero;
    }

    void LerpRecoil()
    {
        if(_totalRotation.magnitude > 0f)
        {
            Vector2 currentRotation = _cameraRotate.RotationValue;
            float t = _recoilCurve.Evaluate(Time.deltaTime);

            currentRotation.x = Mathf.Lerp(currentRotation.x, _targetRotation.x, t);
            currentRotation.y = Mathf.MoveTowards(currentRotation.y, _targetRotation.y, _recoilYDuration);
            
            _cameraRotate.SetRotationValue(currentRotation);
        }
    }
}
