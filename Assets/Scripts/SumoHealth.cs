using System;
using UnityEngine;

public class SumoHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    private float _currentHealth;

    [Header("Sunscreen Settings")]
    [field: SerializeField] public int MaxSunscreen { get; private set; }
    private float _currentSunscreen;

    private SumoHealth sumoHealthRef;

    public float CurrentSunscreen
    {
        get => _currentSunscreen;
        set
        {
            _currentSunscreen = value;
            OnSunScreenValueChange?.Invoke(value);
        }
    }
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthValueChange?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }
    
    public event Action<float> OnSunScreenValueChange; 
    public event Action<float> OnHealthValueChange; 


    // O I I A I O I I A I O I I A I *spinning cat*

    void Awake()
    {
        sumoHealthRef = GetComponent<SumoHealth>();

        CurrentSunscreen = MaxSunscreen;
        CurrentHealth = MaxHealth;
    }
    
    public void Die()
    {
        Debug.Log($"Player :  {this.gameObject}  has died");
    }

    public void TakeDamage(GameObject target, float currentSunscreenVal)
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
