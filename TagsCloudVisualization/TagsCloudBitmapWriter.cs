using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudBitmapWriter
    {
        public static Bitmap Write(IList<Rectangle> rectangles)
        {
            if (!rectangles.Any())
            {
                throw new ArgumentException("rectangles list is empty");
            }
            var bottom = rectangles.Select(r => r.Bottom).Max();
            var right = rectangles.Select(r => r.Right).Max();
            var bitmap = new Bitmap(right, bottom);
            var drawing = Graphics.FromImage(bitmap);
            drawing.FillRectangle(Brushes.White, new Rectangle(0,0, bitmap.Width, bitmap.Height));

            foreach (var rectangle in rectangles)
            {
                drawing.FillRectangle(Brushes.Blue, rectangle);
            }

            return bitmap;
        }
    }
}
