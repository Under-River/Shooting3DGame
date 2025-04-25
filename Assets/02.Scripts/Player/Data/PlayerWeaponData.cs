using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerWeaponData", menuName = "Player/PlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    [Header("Grenade")]
    public int GrenadeCountMax = 3;

    [Header("Bullet")]
    public int BulletCountMax = 50;

}
