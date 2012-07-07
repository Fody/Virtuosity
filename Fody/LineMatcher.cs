public class LineMatcher
{
    public bool StarStart;
    public string Line;
    public bool StarEnd;

    public bool Match(string typeName)
    {
        if (StarStart && StarEnd)
        {
            return typeName.Contains(Line);
        }
        if (StarStart)
        {
            return typeName.EndsWith(Line);
        }
        if (StarEnd)
        {
            return typeName.StartsWith(Line);
        }
        return typeName == Line;
    }
}