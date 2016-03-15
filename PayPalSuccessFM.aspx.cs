using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
public partial class PayPalSuccess : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string token = string.Empty;
            string payerID = string.Empty;

            // Prepopulate EC Token value if coming from another page
            if (Request.Params["token"] != null)
            {
                token = Request.Params["token"];
            }
            if (Request.Params["PayerID"] != null)
            {
                payerID = Request.Params["PayerID"];
            }


            PaypalAPI paypalAPI = new PaypalAPI();
            bool bTransaction = false;
            bool ret = PaypalAPI.GetDetails(token, payerID);
            if (!ret)
            {
                CheckErr("Error with GetDetails function");
            }
            else
            {
                //confirm that payment was taken
                Dictionary<string, string> responseParams = new Dictionary<string, string>();
                List<ErrorType> responseErrors = new List<ErrorType>();
                
                bool ret2 = paypalAPI.DoPayment(token, payerID, ref responseParams, ref responseErrors);
               
                responseValues = responseParams;

                SendEmailIfErr(responseErrors);

                if (responseValues != null)
                {
                    foreach (KeyValuePair<String, String> entry in responseValues)
                    {

                        if (entry.Key.ToLower().Contains("transaction"))
                        {
                            bTransaction = true;
                            CheckSuceess(entry.Value.ToString());
                        }
                    }
                }
                if (!bTransaction)
                    SendEmailIfNotTran(responseValues);

                SendEmailIfErr(responseErrors);
               
               
                //Response.Redirect("SignupStep_3.aspx?status=success&token=" + token + "&PayerID="+payerID+"");
            }

        }
        catch(Exception ex)
        {
            emailUtility.SendMailErr("Paypal success Page load:" + ex.Message);   
        }

    }



    private void SendEmailIfNotTran(Dictionary<string, string> resValues)
    {
        string sInfo = "Paypal not Tran : ";
        if (resValues != null)
        {
            foreach (KeyValuePair<String, String> entry in resValues)
            {
                sInfo= sInfo + " key: " + entry.Key.ToLower();
                sInfo= sInfo + " value: "+entry.Value.ToString();
             }
        }
        if (sInfo != "")
            emailUtility.SendMailErr(sInfo);   
    }
    private void SendEmailIfErr(List<ErrorType> errorMessages)
    {
        string sErr = "";
        if (errorMessages != null)
        {
            foreach (ErrorType error in errorMessages)
            {
                sErr += error.LongMessage.ToString()  + " ";
            }
        }
        if (sErr!="")
            emailUtility.SendMailErr("Paypal Err: "+sErr);   
    }

    #region properties

    protected Dictionary<string, string> m_responseValues = new Dictionary<string, string>();
    protected Dictionary<string, string> responseValues
    {
        get
        {
            return m_responseValues;
        }
        set
        {
            m_responseValues = value;
        }
    }
    protected List<ErrorType> m_errorMessages;
    public List<ErrorType> errorMessages
    {
        get
        {
            return m_errorMessages;
        }
        set
        {
            m_errorMessages = value;
        }
    }
    #endregion


    private void CheckErr(string sErr)
    {
        try
        {
            bool bValid = false;
            IList<TmpOrderObj> TmpOrderList = SessionUtility.GetListTmpOrders();
            if (TmpOrderList != null)
            {
                string currentPaymentSession = "";
                if (Session["currentPayment"] != null)
                {
                    if (Session["currentPayment"].ToString() != "")
                    {
                        currentPaymentSession = Session["currentPayment"].ToString();
                    }
                }

                if (currentPaymentSession != "")
                {
                    for (int i = 0; i < TmpOrderList.Count; i++)
                    {
                        TmpOrderObj t = TmpOrderList[i];
                        if (t.SessionID.ToString() == currentPaymentSession)
                        {
                            bValid = true;
                            t.ChargeWithPayPal = true;
                            t.PayPalTransactionId = "Err";
                            t.PayPalAmountCharge = 0;
                            SessionUtility.AddTmpOrder(t);
                            SessionUtility.AddValue("ValidErrP", sErr );
                            Session["ValidErr"] = sErr;
                            //Session["currentPayment"] = "";
                            emailUtility.SendMailErr("Paypal check err:" + sErr);
                            SessionUtility.AddValue("InvalidData", "Your order can not be processed at this time due to a Communication Problem with the PayPal Server, please try again later, Thanks!");
                            //Response.Redirect("SignupStep_2.aspx?status=error", false);
                            Response.Redirect("FMPartial1.aspx?status=error&c=" + SessionUtility.GetValue("UserCountry"), false);
                        }
                    }
                }

            }

            if (!bValid)
            {
                emailUtility.SendMailErr("Paypal success check err, not valid:" + sErr);
                //Response.Redirect("SignupStep_2.aspx?Err=" + sErr, false);
                Response.Redirect("FMPartial1.aspx?c=" + SessionUtility.GetValue("UserCountry")+"Err=" + sErr, false);
            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("Paypal check err exception:" + e.Message);
        }
    }


    private void CheckSuceess(string sTransactionId)
    {
        try
        {
            decimal dPaidAmount = 0;
            if (Session["CurrentPayPalAmount"]!=null)
                dPaidAmount=Convert.ToDecimal(Session["CurrentPayPalAmount"].ToString());

            bool bValid = false;
            IList<TmpOrderObj> TmpOrderList = SessionUtility.GetListTmpOrders();
            if (TmpOrderList != null)
            {
                string currentPaymentSession = "";
                if (Session["currentPayment"] != null)
                {
                    if (Session["currentPayment"].ToString() != "")
                    {
                        currentPaymentSession = Session["currentPayment"].ToString();
                    }
                }

                if (currentPaymentSession != "")
                {
                    for (int i = 0; i < TmpOrderList.Count; i++)
                    {
                        TmpOrderObj t = TmpOrderList[i];
                        if (t.SessionID.ToString() == currentPaymentSession && Convert.ToInt32(t.PayPalAmountCharge) == Convert.ToInt32(dPaidAmount))
                        {
                            bValid = true;
                            t.ChargeWithPayPal = true;
                            t.PayPalTransactionId = sTransactionId;
                            t.PayPalAmountCharge = dPaidAmount;
                            SessionUtility.AddTmpOrder(t);
                            Session["currentPayment"] = "";

                            if (sTransactionId != "")
                            {
                                if (SignupUtility.FillOrderObjAndSubmit(t))
                                {
                                    t.PageNo = 3;
                                    Response.Redirect("SignupStep_4.aspx?status=success", false);
                                }
                                else
                                {
                                    emailUtility.SendMailErr("Paypal check success: failed on SignupUtility.FillOrderObjAndSubmit");
                                    SessionUtility.AddValue("InvalidData", "Your order can not be processed at this time due to a Communication Problem with the PayPal Server, please try again later, Thanks!");
                                    //Response.Redirect("SignupStep_2.aspx?status=error", false);
                                    Response.Redirect("FMPartial1.aspx?status=error&c=" + SessionUtility.GetValue("UserCountry"), false);
                                }
                            }

                            else
                            {
                                emailUtility.SendMailErr("Paypal check success: Invalid PayPal TransactionId");
                                SessionUtility.AddValue("InvalidData", "Your order can not be processed at this time due to a Communication Problem with the PayPal Server, please try again later, Thanks!");
                                //Response.Redirect("SignupStep_2.aspx?status=error", false);
                                Response.Redirect("FMPartial1.aspx?status=error&c=" + SessionUtility.GetValue("UserCountry"), false);

                            }
                        }
                        else //t.SessionID.ToString() == currentPaymentSession && t.PayPalAmountCharge == dPaidAmount
                        {
                            //emailUtility.SendMailErr("Paypal check success: Invalid Amount, dPaidAmount:" + dPaidAmount.ToString() + " t.PayPalAmountCharge" + t.PayPalAmountCharge.ToString() + ", t.SessionID.ToString():" + t.SessionID.ToString() + " ,currentPaymentSession:" + currentPaymentSession);
                        }
                    }
                }

            }
            if (!bValid)
            {
                string sE="";
                sE="Paypal check success: not valid, TransactionId:" + sTransactionId;

                sE=sE + " dPaidAmount:" + dPaidAmount.ToString();
                if (Session["currentPayment"]!=null)
                    sE =sE +" Session[currentPayment]:"+ Session["currentPayment"].ToString() ;

                if (TmpOrderList != null)
                {
                    sE = sE + " TmpOrderList.count:" + TmpOrderList.Count;

                    for (int i = 0; i < TmpOrderList.Count; i++)
                    {
                        TmpOrderObj t = TmpOrderList[i];

                        sE = sE + " t.SessionID:" + t.SessionID.ToString() + " PayPalAmountCharge: " + t.PayPalAmountCharge.ToString();
                    }

                }
                else
                {
                    sE = sE + " No TmpOrderList object. (session experied??)";
                    SessionUtility.AddValue("InvalidData", "Your order can not be processed at this time due to a Communication Problem with the PayPal Server, please try again later, Thanks!");
                    //Response.Redirect("SignupStep_2.aspx?status=error", false);
                    Response.Redirect("FMPartial1.aspx?status=error&c=" + SessionUtility.GetValue("UserCountry"), false);

                }

                emailUtility.SendMailErr(sE);
                
                CheckErr("Unknown Err");
            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("Paypal success check success:" + e.Message);
        }
    }


    //private void CheckSuceessWithoutTrans()
    //{
    //    try
    //    {
    //        decimal dPaidAmount = 0;
    //        if (Session["CurrentPayPalAmount"] != null)
    //            dPaidAmount = Convert.ToDecimal(Session["CurrentPayPalAmount"].ToString());

    //        bool bValid = false;
    //        IList<TmpOrderObj> TmpOrderList = SessionUtility.GetListTmpOrders();
    //        if (TmpOrderList != null)
    //        {
    //            string currentPaymentSession = "";
    //            if (Session["currentPayment"] != null)
    //            {
    //                if (Session["currentPayment"].ToString() != "")
    //                {
    //                    currentPaymentSession = Session["currentPayment"].ToString();
    //                }
    //            }

    //            if (currentPaymentSession != "")
    //            {
    //                for (int i = 0; i < TmpOrderList.Count; i++)
    //                {
    //                    TmpOrderObj t = TmpOrderList[i];
    //                    if (t.SessionID.ToString() == currentPaymentSession)
    //                    {
    //                        bValid = true;
    //                        t.ChargeWithPayPal = true;
    //                        t.PayPalTransactionId = "";
    //                        t.PayPalAmountCharge = 0;
    //                        SessionUtility.AddTmpOrder(t);
    //                        Session["currentPayment"] = "";

    //                        if (SignupUtility.FillOrderObjAndSubmit(t))
    //                        {
    //                            t.PageNo = 3;
    //                            OnlineOrderObj order = SignupUtility.getOnlineOrder();
    //                            emailUtility.SendMail("shraga@talknsave.net", "TELAWAY PayPal Issue: Invalid TransactionId", "please check about paypal amount of the order:" + GetInt(order.OnlineOrderCode) + " " + t.ClientEmail + " " + t.ClientLastName + " " + t.ClientFirstName);
    //                            Response.Redirect("SignupStep_4.aspx?status=success", false);
    //                        }
    //                        else
    //                        {
    //                            emailUtility.SendMailErr("Paypal check success without trans: failed on SignupUtility.FillOrderObjAndSubmit");
    //                            Response.Redirect("SignupStep_2.aspx?status=error", false);
    //                        }

    //                    }
    //                }
    //            }

    //        }
    //        if (!bValid)
    //        {
    //            string sE = "";
    //            sE = "Paypal check success without Trans: not valid";

    //            sE = sE + " dPaidAmount:" + dPaidAmount.ToString();
    //            if (Session["currentPayment"] != null)
    //                sE = sE + " Session[currentPayment]:" + Session["currentPayment"].ToString();

    //            if (TmpOrderList != null)
    //            {
    //                sE = sE + " TmpOrderList.count:" + TmpOrderList.Count;

    //                for (int i = 0; i < TmpOrderList.Count; i++)
    //                {
    //                    TmpOrderObj t = TmpOrderList[i];
    //                    sE = sE + " t.SessionID:" + t.SessionID.ToString();
    //                }

    //            }
    //            emailUtility.SendMailErr(sE);

    //            CheckErr("Unknown Err");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        emailUtility.SendMailErr("Paypal success check success without trans:" + e.Message);
    //    }
    //}


    private string GetInt(int? i)
    {
        try
        {
          return  Convert.ToString(i);
        }
        catch
        {
            return "";
        }

    }
}
