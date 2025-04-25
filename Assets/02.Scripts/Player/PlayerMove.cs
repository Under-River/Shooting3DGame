using System.Collections;
using UnityEngine;

public enum PlayerActionState
{
    None,
    Dash,
    Climb,
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerActionState _playerActionState;
    [SerializeField] private PlayerMoveData _playerMoveData;
    [SerializeField] private UI_Player _uiPlayer;

    private CharacterController _controller;
    private Transform _cameraTransform;

    public Vector3 _velocity;
    private float _speed;
    private float _stamina;
    private int _currentJumpCount;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
        _speed = _playerMoveData.DefaultSpeed;
    }
    private void FixedUpdate()
    {
        Move();
        Gravity();
    }
    private void Update()
    {
        UpdateVelocity();
        UpdateSpeed();
        UpdateStamina();
        
        Dash();
        Climb();

        _uiPlayer.UpdateStaminaUI(_stamina, _playerMoveData.StaminaMax);
    }
    private void UpdateVelocity()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MoveInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            MoveInput(0,0);
        }
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void UpdateSpeed()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && _stamina > 0)
        {
            _speed = _playerMoveData.RunSpeed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) && _stamina <= 0)
        {
            _speed = _playerMoveData.DefaultSpeed;
        }
    }
    private void UpdateStamina()
    {
        if(Input.GetKey(KeyCode.LeftShift) && _playerActionState == PlayerActionState.Climb)
        {
            _stamina -= Time.deltaTime * _playerMoveData.StaminaMinusSpeed;
        }
        else if(_stamina < _playerMoveData.StaminaMax && _playerActionState == PlayerActionState.None
        && _controller.isGrounded)
        {
            _stamina += Time.deltaTime * _playerMoveData.StaminaPlusSpeed;
        }
    }
    private void MoveInput(float x, float z)
    {
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 _moveDir = camRight * x + camForward * z;
        Vector3 move = _moveDir.normalized * _speed;

        _velocity.x = move.x;
        _velocity.z = move.z;
    }
    private void Move()
    {
        if(_playerActionState == PlayerActionState.None)
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
    private void Jump()
    {
        if (_controller.isGrounded || _currentJumpCount < _playerMoveData.MaxJumpCount)
        {
            _currentJumpCount++;
            _velocity.y = Mathf.Sqrt(_playerMoveData.JumpPower * -_playerMoveData.Gravity * _playerMoveData.GravityMultiplier);
        }
    }
    private void Dash()
    {
        if(Input.GetKeyDown(KeyCode.E) && _stamina > 1f)
        {
            StartCoroutine(Dash_Coroutine());
        }
    }
    private IEnumerator Dash_Coroutine()
    {
        _playerActionState = PlayerActionState.Dash;
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
        while (elapsed < _playerMoveData.DashDuration)
        {
            _controller.Move(dir.normalized * _playerMoveData.DashPower * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _playerActionState = PlayerActionState.None;
    }
    private void Climb()
    {
        if(_stamina > 0 && Input.GetButton("Jump"))
        {
            if(Physics.CheckSphere(transform.position, 1f, 1 << LayerMask.NameToLayer("Wall")))
            {
                _playerActionState = PlayerActionState.Climb;
                _stamina -= Time.deltaTime * _playerMoveData.StaminaMinusSpeed;
                _controller.Move(Vector3.up * _playerMoveData.ClimbSpeed * Time.deltaTime);
            }
        }
        else    
        {
            _playerActionState = PlayerActionState.None;
        }
    }
    private void Gravity()
    {
        if(_playerActionState == PlayerActionState.None)
        {
            if(!_controller.isGrounded)
            {   
                _velocity.y += _playerMoveData.Gravity * _playerMoveData.GravityMultiplier * Time.deltaTime;
                _velocity.y = Mathf.Clamp(_velocity.y, -_playerMoveData.GravityMaxSpeed, _playerMoveData.GravityMaxSpeed);
            }
            else
            {
                _currentJumpCount = 0;
                _velocity.y = -1f;
            }
        }

    }
}
