using UnityEngine;

public class Player : MonoBehaviour
{
    [field: SerializeField] public SumoMovement Movement { get; private set; }
    [field: SerializeField] public SumoHealth Health { get; private set; }
}
