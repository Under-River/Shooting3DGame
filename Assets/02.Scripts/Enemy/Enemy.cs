using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle    = 0,
        Trace   = 1,
        Return  = 2,
        Attack  = 3,
        Damaged = 4,
        Die     = 5,
        Patrol  = 6
    }
    public EnemyState CurrentState = EnemyState.Idle;
    public float FindDistance = 7.0f;
    public float AttackDistance = 5.0f;
    public float MoveSpeed = 3.3f;
    public int Health = 100;
    public float AttackCooltime = 2.0f;
    public float DamagedTime = 0.5f;
    public float KnockBackDuration = 0.5f;
    public float KnockBackPower = 10.0f;
    public float SwitchPatrolTime = 5.0f;
    public float PatrolPointSwitchTime = 1.0f;
    public Transform Player;
    public Transform[] PatrolPoints;
    private int _patrolIndex = 0;
    private float _knockBackTimer = 0.0f;
    private float _idleTimer = 0.0f;
    private float _attackTimer = 0.0f;
    private CharacterController _characterController;
    private Vector3 _startPosition;
    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                Idle();
                break;
            }
            case EnemyState.Trace:
            {
                Trace();
                break;
            }
            case EnemyState.Return:
            {
                Return();
                break;
            }
            case EnemyState.Attack:
            {
                Attack();
                break;
            }
            case EnemyState.Patrol:
            {
                Patrol();
                break;
            }
        }
    }
    public void TakeDamage(Damage damage)
    {
        if(CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;
        if(Health <= 0)
        {
            Debug.Log($"상태 전환 : {CurrentState} -> Die");
            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }
        Debug.Log($"상태 전환 : {CurrentState} -> Damaged");

        _knockBackTimer = 0.0f;
        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }
    private void Idle()
    {
        if(Vector3.Distance(transform.position, Player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
        if(_idleTimer < SwitchPatrolTime)
        {
            _idleTimer += Time.deltaTime;
        }
        else if(_idleTimer >= SwitchPatrolTime)
        {
            _idleTimer = 0.0f;
            _patrolIndex = 0;
            CurrentState = EnemyState.Patrol;
        }
    }
    private void Trace()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) >= FindDistance)
        {
            Debug.Log("상태 전환 : Trace -> Return");
            CurrentState = EnemyState.Return;
        }
        if (Vector3.Distance(transform.position, Player.transform.position) < AttackDistance)
        {
            Debug.Log("상태 전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
        }
        Vector3 dir = Player.transform.position - transform.position;
        _characterController.Move(dir.normalized * MoveSpeed * Time.deltaTime);
    }
    private void Return()
    {
        if (Vector3.Distance(transform.position, _startPosition) < _characterController.minMoveDistance)
        {
            Debug.Log("상태 전환 : Return -> Idle");
            transform.position = _startPosition;
            _idleTimer = 0.0f;
            CurrentState = EnemyState.Idle;
        }
        if (Vector3.Distance(transform.position, Player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환 : Return -> Trace");
            CurrentState = EnemyState.Trace;
        }
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
    private void Attack()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태 전환 : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0.0f;
            return;
        }
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격");
            _attackTimer = 0.0f;
            
        }
    }
    private void Patrol()
    {
        if(Vector3.Distance(transform.position, Player.transform.position) < FindDistance)
        {
            Debug.Log("상태 전환 : Patrol -> Trace");
            _patrolIndex = 0;
            CurrentState = EnemyState.Trace;
        }
        if(Vector3.Distance(transform.position, PatrolPoints[_patrolIndex].position) < _characterController.minMoveDistance)
        {
            print($"패트롤 포인트 전환 : {_patrolIndex} -> {_patrolIndex + 1}");
            _patrolIndex++;
            if(_patrolIndex >= PatrolPoints.Length)
            {
                _patrolIndex = 0;
            }
        }
        Vector3 dir = (PatrolPoints[_patrolIndex].position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
    private IEnumerator Damaged_Coroutine()
    {
        while(_knockBackTimer < KnockBackDuration)
        {
            Vector3 dir = transform.position - Player.transform.position;
            dir.y = 0;
            _characterController.Move(dir.normalized * KnockBackPower * Time.deltaTime);
            _knockBackTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(DamagedTime);

        Debug.Log("상태 전환 : Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }
    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
