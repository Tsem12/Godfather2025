using System.Runtime.InteropServices.WindowsRuntime;
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

    // O I I A I O I I A I O I I A I *spinning cat*

    public int CurrentSunscreen
    {
        get { return _currentSunscreen; }
        set
        {
            _currentSunscreen = value;
        }
    }

    void Start()
    {
        sumoHealthRef = GetComponent<SumoHealth>();

        _currentSunscreen = _maxSunscreen;
        _currentHealth = _maxHealth;
    }

    void Update()
    {

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

                if (targetHealth.CurrentHealth <= 0)
                {
                    Die();
                }

            } else {

                Debug.LogWarning(target.name + " is already dead.");
            }
        }
    }
}
