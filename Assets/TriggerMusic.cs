using UnityEngine;

public class TriggerMusic : MonoBehaviour
{

    public bool badEnding = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (badEnding) {
            Debug.Log("Moi");
        }
    }

}
