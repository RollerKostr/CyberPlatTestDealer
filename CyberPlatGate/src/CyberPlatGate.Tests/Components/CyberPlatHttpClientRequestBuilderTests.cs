using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using CyberPlatGate.Components;
using CyberPlatGate.Tests.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatHttpClientRequestBuilderTests
    {
        private static CyberPlatHttpClientRequestBuilderConfiguration TestConf
        {
            get
            {
                var validConf = ValidContracts.BuilderConfiguration;
                // Required for NUnit v3.0+ due to different working folder
                validConf.SecretKeyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "sec.txt");
                validConf.PublicKeyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "pub.txt");
                return validConf;
            }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            File.WriteAllBytes(TestConf.SecretKeyPath, Resources.secret);
            File.WriteAllBytes(TestConf.PublicKeyPath, Resources.pubkeys);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            File.Delete(TestConf.SecretKeyPath);
            File.Delete(TestConf.PublicKeyPath);
        }

        [Test]
        public void InitializingTest()
        {
            Action action = () =>
            {
                using (var builder = new CyberPlatHttpClientRequestBuilder(TestConf))
                {
                }
            };
            // possibly can throw any of: ArgumentException (incl. ArgumentNullException), DllNotFoundException, BadImageFormatException, CryptographicException
            action.ShouldNotThrow();
        }

        [Test]
        [TestCaseSource(nameof(ValidRequests))]
        public void BuildTest(dynamic request)
        {
            using (var builder = new CyberPlatHttpClientRequestBuilder(TestConf))
            {
                Action action = () => { var result = builder.Build(request); };
                action.ShouldNotThrow();
            }
        }

        [Test]
        public void VerifyTest()
        {
            using (var builder = new CyberPlatHttpClientRequestBuilder(TestConf))
            {
                Action action = () => { builder.Verify(Resources.ServerCheckResponse); };
                action.ShouldNotThrow<CryptographicException>();
            }
        }

        #region Test cases

        private static IEnumerable<object> ValidRequests
        {
            get
            {
                yield return ValidContracts.CheckRequest;

            }
        }

        #endregion Test cases
    }
}
