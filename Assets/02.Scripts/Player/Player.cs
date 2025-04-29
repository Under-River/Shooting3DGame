using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour, Idamgeable
{
    [SerializeField] private int _healthmax = 100;
    [SerializeField] private Volume _volume;
    private int _health;
    private Vignette _vignette;

    private void Start()
    {
        _health = _healthmax;
        _volume.profile.TryGet(out _vignette);
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
