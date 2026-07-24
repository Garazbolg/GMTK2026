using System;
using UnityEngine;

public class AfterSecondsBecomes : MonoBehaviour
{
    public float secondsBecomes;
    public GameObject Prefab;
    public TMPro.TextMeshProUGUI text;

    public float timeStart;
    
    private void Start()
    {
        timeStart = Time.time;
    }

    private void Update()
    {
        if(text != null)
            text.text = (secondsBecomes - (Time.time - timeStart)).ToString("F1");
        
        if(Time.time - timeStart > secondsBecomes)
        {
            if(Prefab != null)
                Instantiate(Prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}