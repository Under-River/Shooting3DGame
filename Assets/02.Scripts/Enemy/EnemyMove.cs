using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private float _findDistance = 10f;
    [SerializeField] private float _patrolSwitchTime = 3f;
    [SerializeField] private float _moveSpeed = 3.0f;

    private EnemyStateManager _enemyStateManager;
    private EnemyAttack _enemyAttack;
    private NavMeshAgent _agent;
    private CharacterController _controller;
    private Vector3 _startPosition;
    private int _patrolIndex = 0;
    private float _idleTimer = 0.0f;

    private void Awake()
    {
        _enemyStateManager = GetComponent<EnemyStateManager>();
        _enemyAttack = GetComponent<EnemyAttack>();
        _agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        _agent.speed = _moveSpeed;
        _startPosition = transform.position;
        _enemyStateManager.SetState(EnemyState.Idle);
    }
    public void IdleState()
    {
        CheckFindPlayer();
        CheckPatrolTime();
    }
    public void TraceState()
    {
        CheckSoFarPlayer();
        CheckAbleAttack();
        Move(_player.transform.position);
    }
    public void ReturnState()
    {
        CheckFindPlayer();
        CheckArriveStartPoint();
        Move(_startPosition);
    }
    public void PatrolState()
    {
        CheckFindPlayer();
        UpdatePatrolIndex();
        Move(_patrolPoints[_patrolIndex].position);
    }
    private void CheckFindPlayer()
    {
        if(IsFindPlayer())
        {
            _enemyStateManager.SetState(EnemyState.Trace);
            return;
        }
    }
    private void CheckSoFarPlayer()
    {
        if(!IsFindPlayer())
        {
            _enemyStateManager.SetState(EnemyState.Return);
            return;
        }
    }
    private void CheckArriveStartPoint()
    {
        if(GetDistanceXZ(transform.position, _startPosition) < _controller.minMoveDistance)
        {
            _patrolIndex = 0;
            _enemyStateManager.SetState(EnemyState.Idle);
        }
    }
    private void CheckAbleAttack()
    {
        if(GetDistanceXZ(transform.position, _player.transform.position) < _enemyAttack.AttackDistance)
        {
            _enemyStateManager.SetState(EnemyState.Attack);
        }
    }
    private void CheckPatrolTime()
    {
        if(_idleTimer < _patrolSwitchTime)
        {
            _idleTimer += Time.deltaTime;
        }
        else if(_idleTimer >= _patrolSwitchTime)
        {
            _idleTimer = 0.0f;
            _patrolIndex = 0;
            _enemyStateManager.SetState(EnemyState.Patrol);
        }
    }
    private void UpdatePatrolIndex()
    {
        if(GetDistanceXZ(transform.position, _patrolPoints[_patrolIndex].position) < _controller.minMoveDistance)
        {
            _patrolIndex++;
            if(_patrolIndex >= _patrolPoints.Length)
            {
                _patrolIndex = 0;
            }
        }
    }
    private void Move(Vector3 targetPosition)
    {
        if (!_agent.enabled || !_agent.isOnNavMesh) return;

        _agent.SetDestination(targetPosition);
    }
    private bool IsFindPlayer()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < _findDistance;
    }
    private float GetDistanceXZ(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(new Vector3(a.x, 0, a.z), new Vector3(b.x, 0, b.z));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.greenYellow;
        Gizmos.DrawWireSphere(transform.position, _findDistance);
    }
}
