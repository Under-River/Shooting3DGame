using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Idamgeable
{
    [SerializeField] private Transform _player;
    [SerializeField] private UI_Enemy _uiEnemy;
    [SerializeField] private int _healthMax = 100;
    [SerializeField] private float _knockBackDuration = 0.5f;
    [SerializeField] private float _knockBackPower = 10.0f;

    private EnemyStateManager _enemyStateManager;
    private NavMeshAgent _agent;
    private CharacterController _char;
    private int _health;
    private float _knockBackTimer;

    private void Awake()
    {
        _enemyStateManager = GetComponent<EnemyStateManager>();
        _agent = GetComponent<NavMeshAgent>();
        _char = GetComponent<CharacterController>();
    }
    private void Start()
    {
        _health = _healthMax;
    }
    public void TakeDamage(int damage)
    {
        if(_enemyStateManager.CurrentState == EnemyState.Die) return;

        _knockBackTimer = 0.0f;
        _health -= damage;

        _uiEnemy.UpdateHealthUI(_health, _healthMax);

        if(_health <= 0)
        {
            _enemyStateManager.SetState(EnemyState.Die);
            StartCoroutine(Die_Coroutine());
            return;
        }
    }
    public void DamageState()
    {
        if(_knockBackTimer < _knockBackDuration)
        {
            _knockBackTimer += Time.deltaTime;

            Vector3 dir = transform.position - _player.transform.position;
            dir.y = 0;
            _char.Move(dir.normalized * _knockBackPower * Time.deltaTime);
        }
        else
        {
            _enemyStateManager.SetState(EnemyState.Trace);
        }
    }
    private IEnumerator Die_Coroutine()
    {
        _agent.enabled = false;
        _char.enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
