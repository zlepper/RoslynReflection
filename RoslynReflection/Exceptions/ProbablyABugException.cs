using System;

namespace RoslynReflection.Exceptions
{
    public class ProbablyABugException : Exception
    {
        public ProbablyABugException(string message) : base("This is probably a bug: " + message)
        {
        }
    }
}