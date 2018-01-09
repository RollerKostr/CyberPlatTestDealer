using System.Net.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CyberPlatGate;
using CyberPlatGate.Components;
using CyberPlatGate.Components.Utility;
using CyberPlatGate.Contracts.Configurations;

namespace DealerSite.Windsor
{
    public class WebApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ICyberPlatGate>().ImplementedBy<CyberPlatGate.CyberPlatGate>(),
                Component.For<ICyberPlatHttpClient>().ImplementedBy<CyberPlatHttpClient>(),
                    //.DependsOn(Dependency.OnValue<HttpMessageHandler>(new ConsoleLoggingHttpHandler())), // TODO[mk] implement fileLoggingHandler with Windsor.Logger
                Component.For<ICyberPlatSignatureManager>().ImplementedBy<CyberPlatSignatureManager>()
            );

            // Configurations
            var gateConf = new CyberPlatGateConfiguration()
            {
                AP = AppSettings.AP,
                SD = AppSettings.SD,
                OP = AppSettings.OP,
                PAY_TOOL = AppSettings.PAY_TOOL,
                TERM_ID  = AppSettings.TERM_ID,
                NO_ROUTE = AppSettings.NO_ROUTE,
            };
            var clientConf = new CyberPlatHttpClientConfiguration()
            {
                CheckUrl   = AppSettings.CheckUrl,
                PayUrl     = AppSettings.PayUrl,
                StatusUrl  = AppSettings.StatusUrl,
                TimeoutSec = AppSettings.TimeoutSec,
            };
            var managerConf = new CyberPlatSignatureManagerConfiguration()
            {
                PublicKeyPath     = AppSettings.PublicKeyPath,
                SecretKeyPath     = AppSettings.SecretKeyPath,
                PublicKeySerial   = AppSettings.PublicKeySerial,
                SecretKeyPassword = AppSettings.SecretKeyPassword,
            };
            container.Register(
                Component.For<CyberPlatGateConfiguration>().Instance(gateConf),
                Component.For<CyberPlatHttpClientConfiguration>().Instance(clientConf),
                Component.For<CyberPlatSignatureManagerConfiguration>().Instance(managerConf)
            );
        }
    }
}