using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;

        public IList<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectange(Size rectangleSize)
        {
            var rectangle = new Rectangle(center, rectangleSize);
            rectangle.Offset(-1 * rectangle.Width / 2, -1 * rectangle.Height / 2);
            Rectangles.Add(rectangle);
            return rectangle;
        }
    }
}
