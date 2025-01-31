using System;

public interface ITimeCounter
{
    public event Action<float> OnGameTimeChanged;
    public float GameTime { get; }
}
