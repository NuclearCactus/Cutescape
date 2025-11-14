using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint current; // keeps track of active checkpoint

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            current = this;
            // optional: add visual feedback
        }
    }
}