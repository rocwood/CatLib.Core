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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests
{
    [TestClass]
    public class FacaedTests
    {

        public class FacaedTestClass
        {

        }

        public class TestClassFacaed : Facade<FacaedTestClass>
        {

        }

        /// <summary>
        /// 门面测试
        /// </summary>
        [TestMethod]
        public void FacadeTest()
        {
            var app = new Application();
            app.Bootstrap();
            var obj = new FacaedTestClass();
            app.Singleton<FacaedTestClass>((c, p) =>
            {
                return obj;
            });

            Assert.AreEqual(obj, TestClassFacaed.Instance);
            //double run
            Assert.AreEqual(obj, TestClassFacaed.Instance);
        }

        /// <summary>
        /// 无Application支持下测试
        /// </summary>
        [TestMethod]
        public void NullApplicationFacadeTest()
        {
            App.Handler = null;

            ExceptionAssert.Throws<NullReferenceException>(() =>
            {
                var f = TestClassFacaed.Instance;
            });
        }
    }
}
