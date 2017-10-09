using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtentions
    {
        public static IList<Point> GetVertices(this Rectangle rectangle)
        {
            return new List<Point>
            {
                rectangle.Location,
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom),
                new Point(rectangle.Left, rectangle.Bottom)
            };
        }

        public static Rectangle FromTriangle(Tuple<Point, Point, Point> vertices)
        {
            var points = new List<Point> { vertices.Item3, vertices.Item2, vertices.Item1 };
            var left = points.Min(p => p.X);
            var right = points.Max(p => p.X);
            var top = points.Min(p => p.Y);
            var bottom = points.Max(p => p.Y);
            return new Rectangle(left, top, right - left, bottom - top);
        }
    }
}