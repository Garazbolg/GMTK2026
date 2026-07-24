using System;
using UnityEngine;
using UnityEngine.Events;

public class Loot : MonoBehaviour
{
    public UnityEvent<GameObject> onLoot;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onLoot?.Invoke(other.gameObject);
        }
    }
}