using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private int _poolSize = 10;            
    [SerializeField] private GameObject _prefab;        
    private Queue<GameObject> _pool;   
    private Transform _poolParent;         

    private void Start()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        _poolParent = this.transform;
        _poolParent.name = "Pool - " + _prefab.name;
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

        GameObject effect = _pool.Dequeue();
        SetupDust(effect, pos, normal);
        StartCoroutine(ReturnToPoolWhenInactive(effect));
    }
    private void SetupDust(GameObject effect, Vector3 pos, Vector3 normal)
    {
        effect.SetActive(true);
        effect.transform.position = pos;
        effect.transform.rotation = Quaternion.LookRotation(normal);
    }
    private IEnumerator ReturnToPoolWhenInactive(GameObject effect)
    {
        yield return new WaitWhile(() => effect.activeSelf);
        _pool.Enqueue(effect);
    }
}

