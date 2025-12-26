using UnityEngine;

public class BobbingMotion : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float amplitude = 0.15f;   // How high it moves
    public float frequency = 1.5f;    // How fast it bobs

    private Vector3 startLocalPos;

    public bool active = true;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (!active) return;

        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startLocalPos + Vector3.up * yOffset;
    }

    public void StopBobbing()
    {
        active = false;
        transform.localPosition = startLocalPos;
    }

    
}
