using UnityEngine;

public class BulletHitEvent : MonoBehaviour
{
    private PoolingSystem _bulletEffectPool;
    private PlayerFire _playerFire;

    private void Awake()
    {
        _bulletEffectPool = PoolSelector.instance.BulletEffectPool;
        _playerFire = FindAnyObjectByType<PlayerFire>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            HitDamage(collision);
            HitEffect(collision);
        }
    }
    private void HitEffect(Collision collision)
    {
        transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        _bulletEffectPool.SpawnObject(collision.contacts[0].point, collision.contacts[0].normal);
        gameObject.SetActive(false);
    }
    private void HitDamage(Collision collision)
    {
        Idamgeable damageable = collision.transform.GetComponent<Idamgeable>();
        if(damageable != null)
        {
            damageable.TakeDamage(_playerFire.WeaponStatData.Damage);
        }
    }
}
