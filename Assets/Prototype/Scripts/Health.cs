using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    
    public bool invincible = false;
    public float invincibilityOnHitDuration;
    
    public UnityEvent onDeath;
    public bool destroyOnDeath = false;
    public GameObject deathEffectPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(invincible)
            return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (invincibilityOnHitDuration > 0)
            {
                invincible = true;
                Invoke(nameof(ResetInvincibility), invincibilityOnHitDuration);
            }
        }
    }

    private void Die()
    {
        onDeath?.Invoke();
        if(deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        if(destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    private void ResetInvincibility()
    {
        invincible = false;
    }
}