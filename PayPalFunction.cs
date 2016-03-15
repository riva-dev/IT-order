using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GetstartedPayPal
/// </summary>
public class PayPalFunction
{
    public PayPalFunction() { }

    //public void Cart(List<CartItems> cartItems)
    //{
    //    PaymentDetailsType[] pmtDetails = new PaymentDetailsType[1];
    //    pmtDetails[0] = new PaymentDetailsType();
    //    var pmtIndex = 0;

    //    PaymentDetailsItemType[] items = new PaymentDetailsItemType[cartItems.Count];
    //    CurrencyCodeType currency = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), "USD");
    //    foreach (var item in cartItems)
    //    {
    //        var i = new PaymentDetailsItemType()
    //        {
    //            Name = item.Name,
    //            Number = item.Id.ToString(),
    //            Quantity = item.Quantity,
    //            Amount = new BasicAmountType() { currencyID = currency, value = item.Price.ToString() },
    //            ItemCategory = (ItemCategoryType)Enum.Parse(typeof(ItemCategoryType), "PHYSICAL")
    //        };
    //        items[pmtIndex] = i;
    //        pmtIndex++;
    //    }
    //    //reqDetails.p
    //    //reqDetails.PaymentDetails = pmtDetails;
    //    //hOrderTotal.Value
    //    // 
    //    pmtDetails[0].PaymentDetailsItem = items;
    //    pmtDetails[0].OrderTotal = new BasicAmountType() { currencyID = CurrencyCodeType.GBP, Value = HttpContext.Current.Session["_OrderTotalLessShippingAmount"].ToString() };
    //    reqDetails.PaymentDetails = pmtDetails;
    //}

    /// <summary>
    /// set all the values of paypal purches 
    /// </summary>
    /// <param name="simDetails"></param>
    /// <param name="sPromoCode"></param>
    /// <param name="shipFee"></param>
    /// <param name="tmpOrderObj"></param>
    /// <returns>if everything good return the url of Pypal</returns>
    public string CallPayPalAPIFunc(TmpOrderObj tmpOrderObj, Promocode promoCode, List<CartItems> AccessoriesCartItems, string conversionRate)
    {
        try
        {
            List<SIMDetails> simDetails = tmpOrderObj.SimDetails;
            decimal shipFee = Convert.ToDecimal(tmpOrderObj.ShipFee);
            CurrencyCodeType currency = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), "USD");

            PaymentDetailsType paymentDetails = new PaymentDetailsType();
            PaymentDetailsItemType itemDetaiel;
            PaymentDetailsItemType itemPlanDetaiel;
            paymentDetails.PaymentAction = (PaymentActionCodeType)Enum.Parse(typeof(PaymentActionCodeType), "SALE");
            double ConversionRate = Convert.ToDouble(conversionRate);
            double itemTotal1Order = Convert.ToDouble(simDetails[0].SimPrice) + Convert.ToDouble(simDetails[0].PaypalAmount);
            double itemTotal = 0.0;
            double ItemTotalAcc = 0.0;
            //int Quantity = 1;

            //Go on all item : SIM cards and Plans 
            foreach (SIMDetails sd in simDetails)
            {

                double ItemPrice = Convert.ToDouble(sd.PaypalAmount);
                double ItemSimPrice = Convert.ToDouble(sd.SimPrice);
                if (ItemSimPrice > 0)
                {
                    itemDetaiel = new PaymentDetailsItemType();
                    itemDetaiel.Name = sd.EquipmentName;
                    itemDetaiel.Quantity = 1;
                    itemDetaiel.Amount = new BasicAmountType(currency, String.Format("{0:0.00}", ItemSimPrice));
                    itemDetaiel.ItemCategory = (ItemCategoryType)Enum.Parse(typeof(ItemCategoryType), "PHYSICAL");
                    itemTotal += ItemSimPrice;
                    paymentDetails.PaymentDetailsItem.Add(itemDetaiel);
                }
                itemPlanDetaiel = new PaymentDetailsItemType();
                itemPlanDetaiel.Name = otherUtility.getResourceString("Plan");
                itemPlanDetaiel.Amount = new BasicAmountType(currency, String.Format("{0:0.00}", ItemPrice));
                itemPlanDetaiel.Quantity = 1;
                itemPlanDetaiel.ItemCategory = (ItemCategoryType)Enum.Parse(typeof(ItemCategoryType), "PHYSICAL");
                itemTotal += ItemPrice;
                paymentDetails.PaymentDetailsItem.Add(itemPlanDetaiel);
            }

            tmpOrderObj.PlanCostUSD = Convert.ToDecimal(itemTotal1Order);


            //Accessories
            foreach (CartItems item in AccessoriesCartItems)
            {
                itemDetaiel = new PaymentDetailsItemType();
                itemDetaiel.Name = item.Name;
                itemDetaiel.Quantity = item.Quantity;
                itemDetaiel.Amount = new BasicAmountType(currency, String.Format("{0:0.00}", item.PriceUSA));
                itemDetaiel.ItemCategory = (ItemCategoryType)Enum.Parse(typeof(ItemCategoryType), "PHYSICAL");
                itemTotal += item.PriceUSA * item.Quantity;
                ItemTotalAcc += item.PriceUSA * item.Quantity;

                paymentDetails.PaymentDetailsItem.Add(itemDetaiel);
            }
            SessionUtility.AddValue("ItemTotalAcc", ItemTotalAcc);

            //PromoCode 
            double ItemDiscount = 0;
            if (promoCode.IsVaild)
            {

                ItemDiscount = promoCode.getDiscountCopun(promoCode, itemTotal, itemTotal1Order, simDetails.Count);
                if (ItemDiscount !=0)
                {
                    itemDetaiel = new PaymentDetailsItemType();
                    itemDetaiel.Name = otherUtility.getResourceString("Discount");
                    itemDetaiel.Amount = new BasicAmountType(currency, ItemDiscount.ToString("F"));
                    itemDetaiel.Quantity = 1;
                    itemDetaiel.ItemCategory = (ItemCategoryType)Enum.Parse(typeof(ItemCategoryType), "PHYSICAL");
                    itemTotal += ItemDiscount;
                    itemTotal = Math.Round(itemTotal, 2);
                    paymentDetails.PaymentDetailsItem.Add(itemDetaiel);
                    //Aplly the discount on the item 
                    for (int i = 0; i < simDetails.Count; i++)
                        if (i == 0)
                        {
                            tmpOrderObj.SimDetails[i].PaypalDiscount = Convert.ToDecimal(promoCode.getDiscount(promoCode, itemTotal1Order, ItemTotalAcc, simDetails.Count,i));
                        }
                        else tmpOrderObj.SimDetails[i].PaypalDiscount = Convert.ToDecimal(promoCode.getDiscount(promoCode, itemTotal1Order, 0, simDetails.Count,i));
                }
            }
            if (ItemDiscount == 0 || !promoCode.IsVaild)
                for (int i = 0; i < simDetails.Count; i++)
                    tmpOrderObj.SimDetails[i].PaypalDiscount = 0;

            //Shipping 
            paymentDetails.ShippingTotal = new BasicAmountType(currency, shipFee.ToString("F"));

            //total purches 
            paymentDetails.ItemTotal = new BasicAmountType(currency, String.Format("{0:0.00}", itemTotal));
            paymentDetails.OrderTotal = new BasicAmountType(currency, String.Format("{0:0.00}", itemTotal + Convert.ToDouble(shipFee)));

            SessionUtility.AddValue("totalAmountCharged", itemTotal);
            tmpOrderObj.PayPalLogCounter = -1;
            tmpOrderObj.PayPalTransactionId = "";
            tmpOrderObj.PayPalAmountCharge = Convert.ToDecimal(itemTotal) + Convert.ToDecimal(shipFee);
            tmpOrderObj.CCNum = "1212121212121210";
            tmpOrderObj.CCTitle = "Paypal";
            tmpOrderObj.CCCode = "N/A";
            tmpOrderObj.CCExpDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
            tmpOrderObj.PayPalLogCounter = dbUtility.AddPayPalLog(tmpOrderObj.PayPalTransactionId, Convert.ToDecimal(itemTotal), false, tmpOrderObj.UserName, tmpOrderObj.ClientEmail, tmpOrderObj.SessionID, "");

            HttpContext.Current.Session["currentPayment"] = tmpOrderObj.SessionID.ToString();
            HttpContext.Current.Session["CurrentPayPalAmount"] = tmpOrderObj.PayPalAmountCharge;

            SessionUtility.AddTmpOrder(tmpOrderObj);
            SessionUtility.AddListTmpOrder(tmpOrderObj);

            string strBodyPaypal = string.Format(@"<b>Date</b>: {2} <br /> 
                                                           <b>Order number:</b> {0} <br /> 
                                                           <b>Name:</b> {3} <br /> 
                                                           <b>Dollar Amount Charged with Paypal:</b> ${1} <br /> 
                                                           <b>Email Address: {4} </b>. ", "",
                                                    tmpOrderObj.PayPalAmountCharge,
                                                    DateTime.Now.ToString(),
                                                    tmpOrderObj.UserNamePayPal,
                                                    tmpOrderObj.ClientEmail);
            
            emailUtility.SendGeneralMail(tmpOrderObj.MainEmail, "Web-Master@talknsave.net", strBodyPaypal, "Before placing paypal order", "", tmpOrderObj.SiteSourceCountry);



            if (paymentDetails.NoteText != null)
            {
                if (paymentDetails.NoteText.Length > 200)
                    paymentDetails.NoteText = paymentDetails.NoteText.Substring(0, 200);
            }
            BillingCodeType billingCodeType = (BillingCodeType)Enum.Parse(typeof(BillingCodeType), "NONE");
            BillingAgreementDetailsType baType = new BillingAgreementDetailsType(billingCodeType);
            baType.BillingAgreementDescription = "TELAWAY";

            //option to Set Express Checkout Request Details Type
            SetExpressCheckoutRequestDetailsType ecDetails = new SetExpressCheckoutRequestDetailsType();
            ecDetails.PaymentDetails.Add(paymentDetails);
            ecDetails.BrandName = "TELAWAY";
            ecDetails.BillingAgreementDetails.Add(baType);
            ecDetails.LocaleCode = string.IsNullOrEmpty(tmpOrderObj.PayPalLocaleCode) ? "US" : tmpOrderObj.PayPalLocaleCode;
            ecDetails.ReturnURL = "http://localhost:49854/PayPalSuccessFM.aspx";
            ecDetails.CancelURL = "http://localhost:49854/PayPalCancelFM.aspx";
            //ecDetails.ReturnURL = "https://www.telaway.net/order/PayPalSuccessFM.aspx";
            //ecDetails.CancelURL = "https://www.telaway.net/order/PayPalCancelFM.aspx";
            //ecDetails.OrderTotal = paymentDetails.OrderTotal = new BasicAmountType(currency, String.Format("{0:0.00}", itemTotal + Convert.ToDouble(shipFee)));
            SetExpressCheckoutRequestType request = new SetExpressCheckoutRequestType();
            request.SetExpressCheckoutRequestDetails = ecDetails;

            SessionUtility.AddValue("CurrentPayPalAmount", Convert.ToDecimal(itemTotal) + shipFee);

            //call Paypal API
            string redirectUrl = string.Empty;
            PaypalAPI Paypal = new PaypalAPI();
            if (Paypal.SetExpressCheckout(request, ref redirectUrl) == true)

                return redirectUrl;
            return redirectUrl;
        }

        catch (Exception e)
        {
            emailUtility.SendMailErr("CallPaypalAPIFunc " + e.Message);
            return "";
        }
    }
}