using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour, Idamgeable
{
    [SerializeField] private PoolingSystem _boomEffectPool;
    [SerializeField] private int _damage = 100;
    [SerializeField] private float _radiusRange = 2f;
    [SerializeField] private float _explosionForce = 10f;
    [SerializeField] private float _destroyTime = 3f;
    private Rigidbody _rigidbody;
    private bool _isExploding = false;

    public int _health = 30;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _boomEffectPool = PoolSelector.instance.BoomEffectPool;
    }
    public void TakeDamage(int damage)
    {
        if (_isExploding) return;
        
        _health -= damage;
        if(_health <= 0)
        {
            ExplosionBarrel();
        }
    }
    private void ExplosionBarrel()
    {
        if (_isExploding) return;
        _isExploding = true;

        _boomEffectPool.SpawnObject(transform.position, transform.localEulerAngles);
        
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.up * _explosionForce, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.one);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radiusRange);
        if(colliders.Length > 0)
        {
            foreach(Collider collider in colliders)
            {
                Idamgeable damageable = collider.GetComponent<Idamgeable>();
                if(damageable != null)
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }

        StartCoroutine(OffBarrel_Coroutine());
    }
    private IEnumerator OffBarrel_Coroutine()
    {
        yield return new WaitForSeconds(_destroyTime);
        gameObject.SetActive(false);
    }

    private void OnDrawExplosionGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusRange);
    }
}
