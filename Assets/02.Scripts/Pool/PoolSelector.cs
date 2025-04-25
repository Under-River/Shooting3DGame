using UnityEngine;

public class PoolSelector : MonoBehaviour
{
    [SerializeField] private PoolingSystem _bulletEffectPool;
    [SerializeField] private PoolingSystem _boomEffectPool;
    [SerializeField] private PoolingSystem _bulletPool;
    [SerializeField] private PoolingSystem _enemyPool;

    public PoolingSystem BulletEffectPool => _bulletEffectPool;
    public PoolingSystem BoomEffectPool => _boomEffectPool;
    public PoolingSystem BulletPool => _bulletPool;
    public PoolingSystem EnemyPool => _enemyPool;

    public static PoolSelector instance;
    private void Awake()
    {
        instance = this;
    }
}
