using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _attackDistance = 5.0f;
    [SerializeField] private float _attackCooltime = 2.0f;
    [SerializeField] private int _attackDamage = 10;

    private EnemyStateManager _enemyStateManager;
    private float _attackTimer;  

    private void Awake()
    {
        _enemyStateManager = GetComponent<EnemyStateManager>();
    }

    public float AttackDistance => _attackDistance;

    public void AttackState()
    {
        CheckAbleAttack();
        CheckAttackTime();
    }
    private void Attack()
    {
        _player.GetComponent<Idamgeable>().TakeDamage(_attackDamage);
    }
    private void CheckAbleAttack()
    {
        if (!IsAbleAttack())
        {
            _enemyStateManager.SetState(EnemyState.Trace);
            _attackTimer = 0.0f;
            return;
        }
    }
    private void CheckAttackTime()
    {
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _attackCooltime)
        {
            _attackTimer = 0.0f;
            Attack();
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
