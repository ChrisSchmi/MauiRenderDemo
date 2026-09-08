namespace MauiRenderDemo;

/// <summary>
/// Definiert die Form des Hintergrunds der IconTileView.
/// </summary>
public enum TileShapeType
{
    /// <summary>Perfekter Kreis (CornerRadius = ShapeSize / 2).</summary>
    Circle,

    /// <summary>Eckiges Viereck ohne Rundung (CornerRadius = 0).</summary>
    Rectangle,

    /// <summary>Viereck mit konfigurierbarem CornerRadius.</summary>
    RoundedRectangle
}
