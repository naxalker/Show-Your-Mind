using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttemptsUI : NetworkBehaviour
{
    [SerializeField] private Image[] _healthImages;

    [Inject]
    private void Construct()
    {

    }

    public void UpdateAttempts(int leftAttemptsAmount)
    {
        for (int i = leftAttemptsAmount; i < _healthImages.Length; i++)
        {
            _healthImages[i].enabled = false;
        }
    }
}
