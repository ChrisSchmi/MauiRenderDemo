using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MauiRenderDemo.Controls;

public partial class IconTileView : ContentView
{
    public IconTileView()
    {
        InitializeComponent();
    }

    // ---------------------------------------------------------------
    // Form / Geometrie
    // ---------------------------------------------------------------

    public static readonly BindableProperty ShapeTypeProperty =
        BindableProperty.Create(
            nameof(ShapeType),
            typeof(TileShapeType),
            typeof(IconTileView),
            TileShapeType.Circle,
            propertyChanged: OnShapeGeometryChanged);

    /// <summary>Kreis, Rechteck oder Rechteck mit CornerRadius.</summary>
    public TileShapeType ShapeType
    {
        get => (TileShapeType)GetValue(ShapeTypeProperty);
        set => SetValue(ShapeTypeProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(IconTileView),
            12.0,
            propertyChanged: OnShapeGeometryChanged);

    /// <summary>Nur relevant, wenn ShapeType = RoundedRectangle.</summary>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly BindableProperty ShapeSizeProperty =
        BindableProperty.Create(
            nameof(ShapeSize),
            typeof(double),
            typeof(IconTileView),
            64.0,
            propertyChanged: OnShapeGeometryChanged);

    /// <summary>Breite = Höhe der Form in Device-Independent-Pixeln.</summary>
    public double ShapeSize
    {
        get => (double)GetValue(ShapeSizeProperty);
        set => SetValue(ShapeSizeProperty, value);
    }




    public static readonly BindableProperty ClickColorProperty =
    BindableProperty.Create(
        nameof(ClickColor),
        typeof(Color),
        typeof(IconTileView),
        Color.FromHex("#10000000"));

    /// <summary>Füllfarbe der Form - per XAML frei konfigurierbar.</summary>
    public Color ClickColor
    {
        get => (Color)GetValue(ClickColorProperty);
        set => SetValue(ClickColorProperty, value);
    }



    public static readonly BindableProperty ShapeColorProperty =
        BindableProperty.Create(
            nameof(ShapeColor),
            typeof(Color),
            typeof(IconTileView),
            Colors.RoyalBlue);

    /// <summary>Füllfarbe der Form - per XAML frei konfigurierbar.</summary>
    public Color ShapeColor
    {
        get => (Color)GetValue(ShapeColorProperty);
        set => SetValue(ShapeColorProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(IconTileView),
            Colors.Transparent);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BorderThicknessProperty =
        BindableProperty.Create(
            nameof(BorderThickness),
            typeof(double),
            typeof(IconTileView),
            0.0);

    public double BorderThickness
    {
        get => (double)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly BindableProperty HasShadowProperty =
        BindableProperty.Create(
            nameof(HasShadow),
            typeof(bool),
            typeof(IconTileView),
            true,
            propertyChanged: OnHasShadowChanged);

    public bool HasShadow
    {
        get => (bool)GetValue(HasShadowProperty);
        set => SetValue(HasShadowProperty, value);
    }

    // ---------------------------------------------------------------
    // Icon (Font-Awesome-Glyph ODER Unicode-Emoji - beides ist Text)
    // ---------------------------------------------------------------

    public static readonly BindableProperty IconGlyphProperty =
        BindableProperty.Create(
            nameof(IconGlyph),
            typeof(string),
            typeof(IconTileView),
            string.Empty);

    /// <summary>
    /// Der Icon-Inhalt: z.B. eine Font-Awesome-Unicode-Glyphe ("\uf015")
    /// oder direkt ein Emoji ("📷"). Wird als reiner Text gerendert.
    /// </summary>
    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public static readonly BindableProperty IconFontFamilyProperty =
        BindableProperty.Create(
            nameof(IconFontFamily),
            typeof(string),
            typeof(IconTileView),
            defaultValue: null);

    /// <summary>
    /// Font-Family für IconGlyph, z.B. "FASolid" (muss vorher in
    /// MauiProgram.cs via ConfigureFonts registriert werden).
    /// Für Emojis kann dies leer bleiben (Standard-System-Font
    /// rendert Emojis bereits korrekt).
    /// </summary>
    public string IconFontFamily
    {
        get => (string)GetValue(IconFontFamilyProperty);
        set => SetValue(IconFontFamilyProperty, value);
    }

    public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(
            nameof(IconColor),
            typeof(Color),
            typeof(IconTileView),
            Colors.White);

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public static readonly BindableProperty IconFontSizeProperty =
        BindableProperty.Create(
            nameof(IconFontSize),
            typeof(double),
            typeof(IconTileView),
            28.0);

    public double IconFontSize
    {
        get => (double)GetValue(IconFontSizeProperty);
        set => SetValue(IconFontSizeProperty, value);
    }

    // ---------------------------------------------------------------
    // Text unterhalb der Form (optional)
    // ---------------------------------------------------------------

    public static readonly BindableProperty CaptionTextProperty =
        BindableProperty.Create(
            nameof(CaptionText),
            typeof(string),
            typeof(IconTileView),
            defaultValue: null,
            propertyChanged: OnCaptionTextChanged);

    /// <summary>
    /// Text unterhalb der Form. Wenn null/leer, wird die Zeile
    /// automatisch ausgeblendet und die Komponente zeigt nur die Form.
    /// </summary>
    public string CaptionText
    {
        get => (string)GetValue(CaptionTextProperty);
        set => SetValue(CaptionTextProperty, value);
    }

    /// <summary>Rein lesbare Hilfs-Property fürs Binding der IsVisible-Eigenschaft.</summary>
    public bool HasCaption => !string.IsNullOrWhiteSpace(CaptionText);

    public static readonly BindableProperty CaptionColorProperty =
        BindableProperty.Create(
            nameof(CaptionColor),
            typeof(Color),
            typeof(IconTileView),
            Colors.Black);

    public Color CaptionColor
    {
        get => (Color)GetValue(CaptionColorProperty);
        set => SetValue(CaptionColorProperty, value);
    }

    public static readonly BindableProperty CaptionFontSizeProperty =
        BindableProperty.Create(
            nameof(CaptionFontSize),
            typeof(double),
            typeof(IconTileView),
            13.0);

    public double CaptionFontSize
    {
        get => (double)GetValue(CaptionFontSizeProperty);
        set => SetValue(CaptionFontSizeProperty, value);
    }

    // ---------------------------------------------------------------
    // Interaktion
    // ---------------------------------------------------------------

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(IconTileView));

    /// <summary>
    /// Wird ausgeführt, egal ob auf die Form oder (falls vorhanden)
    /// auf den Text getippt wird.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(IconTileView));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>Alternative zu Command, falls kein MVVM genutzt wird.</summary>
    public event EventHandler? Clicked;

    // ---------------------------------------------------------------
    // Internals
    // ---------------------------------------------------------------

    private static void OnShapeGeometryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IconTileView tile)
        {
            tile.UpdateShapeGeometry();
        }
    }

    private static void OnHasShadowChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IconTileView tile)
        {
            tile.UpdateShadow();
        }
    }

    private static void OnCaptionTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IconTileView tile)
        {
            tile.OnPropertyChanged(nameof(HasCaption));
        }
    }

    private void UpdateShapeGeometry()
    {
        double radius = ShapeType switch
        {
            TileShapeType.Circle => ShapeSize / 2,
            TileShapeType.Rectangle => 0,
            TileShapeType.RoundedRectangle => CornerRadius,
            _ => 0
        };

        ShapeGeometry.CornerRadius = new Microsoft.Maui.CornerRadius(radius);
    }

    private void UpdateShadow()
    {
        ShapeBorder.Shadow = HasShadow ? ShapeShadow : null;
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        UpdateShapeGeometry();
        UpdateShadow();
    }

    private void OnTileTapped(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnTileTapped(object sender, TappedEventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
