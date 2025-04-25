using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackDistance = 5.0f;
    [SerializeField] private float _attackCooltime = 2.0f;
    [SerializeField] private int _attackDamage = 10;

    private EnemyStateManager _enemyStateManager;
    private PoolingSystem _attackEffectPool;
    private Transform _player;
    private NavMeshAgent _agent;
    private float _attackTimer;

    public float AttackDistance => _attackDistance;
    private void Awake()
    {
        _enemyStateManager = GetComponent<EnemyStateManager>();
        _player = FindAnyObjectByType<Player>().transform;
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        _attackEffectPool = PoolSelector.instance.EnemyAttackEffectPool;
    }

    public void AttackState()
    {
        if(_agent.enabled == true)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            _player.GetComponent<Idamgeable>().TakeDamage(_attackDamage);
            _attackEffectPool.SpawnObject(transform.position, transform.forward, transform);
            
            Vector3 dir = _player.transform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if(_attackTimer >= _attackCooltime)
        {
            _attackTimer = 0.0f;

            _agent.enabled = true;
            _agent.isStopped = false;
            _enemyStateManager.SetState(EnemyState.Trace);
        }
        _attackTimer += Time.deltaTime;
    }
    private bool IsAbleAttack()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < _attackDistance;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }

}
