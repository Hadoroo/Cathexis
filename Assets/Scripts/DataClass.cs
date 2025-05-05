using System.Collections.Generic;

[System.Serializable]
public class DialogueDatabase
{
    public List<Location> Location;
}

[System.Serializable]
public class Location
{
    public string Name;
    public List<Content> Content;
}

[System.Serializable]
public class Content
{
    public string Initiator;
    public List<Dialogues> Dialogues;
}

[System.Serializable]
public class Dialogues
{
    public string Speaker;
    public string Text;
}