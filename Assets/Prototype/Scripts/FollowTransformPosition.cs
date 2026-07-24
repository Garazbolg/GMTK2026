using UnityEngine;

public class FollowTransformPosition : MonoBehaviour
{
    public Transform target;
    
    public void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
        }
    }
}