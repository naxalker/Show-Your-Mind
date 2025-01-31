using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AllGameConfigs : ScriptableObject
{
    [field: SerializeField] public List<GameConfig> GameConfigs { get; private set; }
}
