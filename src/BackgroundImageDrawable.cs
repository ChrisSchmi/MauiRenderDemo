using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace MauiRenderDemo
{
    public class BackgroundImageDrawable : IDrawable
    {
        // Die obere Hintergrundfarbe als Konstante
        private static readonly Color TopBackgroundColor = Colors.White;


        private IImage _image;
        private string _imageFile;

        public string ImageFile
        {
            get => _imageFile;
            set
            {
                _imageFile = value;
                _ = LoadImageAsync();
            }
        }

        private async Task LoadImageAsync()
        {
            if (string.IsNullOrEmpty(_imageFile)) return;

            try
            {
                // Lädt die Datei fehlerfrei aus den App-Ressourcen (Ordner: Resources/Raw)
                using var stream = await FileSystem.OpenAppPackageFileAsync(_imageFile);
                _image = PlatformImage.FromStream(stream);
            }
            catch (Exception ex)
            {
                _ = 5;
            }
        }

        public BackgroundImageDrawable()
        {
            
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;

            // 1. Das obere Drittel komplett mit der festen Farbe füllen
            float topHeight = height / 3f;
            canvas.FillColor = TopBackgroundColor;
            canvas.FillRectangle(0, 0, width, topHeight);

            // 2. Das Bild über die gesamte Seite zeichnen
            if (_image != null)
            {
                canvas.DrawImage(_image, 0, 0, width, height);
            }

            // 3. Ein präziser linearer Farbverlauf mit Stopps für einen butterweichen Übergang
            var gradient = new LinearGradientPaint
            {
                GradientStops = new PaintGradientStop[]
                {
            // Oben (0.0): Volle Farbe, null Transparenz
            new PaintGradientStop(0.0f, TopBackgroundColor),
            
            // Am Ende des oberen Drittels (0.33): Immer noch volle Farbe, damit es dort einfarbig bleibt
            new PaintGradientStop(topHeight / height, TopBackgroundColor),
            
            // Ganz unten (1.0): Komplett transparent, damit das Bild fließend durchkommt
            new PaintGradientStop(1.0f, TopBackgroundColor.WithAlpha(0f))
                },
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            // Den Verlauf über die gesamte Fläche legen
            canvas.SetFillPaint(gradient, dirtyRect);
            canvas.FillRectangle(0, 0, width, height);
        }

        public void DrawOld1(ICanvas canvas, RectF dirtyRect)
        {
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;

            // 1. Das obere Drittel komplett mit der festen Farbe füllen
            float topHeight = height / 3f;
            canvas.FillColor = TopBackgroundColor;
            canvas.FillRectangle(0, 0, width, topHeight);

            // 2. Das Bild über die gesamte restliche Höhe oder den relevanten Bereich zeichnen
            if (_image != null)
            {
                // Das Bild füllt den Bereich vom oberen Drittel bis zum unteren Rand aus
                float imgY = topHeight;
                float imgHeight = height - topHeight;

                canvas.DrawImage(_image, 0, imgY, width, imgHeight);
            }

            // 3. Den weichen, seidenmatten Übergang (Fade) über die *gesamte* Höhe legen
            // Damit es ganz oben deckend ist und nach unten hin weich ausfadet (bzw. das Bild freigibt),
            // ziehen wir den Farbverlauf von 0 bis zur vollen Höhe auf.
            var gradient = new LinearGradientPaint
            {
                // Oben (StartPoint Y=0) ist die Farbe voll da (deckend)
                StartColor = TopBackgroundColor,
                // Unten (EndPoint Y=1) wird sie komplett transparent, damit das Bild voll durchkommt
                EndColor = TopBackgroundColor.WithAlpha(0f),
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            // Den Farbverlauf über die gesamte Zeichenfläche legen
            canvas.SetFillPaint(gradient, dirtyRect);
            canvas.FillRectangle(0, 0, width, height);
        }

        public void DrawOld2(ICanvas canvas, RectF dirtyRect)
        {
            // 1. Oberen Bereich mit der festen Farbe füllen (z.B. das obere Drittel)
            canvas.FillColor = TopBackgroundColor;
            float topHeight = dirtyRect.Height / 3f;
            canvas.FillRectangle(0, 0, dirtyRect.Width, topHeight);

            // 2. Unteren Bereich (unteres Drittel bis mittleres Drittel) für das Bild definieren
            // Das Bild soll vom unteren Drittel (dirtyRect.Height * 2/3) bis hoch in die Mitte (dirtyRect.Height * 1/3) gehen
            float imgY = dirtyRect.Height / 3f; // Startet in der Mitte
            float imgHeight = (dirtyRect.Height * 2 / 3f); // Geht bis zum unteren Rand
            float imgWidth = dirtyRect.Width;

            if (_image != null)
            {
                // Da ein direkter Alpha-Farbverlauf pro Pixel im plattformunabhängigen MAUI Canvas 
                // kompliziert ist, zeichnen wir das Bild und legen einen vertikalen Farbverlauf 
                // (von Transparent nach TopBackgroundColor) darüber, um den Fade-Effekt nach oben zu erzeugen, 
                // oder nutzen ein geschichtetes Zeichnen.

                canvas.DrawImage(_image, 0, imgY, imgWidth, imgHeight);

                // 3. Den "Fade-Effekt" nach oben simulieren:
                // Wir legen einen linearen Farbverlauf über den oberen Teil des Bildes, 
                // der von transparent nach "TopBackgroundColor" übergeht, damit es nahtlos verschmilzt.
                var gradient = new LinearGradientPaint
                {
                    StartColor = TopBackgroundColor.WithAlpha(0f), // Transparent
                    EndColor = TopBackgroundColor,                 // Volle Farbe der oberen Hälfe
                    StartPoint = new Point(0, 0),                  // Unten am Verlauf (nahe Bildmitte)
                    EndPoint = new Point(0, 1)                     // Oben am Verlauf
                };

                // Überlagerungsbereich für den Fade-Übergang im mittleren Drittel
                RectF fadeRect = new RectF(0, imgY, imgWidth, topHeight);
                canvas.SetFillPaint(gradient, fadeRect);
                canvas.FillRectangle(fadeRect);
            }
        }
    }
}
