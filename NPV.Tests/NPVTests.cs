using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPV.Controllers;
using NPV.Model;
using NPV.Requests;
using NPV.Responses;

namespace NPV.Tests
{
    [TestClass]
    public class NPVTests
    {
        [TestMethod]
        public void CalculatingNpvReturnsNpvResponse()
        {
            NpvController npvc = new NpvController();

            List<decimal> cashFlows = new List<decimal>() { 1000, 1500, 2000 };
            var request = new NpvRequest { InitialValue = 10000m, DiscountRateIncrement = 0.25m, LowerBoundDiscountRate = 1.00m, UpperBoundDiscountRate = 1.5m, CashFlows = cashFlows };

            var httpResponse = npvc.CalculateNPV(request);

            var npvResponse = (OkNegotiatedContentResult<NpvResponse>)httpResponse;

            List<NPVRecord> npvRecords = npvResponse.Content.NPVs;

            Assert.IsTrue(npvResponse.GetType() == typeof(OkNegotiatedContentResult<NpvResponse>));

        }

        [TestMethod]
        public void GetAllNPVResults_HasSomeValues()
        {
            NpvController npvc = new NpvController();

            var httpResponse = npvc.GetAllNpvResults();

            var npvResponse = ((OkNegotiatedContentResult<NpvResponse>)httpResponse).Content;

            Assert.IsTrue(npvResponse.NPVs.Count > 0, "No NPV Records found");
        }
    }
}
