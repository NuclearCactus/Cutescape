using UnityEngine;

public class CuteWorldObject : MonoBehaviour
{
    public void SetCuteMode(bool active)
    {
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.enabled = active;

        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = active;
    }
}