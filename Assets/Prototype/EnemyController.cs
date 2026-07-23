using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static CharacterController[] players;
    
    public Rigidbody2D rb;
    public float speed = 1f;
    public int damageOnCollision = 1;
    
    public bool  canMove = true;

    private void Update()
    {
        if(!canMove) return;
        if (players == null || players.Length == 0) return;
        
        CharacterController closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            var direction = (closestPlayer.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            transform.right = direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageOnCollision);
            }
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }
}