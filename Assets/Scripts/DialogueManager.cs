using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    [Header("UI References")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueTextMesh;
    
    private RectTransform dialogueRect;
    private Camera mainCamera;
    private Coroutine hideCoroutine;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (dialogueBox != null)
        {
            dialogueRect = dialogueBox.GetComponent<RectTransform>();
            dialogueBox.SetActive(false);
        }
    }
    
    public void ShowDialogue(string text, Vector3 worldPosition, float duration)
    {
        if (dialogueBox == null || dialogueTextMesh == null) return;
        
        // Stop any existing hide coroutine
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        
        // Set text and position
        dialogueTextMesh.text = text;
        Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPosition);
        dialogueRect.position = screenPos;
        
        // Show dialogue box
        dialogueBox.SetActive(true);
        
        // Hide after duration
        hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }
    
    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
        
        hideCoroutine = null;
    }
}