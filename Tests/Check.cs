// plain checks for non-test helpers that cannot await TUnit assertions
static class Check
{
    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new($"Expected true: {message}");
        }
    }

    public static void False(bool condition, string message)
    {
        if (condition)
        {
            throw new($"Expected false: {message}");
        }
    }
}
