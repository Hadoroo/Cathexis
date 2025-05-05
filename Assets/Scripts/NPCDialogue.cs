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

    public List<(int dialogueNum, List<(string speaker, string text)> dialogues)> dialogueDictionary = new List<(int, List<(string, string)>)>();

    void Start()
    {
        speechBubble = GetComponent<SpriteRenderer>();
        speechBubble.enabled = false;

        dialogueDatabase = ReadDialogueData.LoadData();

        string soPath = "ScriptableObjects/" + transform.parent.name;
        dialogueSO = Resources.Load<Dialogue>(soPath);

        int dialogueNum = 0;

        foreach (var location in dialogueDatabase.Location)
        {
            if (location.Name == SceneManager.GetActiveScene().name)
            {
                foreach (var content in location.Content)
                {
                    if (content.Initiator == transform.parent.name)
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

        string queue = "0";

        foreach (var dialoguePair in dialogueDictionary)
        {
            foreach (var line in dialoguePair.dialogues)  // dialogues is the List<(string, string)>
            {
                Debug.Log($"{line.speaker}: {line.text}");  // Access tuple elements correctly
            }   

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
