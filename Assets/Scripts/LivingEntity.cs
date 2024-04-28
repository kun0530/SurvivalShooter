using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float startHealth = 100f; // 시작 체력
    public float currentHealth { get; private set; } // 현재 체력
    public bool isDead { get; private set; } // 사망 상태
    // public event Action onDeath; 사망시 발동할 이벤트

    // RestoreHealth와 ApplyUpdateHealth는 필요하면 추가

    protected virtual void OnEnable()
    {
        // 리셋
        isDead = false;
        currentHealth = startHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentHealth -= damage;

        if (!isDead && currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        // onDeath 이벤트가 있으면 실행

        isDead = true;
    }
}
