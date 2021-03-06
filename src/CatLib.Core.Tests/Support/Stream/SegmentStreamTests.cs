﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Core.Tests.Support.Stream
{
    [TestClass]
    public class SegmentStreamTests
    {
        [TestMethod]
        public void TestRead()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            Assert.AreEqual("hello world", segmentStream.ToText());
        }

        [TestMethod]
        public void TestReadMiddle()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            baseStream.Seek(14, SeekOrigin.Begin);
            var segmentStream = new SegmentStream(baseStream, 10);

            Assert.AreEqual("my name is", segmentStream.ToText());
        }

        [TestMethod]
        public void TestReadEnd()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            baseStream.Seek(14, SeekOrigin.Begin);
            var segmentStream = new SegmentStream(baseStream);

            Assert.AreEqual("my name is miaomiao", segmentStream.ToText());
        }

        
        [TestMethod]
        public void TestSeekEnd()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.Seek(-5, SeekOrigin.End);

            Assert.AreEqual("world", segmentStream.ToText());
        }
        
        [TestMethod]
        public void TestSeekCurrent()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            baseStream.Seek(2, SeekOrigin.Begin);
            var segmentStream = new SegmentStream(baseStream, 9);
            segmentStream.Seek(4, SeekOrigin.Current);
            Assert.AreEqual("world", segmentStream.ToText());
        }

        [TestMethod]
        public void TestSeekBegin()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.ToText(null, false);
            segmentStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual("hello world", segmentStream.ToText());
        }

        [TestMethod]
        public void TestGetLength()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            Assert.AreEqual(11, segmentStream.Length);
        }

        [TestMethod]
        public void TestGetPosition()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            baseStream.Seek(2, SeekOrigin.Begin);
            var segmentStream = new SegmentStream(baseStream, 9) {Position = 4};
            Assert.AreEqual(4, segmentStream.Position);
        }

        [TestMethod]
        public void TestReadBuffer()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            var buffer = new byte[255];
            Assert.AreEqual(11, segmentStream.Read(buffer, 0, 255));
            Assert.AreEqual("hello world", CatLib.Util.Encoding.GetString(buffer, 0, 11));
        }

        [TestMethod]
        public void TestReadBufferEnd()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            var buffer = new byte[255];
            Assert.AreEqual(11, segmentStream.Read(buffer, 0, 255));
            Assert.AreEqual("hello world", CatLib.Util.Encoding.GetString(buffer, 0, 11));
            Assert.AreEqual(0, segmentStream.Read(buffer, 0, 255));
        }

        [TestMethod]
        public void TestReadBufferMin()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            var buffer = new byte[5];
            Assert.AreEqual(5, segmentStream.Read(buffer, 0, 5));
            Assert.AreEqual("hello", CatLib.Util.Encoding.GetString(buffer, 0, 5));
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSetLength()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.SetLength(100);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestWrite()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            var buffer = CatLib.Util.Encoding.GetBytes("hello world");

            segmentStream.Write(buffer, 0, buffer.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestNotSupportSeekOrigin()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.Seek(0, (SeekOrigin) 100);
        }

        [TestMethod]
        public void TestSeekEndToRead()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.Seek(100, SeekOrigin.End);
            Assert.AreEqual(string.Empty, segmentStream.ToText());
        }

        [TestMethod]
        public void TestSeekSmallThenStart()
        {
            var baseStream = "hello world , my name is miaomiao".ToStream();
            var segmentStream = new SegmentStream(baseStream, 11);
            segmentStream.Seek(-100, SeekOrigin.Begin);
            Assert.AreEqual("hello world", segmentStream.ToText());
        }


        private class CanNotSeekStream : WrapperStream
        {
            public override bool CanSeek => false;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGivenCanNotSeek()
        {
            var segmentStream = new SegmentStream(new CanNotSeekStream());
        }
    }
}
