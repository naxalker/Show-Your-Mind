using UnityEngine;

[CreateAssetMenu()]
public class ColorPalette : ScriptableObject
{
    [field: SerializeField] public Color BackgroundColor { get; private set; }
    [field: SerializeField] public Color BackgroundColor_1 { get; private set; }
    [field: SerializeField] public Color MainColor { get; private set; }
    [field: SerializeField] public Color MainColor_1 { get; private set; }
    [field: SerializeField] public Color DarkFontColor { get; private set; }
}
