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

    [Header("Panel Animation")]
    public float scaleDuration = 0.3f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Coroutine scaleCoroutine;

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
        dialoguePanel.transform.localScale = Vector3.zero;
        
        // Fix: Use the actual method instead of OnContinueClick
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

        // Start scale in animation
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScalePanel(true));
        
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
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScalePanel(false));
        
        playerController.SetDialogueState(false);
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

    IEnumerator ScalePanel(bool show)
    {
        if (show) dialoguePanel.SetActive(true);
        
        float timer = 0f;
        Vector3 startScale = show ? Vector3.zero : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.zero;

        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            float t = scaleCurve.Evaluate(timer / scaleDuration);
            dialoguePanel.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        dialoguePanel.transform.localScale = endScale;
        if (!show) dialoguePanel.SetActive(false);
    }
}