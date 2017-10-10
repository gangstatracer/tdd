using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class ConvexHull_Should
    {
        [Test]
        public void BuildZeroLengthSegment_OnOnePoint()
        {
            var a = new Point(3, 7);
            ConvexHull.GetSides(new[] {a})
                .Should()
                .Equal(Tuple.Create(a, a));
        }
        [Test]
        public void BuildSegment_OnTwoPoints()
        {
            var a = new Point(10, 10);
            var b = new Point(1, 2);
            ConvexHull.GetSides(new[] { a, b})
                .Should()
                .BeEquivalentTo(new List<Tuple<Point, Point>>
                {
                    Tuple.Create(b, a),
                    Tuple.Create(a, b)
                });
        }
        [Test]
        public void BuildTriangle_OnThreePoints()
        {
            var a = new Point(10, 10);
            var b = new Point(1, 2);
            var c = new Point(-4, -7);
            ConvexHull.GetSides(new[] {a, b, c})
                .Should()
                .BeEquivalentTo(new List<Tuple<Point, Point>>
                {
                    Tuple.Create(a, b),
                    Tuple.Create(b, c),
                    Tuple.Create(c, a)
                });
        }

        [Test]
        public void SkipInnerPoint_OnFourPoints()
        {
            var a = new Point(0, 0);
            var b = new Point(0, 4);
            var c = new Point(4, 0);
            var d = new Point(1, 1);
            ConvexHull.GetSides(new[] { a, b, c, d })
                .Should()
                .BeEquivalentTo(new List<Tuple<Point, Point>>
                {
                    Tuple.Create(a, c),
                    Tuple.Create(c, b),
                    Tuple.Create(b, a)
                });
        }
    }
}