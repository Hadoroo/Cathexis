using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer speechBubble;
    private DialogueDatabase dialogueDatabase;

    private bool canInteract = false;

    [Header("Dialogue Settings")]
    public string npcID; // Unique identifier for this NPC
    public UnityEvent<NPCDialogue> OnPlayerInteract; // Passes this NPC reference

    public List<(int dialogueNum, List<(string speaker, string text)> dialogues)> dialogueDictionary = new List<(int, List<(string, string)>)>();

    void Start()
    {
        speechBubble = GetComponent<SpriteRenderer>();
        speechBubble.enabled = false;

        // If no NPC ID specified, use parent name
        if (string.IsNullOrEmpty(npcID))
        {
            npcID = transform.parent.name;
        }

        LoadDialogues();
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            OnPlayerInteract?.Invoke(this);
        }
    }

    private void LoadDialogues()
    {
        dialogueDatabase = ReadDialogueData.LoadData();
        int dialogueNum = 0;

        foreach (var location in dialogueDatabase.Location)
        {
            if (location.Name == SceneManager.GetActiveScene().name)
            {
                foreach (var content in location.Content)
                {
                    if (content.Initiator == npcID) // Use npcID instead of transform.parent.name
                    {
                        var dialogueList = new List<(string speaker, string text)>();

                        foreach (var dialogue in content.Dialogues)
                        {
                            dialogueList.Add((dialogue.Speaker, dialogue.Text));
                        }

                        dialogueDictionary.Add((dialogueNum, dialogueList));
                        dialogueNum++;
                    }
                }
            }
        }

        Debug.Log($"Loaded {dialogueDictionary.Count} dialogue sets for NPC {npcID}");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.enabled = true;
            player = other.transform;

            // Face player
            float direction = player.position.x > transform.position.x ? 1 : -1;
            transform.parent.localScale = new Vector3(direction,
                                                   transform.parent.localScale.y,
                                                   transform.parent.localScale.z);

            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.enabled = false;
            canInteract = false;
        }
    }
}