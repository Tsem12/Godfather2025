using UnityEngine;

[CreateAssetMenu(menuName = "PlayerMetics")]
public class PlayerMovementMetricsData : ScriptableObject
{
    [field: SerializeField] public float DashMaxDurationCharge { get; private set; }
    [field: SerializeField] public float DashCoolDown { get; private set; }
    [field: SerializeField] public float DashStateDuration { get; private set; }
    [field: SerializeField] public Vector2 MinMaxDashVelocity { get; private set; }
    [field: SerializeField] public AnimationCurve DashVelocityCurve { get; private set; }
    [field: SerializeField] public float GravityScale { get; private set; }
}
