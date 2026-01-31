using UnityEngine;

public static class MaskColor
{
    public static Color GetMaskColor(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red:
                return Color.red;
            case MaskType.Green:
                return Color.green;
            case MaskType.Blue:
                return Color.blue;
            case MaskType.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}
