using System.ComponentModel;
using Cyotek.Windows.Forms;
using MothManager.Core;

namespace MothManagerTrayApp.Controls;

public class TemperatureColorSlider : ColorSlider
{
    #region Constructors

    public TemperatureColorSlider()
    {
        this.BarStyle = ColorBarStyle.Custom;
        this.Minimum = 3200f;
        this.Maximum = 5600f;
    }

    #endregion

    #region Properties
    
    
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ColorBarStyle BarStyle
    {
        get { return base.BarStyle; }
        set { base.BarStyle = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color Color1
    {
        get { return base.Color1; }
        set { base.Color1 = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color Color2
    {
        get { return base.Color2; }
        set { base.Color2 = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color Color3
    {
        get { return base.Color3; }
        set { base.Color3 = value; }
    }

    [DefaultValue(5600f)]
    public override float Maximum
    {
        get { return base.Maximum; }
        set
        {
            base.Maximum = value;
            InitializeColors();
        }
    }

    [DefaultValue(3200f)]
    public override float Minimum
    {
        get { return base.Minimum; }
        set
        {
            base.Minimum = value;
            InitializeColors();
        }
    }

    public override float Value
    {
        get { return base.Value; }
        set { base.Value = (int)value; }
    }

    #endregion
    
    #region Functions

    private void InitializeColors()
    {
        CustomColors = new ColorCollection();
        for (var i = Minimum; i <= Maximum; i += 100f)
        {
            CustomColors.Add(ColorStruct.RGBInt.FromKelvin((int) i).ToColor());
        }
           // this.CustomColors = new ColorCollection(Enumerable.Range(0, 359).Select(h => HslColor.HslToRgb(h, 1, 0.5)));
    }
    
    #endregion
}