using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Needed for IEnumerator

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public PlayerBehavior playerController;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;
    public Image charImage;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f; // Time between characters
    public bool skipTyping = false; // Allow players to skip animation

    public NPCDialogue currentNPC;
    private int currentDialogueSet = 0;
    private int currentLine = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

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
        dialoguePanel.SetActive(false);
        continueButton.onClick.AddListener(() =>
        {
            if (isTyping) skipTyping = true;
            else ContinueDialogue();
        });
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
        playerController.SetDialogueState(true);
    }

    private void DisplayCurrentLine()
    {
        var currentDialogue = currentNPC.dialogueDictionary[currentDialogueSet];
        var line = currentDialogue.dialogues[currentLine];

        speakerText.text = line.speaker;
        charImage.sprite = Resources.Load<Sprite>(line.speaker);
        charImage.SetNativeSize();

        // Start typing effect
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;

        foreach (char _ in text.ToCharArray())
        {
            if (skipTyping)
            {
                dialogueText.maxVisibleCharacters = text.Length;
                break;
            }

            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }

        skipTyping = false;
        isTyping = false;
    }

    public void ContinueDialogue()
    {
        var currentDialogue = currentNPC.dialogueDictionary[currentDialogueSet];

        currentLine++;
        if (currentLine >= currentDialogue.dialogues.Count)
        {
            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        playerController.SetDialogueState(false); // Unfreeze player
    }

    public void HandleNPCInteraction(NPCDialogue npc)
    {
        if (currentNPC == null || currentNPC != npc)
        {
            StartDialogue(npc);
        }
        else
        {
            ContinueDialogue();
        }
    }
}