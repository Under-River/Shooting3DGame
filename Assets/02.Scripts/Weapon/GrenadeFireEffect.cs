using UnityEngine;

public class GrenadeFireEffect : MonoBehaviour
{
    public GameObject ExplosionEffectPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            GameObject effectObject = Instantiate(ExplosionEffectPrefab);
            effectObject.transform.position = transform.position;
            
            Destroy(gameObject);
        }
    }
}
