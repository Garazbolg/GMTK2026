using UnityEngine;

public class StartAngularVelocity : MonoBehaviour
{
    public float angularVelocity;
    private void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.angularVelocity = angularVelocity;
    }
}