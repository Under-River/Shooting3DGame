using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    [SerializeField] private int _poolSize = 30;            
    [SerializeField] private GameObject _prefab;    
    private GameObject _target;        
    private List<GameObject> _pool;   
    private Transform _poolParent;   

    public GameObject Target => _target;

    private void Awake()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        _poolParent = transform;
        _poolParent.name = "Pool : " + _prefab.name;
        _poolParent.localPosition = Vector3.zero;
        
        _pool = new List<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            CreateObject();
        }
    }
    private void CreateObject()
    {
        GameObject obj = Instantiate(_prefab, _poolParent);
        obj.SetActive(false);
        _pool.Add(obj);
    }
    public void SpawnObject(Vector3 pos, Vector3 normal, Transform parent = null)
    {
        if (_pool.Count == 0)
        {
            CreateObject();
        }

        GameObject obj = _pool.Find(x => !x.activeSelf);
        if (obj == null)
        {
            CreateObject();
            obj = _pool[_pool.Count - 1];
        }

        _target = obj;
        _target.SetActive(true);
        _target.transform.position = pos;
        _target.transform.LookAt(_target.transform.position + normal, Vector3.up);
        
        if (parent != null)
        {
            _target.transform.SetParent(parent);
        }

        StartCoroutine(ReturnToPoolWhenInactive(_target));
    }
    private IEnumerator ReturnToPoolWhenInactive(GameObject obj)
    {
        yield return new WaitWhile(() => obj.activeSelf);
        obj.transform.SetParent(_poolParent);
    }
}

