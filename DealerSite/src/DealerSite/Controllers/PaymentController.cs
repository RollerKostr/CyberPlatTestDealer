using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using CyberPlatGate;
using CyberPlatGate.Contracts.Gate;
using DealerSite.Models;
using DealerSite.ViewModels;

namespace DealerSite.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICyberPlatGate m_Gate;
        private readonly ILogger m_Logger;

        public PaymentController(ICyberPlatGate gate, ILogger logger)
        {
            if (gate == null) throw new ArgumentNullException(nameof(gate));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            m_Gate = gate;
            m_Logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var paymentViewModel = new PaymentViewModel()
            {
            };

            return View("Index", paymentViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Payment(PaymentInput input)
        {
            var paymentViewModel = new PaymentViewModel()
            {
                PaymentInput = input,
            };
            
            if (!ModelState.IsValid)
                return View("Index", paymentViewModel);

            var gateCheckRequest = new GateCheckRequest()
            {
                Number = input.Number,
                Amount = double.Parse(input.Amount, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
            };

            try
            {
                paymentViewModel.GatePayResponse = await m_Gate.CheckAndPay(gateCheckRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                m_Logger.Error(e.Message, e);
                paymentViewModel.ExceptionMessage = e.Message;
            }

            return View("Index", paymentViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Status(StatusInput input)
        {
            var paymentViewModel = new PaymentViewModel() // TODO[mk] add validation
            {
                StatusInput = input,
            };

            if (!ModelState.IsValid)
                return View("Index", paymentViewModel);

            var gateStatusRequest = new GateStatusRequest()
            {
                Session = input.Session,
                TransId = input.TransId,
            };

            try
            {
                paymentViewModel.GateStatusResponse = await m_Gate.Status(gateStatusRequest).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                m_Logger.Error(e.Message, e);
                paymentViewModel.ExceptionMessage = e.Message;
            }

            return View("Index", paymentViewModel);
        }
    }
}