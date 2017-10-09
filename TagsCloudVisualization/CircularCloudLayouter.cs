using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
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
        private readonly Func<IList<Rectangle>, Rectangle> getHull = RectangleHullBuilder.GetHull;
        private readonly List<Rectangle> vacantSpaces = new List<Rectangle>();

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
            }
            else
            {
                var vacantSpace = vacantSpaces
                    .Where(vs => vs.Height >= rectangleSize.Height && vs.Width >= rectangleSize.Width)
                    .OrderBy(vs => vs.X * vs.Y).FirstOrDefault();
                if (vacantSpace != Rectangle.Empty)
                {
                    vacantSpaces.RemoveAll(vs => vs.Location == vacantSpace.Location);
                    rectangle = new Rectangle(vacantSpace.Location, rectangleSize);
                }
                else
                {
                    var hull = getHull(Rectangles);
                    rectangle = ClingToRectangleMinimizingDistanceToPoint(hull, center, rectangleSize);
                    vacantSpaces.AddRange(FindVacantSpaces(hull, rectangle));
                }
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private static IEnumerable<Rectangle> FindVacantSpaces(Rectangle hull, Rectangle rectangle)
        {
            var newHull = RectangleHullBuilder.GetHull(new List<Rectangle> {hull, rectangle});
            foreach (var vertex in newHull.GetVertices()
                .Where(v => !hull.Contains(v) && !rectangle.Contains(v)))
            {
                Func<Point, Point, int> distance = (p, k) => (p.X - k.X) * (p.X - k.X) + (p.Y - k.Y) * (p.Y - k.Y);
                var oldHullClosestVertex = hull.GetVertices().OrderBy(v => distance(v, vertex)).First();
                var rectangleTwoClosestVertex = rectangle.GetVertices().OrderBy(v => distance(v, vertex)).First();
                yield return RectangleExtentions.FromTriangle(Tuple.Create(vertex, oldHullClosestVertex, rectangleTwoClosestVertex));
            }
        }

        internal Rectangle ClingToRectangleMinimizingDistanceToPoint(Rectangle rectangleTo, Point point, Size size)
        {
            if (!rectangleTo.Contains(point))
            {
                throw new ArgumentException("rectangleTo should contain provided point");
            }
            var distances = new Dictionary<Side, int>
            {
                {Side.Bottom, point.Y - rectangleTo.Bottom},
                {Side.Top,  point.Y - rectangleTo.Top},
                {Side.Left,  point.X - rectangleTo.Left},
                {Side.Right, point.X - rectangleTo.Right}
            };
            var closestTargetSide = distances.First(d => Math.Abs(d.Value) == distances.Min(dd => Math.Abs(dd.Value))).Key;

            var biggerResultSide = Math.Max(size.Width, size.Height);
            var smallerResultSide = Math.Min(size.Width, size.Height);

            var resultSize = closestTargetSide == Side.Left || closestTargetSide == Side.Right
                ? new Size(smallerResultSide, biggerResultSide)
                : new Size(biggerResultSide, smallerResultSide);

            var resultLocation = new Point(point.X - resultSize.Width / 2, point.Y - resultSize.Height / 2);

            switch (closestTargetSide)
            {
                case Side.Left:
                    resultLocation.X = rectangleTo.Left - size.Width;
                    break;
                case Side.Top:
                    resultLocation.Y = rectangleTo.Top - size.Height;
                    break;
                case Side.Bottom:
                    resultLocation.Y = rectangleTo.Bottom;
                    break;
                case Side.Right:
                    resultLocation.X = rectangleTo.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Rectangle(resultLocation, resultSize);

        }
    }
}
