using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private CameraRotate _cameraRotate;
    [SerializeField] private Animator _weapon;
    [SerializeField] private Transform _firePos;
    [SerializeField] private EffectPool _bulletEffect;
    [SerializeField] private ParticleSystem _oneFireEffect;
    [SerializeField] private ParticleSystem _loopFireEffect;
    [SerializeField] private GameObject _fireLight;
    [SerializeField] private float _bulletEffectTime = 0.5f;
    private bool _isFire = false;

    private void Start()
    {
        _fireLight.SetActive(false);
        _loopFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        Cursor.lockState = CursorLockMode.Locked; 
    }
    private void Update()
    {
        FireEffect();
        BulletEffect();
    }
    private void FireEffect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _oneFireEffect.Play();
            _fireLight.SetActive(true);
        }
        else if(Input.GetMouseButton(0))
        {
            if(_loopFireEffect.isPlaying) return;
            _loopFireEffect.Play();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _loopFireEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _fireLight.SetActive(false);
        }
    }
    private void BulletEffect()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _isFire = true;
            StartCoroutine(C_bulletEffect());
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _isFire = false;
            _cameraRotate.ResetRecoil();
            _weapon.SetBool("IsFire", false);
        }
    }
    IEnumerator C_bulletEffect()
    {
        while(_isFire)
        {
            Ray ray = new Ray(_firePos.transform.position, Camera.main.transform.forward);
            
            RaycastHit hitInfo = new RaycastHit();
         
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                _bulletEffect.SpawnEffect(hitInfo.point, hitInfo.normal);
            }
            _cameraRotate.AddRecoil();
            _weapon.SetBool("IsFire", true);
            yield return new WaitForSeconds(_bulletEffectTime);
        }
    }
}
