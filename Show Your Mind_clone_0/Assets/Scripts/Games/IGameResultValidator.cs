public interface IGameResultValidator
{
    public void ProcessVictory(ulong clientId);
    public void ProcessDefeat(ulong clientId);
}
