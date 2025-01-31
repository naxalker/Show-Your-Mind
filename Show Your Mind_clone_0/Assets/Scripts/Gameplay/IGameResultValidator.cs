using System;

public interface IGameResultValidator
{
    event Action<GameOverResultType, ulong> OnGameOver;

    void ProcessGameOver(GameOverResultType resultType, ulong clientId = 0);
}
