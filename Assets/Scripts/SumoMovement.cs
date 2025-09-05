using System;
using UnityEngine;

public class SumoMovement : MonoBehaviour
{
    public enum MovementState
    {
        ChargingDash,
        Dashing,
        Idle
    }

    [SerializeField] private Animator _animator;
    [SerializeField] private InputHandeler _inputHandler;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _gfx;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayGroundDetectionLenght;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Dash metrics")] [SerializeField]
    private PlayerMovementMetricsData _metricsData;

    private float _currentDashCoolDown;
    private float _currentBumpCoolDown;
    private float _currentJumpCoolDown;
    private float _currentDashChargeDate;
    private float _currentDashDirection;
    private MovementState _currentMovementState;

    [Header("debug")] private const float _colliderDetection = 2;


    private Vector2 _groundNormal;
    private float OrientX = 1;
    private Vector2 _contactPoint;
    private Quaternion _targetRotation;


    public Vector3 GroundDirection => Quaternion.AngleAxis(90 * -transform.localScale.x, Vector3.forward) * _groundNormal;
    public bool IsOnGround { get; private set; }
    public bool CanUseDash => _currentDashCoolDown <= 0;
    public bool IsBumped => _currentBumpCoolDown > 0;
    public bool CanJump => _currentJumpCoolDown <= 0;
    public Vector3 CurrentVelocityDirection => _rb.linearVelocity.normalized;
    public float CurrentVelocityMagnitude => _rb.linearVelocity.sqrMagnitude;

    public float GroundInclination =>
        (int)Vector2.Angle(Quaternion.AngleAxis(-90, Vector3.forward) * _groundNormal, Vector2.right);

    private void Awake()
    {
        _inputHandler.OnMovementInputOccured += ComputeDash;
        _inputHandler.OnMovementInputRelease += Dash;
        _inputHandler.OnJumpInputPressed += Jump;

        _currentMovementState = MovementState.Idle;
    }

    private void OnDestroy()
    {
        _inputHandler.OnMovementInputOccured -= ComputeDash;
        _inputHandler.OnMovementInputRelease -= Dash;
        _inputHandler.OnJumpInputPressed -= Jump;
    }

    private void Update()
    {
        ComputeGroundInformations();
        ComputeOrientation();
        ComputeDashCoolDown();
        ComputeGravity();
        ComputeBumpCoolDown();
        ComputeJumpCoolDown();
        
        _animator.SetFloat("Velocity", CurrentVelocityMagnitude);
        _animator.SetFloat("YVelocity", _rb.linearVelocityY);
    }




    private void ComputeGroundInformations()
    {
        RaycastHit2D isOnGround =
            Physics2D.Raycast(_rayOrigin.position, -_rayOrigin.up, _rayGroundDetectionLenght, _groundLayer);
        RaycastHit2D raycastHit2D =
            Physics2D.Raycast(_rayOrigin.position, -_rayOrigin.up, _colliderDetection, _groundLayer);

        _contactPoint = raycastHit2D.point;
        if (isOnGround.collider != IsOnGround)
        {
            IsOnGround = isOnGround.collider;
        }

        _groundNormal = raycastHit2D.normal;
    }

    private void ComputeOrientation()
    {
        _targetRotation = Quaternion.AngleAxis(Vector2.Angle(Vector2.up, _groundNormal),
            Mathf.Sign(_groundNormal.x) * Vector3.back);
        if (_gfx.rotation != _targetRotation)
        {
            _gfx.rotation = Quaternion.Slerp(_gfx.rotation, _targetRotation, Time.deltaTime * 5);
            _rayOrigin.rotation = _targetRotation;
        }
    }

    private void ComputeOrientX(float direction)
    {
        OrientX = direction;
        _gfx.localScale = new Vector3(_gfx.localScale.x * OrientX, _gfx.localScale.y, _gfx.localScale.z);
    }
    
    private void ComputeGravity()
    {
        _rb.linearVelocity -= -Physics2D.gravity * Time.deltaTime * 2;
    }

    private void ComputeDash(Vector2 direction)
    {
        if (_currentMovementState == MovementState.Dashing || !CanUseDash || IsBumped || !IsOnGround)
            return;

        if (_currentMovementState == MovementState.Idle)
        {
            _currentMovementState = MovementState.ChargingDash;
            _currentDashChargeDate = Time.time;
            _animator.SetFloat("ChargeDash", _rb.linearVelocityY);
            Debug.Log($"Start Dash");
        }

        if (Mathf.Abs(direction.x) > 0.9f)
        {
            _currentDashDirection = direction.x;
        }
    }

    private void ComputeDashCoolDown()
    {
        if(_currentDashCoolDown <= 0)
            return;

        _currentDashCoolDown -= Time.deltaTime;
        if (_currentDashCoolDown <= 0)
        {
            _currentMovementState = MovementState.Idle;
        }
        
    }

    private void ComputeBumpCoolDown()
    {
        if(_currentBumpCoolDown <= 0)
            return;

        _currentBumpCoolDown -= Time.deltaTime;
    }
    
    private void ComputeJumpCoolDown()
    {
        if(_currentJumpCoolDown <= 0)
            return;

        _currentJumpCoolDown -= Time.deltaTime;
    }

    public void Bump()
    {
        if(IsBumped)
            return;
        
        _rb.linearVelocity = Vector2.zero;
        _currentBumpCoolDown = _metricsData.BumpCooldown;
        EffetLauncher.Instance.TriggerBumpFeedback();
        Debug.Log("Bump");
        _animator.SetTrigger("Death");
    }

    private void Dash(Vector2 direction)
    {
        if (_currentDashChargeDate <= 0 || !IsOnGround)
            return;

        ComputeOrientX(Mathf.Sign(_currentDashDirection));
        _rb.linearVelocity = GroundDirection * OrientX *
                             Mathf.Lerp(_metricsData.MinMaxDashVelocity.x, _metricsData.MinMaxDashVelocity.y,
                                 _metricsData.DashVelocityCurve.Evaluate(
                                     (Time.time - _currentDashChargeDate) / _metricsData.DashMaxDurationCharge));

        _currentMovementState = MovementState.Dashing;
        _currentDashCoolDown = _metricsData.DashCoolDown;
        _currentDashChargeDate = -1;
        _animator.SetTrigger("Dash");
    }

    private void Jump()
    {
        if(!IsOnGround || !CanJump)
            return;
        
        _rb.linearVelocity += _groundNormal * _metricsData.JumpForce;
        _currentJumpCoolDown = _metricsData.JumpCoolDown;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(IsBumped)
            return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            Player oponent = other.gameObject.GetComponentInParent<Player>();
            
            if(oponent.Movement.IsBumped)
                return;

            if(_currentMovementState != MovementState.Dashing)
                return;
            
            if (_currentMovementState == MovementState.Dashing && oponent.Movement._currentMovementState == MovementState.Dashing)
            {
                SumoMovement playerToBump = CurrentVelocityMagnitude > oponent.Movement.CurrentVelocityMagnitude ? oponent.Movement : this;
                playerToBump.Bump();
            }
            else
            {
                oponent.Movement.Bump();
            }
        }
    }


#if UNITY_EDITOR
    [Header("Debug")] [SerializeField] private bool _guiDebug;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _colliderDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _rayGroundDetectionLenght);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position + GroundDirection * _rayGroundDetectionLenght);
    }

    private void OnGUI()
    {
        if (!_guiDebug)
            return;

        GUILayout.Label($"Is on ground: {IsOnGround}");
        GUILayout.Label($"State: {_currentMovementState}");
        GUILayout.Label($"Charge state: {_currentMovementState}");
        GUILayout.Label($"Charge duration: {_metricsData.DashMaxDurationCharge - _currentDashChargeDate}");
        GUILayout.Label($"Jump cooldown: {(int)_currentJumpCoolDown} can jump {CanJump}");
        GUILayout.Label($"Can dash: {CanUseDash}");
        GUILayout.Label($"Can Be bumped: {IsBumped}");

        if (_currentMovementState == MovementState.ChargingDash)
        {
            GUILayout.Label(
                $"Charge Force: {Mathf.Lerp(_metricsData.MinMaxDashVelocity.x, _metricsData.MinMaxDashVelocity.y, _metricsData.DashVelocityCurve.Evaluate((Time.time - _currentDashChargeDate) / _metricsData.DashMaxDurationCharge))}");
        }
        GUILayout.Label($"Ground inclinaison: {GroundInclination}");
    }
#endif
}