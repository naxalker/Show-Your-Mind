using System;
using UnityEngine;

public class SudokuGameManager : GameManager, ISudokuModeHandler, IAttemptsCounter
{
    public event Action<uint> OnAttemptSpent;

    private const uint MaxAttempts = 3;

    [SerializeField] private Field _fieldPrefab;
    [SerializeField] private Transform _topUIGroup;
    [SerializeField] private Transform _bottomUIGroup;

    private Field _field;
    private SudokuDifficultyConfig _config;

    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;
    private uint _leftAttempts;

    public override void Initialize(DifficultyConfig config, GameHUD gameHUD)
    {
        base.Initialize(config, gameHUD);

        _config = base.Config as SudokuDifficultyConfig;
    }

    public new SudokuDifficultyConfig Config => _config;

    public bool IsEraseMode
    {
        get { return _isEraseMode; }
        set { _isEraseMode = value; }
    }

    public bool IsNotesMode
    {
        get { return _isNotesMode; }
        set { _isNotesMode = value; }
    }

    public int SelectedDigit
    {
        get { return _selectedDigit; }
        set { _selectedDigit = value; }
    }

    protected override void Start()
    {
        base.Start();

        _leftAttempts = MaxAttempts;

        SpawnField();
        SpawnUI();

        _field.OnFieldCompleted += FieldCompletedHandler;
        _field.OnIncorrectPlaced += IncorrectPlacedHandler;
    }

    private void OnDestroy()
    {
        _field.OnFieldCompleted -= FieldCompletedHandler;
        _field.OnIncorrectPlaced -= IncorrectPlacedHandler;
    }

    private void SpawnField()
    {
        _field = Container.InstantiatePrefabForComponent<Field>(_fieldPrefab);
    }

    private void SpawnUI()
    {
        var bottomUIGroupInstance = Container.InstantiatePrefab(_bottomUIGroup, GameHUD.transform);
        bottomUIGroupInstance.transform.SetAsFirstSibling();

        var topUIGroupInstance = Container.InstantiatePrefab(_topUIGroup, GameHUD.transform);
        topUIGroupInstance.transform.SetAsFirstSibling();
    }

    private void FieldCompletedHandler()
    {
        ProcessGameOver(GameOverResultType.UserWon);
    }

    private void IncorrectPlacedHandler()
    {
        _leftAttempts--;

        OnAttemptSpent?.Invoke(_leftAttempts);

        if (_leftAttempts == 0)
        {
            ProcessGameOver(GameOverResultType.UserLost);
        }
    }
}
