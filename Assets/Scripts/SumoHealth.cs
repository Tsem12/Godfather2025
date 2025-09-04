using UnityEngine;

public class SumoHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [Header("Sunscreen Settings")]
    [SerializeField] private int _maxSunscreen;
    [SerializeField] private int _currentSunscreen;

    private SumoHealth sumoHealthRef;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Start()
    {
        sumoHealthRef = GetComponent<SumoHealth>();
    }

    void Update()
    {
        SunOuchy();
    }

    public void Die()
    {
        Debug.Log($"Player :  {this.gameObject}  has died");

        Destroy(this.gameObject);
    }

    public void TakeDamage(GameObject target, int currentSunscreenVal)
    {
        SumoHealth targetHealth = target.GetComponent<SumoHealth>();

        if (targetHealth._currentSunscreen <= 0)
        {
            if (targetHealth != null)
            {
                targetHealth.CurrentHealth -= 1;

            } else {

                Debug.LogWarning(target.name + " is already dead.");
            }
        }
    }

    public void SunOuchy()
    {
        if (this.sumoHealthRef._currentSunscreen < 0 && this._currentHealth != 0)
        {
            TakeDamage(this.gameObject, _currentSunscreen);
        }
    }
}
