using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class ConvexHull
    {
        public static IEnumerable<Tuple<Point, Point>> GetSides(IList<Point> points)
        {
            var unvisitedPoints = points.ToList();
            var start = points.First(p => p.X == points.Min(pp => pp.X));
            unvisitedPoints.Remove(start);
            var previous = start;
            Point next;
            do
            {
                next = unvisitedPoints.First();
                foreach (var point in unvisitedPoints.Skip(1))
                {
                    if (Rotate(previous, next, point) >= 0)
                    {
                        continue;
                    }
                    next = point;
                    break;
                }
                yield return Tuple.Create(previous, next);
                previous = next;
                unvisitedPoints.Remove(next);
            } while (next != start);

        }

        private static int Rotate(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - b.Y) - (b.Y - a.Y) * (c.X - b.X);
        }
    }
}
