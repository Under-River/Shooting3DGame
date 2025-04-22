using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public float recoilStrength = 0.1f;
    public float returnSpeed = 5f;
    public float recoilRecoveryDelay = 0.2f;

    private Vector3 originalPosition;
    private Vector3 currentRecoilOffset;
    private float lastFireTime;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (Time.time - lastFireTime > recoilRecoveryDelay)
        {
            currentRecoilOffset = Vector3.Lerp(currentRecoilOffset, Vector3.zero, Time.deltaTime * returnSpeed);
        }

        transform.localPosition = originalPosition + currentRecoilOffset;
    }

    public void AddRecoil(float magnitude)
    {
        lastFireTime = Time.time;

        float x = Random.Range(-0.05f, 0.05f);
        float y = Random.Range(0.05f, 0.1f);

        Vector3 recoil = new Vector3(x, y, -magnitude);
        currentRecoilOffset += recoil;
    }
}
