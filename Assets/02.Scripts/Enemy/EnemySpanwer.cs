using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnTime = 5f;

    private PoolingSystem _enemyPool;
    private void Start()
    {
        _enemyPool = PoolSelector.instance.EnemyPool;
        StartCoroutine(SpawnEnemy_Coroutine());
    }

    private IEnumerator SpawnEnemy_Coroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_spawnTime);
            
            _enemyPool.SpawnObject(_spawnPoint.position, Vector3.zero);
        }
    }
}
