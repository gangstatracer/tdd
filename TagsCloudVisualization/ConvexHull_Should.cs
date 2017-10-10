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
    }
}