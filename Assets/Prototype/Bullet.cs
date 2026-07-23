using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float knockbackForce;
    public float knockbackDuration;
    public Rigidbody2D rb;

    void Update()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with other objects
        // For example, apply damage to the object hit
        var health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        if(enemy  != null)
        {
            enemy.rb.AddForceAtPosition(rb.linearVelocity.normalized * knockbackForce, collision.contacts[0].point);
            enemy.Stun(knockbackDuration);
        }

        // Destroy the bullet after collision
        Destroy(gameObject);
    }
}