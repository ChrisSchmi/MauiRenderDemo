using System;
using System.Collections.Generic;
using System.Text;

// by CS
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace MauiRenderDemo.Renderers
{
    public sealed class FadingColorBackgroundDrawable : BindableObject, IDrawable
    {
        public List<Color> ColorsList { get; set; }
        public FadingColorBackgroundDrawable()
        {
            ColorsList = new();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (ColorsList == null || ColorsList.Count == 0) return;

            // Fall 1: Nur eine Farbe -> flächig füllen
            if (ColorsList.Count == 1)
            {
                canvas.FillColor = ColorsList[0];
                canvas.FillRectangle(dirtyRect);
                return;
            }

            // Fall 2: Mehrere Farben -> Array für die Farbstopps erstellen
            int count = ColorsList.Count;
            var stops = new PaintGradientStop[count];

            for (int i = 0; i < count; i++)
            {
                float offset = (float)i / (count - 1);
                stops[i] = new PaintGradientStop(offset, ColorsList[i]);
            }

            // LinearGradientPaint mit dem fertigen Array initialisieren
            var gradient = new LinearGradientPaint(stops)
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            canvas.SetFillPaint(gradient, dirtyRect);
            canvas.FillRectangle(dirtyRect);
        }
    }
}
