using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class RectangleHullBuilder_Should
    {
        [Test]
        public void ReturnSameRectange_WhenOnePassed()
        {
            var rectangle = new Rectangle(0, 0, 13, 17);
            RectangleHullBuilder.GetHull(new List<Rectangle> {rectangle}).Should().Be(rectangle);
        }

        [Test]
        public void ThrowArgumentException_WhenEmptyListPassed()
        {
            Action getAction = () => RectangleHullBuilder.GetHull(new List<Rectangle>());
            getAction.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ReturnResultContainingAllPassedRectangles()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(0, 0, 10, 11),
                new Rectangle(10, 541, 3, 9),
                new Rectangle(-101, 22, 47, 96)
            };
            RectangleHullBuilder.GetHull(rectangles)
                .Should()
                .Match<Rectangle>(h => rectangles.All(h.Contains));
        }

        [Test]
        public void HullsTopSideOrdinateEqualsHightestRectangleSideOrdinate()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(1, 1, 10, 11),
                new Rectangle(10, 541, 3, 9)
            };
            RectangleHullBuilder.GetHull(rectangles).Should().Match<Rectangle>(h => h.Y == 1);
        }

        [Test]
        public void HullsBottomSideOrdinateEqualsLowestRectangleSideOrdinate()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(1, 1, 10, 11),
                new Rectangle(10, 541, 3, 9)
            };
            RectangleHullBuilder.GetHull(rectangles).Should().Match<Rectangle>(h => h.Y + h.Bottom == 551);
        }

        [Test]
        public void HullsLeftSideAbscissaeEqualsLeftMostRectangleSideAbscissae()
        {
            var rectangles = new List<Rectangle>
            {   
                new Rectangle(0, 0, 10, 11),
                new Rectangle(-34, 541, 3, 9)
            };
            RectangleHullBuilder.GetHull(rectangles).Should().Match<Rectangle>(h => h.X == -34);
        }

        [Test]
        public void HullsRightSideAbscissaeEqualsRightMostRectangleSideAbscissae()
        {
            var rectangles = new List<Rectangle>
            {
                new Rectangle(50, 99, 10, 11),
                new Rectangle(-34, 541, 3, 9)
            };
            RectangleHullBuilder.GetHull(rectangles).Should().Match<Rectangle>(h => h.X + h.Width == 60);
        }
    }
}