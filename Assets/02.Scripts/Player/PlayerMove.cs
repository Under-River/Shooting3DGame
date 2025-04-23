using System.Collections;
using UnityEngine;

public enum PlayerMoveState
{
    Idle,
    Walk,
    Run,
    Dash,
    Jump,
    Climb,
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerMoveState _playerMoveState;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private UI_Player _uiPlayer;

    private CharacterController _controller;
    private Vector3 _velocity;
    private Transform _cameraTransform;

    private bool _isGrounded;
    private float _speed;
    private float _stamina;
    private int _currentJumpCount;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        Gravity();
        GroundCheck();

        Walk();
        Run();

        Dash();
        Jump();
        Climb();

        UpdateStamina();

        _uiPlayer.UpdateStaminaUI(_stamina, _playerData.StaminaMax);
    }
    private void Walk()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            _playerMoveState = PlayerMoveState.Walk;    

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 camForward = _cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = _cameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 _moveDir = camForward * v + camRight * h;

            _controller.Move(_moveDir * _speed * Time.deltaTime);
        }
    }
    private void Run()
    {
        if(Input.GetKey(KeyCode.LeftShift) && _stamina > 0)
        {
            _playerMoveState = PlayerMoveState.Run;    
            _speed = _playerData.RunSpeed;
            _stamina -= Time.deltaTime * _playerData.StaminaMinusSpeed;
        }
        else
        {
            _playerMoveState = PlayerMoveState.Walk;
            _speed = _playerData.DefaultSpeed;
        }
    }
    private void Dash()
    {
        if(_playerMoveState == PlayerMoveState.Dash) return;

        if(Input.GetKeyDown(KeyCode.E) && _stamina > 1f)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        _playerMoveState = PlayerMoveState.Dash;
        _stamina -= 1f;

        Vector3 dir = Vector3.zero;

        if(CameraTypeManager.Instance.CameraType != CameraType.QuarterView)
        {
            Vector3 camForward = _cameraTransform.forward;
            camForward.y = 0;

            dir = camForward;
        }
        else
        {
            dir = transform.GetComponent<PlayerRotate>().TargetX.forward;
        }

        float elapsed = 0f;
        while (elapsed < _playerData.DashTime)
        {
            _controller.Move(dir.normalized * _playerData.DashPower * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _playerMoveState = PlayerMoveState.Idle;
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_isGrounded || _currentJumpCount < _playerData.MaxJumpCount)
            {
                _playerMoveState = PlayerMoveState.Jump;
                _currentJumpCount++;
                _velocity.y = Mathf.Sqrt(_playerData.JumpPower * -_playerData.GravityMultiplier * _playerData.Gravity);
                _controller.Move(_velocity * Time.deltaTime);
            }
        }
    }
    private void Climb()
    {
        if (WallCheck() && Input.GetButton("Jump") && _stamina > 0)
        {
            _playerMoveState = PlayerMoveState.Climb;
            _stamina -= Time.deltaTime * _playerData.StaminaMinusSpeed;
            _controller.Move(Vector3.up * _playerData.ClimbSpeed * Time.deltaTime);
        }
    }
    private void Gravity()
    {
        if(_playerMoveState == PlayerMoveState.Climb) return;

        if(!_isGrounded)
        {
            _velocity.y += _playerData.Gravity * _playerData.GravityMultiplier * Time.deltaTime;
            _velocity.y = Mathf.Clamp(_velocity.y, -_playerData.GravityMaxSpeed, _playerData.GravityMaxSpeed);
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
    private void GroundCheck()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _currentJumpCount = 0;
            _velocity.y = -_playerData.GravityMultiplier;
        }
    }
    private bool WallCheck()
    {
        int wallLayer = LayerMask.NameToLayer("Wall");
        return Physics.CheckSphere(transform.position, 1f, 1 << wallLayer);
    }
    private void UpdateStamina()
    { 
        if(_playerMoveState == PlayerMoveState.Climb || _playerMoveState == PlayerMoveState.Run) return;

        if(_isGrounded && _stamina < _playerData.StaminaMax)
        {
            _stamina += Time.deltaTime * _playerData.StaminaPlusSpeed;
        }
        _stamina = Mathf.Clamp(_stamina, 0, _playerData.StaminaMax);

    }
}
