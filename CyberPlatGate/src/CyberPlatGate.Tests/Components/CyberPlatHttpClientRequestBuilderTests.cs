using System;
using System.IO;
using System.Security.Cryptography;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Http;
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

        private ICyberPlatHttpClientRequestBuilder m_Builder;

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
                var result = builder.Build(ValidCheckRequest);
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

        private static CheckRequest ValidCheckRequest => new CheckRequest()
        {
            SD = "17031",
            AP = "17032",
            OP = "17034",
            DATE = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
            SESSION = Guid.NewGuid().ToString("N"), // TODO[mk] Implement random string(20) generator
            NUMBER = "9261112233",
            ACCOUNT = null,
            AMOUNT = "1234.56",
            AMOUNT_ALL = "1249.99",
            REQ_TYPE = "1",
            PAY_TOOL = "0",
            TERM_ID = null,
            COMMENT = "test test 0123456789",
            ACCEPT_KEYS = "64182",
            NO_ROUTE = "1",
        };
    }
}
