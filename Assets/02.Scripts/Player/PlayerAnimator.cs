using UnityEngine;

public enum PlayerAniType
{
    Idle,
    Fire,
    Reload,
    Swap
}
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerAniType _playerAniType;
    [SerializeField] private WeaponStatData _weaponStatData;
    [SerializeField] private Animator _playerAni;
    [SerializeField] private PlayerFire _playerFire;
    public WeaponType _weaponType = WeaponType.Gun;
    public PlayerAniType PlayerAniType => _playerAniType;

    private void Start()
    {
        _playerAni.SetFloat("Speed", 1 / _weaponStatData.DelayValue);
        _playerAni.SetFloat("Reload Time", 1/_weaponStatData.ReloadTime);
    }

    // 현재 재생 중인 애니메이션 이름을 반환하는 메서드
    public string GetAniName()
    {
        AnimatorStateInfo stateInfo = _playerAni.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle")) return "Idle";
        if (stateInfo.IsName("Reload")) return "Reload";
        if (stateInfo.IsName("Reload End")) return "Reload End";
        if (stateInfo.IsName("Swap")) return "Swap";
        return "Unknown";
    }

    void Update()
    {
        PlayAniFire();
        PlayAniReload();
        PlayAniCancleReload();
        PlayAniSwap();
    }
    private void PlayAniFire()
    {
        if(Input.GetMouseButtonDown(0) && _playerFire.BulletCount > 0 && _playerAniType == PlayerAniType.Idle && _weaponType == WeaponType.Gun)
        {
            _playerAniType = PlayerAniType.Fire;
            _playerAni.SetBool("Is Fire", true);
        }
        else if(Input.GetMouseButtonUp(0) || _playerFire.BulletCount <= 0)
        {
            _playerAniType = PlayerAniType.Idle;
            _playerAni.SetBool("Is Fire", false);
        }
    }
    private void PlayAniReload()
    {
        if(_weaponType == WeaponType.Gun)
        {
            if(_playerAniType == PlayerAniType.Idle)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    _playerAniType = PlayerAniType.Reload;
                    _playerAni.SetTrigger("Reload");
                }
            }
        }
    }
    private void PlayAniCancleReload()
    {
        if(_weaponType == WeaponType.Gun)
        {
            if(GetAniName() == "Reload")
            {
                if(Input.GetMouseButtonDown(0))
                {
                    _playerAniType = PlayerAniType.Idle;
                    _playerAni.SetTrigger("Cancle Reload");
                }
            }
        }
    }
    public void EventSetIdle()
    {
        _playerAniType = PlayerAniType.Idle;
    }
    public void PlayAniSwap()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _weaponType = WeaponType.Gun;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            _weaponType = WeaponType.Knife;
        }
        if(_playerFire.WeaponType != _weaponType)
        {
            if(GetAniName() == "Reload")
            {
                _playerAni.SetTrigger("Cancle Reload");
            }
            _playerFire.SetWeaponType(_weaponType);
            _playerAniType = PlayerAniType.Swap;
            _playerAni.SetTrigger("Swap");
        }
    }
}
