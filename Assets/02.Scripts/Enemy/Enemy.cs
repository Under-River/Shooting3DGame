using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour, Idamgeable
{
    [SerializeField] private UI_Enemy _uiEnemy;
    [SerializeField] private int _healthMax = 100;
    [SerializeField] private float _knockBackDuration = 0.5f;
    [SerializeField] private float _knockBackPower = 10.0f;
    [SerializeField] private float _dieTime = 2f;

    private EnemyStateManager _enemyStateManager;
    private PoolingSystem _bulletEffectPool;
    private Transform _player;
    private NavMeshAgent _agent;
    private int _health;
    private float _knockBackTimer;

    private void Awake()
    {
        _enemyStateManager = GetComponent<EnemyStateManager>();
        _player = FindAnyObjectByType<Player>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        _bulletEffectPool = PoolSelector.instance.BulletEffectPool;
    }
    private void OnEnable()
    {
        _health = _healthMax;
        _uiEnemy.UpdateHealthUI(_health, _healthMax);
    }
    public void TakeDamage(int damage)
    {
        if(_enemyStateManager.CurrentState == EnemyState.Die) return;

        _enemyStateManager.SetState(EnemyState.TakeDamage);
        _knockBackTimer = 0.0f;
        _health -= damage;

        _uiEnemy.UpdateHealthUI(_health, _healthMax);

        if(_health <= 0)
        {
            _enemyStateManager.SetState(EnemyState.Die);
            StartCoroutine(Die_Coroutine());
        }
    }
    public void DamageState()
    {
        if(_knockBackTimer < _knockBackDuration)
        {
            _knockBackTimer += Time.deltaTime;

            Vector3 dir = transform.position - _player.transform.position;
            dir.y = 0;
            transform.position += dir.normalized * _knockBackPower * Time.deltaTime;
        }
        else
        {
            _enemyStateManager.SetState(EnemyState.Trace);
        }
    }
    private IEnumerator Die_Coroutine()
    {
        _agent.isStopped = true;

        yield return new WaitForSeconds(_dieTime);

        gameObject.SetActive(false);
    }
}
