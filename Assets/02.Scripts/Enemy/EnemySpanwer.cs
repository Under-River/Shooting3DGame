using System.Collections;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _spawnTime = 5f;
    [SerializeField] private float _spawnDistance = 5f;

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
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            randomDirection.y = 0;
            Vector3 spawnPosition = _playerTransform.position + randomDirection * _spawnDistance;
            
            _enemyPool.SpawnObject(spawnPosition, transform.localEulerAngles);
        }
    }
}
