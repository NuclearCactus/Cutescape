using UnityEngine;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public string realWorldText = "Real";
    public string cuteWorldText = "Cute";
    public float dialogueDuration = 2f;

    [Header("References")]
    public GameObject dialoguePrefab; // assign DialogueBubble prefab here
    private GameObject dialogueInstance;

    private CuteWorldManager worldManager; // reference to know current world

    void Start()
    {
        worldManager = CuteWorldManager.Instance;
    }

    public void ShowDialogue()
    {
        if (dialogueInstance != null)
            Destroy(dialogueInstance);

        dialogueInstance = Instantiate(dialoguePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, GameObject.Find("Canvas").transform);

        TextMeshProUGUI textComp = dialogueInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (worldManager != null && worldManager.isCuteMode)
            textComp.text = cuteWorldText;
        else
            textComp.text = realWorldText;

        StartCoroutine(HideDialogueAfterDelay());
    }

    private IEnumerator HideDialogueAfterDelay()
    {
        yield return new WaitForSeconds(dialogueDuration);
        if (dialogueInstance != null)
            Destroy(dialogueInstance);
    }
}