using System;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("Horizontal Parallax")]
    public float startPos;
    public float length;
    public GameObject cam;
    public float parallaxEffect; // speed at which bg moves relative to cam
    
    [Header("Vertical Parallax")]
    public float verticalParallaxEffect = 0.5f; // 0 = no vertical movement, 1 = moves with camera
    public float minYPosition = -10f; // Minimum Y position (bottom of game world)
    public float maxYPosition = 50f;  // Maximum Y position (top of game world)
    
    private float startYPos;

    private void Start()
    {
        startPos = transform.position.x;
        startYPos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // HORIZONTAL PARALLAX (existing code)
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        
        // VERTICAL PARALLAX (new)
        float verticalDistance = cam.transform.position.y * verticalParallaxEffect;
        float clampedY = Mathf.Clamp(startYPos + verticalDistance, minYPosition, maxYPosition);
        
        // Apply both horizontal and vertical positioning
        transform.position = new Vector3(startPos + distance, clampedY, transform.position.z);
        
        // Infinite horizontal scrolling (existing code)
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