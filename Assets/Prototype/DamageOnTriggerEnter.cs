using System.Collections.Generic;
using UnityEngine;

public class DamageOnTriggerEnter : MonoBehaviour
{
    public int damageAmount = 1;
    private List<Health> healths = new List<Health>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();
        if (health != null && healths.Contains(health) == false)
        {
            healths.Add(health);
            health.TakeDamage(damageAmount);
        }
    }
}