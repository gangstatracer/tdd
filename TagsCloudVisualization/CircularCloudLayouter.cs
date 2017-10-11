using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;

        private readonly Func<IEnumerable<Point>, IEnumerable<Segment>> getHull =
            vertices => ConvexHull.GetSides(vertices);

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
                var hull = getHull(Rectangles.SelectMany(r => r.GetVertices())).ToList();
                var closestSide = hull.First(h => h.DistanceTo(center) == hull.Min(hh => hh.DistanceTo(center)));
                rectangle = ClingToSegment(closestSide, rectangleSize);
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle ClingToSegment(Segment segment, Size rectangleSize)
        {
            Point location;
            if (segment.IsVertical)
            {
                location = new Point(segment.Item1.X, Math.Abs(segment.Item1.Y - segment.Item2.Y) / 2 + Math.Min(segment.Item1.Y, segment.Item2.Y) - rectangleSize.Height / 2);
                if (segment.Item1.X < center.X)
                {
                    location.X -= rectangleSize.Width;
                }
            }
            else
            {
                if (segment.IsHorizontal)
                {
                    location = new Point(Math.Abs(segment.Item1.X - segment.Item2.X) / 2 + Math.Min(segment.Item1.X, segment.Item2.X) - rectangleSize.Width / 2, segment.Item1.Y);
                    if (segment.Item1.Y < center.Y)
                    {
                        location.Y -= rectangleSize.Height;
                    }
                }
                else
                {
                    location = segment.Middle;
                    segment.OrderFromLeftToRight();
                    var lineVector = new Point(segment.Item2.X - segment.Item1.X, segment.Item2.Y - segment.Item1.Y);
                    var centerVector = new Point(center.X - segment.Item1.X, center.Y - segment.Item1.Y);
                    var pseudoscalar = lineVector.X * centerVector.Y - lineVector.Y * centerVector.X;
                    if (pseudoscalar == 0)
                    {
                        throw new Exception($"Hull should not contain center. Segment: ({segment.Item1};{segment.Item2}), center: {center}.");
                    }
                    if (pseudoscalar * (segment.Item2.Y - segment.Item1.Y) < 0)
                    {
                        if (segment.Item2.Y - segment.Item1.Y < 0)
                        {
                            location.X -= rectangleSize.Width;
                            location.Y -= rectangleSize.Height;
                        }
                        else
                        {
                            if (pseudoscalar > 0)
                            {
                                location.X -= rectangleSize.Width;
                            }
                            else
                            {
                                location.Y -= rectangleSize.Height;
                            }
                        }
                    }
                }
            }
            return new Rectangle(location, rectangleSize);
        }        
    }
}
