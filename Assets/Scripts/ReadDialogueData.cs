using UnityEngine;

public static class ReadDialogueData
{
    public static DialogueDatabase LoadData()
    {
        DialogueDatabase dialogueDatabase;

        TextAsset dialogueDatabaseText = Resources.Load<TextAsset>("DialogueDatabase");

        dialogueDatabase = JsonUtility.FromJson<DialogueDatabase>(dialogueDatabaseText.text);
        
        return dialogueDatabase;
    }
}
