using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator _enemyAni;
    [SerializeField] private float _attackDistance = 5.0f;
    [SerializeField] private float _attackCooltime = 2.0f;
    [SerializeField] private int _attackDamage = 10;

    private EnemyStateManager _enemyStateManager;
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
    void Start()
    {
        _attackTimer = _attackCooltime;
    }

    public void AttackState()
    {
        _attackTimer += Time.deltaTime;

        if(IsAbleAttack())
        {
            _agent.isStopped = true;
            _enemyAni.SetFloat("Move Amount", 0);

            if(_attackTimer >= _attackCooltime)
            {
                _attackTimer = 0.0f;
                _enemyAni.SetTrigger("Attack");
            }
        }
        else
        {
            _agent.isStopped = false;
            _enemyStateManager.SetState(EnemyState.Trace);
        }
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
