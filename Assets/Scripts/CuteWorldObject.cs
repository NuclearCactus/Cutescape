using System;
using UnityEngine;

public class CuteWorldObject : MonoBehaviour
{
    private void Awake()
    {
        // Disable renderers and colliders on start
        foreach (var rend in GetComponentsInChildren<Renderer>())
        {
            rend.enabled = false;
        }
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }

    public void SetCuteMode(bool active)
    {
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.enabled = active;

        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = active;
    }
}