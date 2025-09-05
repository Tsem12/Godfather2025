using System;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{

    [SerializeField] private float _creamCapacityMax;
    [SerializeField] private float _creamUnityPerSecond;
    [SerializeField] private Slider _slider;

    private float _currentCapacity;

    public float CurrentCapacity
    {
        get => _currentCapacity;
        set
        {
            _currentCapacity = value;
            _slider.value = value;
        }
    }

    private void Awake()
    {
        _slider.maxValue = _creamCapacityMax;
        _slider.value = 0;
    }

    private void Update()
    {
        CurrentCapacity = Mathf.Clamp(CurrentCapacity + Time.deltaTime * _creamUnityPerSecond, 0, _creamCapacityMax);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponentInParent<Player>();
            float creamNeeded = p.Health.MaxSunscreen - p.Health.CurrentSunscreen;

            float availaleCream = Mathf.Clamp(creamNeeded, 0, CurrentCapacity);

            p.Health.CurrentSunscreen += availaleCream;
            CurrentCapacity -= availaleCream;
            _slider.value = CurrentCapacity - availaleCream;
        }
    }
}
