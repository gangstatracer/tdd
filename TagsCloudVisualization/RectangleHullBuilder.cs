using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RectangleHullBuilder 
    {
        public static Rectangle GetHull(IList<Rectangle> rectangles)
        {
            if (rectangles.Count == 0)
            {
                throw new ArgumentException("List should contain at least one element");
            } 
            var top = rectangles.Select(r => r.Top).Min();
            var bottom = rectangles.Select(r => r.Bottom).Max();
            var left = rectangles.Select(r => r.Left).Min();
            var right = rectangles.Select(r => r.Right).Max();
            return new Rectangle(left, top, right - left, bottom - top);
        }
    }
}