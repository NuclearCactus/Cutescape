using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SFXManager.Instance.PlaySFX(SFXManager.Instance.fallDeathSound);
            collision.GetComponent<PlayerController>().Respawn();
        }
    }
}