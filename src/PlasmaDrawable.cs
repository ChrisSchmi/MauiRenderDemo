#region V1
#if V1
//using Microsoft.Maui.Graphics;
//using Microsoft.Maui.Graphics.Platform;
//using System.Diagnostics;

//// Eindeutige MAUI-Typen festlegen
//using MauiColor = Microsoft.Maui.Graphics.Color;
//using MauiICanvas = Microsoft.Maui.Graphics.ICanvas;
//using MauiRectF = Microsoft.Maui.Graphics.RectF;

//namespace MauiRenderDemo;

public class PlasmaDrawable : IDrawable
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    // Auflösung der internen Plasma-Berechnung (niedriger = schneller!)
    private const int Width = 192;
    private const int Height = 192;
    private readonly int[] _pixelBuffer = new int[Width * Height];

    // Wunschfarben konfigurieren
    public MauiColor ColorBg { get; set; } = Colors.DarkSlateBlue;
    public MauiColor Color1 { get; set; } = Colors.DeepPink;
    public MauiColor Color2 { get; set; } = Colors.SpringGreen;
    public MauiColor Color3 { get; set; } = Colors.Gold;

    public void Draw(MauiICanvas canvas, MauiRectF dirtyRect)
    {
        float time = (float)_stopwatch.Elapsed.TotalSeconds * 2.0f;

        // 1. Plasma auf der CPU berechnen (im kleinen Buffer)
        Parallel.For(0, Height, y =>
        {
            float uvY = (float)y / Height;
            for (int x = 0; x < Width; x++)
            {
                float uvX = (float)x / Width;

                // Deine Wellenfunktionen (normalisiert)
                float v1 = MathF.Sin(uvX * 10f + time);
                float v2 = MathF.Cos(uvY * 10f + time);
                float v3 = MathF.Sin(MathF.Sqrt(uvX * uvX + uvY * uvY) * 10f + time);

                float gesamt = (v1 + v2 + v3 + 3.0f) / 6.0f;

                // Farbinterpolation
                MauiColor finalColor;
                if (gesamt < 0.33f)
                    finalColor = LerpColor(ColorBg, Color1, gesamt * 3.0f);
                else if (gesamt < 0.66f)
                    finalColor = LerpColor(Color1, Color2, (gesamt - 0.33f) * 3.0f);
                else
                    finalColor = LerpColor(Color2, Color3, (gesamt - 0.66f) * 3.0f);

                // Pixel im Buffer setzen (ARGB Format)
                int a = (int)(finalColor.Alpha * 255) << 24;
                int r = (int)(finalColor.Red * 255) << 16;
                int g = (int)(finalColor.Green * 255) << 8;
                int b = (int)(finalColor.Blue * 255);

                _pixelBuffer[y * Width + x] = a | r | g | b;
            }
        });

        // 2. Buffer in ein MAUI-Bild konvertieren und hochskalieren
        using var stream = ConvertBufferToPngStream(_pixelBuffer, Width, Height);
        var image = PlatformImage.FromStream(stream);

        if (image != null)
        {
            // MAUI skaliert das Bild automatisch per GPU auf die volle View-Größe!
            canvas.DrawImage(image, 0, 0, dirtyRect.Width, dirtyRect.Height);
        }
    }

    // Hilfsmethode zur linearen Interpolation (Farbmischung)
    private MauiColor LerpColor(MauiColor c1, MauiColor c2, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return MauiColor.FromRgba(
            c1.Red + (c2.Red - c1.Red) * t,
            c1.Green + (c2.Green - c1.Green) * t,
            c1.Blue + (c2.Blue - c1.Blue) * t,
            c1.Alpha + (c2.Alpha - c1.Alpha) * t
        );
    }

    // Erzeugt einen schnellen unkomprimierten Stream (BMP/PNG-artig) für MAUI
    private MemoryStream ConvertBufferToPngStream(int[] buffer, int width, int height)
    {
        var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, System.Text.Encoding.UTF8, true);

        // BMP Header schreiben (schnellste Option für In-Memory Streams)
        bw.Write((ushort)0x4D42); // BM
        bw.Write((uint)(54 + buffer.Length * 4)); // Dateigröße
        bw.Write((ushort)0); bw.Write((ushort)0);
        bw.Write((uint)54); // Pixel Offset

        // DIB Header
        bw.Write((uint)40); // Header Größe
        bw.Write(width);
        bw.Write(-height); // Negativ, damit das Bild richtig herum steht
        bw.Write((ushort)1); // Color planes
        bw.Write((ushort)32); // 32-bit (ARGB)
        bw.Write((uint)0); // Keine Komprimierung
        bw.Write((uint)(buffer.Length * 4)); // Bildgröße
        bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0);

        // Pixel schreiben (von BGRA auf ARGB achten)
        for (int i = 0; i < buffer.Length; i++)
        {
            int pixel = buffer[i];
            byte a = (byte)((pixel >> 24) & 0xFF);
            byte r = (byte)((pixel >> 16) & 0xFF);
            byte g = (byte)((pixel >> 8) & 0xFF);
            byte b = (byte)(pixel & 0xFF);

            bw.Write(b); bw.Write(g); bw.Write(r); bw.Write(a);
        }

        ms.Position = 0;
        return ms;
    }
}

#endif
#endregion

#region V2
#if V2
using Microsoft.Maui.Graphics;
using System.Diagnostics;

// Eindeutige MAUI-Typen festlegen
using MauiColor = Microsoft.Maui.Graphics.Color;
using MauiICanvas = Microsoft.Maui.Graphics.ICanvas;
using MauiRectF = Microsoft.Maui.Graphics.RectF;

namespace MauiRenderDemo;

public class PlasmaDrawable : IDrawable
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    // Höhere Auflösung für direktes Zeichnen (Pixel-Größe wird dynamisch angepasst)
    private const int Width = 120;
    private const int Height = 120;

    // Wunschfarben konfigurieren
    public MauiColor ColorBg { get; set; } = Colors.DarkSlateBlue;
    public MauiColor Color1 { get; set; } = Colors.DeepPink;
    public MauiColor Color2 { get; set; } = Colors.SpringGreen;
    public MauiColor Color3 { get; set; } = Colors.Gold;

    public void Draw(MauiICanvas canvas, MauiRectF dirtyRect)
    {
        float time = (float)_stopwatch.Elapsed.TotalSeconds * 2.0f;

        // Berechne, wie groß ein einzelner Plasma-Pixel auf dem echten Bildschirm sein muss
        float pixelWidth = dirtyRect.Width / Width;
        float pixelHeight = dirtyRect.Height / Height;

        // Wir zeichnen die Zeilen und Spalten direkt auf das Canvas
        for (int y = 0; y < Height; y++)
        {
            float uvY = (float)y / Height;
            for (int x = 0; x < Width; x++)
            {
                float uvX = (float)x / Width;

                // Deine Wellenfunktionen
                float v1 = MathF.Sin(uvX * 10f + time);
                float v2 = MathF.Cos(uvY * 10f + time);
                float v3 = MathF.Sin(MathF.Sqrt(uvX * uvX + uvY * uvY) * 10f + time);

                float gesamt = (v1 + v2 + v3 + 3.0f) / 6.0f;

                // Farbinterpolation
                MauiColor finalColor;
                if (gesamt < 0.33f)
                    finalColor = LerpColor(ColorBg, Color1, gesamt * 3.0f);
                else if (gesamt < 0.66f)
                    finalColor = LerpColor(Color1, Color2, (gesamt - 0.33f) * 3.0f);
                else
                    finalColor = LerpColor(Color2, Color3, (gesamt - 0.66f) * 3.0f);

                // Farbe für das Canvas setzen
                canvas.FillColor = finalColor;

                // Den skalierten "Pixel" direkt an die richtige Position zeichnen
                canvas.FillRectangle(x * pixelWidth, y * pixelHeight, pixelWidth + 0.5f, pixelHeight + 0.5f);
            }
        }
    }

    // Hilfsmethode zur linearen Interpolation (Farbmischung)
    private MauiColor LerpColor(MauiColor c1, MauiColor c2, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return MauiColor.FromRgba(
            c1.Red + (c2.Red - c1.Red) * t,
            c1.Green + (c2.Green - c1.Green) * t,
            c1.Blue + (c2.Blue - c1.Blue) * t,
            c1.Alpha + (c2.Alpha - c1.Alpha) * t
        );
    }
}
#endif
#endregion

using Microsoft.Maui.Graphics;
using System.Diagnostics;

// Unmissverständliche MAUI-Typdefinitionen
using MauiColor = Microsoft.Maui.Graphics.Color;
using MauiICanvas = Microsoft.Maui.Graphics.ICanvas;
using MauiRectF = Microsoft.Maui.Graphics.RectF;

namespace MauiRenderDemo;

public class PlasmaDrawable : IDrawable
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    // Für perfekte Performance und flüssige Animationen:
    // Wir reduzieren die Berechnungsauflösung etwas. 
    // Dank Anti-Aliasing sieht das Plasma auf dem Display trotzdem super weich aus!
    private const int Width = 40;
    private const int Height = 40;

    // Wunschfarben konfigurieren
    public MauiColor ColorBg { get; set; } = Color.FromRgba(255, 255, 255, 128);
    public MauiColor Color1 { get; set; } = Color.FromRgba(205, 205, 205, 255);
    public MauiColor Color2 { get; set; } = Color.FromRgba(180, 180, 180, 255);
    public MauiColor Color3 { get; set; } = Color.FromRgba(230, 230, 230, 255);

    // Diese Zeile ganz oben in der Klasse (nicht in der Draw-Methode) als Feld anlegen:
    private readonly MauiColor[] _colorBuffer = new MauiColor[Width * Height];

    public void Draw(MauiICanvas canvas, MauiRectF dirtyRect)
    {
        // Zeitfaktor für die Geschwindigkeit der Animation
        float time = (float)_stopwatch.Elapsed.TotalSeconds * 0.75f;

        // 1. SCHRITT: Die komplexe Mathematik parallel berechnen (Thread-sicher!)
        Parallel.For(0, Height, y =>
        {
            float uvY = (float)y / Height;
            int rowIndex = y * Width;

            for (int x = 0; x < Width; x++)
            {
                float uvX = (float)x / Width;

                // Deine Wellenfunktionen
                float v1 = MathF.Sin(uvX * 14f + time / 4.0f);
                float v2 = MathF.Cos(uvY * 16f + time / 2.5f);
                float v3 = MathF.Sin(MathF.Sqrt(uvX * uvX + uvY * uvY) * 8.3f + time);

                float gesamt = (v1 + v2 + v3 + 3.0f) / 6.0f;

                // Farbe berechnen und im Buffer speichern
                if (gesamt < 0.33f)
                    _colorBuffer[rowIndex + x] = LerpColor(ColorBg, Color1, gesamt * 3.0f);
                else if (gesamt < 0.66f)
                    _colorBuffer[rowIndex + x] = LerpColor(Color1, Color2, (gesamt - 0.33f) * 3.0f);
                else
                    _colorBuffer[rowIndex + x] = LerpColor(Color2, Color3, (gesamt - 0.66f) * 3.0f);
            }
        });

        // 2. SCHRITT: Sequentiell zeichnen

        float factor = dirtyRect.Height / dirtyRect.Width;

        // Berechne, wie groß ein einzelner Plasma-Block auf dem echten Bildschirm sein muss
        float blockWidth = factor * dirtyRect.Width / Width;
        float blockHeight = dirtyRect.Height / Height;

        canvas.Antialias = true;

        for (int y = 0; y < Height; y++)
        {
            float posY = y * blockHeight;
            int rowIndex = y * Width;

            for (int x = 0; x < Width; x++)
            {
                // Die fertige Farbe blitzschnell aus dem RAM lesen
                canvas.FillColor = _colorBuffer[rowIndex + x];
                canvas.FillRectangle(x * blockWidth, posY, blockWidth + 1.0f, blockHeight + 1.0f);
            }
        }
    }



    public void DrawOld(MauiICanvas canvas, MauiRectF dirtyRect)
    {
        // Zeitfaktor für die Geschwindigkeit der Animation
        float time = (float)_stopwatch.Elapsed.TotalSeconds * 0.75f;

        float factor = dirtyRect.Height / dirtyRect.Width;

        // Berechne, wie groß ein einzelner Plasma-Block auf dem echten Bildschirm sein muss
        float blockWidth = factor * dirtyRect.Width / Width; 
        float blockHeight = dirtyRect.Height / Height;

        // WICHTIG: Schaltet Kantenglättung für Performance bei Blöcken aus
        canvas.Antialias = true;
        

        for (int y = 0; y < Height; y++)
        {
            float uvY = (float)y / Height;
            float posY = y * blockHeight;

            for (int x = 0; x < Width; x++)
            {
                float uvX = (float)x / Width;
                float posX = x * blockWidth;

                // Deine Wellenfunktionen
                float v1 = MathF.Sin(uvX * 14f + time / 4.0f);
                float v2 = MathF.Cos(uvY * 16f + time / 2.5f);
                float v3 = MathF.Sin(MathF.Sqrt(uvX * uvX + uvY * uvY) * 8.3f + time);

                float gesamt = (v1 + v2 + v3 + 3.0f) / 6.0f;

                // Farbinterpolation
                MauiColor finalColor;
                if (gesamt < 0.33f)
                    finalColor = LerpColor(ColorBg, Color1, gesamt * 3.0f);
                else if (gesamt < 0.66f)
                    finalColor = LerpColor(Color1, Color2, (gesamt - 0.33f) * 3.0f);
                else
                    finalColor = LerpColor(Color2, Color3, (gesamt - 0.66f) * 3.0f);

                // Direktes Setzen der matten Farbe ohne Speicher-Allocation
                canvas.FillColor = finalColor;

                // Block zeichnen. Die minimalen Aufrundungen (+0.1f) verhindern sichtbare Ritzen zwischen den Blöcken
                canvas.FillRectangle(posX, posY, blockWidth + 0.1f, blockHeight + 0.1f);
            }
        }
    }

    private MauiColor LerpColor(MauiColor c1, MauiColor c2, float t)
    {
        t = Math.Clamp(t, 0f, 1f);
        return MauiColor.FromRgba(
            c1.Red + (c2.Red - c1.Red) * t,
            c1.Green + (c2.Green - c1.Green) * t,
            c1.Blue + (c2.Blue - c1.Blue) * t,
            c1.Alpha + (c2.Alpha - c1.Alpha) * t
        );
    }
}

