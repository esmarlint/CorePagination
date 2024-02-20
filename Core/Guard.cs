using System;

namespace Core;

public static class Guard
{
    public static void NotNull<T>(T argument, string paramName) where T : class
    {
        if (argument == null)
        {
            throw new ArgumentNullException(paramName, "Argument cannot be null.");
        }
    }

    public static void NotNegative(int value, string paramName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "Argument cannot be negative.");
        }
    }

    public static void NotNullOrWhiteSpace(string argument, string paramName)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException("Argument cannot be null or whitespace.", paramName);
        }
    }
}