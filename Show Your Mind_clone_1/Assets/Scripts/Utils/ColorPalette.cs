using UnityEngine;

[CreateAssetMenu()]
public class ColorPalette : ScriptableObject
{
    [field: SerializeField] public Color BackgroundColor;
    [field: SerializeField] public Color BackgroundColor_1;
    [field: SerializeField] public Color MainColor;
    [field: SerializeField] public Color MainColor_1;
}
