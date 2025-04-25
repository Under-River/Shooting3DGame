using UnityEngine;

public class Player : MonoBehaviour, Idamgeable
{
    [SerializeField] private int _healthmax = 100;
    public int _health;

    private void Start()
    {
        _health = _healthmax;
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player is dead");
    }
}
