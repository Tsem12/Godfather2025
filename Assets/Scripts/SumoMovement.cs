using System;
using UnityEngine;

public class SumoMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _gfx;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private float _rayGroundDetectionLenght;
    [SerializeField] private LayerMask _groundLayer;

    [Header("debug")] 
    private const float _colliderDetection = 2;
    
    [SerializeField] private float _speed;
    
    private Vector2 _groundNormal;

    private float OrientX;
    private Vector3 _contactPoint;
    private Quaternion _targetRotation;
    
    
    public Vector3 Direction => Quaternion.AngleAxis(90 * -transform.localScale.x, Vector3.forward) * _groundNormal;
    public bool IsOnGround { get; private set; }
    
    public float GroundInclination => (int)Vector2.Angle(Quaternion.AngleAxis(-90, Vector3.forward) * _groundNormal, Vector2.right);
    
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

    private void Update()
    {
        ComputeGroundInformations();
        ComputeOrientation(Time.deltaTime);
        Vector3 Gravity = Physics2D.gravity * Time.deltaTime;
        if (IsOnGround)
        {
            _rb.linearVelocity = Direction * _speed + Gravity;
        }
        else
        {
            _rb.linearVelocity += (Vector2)Gravity;
        }
        
        //TEMP 
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OrientX *= -1;
            transform.localScale = new Vector3(OrientX,1,1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _colliderDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position - _rayOrigin.up * _rayGroundDetectionLenght);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_rayOrigin.position, _rayOrigin.position + Direction * _rayGroundDetectionLenght);
    }
    
    #if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _guiDebug;

    private float _velocity;
    
    private void OnGUI()
    {
        if(!_guiDebug)
            return;
        
        GUILayout.Label($"Is on ground: {IsOnGround}");
    }
#endif
    
}
