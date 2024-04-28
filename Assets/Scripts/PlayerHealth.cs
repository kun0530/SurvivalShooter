using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    private Animator playerAnimator;
    private AudioSource playerAudioPlayer;
    public AudioClip playerHurtClip;
    public AudioClip playerDeathClip;
    public PostProcessVolume postProcessVolume;
    public PostProcessProfile normalProfile;
    public PostProcessProfile hurtProfile;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
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
        playerAudioPlayer.PlayOneShot(playerHurtClip);
        healthSlider.value = currentHealth;
    
        StartCoroutine(CoChangePostProcess(0.5f));
    }

    public override void OnDie()
    {
        base.OnDie();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerShooter>().enabled = false;
        playerAnimator.SetTrigger("Die");
        playerAudioPlayer.PlayOneShot(playerDeathClip);
    }

    private void RestartLevel()
    {
        GameManager.Instance.GameOver();
        Time.timeScale = 0f;
    }

    private IEnumerator CoChangePostProcess(float duration)
    {
        postProcessVolume.profile = hurtProfile;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        postProcessVolume.profile = normalProfile;
    }
}