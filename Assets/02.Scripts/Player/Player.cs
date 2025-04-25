using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour, Idamgeable
{
    [SerializeField] private int _healthmax = 100;
    private int _health;

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
