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
                if (GetEmptySpacesInsideHull().Any())
                {
                    rectangle = new Rectangle();
                }
                else
                {
                    rectangle = ClingToRectangleMinimizingDistanceToPoint(getHull(Rectangles), center, rectangleSize);
                }
            }
            Rectangles.Add(rectangle);
            return rectangle;
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

        internal IEnumerable<Rectangle> GetEmptySpacesInsideHull()
        {
            //var hull = getHull(Rectangles);
            //var xGuideLines = new List<int> {hull.Left, hull.Right};
            //xGuideLines.AddRange(Rectangles.SelectMany(r => new []{r.Left, r.Right}));

            //var yGuideLines = new List<int>();
            return new List<Rectangle>();
        }
    }
}
