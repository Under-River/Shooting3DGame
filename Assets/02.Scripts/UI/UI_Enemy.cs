using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Slider _healthSlider;
    private void Awake()
    {
        _camera = Camera.main.transform;
    }   
    private void Update()
    {
        transform.LookAt(_camera);
    }
    public void UpdateHealthUI(float health, float healthMax)
    {
        _healthSlider.value = health / healthMax;
    }
}
