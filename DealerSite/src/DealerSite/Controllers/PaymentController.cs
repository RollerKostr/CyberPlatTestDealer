using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CyberPlatGate;
using CyberPlatGate.Contracts.Gate;
using DealerSite.Components;
using DealerSite.Models;
using DealerSite.ViewModels;

namespace DealerSite.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICyberPlatGate m_Gate;

        public PaymentController(ICyberPlatGate gate)
        {
            m_Gate = gate;
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

            await Task.Delay(TimeSpan.FromSeconds(2)); // Imitate request

            paymentViewModel.GateCheckResponse = TestGateResponses.ValidCheckResponse;
            paymentViewModel.GatePayResponse = TestGateResponses.ValidPayResponse;

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

            await Task.Delay(TimeSpan.FromSeconds(2)); // Imitate request

            paymentViewModel.GateStatusResponse = TestGateResponses.ValidStatusResponse;

            return View("Index", paymentViewModel);
        }
    }
}