using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = startHealth;
        healthSlider.value = currentHealth;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) 
    {
        if (isDead)
            return;

        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = currentHealth;
    }

    public override void OnDie()
    {
        base.OnDie();
        GetComponent<PlayerMovement>().enabled = false;
        playerAnimator.SetTrigger("Die");
    }

    private void RestartLevel()
    {
        GameManager.Instance.GameOver();
        Time.timeScale = 0f;
    }
}