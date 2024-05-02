using System;
using UnityEngine;
using Zenject;

public class SudokuGameManager : GameManager, ISudokuModeHandler
{
    public event Action<uint> OnAttemptSpent; 

    private const uint MaxAttempts = 3;

    [SerializeField] private Field _fieldPrefab;
    [SerializeField] private Transform _topUIGroup;
    [SerializeField] private Transform _bottomUIGroup;

    private Field _field;
    private SudokuConfig _config;

    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;
    private uint _leftAttempts;

    public override void Initialize(GameConfig config)
    {
        base.Initialize(config);

        Container.Bind<ISudokuModeHandler>().FromInstance(this);

        _config = config as SudokuConfig;
        Container.BindInstance(this);
    }

    public SudokuConfig Config => _config;

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
        var canvas = FindObjectOfType<GameHUD>();

        var topUIGroupInstance = Instantiate(_topUIGroup, canvas.transform);
        Container.InjectGameObject(topUIGroupInstance.gameObject);

        Container.InstantiatePrefab(_bottomUIGroup, canvas.transform);
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
