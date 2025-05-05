using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class NPCDialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform player;
    private SpriteRenderer speechBubble;

    private DialogueDatabase dialogueDatabase;

    public Dialogue dialogueSO;

    public Dictionary<string, List<(string Speaker, string Text)>> dialogueDictionary = new Dictionary<string, List<(string, string)>>();

    void Start()
    {
        speechBubble = GetComponent<SpriteRenderer>();
        speechBubble.enabled = false;

        dialogueDatabase = ReadDialogueData.LoadData();

        string soPath = "ScriptableObjects/" + transform.parent.name;
        dialogueSO = Resources.Load<Dialogue>(soPath);

        foreach (var actors in dialogueDatabase.Actor)
        {
            if (actors.Name == transform.parent.name)
            {
                foreach (var content in actors.Content)
                {
                    if (content.Location == SceneManager.GetActiveScene().name)
                    {
                        foreach (var dialogues in content.Dialogue)
                        {
                            dialogueDictionary[dialogues.Queue].Add(dialogues.Speaker, dialogues.Text);

                        }
                    }
                }
            }
        }

        string queue = "0";
        

        if (dialogueDictionary.TryGetValue(queue, out var speakerDict))
        {
            Debug.Log(speakerDict.value());
            // if (speakerDict.TryGetValue(speaker, out string text))
            // {
            //     Debug.Log(text); // Output: The dialogue text
            // }
            // else
            // {
            //     Debug.LogWarning($"Speaker '{speaker}' not found in queue '{queue}'.");
            // }
        }
        else
        {
            Debug.LogWarning($"Queue '{queue}' not found.");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            speechBubble.enabled = true;

            player = other.gameObject.GetComponent<Transform>();

            float direction = player.position.x > transform.position.x ? 1 : -1;
            transform.parent.localScale = new Vector3(direction,
                                                   transform.parent.localScale.y,
                                                   transform.parent.localScale.z);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            speechBubble.enabled = false;
        }
    }
}
