public interface INetworkGameResultValidator
{
    public void ProcessVictory(ulong clientId);
    public void ProcessDefeat(ulong clientId);
}
