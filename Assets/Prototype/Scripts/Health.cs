using DevCore.ScriptableVariables;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public ScriptableInt currentHealthVariable;
    public ScriptableInt maxHealthVariable;
    
    public bool invincible = false;
    public float invincibilityOnHitDuration;
    
    public UnityEvent onDeath;
    public bool destroyOnDeath = false;
    public GameObject deathEffectPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        if(currentHealthVariable != null)
        {
            currentHealthVariable.value = currentHealth;
        }
        if(maxHealthVariable != null)
        {
            maxHealthVariable.value = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if(invincible)
            return;
        
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
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
        
        if(currentHealthVariable != null)
        {
            currentHealthVariable.value = currentHealth;
        }
        if(maxHealthVariable != null)
        {
            maxHealthVariable.value = maxHealth;
        }
    }

    public void Despawn()
    {
        // Despawn Effect ?
        Destroy(gameObject);
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