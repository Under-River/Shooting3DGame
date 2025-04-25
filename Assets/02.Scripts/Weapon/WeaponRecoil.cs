using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] private WeaponStatData _weaponStatData;
    [SerializeField] private CameraRotate _cameraRotate;
    private Vector3 _totalRotation;
    private Vector3 _targetRotation;

    void Update()
    {
        LerpRecoil();
    }
    public void AddRecoil(int fireCount)
    {
        if(fireCount > 1)
        {
            float x = Random.Range(-_weaponStatData.RecoilStrengthX, _weaponStatData.RecoilStrengthX) * _weaponStatData.RecoilMultiplier;
            _totalRotation = new Vector3(x, -_weaponStatData.RecoilStrengthY * _weaponStatData.RecoilMultiplier, 0);
            _targetRotation = _cameraRotate.RotationValue + _totalRotation;
        }
        _cameraRotate.ActiveShakeCamera(true);
    }
    public void ResetRecoil()
    {
        _totalRotation = Vector2.zero;
        _cameraRotate.ActiveShakeCamera(false);
    }
    private void LerpRecoil()
    {
        if(_totalRotation.magnitude > 0f)
        {
            Vector2 currentRotation = _cameraRotate.RotationValue;
            float t = _weaponStatData.RecoilCurve.Evaluate(Time.deltaTime);

            currentRotation.x = Mathf.Lerp(currentRotation.x, _targetRotation.x, t);
            currentRotation.y = Mathf.MoveTowards(currentRotation.y, _targetRotation.y, _weaponStatData.RecoilYDuration);
            
            _cameraRotate.SetRotationValue(currentRotation);
        }
    }
}
