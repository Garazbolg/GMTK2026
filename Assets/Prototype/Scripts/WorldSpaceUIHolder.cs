using UnityEngine;

public class WorldSpaceUIHolder : MonoBehaviour
{
    public GameObject WorldSpaceUI;

    private void Start()
    {
        if (WorldSpaceUI != null)
        {
            WorldSpaceUI.transform.SetParent(null);
        }
    }

    private void OnDestroy()
    {
        if (WorldSpaceUI != null)
        {
            Destroy(WorldSpaceUI);
        }
    }
}