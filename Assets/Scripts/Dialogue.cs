using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{   
    public string sceneName;
    public List<string> dialogueList;
    public int curDialog = 0;
}
