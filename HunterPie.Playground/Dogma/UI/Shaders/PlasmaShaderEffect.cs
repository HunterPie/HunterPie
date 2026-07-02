using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace HunterPie.Playground.Dogma.UI.Shaders;

internal class PlasmaShaderEffect : ShaderEffect
{
    private static readonly PixelShader _pixelShader = new PixelShader
    {
        UriSource = new Uri("pack://application:,,,/HunterPie.Playground;component/Dogma/UI/Shaders/PlasmaShader.ps")
    };

    public PlasmaShaderEffect()
    {
        PixelShader = _pixelShader;
        UpdateShaderValue(InputProperty);
        UpdateShaderValue(TimeProperty);
        UpdateShaderValue(BaseColorProperty);
        UpdateShaderValue(CoreColorProperty);
    }

    public static readonly DependencyProperty InputProperty =
        RegisterPixelShaderSamplerProperty("Input", typeof(PlasmaShaderEffect), 0);

    public Brush Input
    {
        get => (Brush)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register("Time", typeof(double), typeof(PlasmaShaderEffect),
            new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));

    public double Time
    {
        get => (double)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly DependencyProperty BaseColorProperty =
        DependencyProperty.Register("BaseColor", typeof(Color), typeof(PlasmaShaderEffect),
            new UIPropertyMetadata(Color.FromArgb(255, 128, 0, 204), PixelShaderConstantCallback(1)));

    public Color BaseColor
    {
        get => (Color)GetValue(BaseColorProperty);
        set => SetValue(BaseColorProperty, value);
    }

    public static readonly DependencyProperty CoreColorProperty =
        DependencyProperty.Register("CoreColor", typeof(Color), typeof(PlasmaShaderEffect),
            new UIPropertyMetadata(Colors.White, PixelShaderConstantCallback(2)));

    public Color CoreColor
    {
        get => (Color)GetValue(CoreColorProperty);
        set => SetValue(CoreColorProperty, value);
    }
}
