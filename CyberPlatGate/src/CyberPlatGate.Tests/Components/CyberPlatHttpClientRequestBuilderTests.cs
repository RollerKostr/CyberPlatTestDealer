using System;
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
        // Required for NUnit v3.0+
        private static readonly string SecKeyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "sec.txt");
        private static readonly string PubKeyPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "pub.txt");
        private const string SEC_KEY_PASSWORD = "1111111111";
        private const string PUB_KEY_SERIAL = "17033";

        [OneTimeSetUp]
        public void SetUp()
        {
            File.WriteAllBytes(PubKeyPath, Resources.pubkeys);
            File.WriteAllBytes(SecKeyPath, Resources.secret);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            File.Delete(PubKeyPath);
            File.Delete(SecKeyPath);
        }

        [Test]
        public void InitializingTest()
        {
            Action action = () =>
            {
                using (var builder = new CyberPlatHttpClientRequestBuilder(SecKeyPath, PubKeyPath, SEC_KEY_PASSWORD, PUB_KEY_SERIAL))
                {
                }
            };
            // possibly can throw any of: ArgumentException (incl. ArgumentNullException), DllNotFoundException, BadImageFormatException, CryptographicException
            action.ShouldNotThrow();
        }

        [Test]
        public void BuildTest()
        {
            using (var builder = new CyberPlatHttpClientRequestBuilder(SecKeyPath, PubKeyPath, SEC_KEY_PASSWORD, PUB_KEY_SERIAL))
            {
                var result = builder.Build(ValidContracts.CheckRequest);
            }
        }

        [Test]
        public void VerifyTest()
        {
            using (var builder = new CyberPlatHttpClientRequestBuilder(SecKeyPath, PubKeyPath, SEC_KEY_PASSWORD, PUB_KEY_SERIAL))
            {
                Action action = () => { builder.Verify(Resources.ServerCheckResponse); };
                action.ShouldNotThrow<CryptographicException>();
            }
        }
    }
}
