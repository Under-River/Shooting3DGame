using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    [SerializeField] private int _poolSize = 30;            
    [SerializeField] private GameObject _prefab;    
    private GameObject _target;        
    private Queue<GameObject> _pool;   
    private Transform _poolParent;         

    public GameObject Target => _target;

    private void Start()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        _poolParent = this.transform;
        _poolParent.name = "Pool : " + _prefab.name;
        _poolParent.localPosition = Vector3.zero;
        
        _pool = new Queue<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            CreateDustObject();
        }
    }
    private void CreateDustObject()
    {
        GameObject dust = Instantiate(_prefab, _poolParent);
        dust.SetActive(false);
        _pool.Enqueue(dust);
    }
    public void SpawnEffect(Vector3 pos, Vector3 normal)
    {
        if (_pool.Count == 0)
        {
            CreateDustObject();
        }

        GameObject target = _pool.Dequeue();
        _target = target;
        SetupDust(target, pos, normal);
        StartCoroutine(ReturnToPoolWhenInactive(target));
    }
    private void SetupDust(GameObject target, Vector3 pos, Vector3 normal)
    {
        target.SetActive(true);
        target.transform.position = pos;
        target.transform.rotation = Quaternion.LookRotation(normal);
    }
    private IEnumerator ReturnToPoolWhenInactive(GameObject target)
    {
        yield return new WaitWhile(() => target.activeSelf);
        _pool.Enqueue(target);
    }
}

