﻿using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Lottie
{
    [TestClass]
    public class PerformanceTrackerTest
    {
        private PerformanceTracker _performanceTracker;

        [TestInitialize]
        public void Init()
        {
            _performanceTracker = new PerformanceTracker
            {
                Enabled = true
            };
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestDisabled()
        {
            _performanceTracker.Enabled = false;
            _performanceTracker.RecordRenderTime("Hello", 16f);
            Assert.IsFalse(_performanceTracker.SortedRenderTimes.Any());
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestOneFrame()
        {
            _performanceTracker.RecordRenderTime("Hello", 16f);
            var sortedRenderTimes = _performanceTracker.SortedRenderTimes;
            Assert.AreEqual(1, sortedRenderTimes.Count);
            Assert.AreEqual("Hello", sortedRenderTimes[0].Item1);
            Assert.AreEqual(16f, sortedRenderTimes[0].Item2);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestTwoFrames()
        {
            _performanceTracker.RecordRenderTime("Hello", 16f);
            _performanceTracker.RecordRenderTime("Hello", 8f);
            var sortedRenderTimes = _performanceTracker.SortedRenderTimes;
            Assert.AreEqual(1, sortedRenderTimes.Count);
            Assert.AreEqual("Hello", sortedRenderTimes[0].Item1);
            Assert.AreEqual(12f, sortedRenderTimes[0].Item2);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestTwoLayers()
        {
            _performanceTracker.RecordRenderTime("Hello", 16f);
            _performanceTracker.RecordRenderTime("World", 8f);
            var sortedRenderTimes = _performanceTracker.SortedRenderTimes;
            Assert.AreEqual(2, sortedRenderTimes.Count);
            Assert.AreEqual("Hello", sortedRenderTimes[0].Item1);
            Assert.AreEqual(16f, sortedRenderTimes[0].Item2);
            Assert.AreEqual("World", sortedRenderTimes[1].Item1);
            Assert.AreEqual(8f, sortedRenderTimes[1].Item2);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestTwoLayersAlternatingFrames()
        {
            _performanceTracker.RecordRenderTime("Hello", 16f);
            _performanceTracker.RecordRenderTime("World", 8f);
            _performanceTracker.RecordRenderTime("Hello", 32f);
            _performanceTracker.RecordRenderTime("World", 4f);
            var sortedRenderTimes = _performanceTracker.SortedRenderTimes;
            Assert.AreEqual(2, sortedRenderTimes.Count);
            Assert.AreEqual("Hello", sortedRenderTimes[0].Item1);
            Assert.AreEqual(24f, sortedRenderTimes[0].Item2);
            Assert.AreEqual("World", sortedRenderTimes[1].Item1);
            Assert.AreEqual(6f, sortedRenderTimes[1].Item2);
        }
    }
}
