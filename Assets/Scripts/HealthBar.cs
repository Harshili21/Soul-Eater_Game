using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;
    Damageable playerDamagable;
    
    void Awake(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamagable = player.GetComponent<Damageable>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(playerDamagable.Health, playerDamagable.MaxHealth);
        healthBarText.text = "HP " + playerDamagable.Health + " / "+ playerDamagable.MaxHealth;
        
    }

    private float CalculateSliderPercentage(float currentHealth, float MaxHealth)
    {
        return currentHealth / MaxHealth;
    }

    void OnEnable(){
        playerDamagable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable(){
        playerDamagable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnPlayerHealthChanged(int newHealth, int MaxHealth){
        healthSlider.value = CalculateSliderPercentage(newHealth, MaxHealth);
        healthBarText.text = "HP " + newHealth + " / "+ MaxHealth;
        
    }
}
