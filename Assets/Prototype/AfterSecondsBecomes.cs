using System;
using UnityEngine;

public class AfterSecondsBecomes : MonoBehaviour
{
    public float secondsBecomes;
    public GameObject Prefab;

    private float timeStart;
    
    private void Start()
    {
        timeStart = Time.time;
    }

    private void Update()
    {
        if(Time.time - timeStart > secondsBecomes)
        {
            if(Prefab != null)
                Instantiate(Prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}