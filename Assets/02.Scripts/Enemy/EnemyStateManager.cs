using UnityEngine;

public enum EnemyState
{
    Idle    = 0,
    Trace   = 1,
    Return  = 2,
    Patrol  = 3,
    Attack  = 4,
    TakeDamage  = 5,
    Die     = 6,
}
public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyState _currentState = EnemyState.Idle;

    private Enemy _enemy;
    private EnemyMove _enemyMove;
    private EnemyAttack _enemyAttack;

    public EnemyState CurrentState => _currentState;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyMove = GetComponent<EnemyMove>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }
    public void SetState(EnemyState state)
    {
        _currentState = state;
    }
    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
            {
                _enemyMove.IdleState();
                break;
            }
            case EnemyState.Trace:
            {
                _enemyMove.TraceState();
                break;
            }
            case EnemyState.Return:
            {
                _enemyMove.ReturnState();
                break;
            }
            case EnemyState.Patrol:
            {
                _enemyMove.PatrolState();
                break;
            }
            case EnemyState.Attack:
            {
                _enemyAttack.AttackState();
                break;
            }
            case EnemyState.TakeDamage:
            {
                _enemy.DamageState();
                break;
            }
            case EnemyState.Die:
            {
                break;
            }
        }
    }
}
