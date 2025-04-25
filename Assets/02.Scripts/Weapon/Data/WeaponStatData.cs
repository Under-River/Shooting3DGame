using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStatData", menuName = "Weapon/WeaponStatData")]
public class WeaponStatData : ScriptableObject
{
    [Range(1, 100)] public int Damage = 10;
    [Range(100f, 500f)] public float ShotPower = 100f;
    [Range(0.5f, 4f)] public float ReloadTime = 2f;
    [Range(0.05f, 0.5f)] public float PerDelayTime = 0.1f;
    [Range(1f, 5f)] public float DelayValue = 1f;
    [Range(0.5f, 5f)] public float RecoilStrengthX = 1f;
    [Range(0.5f, 5f)] public float RecoilStrengthY = 1f;
    [Range(0f, 10f)] public float RecoilMultiplier = 1f;
    [Range(0.05f, 0.5f)] public float RecoilYDuration = 0.1f;
    public AnimationCurve RecoilCurve;
}
