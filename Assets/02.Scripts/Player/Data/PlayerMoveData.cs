using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerMoveData", menuName = "Player/PlayerMoveData")]
public class PlayerMoveData : ScriptableObject
{
    [Header("Speed")]
    public float DefaultSpeed = 7f;
    public float RunSpeed = 12f;
    public float ClimbSpeed = 5f;

    [Header("Dash")]
    public float DashDuration = 0.5f;
    public float DashPower = 40f;

    [Header("Jump")]
    public float JumpPower = 3f;
    public int MaxJumpCount = 2;

    [Header("Stamina")]
    public float StaminaMax = 3f;
    public float StaminaPlusSpeed = 2f;
    public float StaminaMinusSpeed = 1f;

    [Header("Rotation")]
    public float RotationSpeed = 500f;
    public float RotationSmoothness = 10f;

    [Header("Gravity")]
    public float Gravity = -9.81f;
    public float GravityMaxSpeed = 20f;
    public float GravityMultiplier = 3.0f;
}
