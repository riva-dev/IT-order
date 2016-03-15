



using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.Collections.Generic;  
/// <summary>
/// Summary description for SignupUtility
/// </summary>
public class SignupUtility
{
    public SignupUtility()
	{
		
	}


    public static OnlineOrderObj getOnlineOrder()
    {
        return (OnlineOrderObj)SessionUtility.GetObjValue("OnlineOrderObj"); 
    }

    public static void CreateOrderObj()
    {
        //Create Order object in session 

        OnlineOrderObj objOrder = new OnlineOrderObj();       
        SessionUtility.AddValue("OnlineOrderObj", objOrder);

        IList<OptionalObj> objOptionals;
        objOptionals = new System.Collections.Generic.List<OptionalObj>();
        SessionUtility.AddOnlineOptionals(objOptionals);
        TmpOrderObj t = new TmpOrderObj();
        t.PageNo = -1;
        SessionUtility.AddTmpOrder(t); 

    }

    public static bool FillOrderObjAndSubmit(TmpOrderObj tmpOrderObj)
    {
        OnlineOrderObj order = SignupUtility.getOnlineOrder();
        bool bValid = true;
        try
        {

            string sParentLink = tmpOrderObj.ParentLink;
            string sSubLink = tmpOrderObj.SubLink;
            int iLinkTypeCode = 70;
            int iAgentCode = tmpOrderObj.AgentCode == null ? 972 : (int)tmpOrderObj.AgentCode;
            int? iSubAgentCode = tmpOrderObj.SubAgentCode;// == null ? null : (int)tmpOrderObj.SubAgentCode;
            string sGroupName = tmpOrderObj.GroupName;
            int iCompanyCode = 100;
            string sSiteSourceCountry = tmpOrderObj.SiteSourceCountry;

            order.LoadGeneralData(tmpOrderObj.SignupSourceID, sParentLink, sSubLink, iLinkTypeCode, iAgentCode,iSubAgentCode,  sGroupName, iCompanyCode, sSiteSourceCountry);

            string confimationEmail = string.IsNullOrEmpty(tmpOrderObj.ConfirmationEmail) ? "customerservice@telaway.co.uk" :tmpOrderObj.ConfirmationEmail;
            order.LoadData_SiteDetails(tmpOrderObj.BccEmail, tmpOrderObj.SalesEmail, tmpOrderObj.CustomerServicEmail, confimationEmail,tmpOrderObj.MainEmail,tmpOrderObj.InfoEmail,tmpOrderObj.TelawayPhone   );

            if (tmpOrderObj.ChargeWithPayPal && tmpOrderObj.PayPalAmountCharge.HasValue)
                order.LoadPayPalData(tmpOrderObj.PayPalTransactionId, Convert.ToDecimal(tmpOrderObj.PayPalAmountCharge));
            else
                order.LoadPayPalData("", 0);

            int iPhonesRequired = otherUtility.GetValidIntVal(tmpOrderObj.PhonesRequired, "PhonesRequired");
            //TODO
            int iEquipmentCode = otherUtility.GetValidIntVal(tmpOrderObj.EquipmentCode, "EquipmentCode");
            int iEquipmentModel = otherUtility.GetValidIntVal(tmpOrderObj.EquipmentModel, "EquipmentModel");
            string sEquipmentNotes = tmpOrderObj.EquipmentNotes;
            string sEquipmentName = tmpOrderObj.EquipmentName;

            List<SIMDetails> iSimDetails = tmpOrderObj.SimDetails;

            bool bIsKosher = otherUtility.GetBoolVal(tmpOrderObj.IsKosher, "IsKosher");
            bool bIsSim = otherUtility.GetBoolVal(tmpOrderObj.IsSim, "IsSim");
            bool bIsEquipmentSNS = otherUtility.GetBoolVal(tmpOrderObj.IsEquipmentSNS, "IsEquipmentSNS");

            //order.LoadData_Page1_1(iPhonesRequired, iEquipmentCode, iEquipmentModel, sEquipmentNotes, sEquipmentName, bIsKosher, bIsSim, bIsEquipmentSNS);
            order.LoadData_Page1_1(iPhonesRequired, iSimDetails, sEquipmentNotes, bIsKosher, bIsSim, bIsEquipmentSNS);

            //Terms-Others
            int iTermsCode = 0;
            string sTermsName = "";
            string sTag = tmpOrderObj.Tag;
            bool bInsurance = otherUtility.GetBoolVal(tmpOrderObj.Insurance, "Insurance");
            bool bSurfAndSave = otherUtility.GetBoolVal(tmpOrderObj.SurfAndSave, "SurfAndSave");
            bool bSpecial = otherUtility.GetBoolVal(tmpOrderObj.Special, "Special");
            bool bBitCallPackageOverageProtection = otherUtility.GetBoolVal(tmpOrderObj.bitCallPackageOverageProtection, "bitCallPackageOverageProtection");
            string sAccessory = tmpOrderObj.AccessoryIdAndQuantity;
            order.LoadData_Page1_4(iTermsCode, sTermsName, sTag, sAccessory, bInsurance, bSurfAndSave, bSpecial, bBitCallPackageOverageProtection);

            int iPlanCode = otherUtility.GetIntVal(tmpOrderObj.PlanCode, "PlanCode");
            int iProductId = otherUtility.GetIntVal(tmpOrderObj.ProductId, "ProductId");
            int iCallPackageCode = otherUtility.GetIntVal(tmpOrderObj.CallPackageCode, "CallPackageCode");
            string sCallPackageName = tmpOrderObj.CallPackageName;
            int iSMSPackageCounter = otherUtility.GetIntVal(tmpOrderObj.SMSPackageCounter, "SMSPackageCounter");
            int iSMSPackageCode = otherUtility.GetIntVal(tmpOrderObj.SMSPackageCode, "SMSPackageCode");
            string sSMSPackageName = tmpOrderObj.SMSPackageName;

            order.LoadData_Page1_2(iPlanCode, iProductId, iCallPackageCode, sCallPackageName, iSMSPackageCounter, iSMSPackageCode, sSMSPackageName);

            int iDataPackageId = otherUtility.GetIntVal(tmpOrderObj.DataPackageId, "DataPackageId");
            int iDataPackageCode = otherUtility.GetIntVal(tmpOrderObj.DataPackageCode, "DataPackageCode");
            string sDataPackageName = tmpOrderObj.DataPackageName;
            string sDataPackageSize = tmpOrderObj.DataPackgeSize;

            //from Chaya, now do'nt send KNT code
            string sKNTName = "";// tmpOrderObj.KNTName;
            int iKITD_PlanCode = -1;// otherUtility.GetIntVal(tmpOrderObj.KITD_PlanCode, "KITD_PlanCode");
            int iKNTRequired = 0;// otherUtility.GetIntVal(tmpOrderObj.KNTRequired, "KNTRequired");
            bool bKITD = false;// otherUtility.GetBoolVal(tmpOrderObj.KITD, "KITD");
            order.LoadData_Page1_3(iDataPackageId, iDataPackageCode, sDataPackageName, sDataPackageSize, sKNTName, iKITD_PlanCode, iKNTRequired, bKITD);

            string sUserName = otherUtility.GetStrVal(tmpOrderObj.UserName, "fill data page 2:  UserName ");
            string sUserStreet = otherUtility.GetStrVal(tmpOrderObj.UserStreet, "fill data page 2:  UserStreet ");
            string sUserCity = otherUtility.GetStrVal(tmpOrderObj.UserCity, "fill data page 2:  UserCity ");
            string sPWD = otherUtility.GetStrVal(tmpOrderObj.PWD, "fill data page 2:  PWD");

            OnlineOrderObj o = getOnlineOrder();

            tmpOrderObj.ClientHomePhone1 = string.IsNullOrEmpty(tmpOrderObj.ClientHomePhone1) ? o.ClientHomePhone1 : tmpOrderObj.ClientHomePhone1;
            if (string.IsNullOrEmpty(tmpOrderObj.ClientHomePhone1))
                tmpOrderObj.ClientHomePhone1=SessionUtility.GetValue("hdPhone");
            string sClientHomePhone1 = otherUtility.GetStrVal(tmpOrderObj.ClientHomePhone1, "fill data page 2:  ClientHomePhone1 ");
            //string sClientHomePhone2 = otherUtility.GetStrVal(tmpOrderObj.ClientHomePhone2, "fill data page 2:  ClientHomePhone2 ");
            string sClientFax = otherUtility.GetStrVal(tmpOrderObj.ClientFax, "fill data page 2:  ClientFax");

            tmpOrderObj.ClientMobile = string.IsNullOrEmpty(tmpOrderObj.ClientMobile) ? o.ClientMobile : tmpOrderObj.ClientMobile;
            if(string.IsNullOrEmpty(tmpOrderObj.ClientMobile))
                tmpOrderObj.ClientMobile= SessionUtility.GetValue("hdnCell"); 
            string sClientMobile = otherUtility.GetStrVal(tmpOrderObj.ClientMobile, "fill data page 2:  ClientMobile ");
            string sClientEmail = otherUtility.GetStrVal(tmpOrderObj.ClientEmail, "fill data page 2:  ClientEmail ");
            string sClientStreet = otherUtility.GetStrVal(tmpOrderObj.ClientStreet, "fill data page 2:  ClientStreet ");

            tmpOrderObj.ClientCity = string.IsNullOrEmpty(tmpOrderObj.ClientCity) ? o.ClientCity : tmpOrderObj.ClientCity;
            string sClientCity = otherUtility.GetStrVal(tmpOrderObj.ClientCity, "fill data page 2:  ClientCity ");
            string sClientCountry = otherUtility.GetStrVal(tmpOrderObj.ClientCountry, "fill data page 2:  ClientCountry ");
            string sClientState = otherUtility.GetStrVal(tmpOrderObj.ClientState, "fill data page 2:  ClientState ");
            string sClientZip = otherUtility.GetStrVal(tmpOrderObj.ClientZip, "fill data page 2:  ClientZip ");
            DateTime dStartDate = Convert.ToDateTime(tmpOrderObj.StartDate);
            DateTime dEndDate = Convert.ToDateTime(tmpOrderObj.EndDate);
            order.LoadData_Page2(sUserName, sUserStreet, sUserCity, sPWD, sClientHomePhone1, sClientFax, sClientMobile, sClientEmail, sClientStreet, sClientCity, sClientCountry, sClientState, sClientZip, sTag, dStartDate, dEndDate);

            if (tmpOrderObj.SimDetails[0].SimPrice>0)
                tmpOrderObj.PurchaseEquipment = true;
            else 
                tmpOrderObj.PurchaseEquipment = false;
            tmpOrderObj.ShipName = otherUtility.GetStrVal(dbUtility.Capitalize(tmpOrderObj.ShipName), "ShipName");
            tmpOrderObj.ShipStreet = otherUtility.GetStrVal(dbUtility.Capitalize(tmpOrderObj.ShipStreet), "ShipStreet");
            tmpOrderObj.ShipCity = otherUtility.GetStrVal(dbUtility.Capitalize(tmpOrderObj.ShipCity), "ShipCity");
            tmpOrderObj.ShipState = otherUtility.GetStrVal(dbUtility.Capitalize(tmpOrderObj.ShipState), "ShipState");
            tmpOrderObj.ShipPostalCode = otherUtility.GetStrVal(tmpOrderObj.ShipPostalCode, "ShipPostalCode");
            tmpOrderObj.ShipPhone = otherUtility.GetStrVal(tmpOrderObj.ShipPhone, "ShipPhone");
            tmpOrderObj.ShipCountry = otherUtility.GetStrVal(dbUtility.Capitalize(tmpOrderObj.ShipCountry), "ShipCountry");
            tmpOrderObj.BaseCode = otherUtility.GetIntVal(tmpOrderObj.BaseCode, "BaseCode");
            tmpOrderObj.ShipMethod = otherUtility.GetStrVal(tmpOrderObj.ShipMethod, "ShipMethod");
            tmpOrderObj.ShipEmail = otherUtility.GetStrVal(tmpOrderObj.ShipEmail, "ShipEmail");
            tmpOrderObj.ShipCommercial = false;
            tmpOrderObj.ShipFee = otherUtility.GetDecimalVal(Convert.ToDouble(tmpOrderObj.ShipFee), "DeliveryFee");
            tmpOrderObj.ShippingName = otherUtility.GetStrVal(tmpOrderObj.ShippingName, "ShippingName");
            tmpOrderObj.ShipDate = otherUtility.GetDateTimeVal(tmpOrderObj.ShipDate, "shipDate");
            tmpOrderObj.DepartureDate = otherUtility.GetDateTimeVal(tmpOrderObj.ShipDate, "shipDate");//#########

            order.LoadData_Page3_1(Convert.ToBoolean(tmpOrderObj.PurchaseEquipment), tmpOrderObj.ShipName, tmpOrderObj.ShipStreet, tmpOrderObj.ShipCity, tmpOrderObj.ShipState, tmpOrderObj.ShipPostalCode, tmpOrderObj.ShipPhone, tmpOrderObj.ShipCountry, Convert.ToInt32(tmpOrderObj.BaseCode));
            order.LoadData_Page3_2(tmpOrderObj.ShipMethod, Convert.ToBoolean(tmpOrderObj.ShipCommercial), Convert.ToDecimal(tmpOrderObj.ShipFee), tmpOrderObj.ShippingName, Convert.ToDateTime(tmpOrderObj.ShipDate), Convert.ToDateTime(tmpOrderObj.DepartureDate), tmpOrderObj.BaseNotes);


            order.LoadData_Page4(tmpOrderObj.CCTitle, Convert.ToInt32(tmpOrderObj.DataPackageCode), tmpOrderObj.DataPackageName, tmpOrderObj.ClientFirstName, tmpOrderObj.ClientLastName, tmpOrderObj.CCTitle, Convert.ToDateTime(tmpOrderObj.CCExpDate), tmpOrderObj.CCNum, tmpOrderObj.ShipEmail,tmpOrderObj.CouponCode , tmpOrderObj.CustomerComment, sTag);//,
        }
        catch (Exception e)
        {
            string errorMsg = "";
            if (tmpOrderObj.ChargeWithPayPal && tmpOrderObj.PayPalAmountCharge > 0)
                errorMsg += "Paypal Order <br/>";
            emailUtility.SendMailErr("Error while loading data in to order object " + e.Message +
                " <br/>" + errorMsg + SessionUtility.GetValue("InvalidData") +
                " <br/>" + SessionUtility.GetValue("ValidErr") +
                " <br/>" + SessionUtility.GetValue("AddOrderErr") +
                " <br/>" + SessionUtility.GetValue("AddOptionalErr") +
                " <br/>" + SessionUtility.GetValue("errMsg") +
                " <br/>" + e.StackTrace +
                " <br/>" + e.TargetSite  
                +" <br/><br/>" + GetOnlineOrderSession()
                );
            bValid = false;
            if (SessionUtility.GetValue("ErrSignup") != "ReloadDone")
                SessionUtility.AddValue("ErrSignup", "Reload");
            return false;
        }



        if ((tmpOrderObj.PayPalTransactionId.Trim() == "" || tmpOrderObj.PayPalTransactionId.Trim() == "Err") && tmpOrderObj.ChargeWithPayPal )
        {
            bValid = false;
            string sErr = "";
            if (tmpOrderObj.PayPalAmountCharge != null)
                sErr = sErr + " tmpOrderObj.PayPalAmountCharge:" +tmpOrderObj.PayPalAmountCharge.ToString();

            sErr = sErr +" PayPalLogCounter"+tmpOrderObj.PayPalLogCounter.ToString() +  " tmpOrderObj.PayPalTransactionId" + tmpOrderObj.PayPalTransactionId+ " ";  
            SessionUtility.AddValue("InvalidData", "Invalid PayPal Transaction Id" + sErr);
            emailUtility.SendMailErr("Error while adding an order: Invalid PayPal Transaction Id" + sErr);
            return false;
        }

       

        if (bValid)
        {
            if (SignupUtility.AddTheOrder())
            {
               
                SignupUtility.SetOrderSubmited();
                order = SignupUtility.getOnlineOrder();

                emailUtility.SendTailEmailConfirmation(order, false);
                emailUtility.SendTailEmailConfirmation(order, true);

                if (tmpOrderObj.ChargeWithPayPal && tmpOrderObj.PayPalAmountCharge > 0)
                {
                    dbUtility.UpdatePayPalLogOrder(tmpOrderObj.PayPalLogCounter, Convert.ToInt32(order.OnlineOrderCode));// get order online
                    dbUtility.UpdatePayPalTransaction(tmpOrderObj.PayPalLogCounter, tmpOrderObj.PayPalTransactionId);

                    // send email 
                    string sOnlineCode="";
                    for (int i = 0; i < order.SimDetails.Count; i++)
                    {
                        if (order.SimDetails[i].OnlineOrderCode.HasValue) 
                            sOnlineCode = sOnlineCode + " #" + order.SimDetails[i].OnlineOrderCode.ToString()+" ";
                    }
                    string strBodyPaypal = string.Format(@"<b>Date</b>: {2} <br /> <b>Order number:</b> {0} <br /> <b>Name:</b> {3} <br /> <b>Dollar Amount Charged with Paypal:</b> ${1}. ", sOnlineCode, tmpOrderObj.PayPalAmountCharge, DateTime.Now.ToString(), tmpOrderObj.UserNamePayPal);
                    emailUtility.SendGeneralMail(order.MainEmail, "web-marketing@talknsave.net", strBodyPaypal, tmpOrderObj.UserNamePayPal + " - Paypal order", "Web-Master@talknsave.net", order.SiteSourceCountry);
                }
                tmpOrderObj.SessionID = "";
                SessionUtility.AddTmpOrder(tmpOrderObj);
                return true;

            }
        }
        else
        {

            if (SessionUtility.GetIntValue("OnlineNewOrderCode") == 0)
            {
                //lblMsg.Text = "Error while adding an order " + SessionUtility.GetValue("InvalidData") + " " + SessionUtility.GetValue("ValidErr") + " " + SessionUtility.GetValue("AddOrderErr") + SessionUtility.GetValue("AddOptionalErr") + SessionUtility.GetValue("errMsg");
                emailUtility.SendMailErr("Error while adding an order: " + SessionUtility.GetValue("InvalidData") + " " + SessionUtility.GetValue("ValidErr") + " " + SessionUtility.GetValue("AddOrderErr") + SessionUtility.GetValue("AddOptionalErr") + SessionUtility.GetValue("errMsg"));
                return false;
            }
            else
            {
                SignupUtility.SetOrderSubmited();
                return true;
            }

        }
        if (SessionUtility.GetIntValue("OnlineNewOrderCode") == 0)
        {
            emailUtility.SendMailErr("Error while adding an order " + SessionUtility.GetValue("InvalidData") + " " + SessionUtility.GetValue("ValidErr") + " " + SessionUtility.GetValue("AddOrderErr") + SessionUtility.GetValue("AddOptionalErr") + SessionUtility.GetValue("errMsg"));
            return false;
        }
        else
            return true;
    }

    public static void DeleteOptionals()
    {
        IList<OptionalObj> OptionalsList = SessionUtility.GetOnlineOptionals();
        if (OptionalsList != null)
            OptionalsList.Clear();
        
        SessionUtility.AddOnlineOptionals(OptionalsList);
    }

    public static void SetSignupStatus(int iStatus)
    {
        SessionUtility.AddValue("status",iStatus);
    }
   
    public static void DeleteOrder()
    {
        OnlineOrderObj Order = getOnlineOrder();       
        Order = null;
        SessionUtility.AddValue("OnlineOrderObj", null);
    }




    public static DataTable getPlanInfo()
    {

        DataTable dtPlanLink = SessionUtility.GetDTValue("dtPlanLink");

        if (dtPlanLink == null)
        {
            dtPlanLink = dbUtility.getPlanInfo(SessionUtility.GetIntValue("PlanId"));
            SessionUtility.AddValue("dtPlanLink", dtPlanLink);
            return dtPlanLink;
        }
        else
            return dtPlanLink;
    }


    public static DataTable ReloadPlan()
    {

        DataTable dtPlanLink;
        dtPlanLink = dbUtility.getPlanInfo(SessionUtility.GetIntValue("PlanId"));
        SessionUtility.AddValue("dtPlanLink", dtPlanLink);
        return dtPlanLink;

    }
    //public static DataTable getSubLinkData()
    //{

    //    DataTable dt = SessionUtility.GetDTValue("dsSubLink");

    //    if (dt == null)
    //    {
    //        dt = dbUtility.getSubLinkData();
    //        SessionUtility.AddValue("dsSubLink", dt);
    //        return dt;
    //    }
    //    else
    //        return dt;
    //}

    //public static string getParentLinkField(string sFieldName)
    //{
    //    DataTable dt = dbUtility.getParentLinkInfo(getSubLinkField("ParentLink"));
    //    string  iValue = "";
    //    try
    //    {
    //        if (dt != null)
    //            if (dt.Rows.Count > 0)
    //                iValue = dt.Rows[0][sFieldName].ToString();
    //    }
    //    catch
    //    {
    //        return "";
    //    }
    //    return iValue;
    //}

    private static string GetBooleanField(bool? bField)
    {
        if (bField.HasValue)
            return bField.ToString();
        else
            return "null";
    }

    public static string GetOrderSession()
    {
        string ret="";
        TmpOrderObj o = SessionUtility.getTmpOrder();
        if (o != null )
        {
            ret = ret + "<br/><b>boolean fields: </b><br/>" ;
            //ret = ret + "<br/><b>Insurance: </b>" + GetBooleanField(o.Insurance);
            //ret = ret + "<br/><b>PurchaseEquipment: </b>" + GetBooleanField(o.PurchaseEquipment);
            //ret = ret + "<br/><b>ShipCommercial: allow null </b>" + GetBooleanField(o.ShipCommercial);
            //ret = ret + "<br/><b>Special: allow null </b>" + GetBooleanField(o.Special);
            //ret = ret + "<br/><b>KITD: allow null </b>" + GetBooleanField(o.KITD);
            //ret = ret + "<br/><b>KITD_BLOCK_ID: allow null </b>" + GetBooleanField(o.KITD_BLOCK_ID);
            //ret = ret + "<br/><b>SurfAndSave: allow null </b>" + GetBooleanField(o.SurfAndSave);
            //ret = ret + "<br/><b>CreditEquipmentPurchase: allow null </b>" + GetBooleanField(o.CreditEquipmentPurchase);
            //ret = ret + "<br/><b>CallPackageOverageProtection: allow null </b>" + GetBooleanField(o.bitCallPackageOverageProtection) + "<br/>";


           
        }
        return ret;
    }


    public static string GetOnlineOrderSession()
    {
        string ret = "";
        TmpOrderObj o = SessionUtility.getTmpOrder();
        if (o != null)
        {

            if (o.ParentLink != null)
                ret = ret + "<br/><b> ParentLink:</b>" + o.ParentLink.ToString();

            if (o.SubLink != null)
                ret = ret + "<br/><b> SubLink:</b>" + o.SubLink.ToString();

            if (o.EquipmentCode != null)
                ret = ret + "<br/><b> EquipmentCode:</b>" + o.EquipmentCode.ToString();

            if (o.EquipmentModel != null)
                ret = ret + "<br/><b> EquipmentModel:</b>" + o.EquipmentModel.ToString();

            if (o.LinkTypeCode != null)
                ret = ret + "<br/><b> LinkTypeCode:</b>" + o.LinkTypeCode.ToString();


            if (o.EquipmentName != null)
                ret = ret + "<br/><b> EquipmentName:</b>" + o.EquipmentName.ToString();

            if (o.PhonesRequired > 0)
                ret = ret + "<br/><b> Phones Required:</b>" + o.PhonesRequired.ToString();

            if (o.Insurance != null)
                ret = ret + "<br/><b> Insurance:</b>" + o.Insurance.ToString();

            //if (o.PlanCode != null)
            //    ret = ret + "<br/><b> PlanCode:</b>" + o.PlanCode.ToString();

            if (o.PlanName != null)
                ret = ret + "<br/><b> PlanName:</b>" + o.PlanName.ToString();


            if (o.CallPackageCode != null)
                ret = ret + "<br/><b> CallPackageCode:</b>" + o.CallPackageCode.ToString();

            if (o.CallPackageName != null)
                ret = ret + "<br/><b> CallPackageName:</b>" + o.CallPackageName.ToString();

            if (o.AgentCode != null)
                ret = ret + "<br/><b> AgentCode:</b>" + o.AgentCode.ToString();

            if (o.SMSPackageCode != null)
                ret = ret + "<br/><b> SMSPackageCode:</b>" + o.SMSPackageCode.ToString();

            if (o.SMSPackageName != null)
                ret = ret + "<br/><b> SMSPackageName:</b>" + o.SMSPackageName.ToString();

            if (o.KITD_PlanCode != null)
                ret = ret + "<br/><b> KNT_Code:</b>" + o.KITD_PlanCode.ToString();

            if (o.KNTName != null)
                ret = ret + "<br/><b> KNT Name:</b>" + o.KNTName.ToString();

            if (o.KNTRequired > 0)
                ret = ret + "<br/><b> KNT Required:</b>" + o.KNTRequired.ToString();

            if (o.DataPackageCode != null)
                ret = ret + "<br/><b> DataPackageCode:</b>" + o.DataPackageCode.ToString();

            if (o.DataPackageId > -1)
                ret = ret + "<br/><b> DataPackageId:</b>" + o.DataPackageId.ToString();

            if (o.DataPackageName != null)
                ret = ret + "<br/><b> DataPackageName:</b>" + o.DataPackageName.ToString();

            if (o.IsKosher != null)
                if (Convert.ToBoolean(o.IsKosher))
                    ret = ret + "<br/><b> IsKosher:</b>" + o.IsKosher.ToString();

            if (o.IsSim != null)
                if (Convert.ToBoolean(o.IsSim))
                    ret = ret + "<br/><b> IsSim:</b>" + o.IsSim.ToString();

            if (o.IsEquipmentSNS != null)
                if (Convert.ToBoolean(o.IsEquipmentSNS))
                    ret = ret + "<br/><b> IsEquipmentSNS:</b>" + o.IsEquipmentSNS.ToString();

            string sOptionals = "";// dbUtility.getOptionals();

            if (sOptionals != "")
                ret = ret + "<br/><b> Optionals:</b>" + sOptionals;

            if (o.UserName != null)
                ret = ret + "<br/><b> UserName:</b>" + o.UserName.ToString();

            if (o.UserStreet != null)
                ret = ret + "<br/><b> UserStreet:</b>" + o.UserStreet.ToString();

            if (o.UserCity != null)
                ret = ret + "<br/><b> UserCity:</b>" + o.UserCity.ToString();

            if (o.ClientHomePhone1 != null)
                ret = ret + "<br/><b> ClientHomePhone1:</b>" + o.ClientHomePhone1.ToString();
            else ret = ret + "<br/><b> ClientHomePhone1:</b>null";
            if (o.ClientHomePhone2 != null)
                ret = ret + "<br/><b> ClientHomePhone2:</b>" + o.ClientHomePhone2.ToString();

            if (o.ClientFax != null)
                ret = ret + "<br/><b> ClientFax:</b>" + o.ClientFax.ToString();

            if (o.ClientMobile != null)
                ret = ret + "<br/><b> ClientMobile:</b>" + o.ClientMobile.ToString();
            else ret = ret + "<br/><b> ClientMobile:</b>null";
            if (o.ClientEmail != null)
                ret = ret + "<br/><b> ClientEmail:</b>" + o.ClientEmail.ToString();

            if (o.ClientStreet != null)
                ret = ret + "<br/><b> ClientStreet:</b>" + o.ClientStreet.ToString();

            if (o.ClientCity != null)
                ret = ret + "<br/><b> ClientCity:</b>" + o.ClientCity.ToString();

            if (o.ClientState != null)
                ret = ret + "<br/><b> ClientState:</b>" + o.ClientState.ToString();

            if (o.ClientCountry != null)
                ret = ret + "<br/><b> ClientCountry:</b>" + o.ClientCountry.ToString();

            if (o.ClientZip != null)
                ret = ret + "<br/><b> ClientZip:</b>" + o.ClientZip.ToString();

            if (o.Tag != null)
                ret = ret + "<br/><b> Tag:</b>" + o.Tag.ToString();

            if (o.StartDate != null)
                ret = ret + "<br/><b> StartDate:</b>" + o.StartDate.ToString();

            if (o.EndDate != null)
                ret = ret + "<br/><b> EndDate:</b>" + o.EndDate.ToString();

            if (o.PurchaseEquipment != null)
                ret = ret + "<br/><b> PurchaseEquipment:</b>" + o.PurchaseEquipment.ToString();

            if (o.ShipName != null)
                ret = ret + "<br/><b> ShipName:</b>" + o.ShipName.ToString();

            if (o.ShipStreet != null)
                ret = ret + "<br/><b> ShipStreet:</b>" + o.ShipStreet.ToString();

            if (o.ShipCity != null)
                ret = ret + "<br/><b> ShipCity:</b>" + o.ShipCity.ToString();

            if (o.ShipState != null)
                ret = ret + "<br/><b> ShipState:</b>" + o.ShipState.ToString();

            if (o.ShipPostalCode != null)
                ret = ret + "<br/><b> ShipPostalCode:</b>" + o.ShipPostalCode.ToString();

            if (o.ShipCountry != null)
                ret = ret + "<br/><b> ShipCountry:</b>" + o.ShipCountry.ToString();

            if (o.BaseCode != null)
                ret = ret + "<br/><b> BaseCode:</b>" + o.BaseCode.ToString();

            if (o.ShipMethod != null)
                ret = ret + "<br/><b> ShipMethod:</b>" + o.ShipMethod.ToString();

            if (o.ShipFee != null)
                ret = ret + "<br/><b> ShipFee:</b>" + o.ShipFee.ToString();

            if (o.ShippingName != null)
                ret = ret + "<br/><b> ShippingName:</b>" + o.ShippingName.ToString();

            if (o.ShipDate != null)
                ret = ret + "<br/><b> ShipDate:</b>" + o.ShipDate.ToString();

            if (o.DepartureDate != null)
                ret = ret + "<br/><b> DepartureDate:</b>" + o.DepartureDate.ToString();

            if (o.BaseNotes != null)
                ret = ret + "<br/><b> BaseNotes:</b>" + o.BaseNotes.ToString();

            if (o.ClientFirstName != null)
                ret = ret + "<br/><b> ClientFirstName:</b>" + o.ClientFirstName.ToString();

            if (o.ClientLastName != null)
                ret = ret + "<br/><b> ClientLastName:</b>" + o.ClientLastName.ToString();

            if (o.CCExpDate != null)
                ret = ret + "<br/><b> CCExpDate:</b>" + o.CCExpDate.ToString();

            if (o.CCNum != null)
            {
                if (o.CCNum.Length > 4)
                    ret = ret + "<br/><b> CCNum:</b>" + o.CCNum.ToString().Substring(o.CCNum.Length - 4, 4);
                else
                    ret = ret + "<br/><b> CCNum:</b>" + o.CCNum.ToString();
            }
            if (o.CCTitle != null)
                ret = ret + "<br/><b> CCTitle:</b>" + o.CCTitle.ToString();

            if (o.ShipEmail != null)
                ret = ret + "<br/><b> ShipEmail:</b>" + o.ShipEmail.ToString();

            if (o.CouponCode != null)
                ret = ret + "<br/><b> CouponCode:</b>" + o.CouponCode.ToString();

            if (o.CustomerComment != null)
                ret = ret + "<br/><b> CustomerComment:</b>" + o.CustomerComment.ToString();

            ret = ret + "<br/><b> bSubmitOrder:</b>" + o.bSubmitOrder.ToString();

            ret = ret + "<br/><b>Show Order Submited: </b>" + SessionUtility.GetBoolValue("showOrderSubmited");

        }
        return ret;
    }

    public static int getIntParentLinkField(string sFieldName)
    {
        DataTable dt = null;// dbUtility.getParentLinkInfo(getSubLinkField("ParentLink"));
        int iValue = -1;
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    iValue = (int)dt.Rows[0][sFieldName];
        }
        catch
        {
            return -1;
        }
        return iValue;
    }

    public static string  getStrParentLinkField(string sFieldName)
    {
        DataTable dt = null;// dbUtility.getParentLinkInfo(getSubLinkField("ParentLink"));
        string sValue = "";
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = (string)dt.Rows[0][sFieldName];
        }
        catch
        {
            return "";
        }
        return sValue;
    }

    public static string getStrParentLinkField(string sFieldName, string sParentLink)
    {
        DataTable dt = null;// dbUtility.getParentLinkInfo(sParentLink);
        string sValue = "";
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = (string)dt.Rows[0][sFieldName];
        }
        catch
        {
            return "";
        }
        return sValue;
    }
    public static Boolean getBoolPlanField(string sFieldName)
    {
        Boolean sValue = false;

        DataTable dt = getPlanInfo();

        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = (Boolean)dt.Rows[0][sFieldName];
        }
        catch
        {
            return false ;
        }
        return sValue;

    }


    public static Boolean getBoolParentLinkField(string sFieldName, string sParentLink)
    {
        Boolean bValue = false;
        DataTable dt = null;// dbUtility.getParentLinkInfo(sParentLink);
        
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    bValue = (bool)dt.Rows[0][sFieldName];
                                        
                }
        }
        catch 
        {
            
            return false ;
        }

        return bValue;

    }

    public static decimal getDecimalPlanField(string sFieldName)
    {
        decimal sValue = 0;

        DataTable dt =getPlanInfo();

        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = Convert.ToDecimal(dt.Rows[0][sFieldName]);
        }
        catch
        {
            return 0;
        }
        return sValue;

    }
    public static DateTime? getDTSubLinkField(string sFieldName)
    {
        DateTime? sValue = null;

        DataTable dt = null;// getSubLinkInfo();
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = (DateTime)dt.Rows[0][sFieldName];
        }
        catch
        {
            return null;
        }
        return sValue;

    }

    public static int getIntPlanField(string sFieldName)
    {
        int iValue = 0;

        DataTable dt = getPlanInfo();
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    iValue =Convert.ToInt32(dt.Rows[0][sFieldName].ToString());
        }
        catch
        {
            return 0;
        }
        return iValue;

    }


    public static string getPlanField(string sFieldName)
    {
        string sValue = "";

        DataTable dt = getPlanInfo();
        try
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                    sValue = dt.Rows[0][sFieldName].ToString();
        }
        catch
        {
            return "";
        }
        return sValue;

    }



    public static void SetOrderSubmited()
    {
        OnlineOrderObj order = getOnlineOrder();       
        if (order != null)
            order.SetOrderSubmited(); 
       
        SessionUtility.AddValue("OnlineOrderObj", order);
    }

    public static void SetOrderFullSubmited()
    {
        OnlineOrderObj order = getOnlineOrder();
        if (order != null)
            order.SetOrderFullSubmited();

        SessionUtility.AddValue("OnlineOrderObj", order);
    }
    public static bool AddTheOrder()
    {
        //var cus = SessionUtility.GetIntValue("OnlineNewCustomer");
        //    if (SessionUtility.GetIntValue("OnlineNewCustomer") > 0)
        //        return false;

            OnlineOrderObj order = getOnlineOrder();
            if (order != null)
            {
                if (order.bSubmitOrder != null)
                    if (Convert.ToBoolean(order.bSubmitOrder))
                        return false;

                if (order.OnlineOrderCode == null)
                {
                    if (SubmitOrder(order))
                    {
                        wsUtility.DownloadOrder();
                        return true;
                    }

                }
            }
        
        return false;
    }

    public static bool SubmitOrder(OnlineOrderObj order)
    {
        int iNewCustomer = 0;
        int PhoneAdded = 0;
        SessionUtility.AddValue("iParentOrderCode", -1); 
        SessionUtility.AddValue("OnlineNewOrderCode", 0);
        SessionUtility.AddValue("OnlineNewCustomer", 0);



        int iParentOrderCode = SessionUtility.GetIntValue("iParentOrderCode");

        try
        {
            //Add the order
         
            for (int i = 1; i <= order.PhonesRequired; i++)
            {

                SIMDetails sd = order.SimDetails[i-1];
                order.LoadData_SimInfo(sd,i);
                order.LoadOrderCoupon(i);

                try
                {

                    if (iParentOrderCode > -1)
                        order.ParentOnlineOrderCode = iParentOrderCode;
                    if (i > 1) { order.Tag = ""; order.AccessoryIdAndQuantity = ""; }
                    else
                    {
                        if (order.PhonesRequired > 1)
                            order.ParentOnlineOrderCode = -1;
                        else
                            order.ParentOnlineOrderCode = null;
                    }

                    iNewCustomer = order.Add();
                    if (i == 1 && iParentOrderCode == -1)
                    {
                        iParentOrderCode = iNewCustomer;

                        if (order.PhonesRequired > 1)
                            SessionUtility.AddValue("iParentOrderCode", iNewCustomer);

                        order.ParentOnlineOrderCode = iParentOrderCode;
                    }
                    decimal dTotalAccessories = 0;
					if (SessionUtility.GetDecimalValue("AccessoriesPrice")>0) 
						dTotalAccessories = SessionUtility.GetDecimalValue("AccessoriesPrice");
                    decimal dTotal = i == 1 ? SessionUtility.GetDecimalValue("totalAmountCharged") + dTotalAccessories : 0;
                    decimal dAllTotal = SessionUtility.GetDecimalValue("totalAmountCharged");

                    sd.OnlineOrderCode = iNewCustomer;

                    order.AddOrderToMyTable(iNewCustomer, dTotal, dAllTotal);
                }
                catch (Exception e)
                {
                    emailUtility.SendMailErr("Error when doing order.add()<br/> " + e.Message);
                }

                PhoneAdded++;

                if ((order.ErrorMessage != "") && (order.ErrorMessage != null))
                {
                    SessionUtility.AddValue("AddOrderErr", order.ErrorMessage);
                    return false;
                }
            }

            if (order.PhonesRequired<=0)
                SessionUtility.AddValue("InvalidData", " Phones Required = 0 " );



            if (iNewCustomer > 0)
            {
                SignupUtility.SetOrderSubmited();
                //SessionUtility.AddValue("OnlineNewCustomer", iNewCustomer);
                SessionUtility.AddValue("OrderSubmited", true);   
                return true;
            }
            else
            {
                SessionUtility.AddValue("ValidErr", "Error Adding New Order!!!");
                return false;
            }
        }//try
       
        catch (Exception ex)
        {
            SessionUtility.AddValue("errMsg", ex.Message+ " Phone Added = " + PhoneAdded.ToString()+" " );
            emailUtility.SendGeneralMail(order.MainEmail, "Web-Master@talknsave.net", ex.Message, "error with order from: ", "Web-Master@talknsave.net", order.SiteSourceCountry);
            return false;
        }

    }

    public static bool AddOrderOptionals(OnlineOrderObj order)
    {
        try
        {
            OptionalObj Option;
            OnlineOrderObj newOrder;            
            IList<OptionalObj> OptionalsList = SessionUtility.GetOnlineOptionals();
            int newOrderCode = -1;
            int newOnlineOrderCode;
            
            bool isAdded = false;
            for (int i = 0; i < OptionalsList.Count; i++)
            {
                Option = OptionalsList[i];

                if (Option.PlanCode > -1 && Option.EquipmentCode > -1)
                {
                    newOrder = new OnlineOrderObj();
                    newOrder = newOrder.GetOrderFieldsForOptionals(Option, newOrder, order); 

                    
                    for (int j = 0; j < Option.Quantity; j++)
                    {
                        try
                        {
                            if (newOrderCode > -1)
                                newOrder.ParentOnlineOrderCode = newOrderCode;
                            else
                                newOrder.ParentOnlineOrderCode = -1;                                
                           
                            newOnlineOrderCode = newOrder.Add();
                            if (newOnlineOrderCode > 0)
                            {
                                isAdded = true;
                                if (newOrderCode == -1)
                                    newOrderCode = newOnlineOrderCode; 
                            }
                            else
                            {
                                if ((newOrder.ErrorMessage != "") && (newOrder.ErrorMessage != null))
                                {
                                    SessionUtility.AddValue("AddOptionalErr", newOrder.ErrorMessage);
                                    emailUtility.SendMailErr("Err Add Optional:" + newOrder.ErrorMessage);
                                }
                                isAdded = false;

                            }
                        }
                        catch (Exception ex)
                        {
                            SessionUtility.AddValue("ValidErr", ex.ToString());
                            SessionUtility.AddValue("errMsg", ex.Message);
                            SessionUtility.AddValue("AddOptionalErr", ex.Message);
                            isAdded = false;
                        }
                    }//for j                

                }//if
                else
                  isAdded = true;
            }//for i

            SessionUtility.AddValue("iParentOrderCode", newOrderCode);  
            return isAdded;
        }
        catch (Exception ex)
        {
            SessionUtility.AddValue("ValidErr", ex.ToString());
            SessionUtility.AddValue("errMsg", ex.Message);
            SessionUtility.AddValue("AddOptionalErr", ex.Message);
            return false;
        }
    }
}
