using System;
using CyberPlatGate.Components;
using FluentAssertions;
using NUnit.Framework;

namespace CyberPlatGate.Tests.Components
{
    [TestFixture]
    //[Ignore("Integrational")]
    // TODO[mk] make TestFixture SetUp and TearDown with copying files from Test assembly resources, and change Integrational to Unit-test
    class CyberPlatHttpClientRequestBuilderTests
    {
        [Test]
        [TestCase(
            @"C:\Users\RollerKostr\Documents\Visual Studio 2015\Projects\CyberPlatDealer\CyberPlatGate\src\libipriv\pubkeys.key",
            @"C:\Users\RollerKostr\Documents\Visual Studio 2015\Projects\CyberPlatDealer\CyberPlatGate\src\libipriv\secret.key")]
        public void CyberPlatHttpClientRequestBuilderInitializingTest(string pubKeyPath, string secKeyPath)
        {
            Action action = () => { var builder = new CyberPlatHttpClientRequestBuilder(pubKeyPath, secKeyPath); };
            action.ShouldNotThrow(); // possibly can throw any of: ArgumentException (incl. ArgumentNullException), DllNotFoundException, BadImageFormatException
        }
    }
}
