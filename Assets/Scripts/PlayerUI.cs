using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _sunscreenSlider;

    private Player _linkedPlayer;
    
    public void Init(Player linkedPlayer)
    {
        _linkedPlayer = linkedPlayer;
        _linkedPlayer.Health.OnHealthValueChange += UpdateHealthSlider;
        _linkedPlayer.Health.OnSunScreenValueChange += UpdateSunscreenSlider;
        _healthSlider.maxValue = _linkedPlayer.Health.MaxHealth;
        _sunscreenSlider.maxValue = _linkedPlayer.Health.MaxSunscreen;
    }

    private void UpdateSunscreenSlider(float value)
    {
        _sunscreenSlider.value = value;
    }

    private void UpdateHealthSlider(float value)
    {
        _healthSlider.value = value;
    }
}
