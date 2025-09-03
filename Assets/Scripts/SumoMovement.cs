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
    
    [SerializeField] private InputHandeler _inputHandler;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _gfx;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayGroundDetectionLenght;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Dash metrics")] 
    [SerializeField] private float _dashMaxDurationCharge;
    [SerializeField] private Vector2 _minMaxDashVelocity;
    [SerializeField] private AnimationCurve _dashVelocityCurve;

    private float _currentDashChargeDate;
    private float _currentDashDirection;
    private MovementState _currentMovementState;
    
    [Header("debug")] 
    private const float _colliderDetection = 2;
    
    [SerializeField] private float _speed;
    
    private Vector2 _groundNormal;

    private float OrientX = 1;
    private Vector2 _contactPoint;
    private Quaternion _targetRotation;
    
    
    public Vector3 Direction => Quaternion.AngleAxis(90 * -transform.localScale.x, Vector3.forward) * _groundNormal;
    public bool IsOnGround { get; private set; }
    
    public float GroundInclination => (int)Vector2.Angle(Quaternion.AngleAxis(-90, Vector3.forward) * _groundNormal, Vector2.right);

    private void Awake()
    {
        _inputHandler.OnMovementInputOccured += ComputeDash;
        _inputHandler.OnMovementInputRelease += Dash;

        _currentMovementState = MovementState.Idle;
    }
    
    private void OnDestroy()
    {
        _inputHandler.OnMovementInputOccured -= ComputeDash;
        _inputHandler.OnMovementInputRelease -= Dash;
    }

    private void Update()
    {
        ComputeGroundInformations();
        ComputeOrientation(Time.deltaTime);

        if (!IsOnGround)
        {
            _rb.linearVelocity -= -Physics2D.gravity * Time.deltaTime;
        }
    }
    
    
    private void ComputeGroundInformations()
    {
        RaycastHit2D isOnGround = Physics2D.Raycast(_rayOrigin.position, -_rayOrigin.up, _rayGroundDetectionLenght, _groundLayer);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(_rayOrigin.position, -_rayOrigin.up, _colliderDetection, _groundLayer);

        _contactPoint = raycastHit2D.point;
        if (isOnGround.collider != IsOnGround)
        {
            IsOnGround = isOnGround.collider;
        }

        _groundNormal = raycastHit2D.normal;
    
    }
    
    private void ComputeOrientation(float deltaTime)
    {
        _targetRotation = Quaternion.AngleAxis(Vector2.Angle(Vector2.up, _groundNormal), Mathf.Sign(_groundNormal.x) * Vector3.back);
        if (_gfx.rotation != _targetRotation)
        {
            _gfx.rotation = Quaternion.Slerp(_gfx.rotation, _targetRotation, deltaTime * 5);
            _rayOrigin.rotation = _targetRotation;
        }
    }

    private void ComputeOrientX(float direction)
    {
        OrientX = direction;
        _gfx.localScale = new Vector3(_gfx.localScale.x * OrientX, _gfx.localScale.y, _gfx.localScale.z);
    }

    private void ComputeDash(Vector2 direction)
    {
        if (_currentMovementState == MovementState.Dashing)
            return;
        
        if (_currentMovementState == MovementState.Idle)
        {
            _currentMovementState = MovementState.ChargingDash;
            _currentDashChargeDate = Time.time;
            Debug.Log($"Start Dash");
        }
        _currentDashDirection = direction.x;
    }
    
    private void Dash(Vector2 direction)
    {
        if(_currentDashChargeDate <= 0)
            return;
        
        ComputeOrientX(Mathf.Sign(_currentDashDirection));
        _rb.linearVelocity = Direction * OrientX * 
                             Mathf.Lerp(_minMaxDashVelocity.x, _minMaxDashVelocity.y, _dashVelocityCurve.Evaluate(Time.time - _currentDashChargeDate / _dashMaxDurationCharge));
        
    }


    
    #if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _guiDebug;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _colliderDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _rayGroundDetectionLenght);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position + Direction * _rayGroundDetectionLenght);
    }
    
    private void OnGUI()
    {
        if(!_guiDebug)
            return;
        
        GUILayout.Label($"Is on ground: {IsOnGround}");
        GUILayout.Label($"State: {_currentMovementState}");
        GUILayout.Label($"Charge state: {_currentMovementState}");
        GUILayout.Label($"Charge duration: {_dashMaxDurationCharge - _currentDashChargeDate}");
        GUILayout.Label($"Charge Force: {Mathf.Lerp(_minMaxDashVelocity.x, _minMaxDashVelocity.y, _dashVelocityCurve.Evaluate(Time.time - _currentDashChargeDate / _dashMaxDurationCharge))}");
        GUILayout.Label($"Ground inclinaison: {GroundInclination}");
    }
    #endif
    
}
