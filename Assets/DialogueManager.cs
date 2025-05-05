using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    
    private NPCDialogue currentNPC;
    private int currentDialogueSet = 0;
    private int currentLine = 0;
    
    void Start()
    {
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(ContinueDialogue);
    }
    
    public void StartDialogue(NPCDialogue npc)
    {
        currentNPC = npc;
        currentDialogueSet = 0;
        currentLine = 0;
        
        if (currentNPC.dialogueDictionary.Count == 0)
        {
            Debug.LogWarning($"No dialogues found for NPC {currentNPC.npcID}");
            return;
        }
        
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }
    
    private void DisplayCurrentLine()
    {
        var currentDialogue = currentNPC.dialogueDictionary[currentDialogueSet];
        var line = currentDialogue.dialogues[currentLine];
        
        speakerText.text = line.speaker;
        dialogueText.text = line.text;
    }
    
    public void ContinueDialogue()
    {
        var currentDialogue = currentNPC.dialogueDictionary[currentDialogueSet];
        
        currentLine++;
        if (currentLine >= currentDialogue.dialogues.Count)
        {
            // End of this dialogue set
            dialoguePanel.SetActive(false);
            
            // If you want to cycle through multiple dialogue sets:
            // currentDialogueSet = (currentDialogueSet + 1) % currentNPC.dialogueDictionary.Count;
            return;
        }
        
        DisplayCurrentLine();
    }
    
    // Call this from your NPC's OnPlayerInteract event
    public void HandleNPCInteraction(NPCDialogue npc)
    {
        if (currentNPC == null || currentNPC != npc)
        {
            // New NPC interaction
            StartDialogue(npc);
        }
        else
        {
            // Continuing with same NPC
            ContinueDialogue();
        }
    }
}