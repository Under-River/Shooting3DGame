using UnityEngine;

public class PoolSelector : MonoBehaviour
{
    [SerializeField] private PoolingSystem _bulletEffectPool;
    [SerializeField] private PoolingSystem _bulletObjectPool;

    public PoolingSystem BulletEffectPool => _bulletEffectPool;
    public PoolingSystem BulletObjectPool => _bulletObjectPool;

    public static PoolSelector instance;
    private void Awake()
    {
        instance = this;
    }
}
