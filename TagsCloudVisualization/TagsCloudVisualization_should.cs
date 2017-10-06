using System;
using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    internal class TagsCloudVisualization_Should
    {
        private CircularCloudLayouter layouter;

        [SetUp]
        public void SetUp()
        {
            layouter = new CircularCloudLayouter(new Point(100, 100));
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;
            if (context.Result.Status == TestStatus.Passed)
            {
                var fileName = $"{context.Test.Name}_{DateTime.Now:yy-MM-dd-HH-mm-ss}.bmp";
                TagsCloudBitmapWriter.Write(layouter.Rectangles).Save(fileName, ImageFormat.Bmp);
                Assert.Pass($"Test's layout saved to {fileName}");
            }
        }


        [Test]
        public void PutNextRectange_WhenEmpty()
        {
            layouter.PutNextRectange(new Size(20, 10)).Should().Be(new Rectangle(90, 95, 20, 10));
        }

        [Test]
        public void PutNextRectange_AfterOneAdded()
        {
            layouter.PutNextRectange(new Size(20, 10));
            layouter.PutNextRectange(new Size(5, 5)).Should().Be(new Rectangle(90, 95, 20, 10));
        }
    }
}
