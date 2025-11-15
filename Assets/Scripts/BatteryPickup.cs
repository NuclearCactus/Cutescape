using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public static List<BatteryPickup> allPickups = new List<BatteryPickup>();

    void Awake()
    {
        if (!allPickups.Contains(this))
            allPickups.Add(this);
    }

    void Update()
    {
        transform.position += Vector3.up * Mathf.Sin(Time.time * 5f) * 0.001f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CuteWorldManager.Instance.RefillBattery();
            gameObject.SetActive(false);
        }
    }
}