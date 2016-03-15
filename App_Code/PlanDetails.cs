using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PlanDetails
/// </summary>
public class PlanDetails
{
    public int counter { get; set;}
    public string title { get; set;}
    public string callPackageName { get; set;}
    public int planDays { get; set;}
    public string countryName { get; set;}
    public string parentLink { get; set;}
    public string subLink { get; set;}
    public int callPackageCode { get; set;}
    public int planCode { get; set;}
    public int smsPackageCode { get; set;}
    public int kntCode { get; set;}
    public int extendedPackageCode { get; set;}
    public int extendedPackageCodeBB { get; set;}
    public string currency { get; set;}
    public decimal conversionRate { get; set;}
    public string currencySymbol { get; set;}
    public decimal totalAmount { get; set;}
    public decimal totalAmountUSA { get; set;}
    public bool displayUSD { get; set;}
    public decimal asLowPerDay { get; set;}
    public decimal amountForDisplay { get; set;}
    public decimal bbPrice { get; set;}
    public decimal bbUSD { get; set;}
    public string billText { get; set;}
    public decimal simPrice { get; set;}


    public PlanDetails(DataRow plan)
    {
     counter = (int)plan["Counter"];
     callPackageName = plan["CallPackageName"].ToString();
     title = plan["title"].ToString();
     planDays = (int)plan["PlanDays"];
     countryName = plan["CountryName"].ToString();
     parentLink = plan["ParentLink"].ToString();
     subLink = plan["SubLink"].ToString();
     callPackageCode = (int)plan["CallPackageCode"];
     planCode = (int)plan["PlanCode"];
     smsPackageCode = (int)plan["SmsPackageCode"];
     kntCode = (int)plan["KntCode"];
     extendedPackageCode = (int)plan["ExtendedPackageCode"];
     extendedPackageCodeBB = (int)plan["ExtendedPackageCodeBB"];
     currency = plan["Currency"].ToString();
     conversionRate = (decimal)plan["ConversionRate"];
     currencySymbol = plan["CurrencySymbol"].ToString();
     totalAmount = (decimal)plan["TotalAmount"];
     totalAmountUSA = (decimal)plan["TotalAmountUSA"];
     displayUSD = (bool)plan["DisplayUSD"];
     asLowPerDay = (decimal)plan["AsLowPerDay"];
     amountForDisplay = (decimal)plan["AmountForDisplay"];
     bbPrice = (decimal)plan["BBPrice"];
     bbUSD = (decimal)plan["BBUSD"];
     simPrice = (decimal)plan["SimPrice"];
     billText = plan["BillText"].ToString();
    }
}
