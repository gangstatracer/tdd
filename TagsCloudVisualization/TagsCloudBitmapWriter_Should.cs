using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class TagsCloudBitmapWriter_Should
    {
        [Test]
        public void Write_OneRectangle()
        {
            var bitmap = TagsCloudBitmapWriter
                .Write(new List<Rectangle> {new Rectangle(0, 0, 100, 37)});
            bitmap.Width.Should().Be(100);
            bitmap.Height.Should().Be(37);
        }

        [Test]
        public void ThrowException_NoRectangles()
        {
            Action writeAction = () => TagsCloudBitmapWriter.Write(new List<Rectangle>());
            writeAction.ShouldThrow<ArgumentException>();
        }
    }
}
