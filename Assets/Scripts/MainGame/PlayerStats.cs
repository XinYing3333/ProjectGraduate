using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f; 
    public float maxEnergy = 100f; 
    public float energyRechargeRate = 5f; 
    public Slider healthSlider; 
    public Slider energySlider; 

    private float _currentHealth;
    private float _currentEnergy;

    void Start()
    {
        _currentHealth = maxHealth;
        _currentEnergy = maxEnergy;
        UpdateUI();
    }

    void Update()
    {
        RechargeEnergy();
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public bool UseEnergy(float amount)
    {
        if (_currentEnergy >= amount)
        {
            _currentEnergy -= amount;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0, maxEnergy);
            return true;
        }
        return false;
    }

    void RechargeEnergy()
    {
        _currentEnergy += energyRechargeRate * Time.deltaTime;
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, maxEnergy);
    }

    void Die()
    {
        if (_currentHealth <= 0)
        {
            Debug.Log("Player has died.");
        }
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = _currentHealth;
        }
        if (energySlider != null)
        {
            energySlider.value = _currentEnergy;
        }
    }
}