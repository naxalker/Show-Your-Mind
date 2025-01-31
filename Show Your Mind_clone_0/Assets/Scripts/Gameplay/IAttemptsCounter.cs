using System;

public interface IAttemptsCounter
{
    public event Action<uint> OnAttemptSpent;
}
