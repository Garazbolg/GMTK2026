using UnityEngine;

public class StartVelocity : MonoBehaviour
{
    public Vector2 velocity;

    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.TransformDirection(velocity);
        }
    }
}