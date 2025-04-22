using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private UI_Player _uiPlayer;

    private CharacterController _controller;
    private Vector3 _velocity;
    private Transform _cameraTransform;

    private bool _isGrounded;
    private bool _isDashing;
    private bool _isClimbing;

    private float _speed;
    private float _stamina;
    private int _currentJumpCount;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }
    private void FixedUpdate()
    {
        Gravity();

        GroundCheck();

        Move();
        Run();
        Jump();
        UpdateStamina();
        Climb();
        
        Dash();

        _uiPlayer.UpdateStaminaUI(_stamina, _playerData.StaminaMax);
    }
    private void Move()
    {
        if(_isDashing || _isClimbing) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 _moveDir = new Vector3(h, 0, v).normalized;

        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = _cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();
        _moveDir = camForward * v + camRight * h;

        _controller.Move(_moveDir * _speed * Time.deltaTime);
    }
    private void Run()
    {
        if(_isDashing || _isClimbing) return;

        if(Input.GetKey(KeyCode.LeftShift) && _stamina > 0)
        {
            _speed = _playerData.RunSpeed;
        }
        else
        {
            _speed = _playerData.DefaultSpeed;
        }
    }
    private void Dash()
    {
        if(_isDashing || _isClimbing) return;

        if(Input.GetKeyDown(KeyCode.E) && _stamina > 1f)
        {
            _stamina -= 1f;
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        _isDashing = true;

        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0;

        Vector3 dir = camForward;

        float elapsed = 0f;
        while (elapsed < _playerData.DashTime)
        {
            _controller.Move(dir.normalized * _playerData.DashPower * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;
    }
    private void Jump()
    {
        if(_isDashing || _isClimbing) return;

        if (Input.GetButtonDown("Jump") && (_isGrounded || _currentJumpCount < _playerData.MaxJumpCount))
        {
            _currentJumpCount++;
            _velocity.y = Mathf.Sqrt(_playerData.JumpPower * -_playerData.GravityMultiplier * _playerData.Gravity);
        }
    }
    private void Climb()
    {
        if (IsFacingWall() && WallCheck() && Input.GetKey(KeyCode.W) && _stamina > 0)
        {
            _isClimbing = true;
            _controller.Move(Vector3.up * _playerData.ClimbSpeed * Time.deltaTime);
        }
        else
        {
            _isClimbing = false;
        }
    }
    
    private void Gravity()
    {
        if(_isClimbing) return;

        _velocity.y += _playerData.Gravity * _playerData.GravityMultiplier * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
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

    private bool IsFacingWall()
    {
        RaycastHit hit;
        Vector3 forward = _cameraTransform.forward;
        if (Physics.Raycast(transform.position, forward, out hit, 1f))
        {
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall");
        }
        return false;
    }

    private void UpdateStamina()
    { 
        if((Input.GetKey(KeyCode.LeftShift) || _isClimbing) && _stamina > 0)
        {
            _stamina -= Time.deltaTime * _playerData.StaminaMinusSpeed;
        }
        else if(_isGrounded && (!Input.GetKey(KeyCode.LeftShift) || !_isClimbing) && _stamina < _playerData.StaminaMax)
        {
            _stamina += Time.deltaTime * _playerData.StaminaPlusSpeed;
        }
        _stamina = Mathf.Clamp(_stamina, 0, _playerData.StaminaMax);
    }
}
