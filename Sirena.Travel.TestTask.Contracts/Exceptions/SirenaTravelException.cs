namespace Sirena.Travel.TestTask.Contracts.Exceptions;

public class SirenaTravelException : Exception
{
    public SirenaTravelException() : base() { }

    public SirenaTravelException(string message) : base(message) { }
}
