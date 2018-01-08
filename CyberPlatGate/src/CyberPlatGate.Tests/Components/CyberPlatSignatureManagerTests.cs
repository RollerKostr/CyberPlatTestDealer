using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using CyberPlatGate.Components;
using CyberPlatGate.Contracts.Configurations;
using CyberPlatGate.Contracts.Http;
using CyberPlatGate.Tests.Contracts;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    class CyberPlatSignatureManagerTests
    {
        private static CyberPlatSignatureManagerConfiguration TestConf
        {
            get
            {
                var validConf = TestConfigurations.ManagerConfiguration;
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
                using (var manager = new CyberPlatSignatureManager(TestConf))
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
            using (var manager = new CyberPlatSignatureManager(TestConf))
            {
                Action action = () => { var result = manager.Sign(request); };
                action.ShouldNotThrow();
            }
        }

        [Test]
        [TestCaseSource(nameof(ValidServerResponses))]
        public void VerifyTest(string responseStr, string typeName)
        {
            using (var manager = new CyberPlatSignatureManager(TestConf))
            {
                Action action = () => { manager.Verify(responseStr); };
                action.ShouldNotThrow<CryptographicException>();
            }
        }

        [Test]
        [TestCaseSource(nameof(ValidServerResponses))]
        public void ParseTest(string responseStr, string typeName)
        {
            using (var manager = new CyberPlatSignatureManager(TestConf))
            {
                Action action = () =>
                {
                    dynamic response;
                    switch (typeName)
                    {
                        case nameof(CheckResponse):
                            response = manager.Parse<CheckResponse>(responseStr);
                            break;
                        case nameof(PayResponse):
                            response = manager.Parse<PayResponse>(responseStr);
                            break;
                        case nameof(StatusResponse):
                            response = manager.Parse<StatusResponse>(responseStr);
                            break;
                    }
                };
                action.ShouldNotThrow<HttpParseException>();
            }
        }

        #region Test cases

        private static IEnumerable<object> ValidRequests
        {
            get
            {
                yield return ValidContracts.GenerateCheckRequest();
                yield return ValidContracts.GeneratePayRequest(ValidContracts.GenerateCheckRequest());
                yield return ValidContracts.GenerateStatusRequest(ValidContracts.GeneratePayResponse());
            }
        }

        private static IEnumerable<object> ValidServerResponses
        {
            get
            {
                yield return new object[] { Resources.ServerCheckResponse,  nameof(CheckResponse) };
                yield return new object[] { Resources.ServerPayResponse,    nameof(PayResponse) };
                yield return new object[] { Resources.ServerStatusResponse, nameof(StatusResponse) };
            }
        }

        #endregion Test cases
    }
}
