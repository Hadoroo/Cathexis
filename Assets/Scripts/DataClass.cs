using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string Queue;
    public string Speaker;
    public string Text;
}

[Serializable]
public class DialogueLocation
{
    public string Location;
    public List<DialogueLine> Dialogue;
}

[Serializable]
public class ActorData
{
    public string Name;
    public List<DialogueLocation> Content;
}

[Serializable]
public class DialogueDatabase
{
    public List<ActorData> Actor;
}