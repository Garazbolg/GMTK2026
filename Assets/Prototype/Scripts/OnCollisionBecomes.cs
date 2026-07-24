using UnityEngine;

public class OnCollisionBecomes : MonoBehaviour
{
    public float minSecondsBeforeBecomes;
    public GameObject Prefab;

    private float timeStart;
    private void Start()
    {
        timeStart = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(Time.time - timeStart > minSecondsBeforeBecomes)
        {
            if(Prefab != null)
                Instantiate(Prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}