using UnityEngine;

public static class MaskColor
{
    public static Color Red;
    public static Color Blue;
    public static Color Green;
    public static Color Yellow;
    
    public static Color GetMaskColor(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red:
                return Red;
            case MaskType.Green:
                return Green;
            case MaskType.Blue:
                return Blue;
            case MaskType.Yellow:
                return Yellow;
            default:
                return Color.white;
        }
    }
}
