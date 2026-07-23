using UnityEngine;

public class RotatingTransform : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}