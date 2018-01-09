using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CyberPlatGate.Contracts.Gate;

namespace DealerSite.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Index()
        {
            var gateCheckResult = new GateCheckResponse()
            {
                Error = null,
                Session = "abcdef",
                DisplayInfo = "Show this text to user",
            };

            return View(gateCheckResult);
        }
    }
}