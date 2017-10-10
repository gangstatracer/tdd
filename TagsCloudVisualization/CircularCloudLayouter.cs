using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;

        private readonly Func<IEnumerable<Point>, IEnumerable<Tuple<Point, Point>>> getHull = vertices => ConvexHull.GetSides(vertices);

        private readonly Func<Point, Point, Point, int> getArea = (a, b, c) => Math.Abs((a.X - c.X) * (b.Y - c.Y) - (b.X - c.X) * (a.Y - c.Y));

        public IList<Rectangle> Rectangles { get; } = new List<Rectangle>();
        

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectange(Size rectangleSize)
        {
            Rectangle rectangle = new Rectangle();
            if (!Rectangles.Any())
            {
                rectangle = new Rectangle(center, rectangleSize);
                rectangle.Offset(-1 * rectangle.Width / 2, -1 * rectangle.Height / 2);
            }
            else
            {
                var hullWithDistances = getHull(Rectangles.SelectMany(r => r.GetVertices()))
                    .Select(side => Tuple.Create(side, getArea(side.Item1, side.Item2, center))).ToList();
                var closestSide = hullWithDistances.First(h => h.Item2 == hullWithDistances.Min(hh => hh.Item2)).Item1;
                rectangle = ClingToSegment(closestSide.Item1, closestSide.Item2, rectangleSize);
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle ClingToSegment(Point a, Point b, Size rectangleSize)
        {
            Point location;
            if (a.X == b.X)
            {
                location = new Point(a.X, Math.Abs(a.Y - b.Y) / 2 + Math.Min(a.Y, b.Y) + rectangleSize.Height / 2);
                if (a.X < center.X)
                {
                    location.X -= rectangleSize.Width;
                }
            }
            else
            {
                if (a.Y == b.Y)
                {
                    location = new Point(Math.Abs(a.X - b.X) / 2 + Math.Min(a.X, b.X) - rectangleSize.Width / 2, a.Y);
                    if (a.Y < center.Y)
                    {
                        location.Y -= rectangleSize.Height;
                    }
                }
                else
                {
                    if (b.X < a.X)
                    {
                        var s = a;
                        a = b;
                        b = s;
                    }
                    var lineVector = new Point(b.X - a.X, b.Y - a.Y);
                    var centerVector = new Point(center.X - a.X, center.Y - a.Y);
                    var pseudoscalar = (lineVector.X * centerVector.Y) - (lineVector.Y * centerVector.X);
                    location = new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
                }
            }
            return new Rectangle(location, rectangleSize);
        }        
    }
}
