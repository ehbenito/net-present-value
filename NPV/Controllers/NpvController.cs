using NPV.Db;
using NPV.Model;
using NPV.Models;
using NPV.Requests;
using NPV.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NPV.Controllers
{
    public class NpvController : ApiController
    {
        [HttpPost]
        [Route("api/Npv")]
        public IHttpActionResult CalculateNPV(NpvRequest request)
        {
            NpvResponse response = new NpvResponse();

            List<NPVRecord> npvRecords = new List<NPVRecord>();

            npvRecords = CalculatePV(request.LowerBoundDiscountRate, request.UpperBoundDiscountRate, request.DiscountRateIncrement, request.InitialValue, request.CashFlows);

            response.NPVs = npvRecords;

            return Ok(response);
        }

        public List<NPVRecord> CalculatePV(decimal lbDiscountRate, decimal ubDiscountRate, decimal discountRateIncrement, decimal initialValue, List<Decimal> cashFlows)
        {
            List<NPVRecord> npvRecords = new List<NPVRecord>();

            List<decimal> discountRates = this.GetDiscountRate(lbDiscountRate, ubDiscountRate, discountRateIncrement);

            foreach (var discountRatePercentage in discountRates)
            {
                int timePeriods = 0;
                decimal discountRate = discountRatePercentage / 100;

                //if discountRate is not Percentage, replace line above with this line.
                //decimal discountRate = discountRatePercentage;

                List<decimal> netPresentValue = new List<decimal>();
                netPresentValue.Add(-initialValue);
                string cashFlowsTxt = string.Empty;

                foreach (var cashFlow in cashFlows)
                {
                    timePeriods++;
                    decimal presentValue = cashFlow / (Convert.ToDecimal(Math.Pow(Convert.ToDouble(1 + discountRate), timePeriods)));

                    netPresentValue.Add(presentValue);

                    if (timePeriods == 1)
                    {
                        cashFlowsTxt = cashFlow.ToString();
                    }
                    else
                    {
                        cashFlowsTxt = cashFlowsTxt + "," + cashFlow.ToString();
                    }
                }

                npvRecords.Add(new NPVRecord { CashFlows = cashFlowsTxt, DiscountRate = discountRate * 100, InitialValue = initialValue, NetPresentValue = netPresentValue.Sum() });

                using (var dbContext = new NpvContext())
                {
                    //dbContext.NPVRecords.Add(new NPVRecord { CashFlows = cashFlowsTxt, DiscountRate = discountRate * 100, InitialValue = initialValue, NetPresentValue = netPresentValue.Sum() });
                    //dbContext.SaveChanges();
                }
            }

            return npvRecords;
        }

        public List<decimal> GetDiscountRate(decimal lowerBoundDiscountRate, decimal upperBoundDiscountRate, decimal discountIncrement)
        {
            List<decimal> discountRates = new List<decimal>();

            try
            {
                var discountRateCount = (upperBoundDiscountRate - lowerBoundDiscountRate) / discountIncrement;

                decimal discountRate = 0;

                for (int i = 0; i <= discountRateCount; i++)
                {
                    if (i == 0)
                    {
                        discountRate = lowerBoundDiscountRate;
                    }
                    else
                    {
                        discountRate = discountRate + discountIncrement;
                    }

                    discountRates.Add(discountRate);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return discountRates;
        }


        [HttpGet]
        [Route("api/Npv")]
        public IHttpActionResult GetAllNpvResults()
        {
            NpvResponse response = new NpvResponse();

            List<NPVRecord> npvRecords = new List<NPVRecord>();

            using(var dbContext = new NpvContext())
            {
                npvRecords = dbContext.NPVRecords.OrderByDescending(x=>x.Id).Take(100).ToList();
            }

            response.NPVs = npvRecords;

            return Ok(response);
        }
    }
}