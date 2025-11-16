using System;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float startPos, length;
    public GameObject cam;
    public float parallaxEffect; // speed at which bg moves relative to cam

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Calculate distance background moves based on cam movement
        float distance = cam.transform.position.x * parallaxEffect; // 0 = move with cam, 1 = can't move
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        
        // If bg has reached end of its length, adjust its position for infinite scrolling
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
