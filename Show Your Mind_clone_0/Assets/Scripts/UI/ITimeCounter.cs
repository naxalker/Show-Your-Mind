using System;

public interface ITimeCounter
{
    public event Action<float> GameTimeChanged;
    public float GameTime { get; }
}
