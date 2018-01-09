using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberPlatGate.Contracts.Gate;
using DealerSite.Components;
using DealerSite.ViewModels;

namespace DealerSite.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var paymentViewModel = new PaymentViewModel()
            {
                GateCheckResponse = null,
            };
            return View("Index", paymentViewModel);
        }

        [HttpPost]
        public ActionResult Pay(string number, string amount)
        {
            var paymentViewModel = new PaymentViewModel()
            {
                GateCheckResponse = TestGateResponses.ValidCheckResponse,
                GatePayResponse = TestGateResponses.ValidPayResponse,
            };

            return View("Index", paymentViewModel);
        }
    }
}