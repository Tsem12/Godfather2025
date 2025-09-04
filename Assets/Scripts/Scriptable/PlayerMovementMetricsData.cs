using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerMetics")]
public class PlayerMovementMetricsData : ScriptableObject
{
    [field: SerializeField] public float DashMaxDurationCharge { get; private set; }
    [field: SerializeField] public float DashCoolDown { get; private set; }
    [field: SerializeField] public Vector2 MinMaxDashVelocity { get; private set; }
    [field: SerializeField] public AnimationCurve DashVelocityCurve { get; private set; }
    
    [Header("Bump")]
    [field: SerializeField] public float BumpCooldown { get; private set; }
    
    [Header("Jump")]
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float JumpCoolDown { get; private set; }
    
}
