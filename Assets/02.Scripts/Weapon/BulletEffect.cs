using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private PoolingSystem _bulletEffectPool;
    private Enemy _enemy;

    [System.Obsolete]
    private void Awake()
    {
        _bulletEffectPool = PoolSelector.instance.BulletEffectPool;
        _enemy = FindObjectOfType<Enemy>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if(collision.contacts.Length > 0)
            {
                if(collision.contacts[0].otherCollider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    _enemy.TakeDamage(new Damage(10, gameObject));
                }
                transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                _bulletEffectPool.SpawnEffect(collision.contacts[0].point, collision.contacts[0].normal);
                gameObject.SetActive(false);
            }
        }
    }
}
