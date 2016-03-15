using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;

public partial class PayPalCancel : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            try
            {
                CancelPayPal();
            }
            catch (Exception ex)
            {

                emailUtility.SendMailErr("Pay Pal cancel page err" + ex.Message);
            }

        }

    }



    private void CancelPayPal()
    {
        bool bValid = false;
        IList<TmpOrderObj> TmpOrderList = SessionUtility.GetListTmpOrders();
        if (TmpOrderList != null)
        {
            //emailUtility.SendGeneralMail("Web-Master@talknsave.net", "CancelPayPal: TmpOrderList != null", "payapal", "");    

            string currentPaymentSession = "";
            if (Session["currentPayment"] != null)
            {
                if (Session["currentPayment"].ToString() != "")
                {
                    currentPaymentSession = Session["currentPayment"].ToString();
                }
            }

            else //Response.Redirect("SignupStep_2.aspx",false);
                Response.Redirect("FMPartial1.aspx?c=" + SessionUtility.GetValue("UserCountry"), false);

            //emailUtility.SendMailErr("CancelPayPal: currentPaymentSession =" + currentPaymentSession);

            if (currentPaymentSession != "")
            {
                for (int i = 0; i < TmpOrderList.Count; i++)
                {
                    TmpOrderObj t = TmpOrderList[i];
                    //emailUtility.SendMailErr("CancelPayPal: TmpOrderObj =" + t.ClientEmail);

                    if (t.SessionID.ToString() == currentPaymentSession)
                    {
                        bValid = true;
                        t.ChargeWithPayPal = true;
                        t.PayPalTransactionId = "Cancel";
                        t.PayPalAmountCharge = 0;

                        t.PageNo = 1;

                        dbUtility.UpdatePayPalTransactionCanceled(t.PayPalLogCounter);

                        SessionUtility.AddTmpOrder(t);
                        Session["Err"] = "";
                        Session["ValidErr"] = "";
                        //Session["currentPayment"] = "";
                        emailUtility.SendMailErr("Pay Pal canceled <br>Online Order Code: " + t.OnlineOrderCode);
                        //Response.Redirect("SignupStep_2.aspx", false);
                        Response.Redirect("FMPartial1.aspx?c=" + SessionUtility.GetValue("UserCountry"), false);
                    }
                }
            }


        }

        if (!bValid)
        {
            emailUtility.SendMailErr("Pay Pal cancel not valid");
            //Response.Redirect("SignupStep_2.aspx?Err=Cancel", false);
            Response.Redirect("FMPartial1.aspx?Err=Cancel&c=" + SessionUtility.GetValue("UserCountry"), false);
        }
    }



}
