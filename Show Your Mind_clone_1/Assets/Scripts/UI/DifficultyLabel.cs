using TMPro;
using UnityEngine;
using Zenject;

public class DifficultyLabel : MonoBehaviour
{
    private IDifficultyProvider _difficultyProvider;
    private TMP_Text _difficultyLabel;

    [Inject]
    private void Construct(IDifficultyProvider difficultyProvider)
    {
        _difficultyProvider = difficultyProvider;
    }

    private void Start()
    {
        _difficultyLabel = GetComponent<TMP_Text>();

        switch (_difficultyProvider.DifficultyType)
        {
            case DifficultyType.Easy:
                _difficultyLabel.text = "�������:\n˸����";
                break;

            case DifficultyType.Normal:
                _difficultyLabel.text = "�������:\n����������";
                break;

            case DifficultyType.Hard:
                _difficultyLabel.text = "�������:\n�������";
                break;
        }
    }
}
