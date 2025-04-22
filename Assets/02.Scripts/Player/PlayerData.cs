using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float DefaultSpeed = 7f;
    public float RunSpeed = 12f;
    public float ClimbSpeed = 5f;

    public float DashTime = 0.5f;

    public float DashPower = 40f;
    public float JumpPower = 3f;

    public int MaxJumpCount = 2;

    public float StaminaMax = 3f;
    public float StaminaPlusSpeed = 2f;
    public float StaminaMinusSpeed = 1f;

    public float Gravity = -9.81f;
    public float GravityMaxSpeed = 20f;
    public float GravityMultiplier = 3.0f;

    public float RotationSpeed = 500f;
    public float RotationSmoothness = 10f;
}
