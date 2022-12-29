using MothManager.Core;

namespace MothManagerTrayApp.Controls;

public static class ColoUtil
{
    public static Color ToColor(this ColorStruct.HSV hsv)
    {
        ColorStruct.RGBInt rgbInt = hsv;
        return Color.FromArgb(rgbInt.R, rgbInt.G, rgbInt.B);
    }
    
    public static Color ToColor(this ColorStruct.RGBInt rgbInt)
    {
        return Color.FromArgb(rgbInt.R, rgbInt.G, rgbInt.B);
    }
}