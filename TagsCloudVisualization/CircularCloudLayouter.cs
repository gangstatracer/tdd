using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Point> GetVertices(this Rectangle rectangle)
        {
            yield return rectangle.Location;
            yield return new Point(rectangle.Right, rectangle.Top);
            yield return new Point(rectangle.Right, rectangle.Bottom);
            yield return new Point(rectangle.Left, rectangle.Bottom);
        }
    }
    public class CircularCloudLayouter
    {
        private enum Side
        {
            Top,
            Bottom,
            Left,
            Right
        }
        private readonly Point center;
        private readonly List<Tuple<Point, int>> outerPoints = new List<Tuple<Point, int>>();
        private readonly Func<Point, Point, int> distance = (p, k) => (p.X - k.X) * (p.X - k.X) + (p.Y - k.Y) * (p.Y - k.Y);

        public IList<Rectangle> Rectangles { get; } = new List<Rectangle>();
        

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectange(Size rectangleSize)
        {
            Rectangle rectangle;
            if (!Rectangles.Any())
            {
                rectangle = new Rectangle(center, rectangleSize);
                rectangle.Offset(-1 * rectangle.Width / 2, -1 * rectangle.Height / 2);
                outerPoints.AddRange(rectangle.GetVertices().Select(p => Tuple.Create(p, distance(p, center))));
            }
            else
            {
                var closestToCenterOuterPoint = outerPoints.First(point => point.Item2 == outerPoints.Min(op => op.Item2));
                var neighborsOfCloses = outerPoints.OrderBy(op => distance(op.Item1, closestToCenterOuterPoint.Item1))
                    .Take(2);
                var closestHullSide = neighborsOfCloses.Select(n => )
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }        
    }
}
