using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private PoolingSystem _bulletEffectPool;
    private void Awake()
    {
        _bulletEffectPool = PoolSelector.instance.BulletEffectPool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if(collision.contacts.Length > 0)
            {
                transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                _bulletEffectPool.SpawnEffect(collision.contacts[0].point, collision.contacts[0].normal);
                gameObject.SetActive(false);
            }
        }
    }
}
