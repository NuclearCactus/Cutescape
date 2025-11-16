using System;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            Debug.Log("Player triggered");
    }
}
