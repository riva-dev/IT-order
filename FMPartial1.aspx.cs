using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PayPal.Manager;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Services;


public partial class FMPartial : System.Web.UI.Page
{

    #region WebMethod
    [WebMethod]
    public static string UpdateErrHtml1(string sName, string isValid)
    {
        SessionUtility.AddValue(sName, Convert.ToBoolean(isValid));
        return dbUtility.getHtmlErrMsg();
    }

    [WebMethod]
    public static string UpdateErrHtml(string sName, string isValid)
    {
        if (SessionUtility.GetBoolValue("FirstTime"))
        {
            SessionUtility.AddValue(sName, Convert.ToBoolean(isValid));
            return dbUtility.getHtmlErrMsg();
        }
        else
            return "";
    }

    [WebMethod]
    public static string BuildPackageDiv(string SimQnt, string NanoSimTxt, string MicroSimTxt, string SimTxt, string sKntTitle, string commentDiv)
    {
        try
        {
            string newPackageDiv = "";
            SessionUtility.AddValue("SimQnt", SimQnt);
            string sCountryCode = SessionUtility.GetValue("UserCountry");
            DataTable dtRegionAreas = dbUtility.getTableBySQLTAIL("SELECT site, KNTCode,Name  FROM tblTelawayCountriesRegions where IsGeneralKNT=0 and IsActive=1 and site='" + sCountryCode + "' order by orderid");
            DataTable dtRegionGeneralAreas = dbUtility.getTableBySQLTAIL("SELECT site, KNTCode,Name  FROM tblTelawayCountriesRegions where IsGeneralKNT=1  and IsActive=1 and site='" + sCountryCode + "' order by orderid");
            int iCount = dbUtility.ExecIntScalar("select count(*) from tblTelawayCountriesRegions where site='" + sCountryCode + "' and IsActive=1 ");
            int KNTCodeDefault = -1;

            if (iCount == 1)
                KNTCodeDefault = dbUtility.ExecIntScalar("select KNTCode from tblTelawayCountriesRegions where site='" + sCountryCode + "' and IsActive=1");

            string sSelect = "";
            string sOption = "";
            string sRegionName = "";
            string sRegionCode = "";
            string sCountryName = "";

            int iSimQnt = Convert.ToInt32(SimQnt);

            for (int i = 1; i <= iSimQnt; i++)
            {
                newPackageDiv = newPackageDiv + string.Format(@"<div  class='col-lg-4 col-md-4 col-sm-6 col-xs-12 grid cs-style-3 pricing-box packageDetails3' id='packageDiv{0}'>" +
                                        "<b><h3 >Package {0}:</h3> </b> <hr>" +
                                        "<div id='divSimType_{0}' >" +
                                          "<input type='radio' name='xSimtype_{0}' value='1440'  onClick='onSimTypeClick(divSimType_{0});' />" + NanoSimTxt +
                                         "<br/>" +
                                          "<input type='radio' name='xSimtype_{0}' value='1430'  onClick='onSimTypeClick(divSimType_{0});' />" + MicroSimTxt +
                                          "<br/>" +
                                          "<input type='radio' name='xSimtype_{0}' value='1250'  onClick='onSimTypeClick(divSimType_{0});' />" + SimTxt +
                                          "<br/><br/>" +
                                        "</div>	" +
                                   " ", i);

                if (iCount > 1)
                {
                    SessionUtility.AddValue("DefaultKNTCode", -1);
                    sSelect = //"<span class='simTitle'>Sim " + i.ToString() + ":</span>
                        "<hr><span id='KntTitle'>" + sKntTitle + "</span><br><br><select  class='form-control' onChange='onSelectRegionClick(this);' id='SelectRegion" + i.ToString() + "'>";
                    sSelect = sSelect + "<option style='font-weight:bold;' value='0'>(" + otherUtility.getResourceString("ChooseOne") + ")</option>";

                    if (dtRegionAreas != null)
                    {
                        foreach (DataRow dr in dtRegionAreas.Rows)
                        {
                            sRegionName = dr["Name"].ToString();
                            sRegionCode = dr["KNTCode"].ToString();
                            sCountryName = dr["site"].ToString();

                            sOption = "<option  style='font-weight:normal;' value='" + sRegionCode + "'>" + sRegionName + "</option>";
                            sSelect = sSelect + sOption;


                        }
                    }



                    if (dtRegionGeneralAreas != null)
                    {
                        if (dtRegionGeneralAreas.Rows.Count > 0 && sCountryCode != "WW")
                        {
                            sOption = "<option style='font-weight:bold;' value='0'>(" + otherUtility.getResourceString("IfYourCity") + ")</option>";
                            sSelect = sSelect + sOption;
                        }
                        foreach (DataRow dr2 in dtRegionGeneralAreas.Rows)
                        {
                            sRegionName = dr2["Name"].ToString();
                            sRegionCode = dr2["KNTCode"].ToString();
                            sCountryName = dr2["site"].ToString();
                            sOption = "<option style='font-weight:normal;' value='" + sRegionCode + "'>" + sRegionName + "</option>";
                            sSelect = sSelect + sOption;

                        }
                    }

                    sSelect = sSelect + "</select>";

                    newPackageDiv = newPackageDiv + sSelect + "</div>";
                }
                else
                {
                    SessionUtility.AddValue("DefaultKNTCode", KNTCodeDefault);
                    newPackageDiv = newPackageDiv + "</div>";

                }

            }
            return newPackageDiv + commentDiv + "~" + SessionUtility.GetIntValue("DefaultKNTCode");
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("BuildPackageDiv:" + ex.Message);
            return "";
        }
    }

    [WebMethod]
    public static string SecendSimType(string SimType)
    {
        SessionUtility.AddValue("SecendSimType", SimType);
        return "";
    }

    [WebMethod]
    public static void SetShipCode(int ShipId, string ShipName)
    {
        SessionUtility.AddValue("ShipId", ShipId);

    }

    #endregion

    public List<PlanDetails> planDetails;
    public SiteInfo siteInfo;
    public List<CartItems> AccessoriesCartItems;
    protected decimal dPlan3, dPlan4;
    public string currencySymbol;

    #region Page load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (this.Request.Params["c"] != null)
            {
                string c = this.Request.Params["c"].ToString().ToUpper();
                if (c.Length != 2)
                    RedirectToHomeSite();
                else
                {
                    if (dbUtility.ExecIntScalar("select count(*) from tblTelawaySitesInfo where name in ('" + c + "')") == 0)
                    {
                        SessionUtility.AddValue("UserCountry", "WW");
                        RedirectToHomeSite();
                    }
                    else SessionUtility.AddValue("UserCountry", c);
                    HiddenUserCountry.Value = SessionUtility.GetValue("UserCountry").ToUpper();
                }
            }
            else
            {
                SessionUtility.AddValue("UserCountry", "WW");
                HiddenUserCountry.Value = SessionUtility.GetValue("UserCountry").ToUpper();
                // Response.Redirect("FMSignupStep.aspx?c=WW", false); 
                //return; 
            }
            if (!Page.IsPostBack)
            {


                SessionUtility.AddValue("bOnlySpecialDelivery", false);
                HiddenUserCountry.Value = SessionUtility.GetValue("UserCountry").ToUpper();
                //For AGENTS   ag= ? 
                string cookievalue;

                if (this.Request.Params["ag"] != null || Request.Cookies["agent"] != null)
                {
                    string AgentName = "";
                    if (this.Request.Params["ag"] != null)
                        AgentName = this.Request.Params["ag"].ToString();
                    if (AgentName == "" && Request.Cookies["agent"] != null)
                    {
                        cookievalue = Request.Cookies["agent"].ToString();
                        AgentName = Request.Cookies["agent"].Value;
                    }
                    SessionUtility.AddValue("AgentName", "");
                    DataTable dtTelawayAgents = dbUtility.getTableBySQL("SELECT IsOnlySepecialDelivery,DisplayImage, AgentCode,SubAgentCode,Email,Phone FROM tblAllTelawayAgents where site='" + HiddenUserCountry.Value
                           + "' and AgentName='" + AgentName + "'");
                    if (dtTelawayAgents != null && dtTelawayAgents.Rows.Count > 0)
                    {
                        Response.Cookies["agent"].Value = AgentName;
                        Response.Cookies["agent"].Expires = DateTime.Now.AddYears(1); // add expiry time

                        foreach (DataRow dr in dtTelawayAgents.Rows)
                        {
                            SessionUtility.AddValue("bOnlySpecialDelivery", (bool)dr["IsOnlySepecialDelivery"]);
                            SessionUtility.AddValue("bDisplayImage", (bool)dr["DisplayImage"]);
                            SessionUtility.AddValue("AgentName", AgentName);
                            SessionUtility.AddValue("AgentCode", dr["AgentCode"].ToString() == "" ? 972 : (int)dr["AgentCode"]);
                            SessionUtility.AddValue("SubAgentCode", dr["SubAgentCode"].ToString() == "" ? -1 : (int)dr["SubAgentCode"]);
                            SessionUtility.AddValue("AgentEmail", dr["Email"] != null ? dr["Email"].ToString() : "");
                            SessionUtility.AddValue("AgentPhone", dr["Phone"] != null ? dr["Phone"].ToString() : "");
                            //  bool bOnlySpecialDelivery = Convert.ToBoolean(dbUtility.ExecScalarByStrParams("SELECT IsOnlySepecialDelivery  FROM tblAllTelawayAgents where site=@sSite and AgentName='" + SessionUtility.GetValue("AgentName") + "'", "sSite", SessionUtility.GetValue("UserCountry")));
                            //  SessionUtility.AddValue("bOnlySpecialDelivery", bOnlySpecialDelivery);
                        }
                    }
                    else if (dtTelawayAgents == null || dtTelawayAgents.Rows.Count == 0)
                        SessionUtility.AddValue("AgentName", "");
                }
                else SessionUtility.AddValue("AgentName", "");

                if (SessionUtility.GetValue("UserCountry") == "")
                {
                    RedirectToHomeSite();
                    return;
                }

                if (this.Request.QueryString["Err"] != null)
                {
                    if (this.Request.QueryString["Err"].ToString() != "")
                        RedirectToHomeSite();
                }
                else if (this.Request.Params["status"] != null)
                {
                    if (this.Request.Params["status"].ToString() == "error")
                    {
                        lblMsg.InnerHtml = SessionUtility.GetValue("InvalidData");
                        FillScreen();
                        TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
                        //if (tmpOrderObj != null)
                        //    return;
                    }
                }

                otherUtility.SSL();
                ClearBorderAndInvalidValue();
                Session.Timeout = 40;

                RadDateStartDate.MinDate = DateTime.Today.Date;
                RadDateEndDate.MinDate = DateTime.Today.Date;
                RadDeliveryDate.MinDate = DateTime.Today.Date;

                LoadPlansDetails();

                LoadTextByLang();
                LoadText();
                //LoadDiv();
                LoadToolTip();

            }
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("Thread was being aborted"))
                emailUtility.SendMailErr("Signup FM PageLoad\n: " + ex.Message + "\n" + ex.StackTrace);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        string strDisAbleBackButton;
        strDisAbleBackButton = "<SCRIPT language=javascript>\n";
        strDisAbleBackButton += "window.history.forward(1);\n";
        strDisAbleBackButton += "\n</SCRIPT>";
        ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
    }

    void Page_PreInit(Object sender, EventArgs e)
    {
        SessionUtility.AddValue("newOrderSite", true);
        this.MasterPageFile = otherUtility.getMasterName();
    }

    #endregion

    private void AddSimItem(int kntCode, int simCode, string IMEI, int extendedDataPackageCode, decimal payPalAmount, decimal sSimPrice, decimal shipFee, ref List<SIMDetails> simDetails)
    {

        try
        {
            SIMDetails s = new SIMDetails();
            s.KNTCode = kntCode;
            s.EquipmentCode = simCode;
            s.EquipmentModel = simCode;
            s.EquipmentName = GetSIMNameFromCode(simCode);
            s.SimPrice = sSimPrice;
            s.PhoneIMEI = IMEI;
            s.PaypalAmount = payPalAmount;
            s.ShipFee = shipFee;
            s.ExtendedDataPackageCode = extendedDataPackageCode;
            simDetails.Add(s);
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("Signup step1 Add Sim Item: " + ex.Message);
        }
    }

    private static string GetSIMNameFromCode(int simCode)
    {
        if (simCode == 1250)
            return otherUtility.getResourceString("UsaR");// "USA Regular SIM";

        else if (simCode == 1430)
            return otherUtility.getResourceString("MicroS"); //"USA Micro SIM";

        else if (simCode == 1440)
            return otherUtility.getResourceString("NanoS");// "USA Nano SIM";

        else
            return otherUtility.getResourceString("UsaR");// "USA Regular SIM";
    }



    private void ClearBorderAndInvalidValue()
    {//set the max allowed height of the combo  
        //int MAX_ALLOWED_HEIGHT = 120;
        //this is the single item's height  
        //int SINGLE_ITEM_HEIGHT = 12;
        //calculate the Height based on number of items  
        //int calculatedHeight = RadComboShipping.Items.Count * SINGLE_ITEM_HEIGHT;

        //if (calculatedHeight > MAX_ALLOWED_HEIGHT)
        //{
        //    RadComboShipping.Height = MAX_ALLOWED_HEIGHT;
        //}
        //else
        //{
        //    RadComboShipping.Height = calculatedHeight;
        //}  

        SessionUtility.AddValue("InValidDates", true);
        SessionUtility.AddValue("InValidSimType", true);
        SessionUtility.AddValue("InValidQuantity", true);
        SessionUtility.AddValue("InValidKntCode", true);
        //SessionUtility.AddValue("InValidFName", true);
        //SessionUtility.AddValue("InValidLName", true);
        //SessionUtility.AddValue("InValidEmail", true);
        //SessionUtility.AddValue("InValidEmail2", true);
        //SessionUtility.AddValue("InValidPhone", true);
        //SessionUtility.AddValue("InValidCell", true);
        //SessionUtility.AddValue("InValidAddress", true);
        //SessionUtility.AddValue("InValidCity", true);
        //SessionUtility.AddValue("InValidCountry", true);
        SessionUtility.AddValue("InValidshipOpAddress", true);
        SessionUtility.AddValue("InValidshipOpAddress2", true);
        SessionUtility.AddValue("InValidDateLeave", true);
        SessionUtility.AddValue("InValidShipOp", true);
        SessionUtility.AddValue("InValidState", true);
        //SessionUtility.AddValue("InValidZip", true);
        //SessionUtility.AddValue("InValidShipName", true);
        //SessionUtility.AddValue("InValidShipAddress", true);
        //SessionUtility.AddValue("InValidShipCity", true);
        ////SessionUtility.AddValue("InValidShipState", true);
        //SessionUtility.AddValue("InValidShipZip", true);
        //SessionUtility.AddValue("InValidShipPhone", true);
        //SessionUtility.AddValue("InValidShipCountry", true);
        SessionUtility.AddValue("InValidPayment", true);
        //SessionUtility.AddValue("InValidCCType", true);
        SessionUtility.AddValue("InValidCCNum", true);
        //SessionUtility.AddValue("InValidCCFirstName", true);
        //SessionUtility.AddValue("InValidCCLastName", true);
        //SessionUtility.AddValue("InValidCCEmail", true);
        SessionUtility.AddValue("InValidCCExpDate", true);
        SessionUtility.AddValue("InValidAgree", true);

        if (RadDateStartDate.SelectedDate != null && RadDateEndDate.SelectedDate != null)
            SessionUtility.AddValue("InValidDates", false);
        int number;
        if (!Int32.TryParse(HiddenSIMCount.Value, out number))
            SessionUtility.AddValue("InValidQuantity", true);

        else
        {
            SessionUtility.AddValue("InValidQuantity", false);
            if (RadSimDetails.Value != "")
                SessionUtility.AddValue("InValidSimType", false);

            if (KNTSimDetails.Value != "")
                SessionUtility.AddValue("InValidKntCode", false);
        }
        if (RadTxtFName.Text.Trim() != "")
            SessionUtility.AddValue("InValidFName", false);

        if (RadTxtLName.Text.Trim() != "")
            SessionUtility.AddValue("InValidLName", false);

        if (RadTxtEmail.Text.Trim() != "")
            SessionUtility.AddValue("InValidEmail", false);

        if (RadTxtEmail2.Text.Trim() != "")
            SessionUtility.AddValue("InValidEmail2", false);

        if (RadTxtPhone.Text.Trim() != "" && RadTxtPhone.Text.Length > 8)
            SessionUtility.AddValue("InValidPhone", false);

        if (RadTxtCell.Text.Trim() != "" && RadTxtCell.Text.Length > 9)
            SessionUtility.AddValue("InValidCell", false);


        if (RadTxtStreet.Text.Trim() != "")
            SessionUtility.AddValue("InValidAddress", false);

        if (RadTxtCity.Text.Trim() != "")
            SessionUtility.AddValue("InValidCity", false);

        if (RadComboCountry.Text != "")
            SessionUtility.AddValue("InValidCountry", false);
        SessionUtility.AddValue("InValidState", false);

        if (!rbCusAddress.Checked & !rbDiffAddress.Checked)
            SessionUtility.AddValue("InValidshipOpAddress", false);

        SessionUtility.AddValue("InValidshipOpAddress", false);
        SessionUtility.AddValue("InValidshipOpAddress2", false);

        if (RadComboShipping.SelectedValue != "")
            SessionUtility.AddValue("InValidshipOp", false);

        if (RadDeliveryDate.SelectedDate != null)
            SessionUtility.AddValue("InValidDateLeave", false);

        if (RadComboCardType.Text != "")
            SessionUtility.AddValue("InValidCCType", false);

        if (RadTxtCC.Text != "")
            SessionUtility.AddValue("InValidCCNum", false);

        if (RadClientFName.Text != "")
            SessionUtility.AddValue("InValidCCFirstName", false);

        if (RadClientLName.Text != "")
            SessionUtility.AddValue("InValidCCLastName", false);

        if (RadCCEmail.Text != "")
            SessionUtility.AddValue("InValidCCEmail", false);

        if (RadComboMonth.Text != "" && RadComboYear.Text != "")
            SessionUtility.AddValue("InValidCCExpDate", false);

        if (CheckBoxAgree.Checked)
            SessionUtility.AddValue("InValidAgree", false);
        IfMissingField(1, "Remove");
        IfMissingField(2, "Remove");
        DivFirstName.Style.Add("border", "none");
        DivLastName.Style.Add("border", "none");
        DivEmail.Style.Add("border", "none");
        DivEmail2.Style.Add("border", "none");
        DivPhone.Style.Add("border", "none");
        DivCell.Style.Add("border", "none");

        DivAddress.Style.Add("border", "none");
        DivCity.Style.Add("border", "none");
        DivCountry.Style.Add("border", "none");
        StateDiv.Style.Add("border", "none");
        ZipDiv.Style.Add("border", "none");
        shipOpAddress.Style.Add("border", "none");
        DateLeaveDiv.Style.Add("border", "none");
        DivComboShipping.Style.Add("border", "none");
        IfMissingField(3, "Remove");

        //ship options
        DivShipName.Style.Add("border", "none");
        DivShipAddress.Style.Add("border", "none");
        DivShipCity.Style.Add("border", "none");
        StateDivShip.Style.Add("border", "none");
        ZipDivShip.Style.Add("border", "none");
        DivShipCountry.Style.Add("border", "none");
        DivShipPhone.Style.Add("border", "none");
        IfMissingField(4, "Remove");

        //billing options
        DivCCType.Style.Add("border", "none");
        DivCCNum.Style.Add("border", "none");
        DivCCFirstName.Style.Add("border", "none");
        DivCCLastName.Style.Add("border", "none");
        DivCCEmail.Style.Add("border", "none");
        DivCCExpDate.Style.Add("border", "none");
        CheckBoxAgree.Style.Add("border", "none");
        IfMissingField(5, "Remove");

    }

    private void IfMissingField(int gruop, string Action)
    {
        switch (Action)
        {
            case "Remove":
                switch (gruop)
                {
                    case 1: PleaseSelectStartDate.Attributes.Remove("class"); break;
                    case 2: SimDetails.Attributes.Remove("class"); break;
                    case 3: BillingInformation.Attributes.Remove("class"); break;
                    case 4: ShippingInformation.Attributes.Remove("class"); break;
                    case 5: ChoosePaymentMethod.Attributes.Remove("class"); break;
                }
                ;
                break;
            case "Add":

                switch (gruop)
                {
                    case 1: PleaseSelectStartDate.Attributes.Add("class", "missingField"); break;
                    case 2: SimDetails.Attributes.Add("class", "missingField"); break;
                    case 3: BillingInformation.Attributes.Add("class", "missingField"); break;
                    case 4: ShippingInformation.Attributes.Add("class", "missingField"); break;
                    case 5: ChoosePaymentMethod.Attributes.Add("class", "missingField"); break;
                }
                ;
                break;

        }

    }
    private bool IsValidData()
    {
        try
        {
            //SetEvents();
            SessionUtility.AddValue("FirstTime", true);
            ClearBorderAndInvalidValue();
            SessionUtility.AddValue("ValidErr", null);
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            lblMsg.InnerHtml = "";
            if (RadDateStartDate.SelectedDate == null || RadDateEndDate.SelectedDate == null)
            {
                IfMissingField(1, "Add");
                SessionUtility.AddValue("InValidDates", true);
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseSelectStartDate"));

            }
            int number;
            if (!Int32.TryParse(HiddenSIMCount.Value, out number))
            {
                IfMissingField(2, "Add");
                SessionUtility.AddValue("InValidQuantity", true);
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("pleaseSelectQnt"));
            }
            else
            {
                if (SessionUtility.GetValue("DefaultKNTCode") == "-1")
                    if ((KNTSimDetails.Value == "" || KNTSimDetails.Value.Contains("null")))
                    {
                        IfMissingField(2, "Add");
                        SessionUtility.AddValue("InValidKntCode", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseKntCode"));
                    }
                    else
                    {
                        string[] kntSimV = KNTSimDetails.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (kntSimV.Length != int.Parse(HiddenSIMCount.Value))
                        {
                            IfMissingField(2, "Add");
                            SessionUtility.AddValue("InValidKntCode", true);
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseKntCode"));
                        }
                    }
                else SessionUtility.AddValue("InValidKntCode", false);
                if (RadSimDetails.Value == "" || RadSimDetails.Value.Contains("null"))
                {
                    IfMissingField(2, "Add");
                    SessionUtility.AddValue("InValidSimType", true);
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseSimType"));
                }
                else
                {
                    string[] detailsV = RadSimDetails.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (detailsV.Length != int.Parse(HiddenSIMCount.Value))
                    {
                        IfMissingField(2, "Add");
                        SessionUtility.AddValue("InValidSimType", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseSimType"));
                    }
                }
            }
            if (RadTxtFName.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValue("InValidFName", true);
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterFirstName"));
                DivFirstName.Style.Add("border", "solid 2px red");
                //return false;
            }

            if (RadTxtLName.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValue("InValidLName", true);
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterLastName"));
                DivLastName.Style.Add("border", "solid 2px red");
                // return false;
            }

            if (RadTxtEmail.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValue("InValidEmail", true);
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                DivEmail.Style.Add("border", "solid 2px red");
                //return false;
            }
            if (RadTxtEmail2.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                DivEmail2.Style.Add("border", "solid 2px red");
                //return false;
            }

            Regex objValidEmail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

            if (RadTxtEmail.Text.Trim() != "")
            {
                if (RadTxtEmail.Text.Trim() != RadTxtEmail2.Text.Trim())
                {
                    IfMissingField(3, "Add");
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("EmailMismatch"));
                    DivEmail.Style.Add("border", "solid 2px red");
                    DivEmail2.Style.Add("border", "solid 2px red");
                    //return false;
                }

                if (!objValidEmail.IsMatch(RadTxtEmail.Text.Trim()))
                {
                    IfMissingField(3, "Add");
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                    DivEmail.Style.Add("border", "solid 2px red");
                    //return false;
                }
            }
            if (RadTxtEmail2.Text.Trim() != "")
            {

                if (!objValidEmail.IsMatch(RadTxtEmail2.Text.Trim()))
                {
                    IfMissingField(3, "Add");
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                    DivEmail2.Style.Add("border", "solid 2px red");
                    //return false;
                }
            }
            if (RadTxtPhone.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterPhone"));
                DivPhone.Style.Add("border", "solid 2px red");
                // return false;
            }
            else if (RadTxtPhone.Text.Trim().Length < 9)
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterPhone"));
                DivPhone.Style.Add("border", "solid 2px red");
                //return false;
            }

            if (RadTxtCell.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                // SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCell"));
                DivCell.Style.Add("border", "solid 2px red");
                //return false;
            }
            else if (RadTxtCell.Text.Trim().Length < 10)
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCell"));
                DivCell.Style.Add("border", "solid 2px red");
                //return false;
            }


            if (RadTxtStreet.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterStreet"));
                DivAddress.Style.Add("border", "solid 2px red");
                //return false;
            }

            if (RadTxtCity.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCity"));
                DivCity.Style.Add("border", "solid 2px red");
                //return false;
            }

            if (RadComboCountry.Text == "")// .SelectedValue.ToString() == "")
            {
                IfMissingField(3, "Add");
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseContry"));
                DivCountry.Style.Add("border", "solid 2px red");
                // return false;
            }

            else
            {
                if (RadComboCountry.SelectedValue.ToString().ToUpper() == "USA")
                {
                    if (RadComboStateUSA.SelectedValue.ToString() == "")
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidState", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseState"));
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "IfNeedState();", true);
                        StateDiv.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }
                if (RadComboCountry.SelectedValue.ToString().ToUpper() == "AUSTRALIA")
                {
                    if (RadComboStateAU.SelectedValue.ToString() == "")
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidState", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseState"));
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "IfNeedState();", true);
                        StateDiv.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }
                if (RadComboCountry.SelectedValue.ToString().ToUpper() == "CANADA")
                {
                    if (RadComboStateC.SelectedValue.ToString() == "")
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidState", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseState"));
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "IfNeedState();", true);
                        StateDiv.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "IfNeedState();", true);


            }
            if (RadTxtShipCountry.Text.ToString().ToUpper() == "USA" ||
                            RadTxtShipCountry.Text.ToString().ToUpper() == "CANADA" ||
                            RadTxtShipCountry.Text.ToString().ToUpper() == "AUSTRALIA")
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "IfNeedState();", true);

            if (RadComboShipping.SelectedIndex > 0 && !rbDiffAddress.Checked)
            {
                string sShipCountry = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString().ToUpper();

                if (sShipCountry.ToUpper() != RadComboCountry.SelectedValue.ToUpper() && sShipCountry.ToUpper() != "EU")
                {
                    IfMissingField(3, "Add");
                    SessionUtility.AddValue("InValidshipOpAddress2", true);
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("WeAreSorryDeliveryMethod"));
                    DivCountry.Style.Add("border", "solid 2px red");
                    //return false;
                }

                if (sShipCountry.ToUpper() == "EU" && !rbDiffAddress.Checked)
                {
                    if (RadComboCountry.SelectedIndex != -1 && RadComboCountry.Items[RadComboCountry.SelectedIndex].Attributes["IsEU"].ToString().ToUpper() != "TRUE")
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidshipOpAddress2", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("WeAreSorryDeliveryMethod"));
                        DivCountry.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }



                string sShipM = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["ShipMethod"].ToString();
                if (sShipM.ToUpper().Contains("UPS_"))
                {
                    if (RadComboCountry.SelectedValue.ToUpper() != "USA" && !rbDiffAddress.Checked)
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidshipOpAddress2", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("WeAreSorryDeliveryMethod"));
                        DivCountry.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }


                if (!rbDiffAddress.Checked)
                {
                    if (sShipM.ToUpper().Contains("SHIPPING") || sShipM.ToUpper().Contains(HiddenUserCountry.Value) || sShipM.ToUpper().Contains("AUSTRALIA_OVERNIGHT"))
                    {
                        if (RadComboCountry.SelectedValue.ToUpper() != sShipCountry && sShipCountry.ToUpper() != "EU")
                        {
                            IfMissingField(3, "Add");
                            SessionUtility.AddValue("InValidshipOpAddress2", true);
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("WeAreSorryDeliveryMethod"));
                            DivCountry.Style.Add("border", "solid 2px red");
                            // return false;
                        }
                    }
                }
            }
            if (RadTxtEmail.Text.Trim() == "")
            {
                IfMissingField(3, "Add");
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                DivEmail.Style.Add("border", "solid 2px red");
                //return false;
            }
            //shipping
            if (RadComboShipping.SelectedIndex <= 0)
            {
                IfMissingField(3, "Add");
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PlaeseChooseShippingOptions"));
                shipOpAddress.Style.Add("border", "solid 2px red");
                DivComboShipping.Style.Add("border", "solid 2px red");
                //return false;
            }


            if (RadComboShipping.SelectedIndex > 0)
            {
                if (!Convert.ToBoolean(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["optLocalPickup"].ToString()))
                {
                    if (!rbCusAddress.Checked && !rbDiffAddress.Checked)
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidshipOpAddress", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PlaeseChooseShippingOptions"));
                        shipOpAddress.Style.Add("border", "solid 2px red");
                        shipOpAddress.Style.Add("display", "block");
                        //return false;
                    }
                    if (RadDeliveryDate.SelectedDate == null)
                    {
                        IfMissingField(3, "Add");
                        SessionUtility.AddValue("InValidDateLeave", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterDeliveryDate"));
                        DateLeaveDiv.Style.Add("border", "solid 2px red");
                        //return false;
                    }

                    else
                    {
                        if (RadDeliveryDate.SelectedDate < DateTime.Now)
                        {
                            IfMissingField(3, "Add");
                            SessionUtility.AddValue("InValidDateLeave", true);
                            SessionUtility.AddValue("DateLeave", otherUtility.getResourceString("PleaseMakeSureDeliveryAFuture"));
                            SessionUtility.AddValuePlus("ValidErr", SessionUtility.GetValue("DateLeave"));
                            DateLeaveDiv.Style.Add("border", "solid 2px red");
                            //return false;
                        }

                        if (RadDeliveryDate.SelectedDate > RadDateStartDate.SelectedDate)//Convert.ToDateTime(tmpOrderObj.StartDate))
                        {
                            IfMissingField(3, "Add");
                            SessionUtility.AddValue("InValidDateLeave", true);
                            SessionUtility.AddValue("DateLeave", otherUtility.getResourceString("PleaseMakeSureLeavingDateBefore") + " < " +
                                Convert.ToDateTime(RadDateStartDate.SelectedDate).ToString(otherUtility.getDateFormat())
                                + ">");
                            SessionUtility.AddValuePlus("ValidErr", SessionUtility.GetValue("DateLeave"));
                            DateLeaveDiv.Style.Add("border", "solid 2px red");
                            //return false;

                        }
                    }
                    if (rbDiffAddress.Checked)
                    {
                        if (RadTxtDeliveryName.Text.Trim() == "")
                        {
                            IfMissingField(4, "Add");
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipName"));
                            DivShipName.Style.Add("border", "solid 2px red");
                            //return false;
                        }

                        if (RadShipAddress.Text.Trim() == "")
                        {
                            IfMissingField(4, "Add");
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipAddress"));
                            DivShipAddress.Style.Add("border", "solid 2px red");
                            //return false;
                        }

                        if (RadShipCity.Text.Trim() == "")
                        {
                            IfMissingField(4, "Add");
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipCity"));
                            DivShipCity.Style.Add("border", "solid 2px red");
                            //return false;
                        }
                        if (RadShipZip.Text.Trim() == "")
                        {
                            IfMissingField(4, "Add");
                            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipCity"));
                            ZipDivShip.Style.Add("border", "solid 2px red");
                        }
                        if (RadTxtShipCountry.Text.ToString().ToUpper() == "USA")
                        {
                            if (RadComboBoxStateUShip.SelectedValue.ToString() == "")
                            {
                                IfMissingField(4, "Add");
                                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseShipState"));
                                StateDivShip.Style.Add("border", "solid 2px red");
                                //return false;
                            }
                        }
                        if (RadTxtShipCountry.Text.ToString().ToUpper() == "CANADA")
                        {
                            if (RadComboBoxStateCShip.SelectedValue.ToString() == "")
                            {
                                IfMissingField(4, "Add");
                                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseShipState"));
                                StateDivShip.Style.Add("border", "solid 2px red");
                                //return false;
                            }
                        }
                        if (RadTxtShipCountry.Text.ToString().ToUpper() == "AUSTRALIA")
                        {
                            if (RadComboBoxStateAUShip.SelectedValue.ToString() == "")
                            {
                                IfMissingField(4, "Add");
                                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseShipState"));
                                StateDivShip.Style.Add("border", "solid 2px red");
                                //return false;
                            }
                        }
                        if (RadTxtDeliveryPhone.Text.Trim() == "")
                        {
                            IfMissingField(4, "Add");
                            SessionUtility.AddValue("ValidErr", otherUtility.getResourceString("PleaseEnterShipPhone"));
                            DivShipPhone.Style.Add("border", "solid 2px red");
                            //return false;
                        }

                    }
                    else ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "HideShipDetails('NO');", true);

                }
            }
            //shipping

            //if (RadComboShipping.SelectedValue.ToString() != "")
            //{
            //    if (RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString() == "USA" && rbDiffAddress.Checked)
            //    {
            //        if (RadComboBoxStateUShip.SelectedValue.ToString() == "")
            //        {
            //            SessionUtility.AddValue("ValidErr", "Please choose a shipping State/Province");
            //            return false;
            //        }
            //    }
            //}




            //cc
            if (rbCC.Checked && !rbPayPal.Checked)
            {
                if (RadComboCardType.Text.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    SessionUtility.AddValue("InValidCCType", true);

                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseCCType"));
                    DivCCType.Style.Add("border", "solid 2px red");
                    //return false;
                }


                if (RadTxtCC.Text.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    //SessionUtility.AddValue("InValidCCNum", true);

                    //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidCCNum"));
                    DivCCNum.Style.Add("border", "solid 2px red");
                    // return false;
                }
                else
                {
                    if (RadTxtCC.Text.Trim().Length < 14)//minimum CC numbers
                    {
                        IfMissingField(5, "Add");
                        SessionUtility.AddValue("InValidCCNum", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("IncorrectCreditCardNumber"));
                        DivCCNum.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                    if (RadTxtCC.Text.Trim().Length < 16 && RadComboCardType.SelectedValue != "Amex")//minimum CC numbers
                    {
                        IfMissingField(5, "Add");
                        SessionUtility.AddValue("InValidCCNum", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("IncorrectCreditCardNumber"));
                        DivCCNum.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                    if (!(dbUtility.IsValidCCNum(RadTxtCC.Text.Trim())))
                    {
                        IfMissingField(5, "Add");
                        SessionUtility.AddValue("InValidCCNum", true);
                        SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("IncorrectCreditCardNumber"));
                        DivCCNum.Style.Add("border", "solid 2px red");
                        //return false;
                    }
                }
                if (RadComboMonth.SelectedValue.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    // SessionUtility.AddValue("InValidCCExpDate", true);
                    // SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCExpiredMonth"));
                    DivCCExpDate.Style.Add("border", "solid 2px red");
                    //return false;
                }
                if (RadComboYear.SelectedValue.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    // SessionUtility.AddValue("InValidCCExpDate", true);
                    // SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCExpiredyear"));
                    DivCCExpDate.Style.Add("border", "solid 2px red");
                    //return false;
                }
                //expiration Date validation
                DateTime expDate = getExpDate();
                if (DateTime.Now > expDate)
                {
                    IfMissingField(5, "Add");
                    SessionUtility.AddValue("InValidCCExpDate", true);
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ItSeemsCCExpired"));
                    DivCCExpDate.Style.Add("border", "solid 2px red");
                    //return false;
                }

                if (RadClientFName.Text.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    // SessionUtility.AddValue("InValidCCFirstName", true);

                    //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCFName"));
                    DivCCFirstName.Style.Add("border", "solid 2px red");
                    //return false;
                }

                if (RadClientLName.Text.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    //SessionUtility.AddValue("InValidCCLastName", true);

                    //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCLName"));
                    DivCCLastName.Style.Add("border", "solid 2px red");
                    //return false;
                }

                if (RadCCEmail.Text.Trim() == "")
                {
                    IfMissingField(5, "Add");
                    //SessionUtility.AddValue("InValidCCEmail", true);
                    //SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnteraCCEmail"));
                    DivCCEmail.Style.Add("border", "solid 2px red");
                    //return false;
                }
                Regex objValidEmail2 = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

                if (!objValidEmail2.IsMatch(RadCCEmail.Text.Trim()))
                {
                    IfMissingField(5, "Add");
                    SessionUtility.AddValue("InValidCCEmail", true);
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                    DivCCEmail.Style.Add("border", "solid 2px red");
                    //return false;
                }
            }
            else
            {
                SessionUtility.AddValue("InValidCCType", false);
                SessionUtility.AddValue("InValidCCNum", false);
                SessionUtility.AddValue("InValidCCFirstName", false);
                SessionUtility.AddValue("InValidCCLastName", false);
                SessionUtility.AddValue("InValidCCEmail", false);
                SessionUtility.AddValue("InValidCCExpDate", false);
                /*if (!rbCC.Checked && !rbPayPal.Checked)
                {
                    SessionUtility.AddValue("InValidPayment", true);
                    SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ChoosePaymentMethod"));
                }*/
            }


            try
            {
                if (RadPromoCode.Text.Trim() != "")//TO DO ? 
                {
                    /* bool bOnlyToSecondOrder = dbUtility.ExecBoolScalar_DBOR("SELECT OnlyToSecondOrder  FROM tblSignupCoupons where CouponCode='" + RadPromoCode.Text.Trim() + "'");

                     if (tmpOrderObj.PhonesRequired == 1)//&& bOnlyToSecondOrder && UPImg.Src == "")
                     {
                         SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("InvalidCoupon"));

                     }
                     if (!dbUtility.IsValidCoupon(RadPromoCode.Text.Trim()))
                     {
                         SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("InvalidCoupon"));
                         //return false;
                     }*/
                }
            }
            catch (Exception ex)
            {
                SessionUtility.AddValuePlus("errMsg", otherUtility.getResourceString("InvalidCoupon") + ": " + ex.Message);
                //return false;
            }


            if (!CheckBoxAgree.Checked)
            {
                IfMissingField(5, "Add");
                SessionUtility.AddValue("InValidAgree", true);
                SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("YouMustAgree"));
                CheckBoxAgree.Style.Add("border", "solid 2px red");

            }

            if (tmpOrderObj != null)
            {
                if (tmpOrderObj.ChargeWithPayPal && tmpOrderObj.PayPalTransactionId == "Err")
                {
                    return false;
                }
                if (tmpOrderObj.PayPalTransactionId == "Err")
                {
                    if (SessionUtility.GetValue("ValidErr") == "")
                        SessionUtility.AddValue("ValidErr", "PayPal Err:" + SessionUtility.GetValue("ValidErrP"));

                    return false;
                }
            }
            if (SessionUtility.GetValue("ValidErr") != "")
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("IsValidData " + e.Message);
            return false;
        }

    }

    /// <summary>
    /// 2nd step of FILL DATA : Payments, Shipping, Billink information 
    /// </summary>
    /// <returns></returns>
    private string FillData()
    {
        try
        {
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            if (tmpOrderObj == null) return "Err";
            if (!string.IsNullOrEmpty(tmpOrderObj.ShipCountry))
                shipcountryHidden.Value = tmpOrderObj.ShipCountry.ToLower();
            tmpOrderObj.UserName = otherUtility.GetStringSpecialChars(dbUtility.Capitalize(RadTxtLName.Text.Trim()) + " " + dbUtility.Capitalize(RadTxtFName.Text.Trim()));

            tmpOrderObj.UserNamePayPal = otherUtility.GetStringSpecialChars(dbUtility.Capitalize(RadTxtFName.Text.Trim()) + " " + dbUtility.Capitalize(RadTxtLName.Text.Trim()));


            SessionUtility.AddValue("hdPhone", hdPhone.Value);
            SessionUtility.AddValue("hdnCell", hdnCell.Value);
            SessionUtility.AddValue("hdDeliveryPhone", hdDeliveryPhone.Value);
            tmpOrderObj.ClientHomePhone1 = RadTxtPhone.Text.Trim() == "" ? hdPhone.Value : RadTxtPhone.Text.Trim();
            //tmpOrderObj.ClientHomePhone2 = RadTxtIMEI.Text.Trim();
            tmpOrderObj.ClientMobile = RadTxtCell.Text.Trim() == "" ? hdnCell.Value : RadTxtCell.Text.Trim();
            tmpOrderObj.ClientStreet = otherUtility.GetStringSpecialChars(RadTxtStreet.Text.Trim());
            tmpOrderObj.ClientCity = otherUtility.GetStringSpecialChars(RadTxtCity.Text.Trim());
            tmpOrderObj.ClientCountry = otherUtility.GetStringSpecialChars(RadComboCountry.SelectedValue.ToString());

            if (RadComboCountry.SelectedValue.ToString().ToUpper() == "USA")
                tmpOrderObj.ClientState = RadComboStateUSA.SelectedValue.ToString();
            else if (RadComboCountry.SelectedValue.ToString().ToUpper() == "CANADA")
                tmpOrderObj.ClientState = RadComboStateC.SelectedValue.ToString();
            else if (RadComboCountry.SelectedValue.ToString().ToUpper() == "AUSTRALIA")
            {
                tmpOrderObj.ClientCity = otherUtility.GetStringSpecialChars(tmpOrderObj.ClientCity + " ," + RadComboStateAU.Text.ToString());
                tmpOrderObj.ClientState = "NA";
            }
            else tmpOrderObj.ClientState = "N/A";

            if (RadTxtZip.Text.Trim() == "") tmpOrderObj.ClientZip = "0";
            else tmpOrderObj.ClientZip = otherUtility.GetStringSpecialChars(RadTxtZip.Text.Trim());//??

            tmpOrderObj.ClientEmail = RadTxtEmail.Text.Trim();
            tmpOrderObj.ClientFirstName = otherUtility.GetStringSpecialChars(RadTxtFName.Text.Trim());
            tmpOrderObj.ClientLastName = otherUtility.GetStringSpecialChars(RadTxtLName.Text.Trim());

            tmpOrderObj.EquipmentCode = 1250;
            tmpOrderObj.EquipmentModel = 1250;
            tmpOrderObj.EquipmentName = "Sim";
            tmpOrderObj.ClientHomePhone2 = ""; //IMEI code

            tmpOrderObj.ShipName = RadComboShipping.Text;

            bool bSamePriceForMultipleSims = Convert.ToBoolean(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["SamePriceForMultipleSims"].ToString());

            if (tmpOrderObj.PhonesRequired > 1 && !bSamePriceForMultipleSims)
            {
                for (int i = 0; i < tmpOrderObj.PhonesRequired; i++)
                    tmpOrderObj.SimDetails[i].ShipFee = Convert.ToDecimal(tmpOrderObj.ShipFee);
            }
            else
            {
                tmpOrderObj.ShipFee = Convert.ToDecimal(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["decimalCost"].ToString());
                tmpOrderObj.ShipGBPFee = Convert.ToDecimal(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["ShipCostGBP"].ToString());

                for (int i = 0; i < tmpOrderObj.PhonesRequired; i++)
                    tmpOrderObj.SimDetails[i].ShipFee = 0;

                tmpOrderObj.SimDetails[0].ShipFee = Convert.ToDecimal(tmpOrderObj.ShipFee);

                decimal? ItemTotal = SessionUtility.GetDecimalValue("totalAmountCharged");
                ItemTotal += tmpOrderObj.ShipGBPFee;
                SessionUtility.AddValue("totalAmountCharged", ItemTotal);

            }

            tmpOrderObj.ShipCountry = "USA";//default
            tmpOrderObj.BaseCode = 1;//default
            tmpOrderObj.ShippingName = RadComboShipping.Items[RadComboShipping.SelectedIndex].Text;


            //tmpOrderObj.ShipMethod = otherUtility.GetStrVal(tmpOrderObj.ShipMethod, "ShipMethod");


            //pickup
            if (Convert.ToBoolean(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["optLocalPickup"].ToString()))
            {
                tmpOrderObj.ShipCountry = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString();
                if (tmpOrderObj.ShipCountry.ToUpper() == "EU")
                    tmpOrderObj.ShipCountry = RadComboCountry.SelectedValue.ToString();

                tmpOrderObj.BaseCode = Convert.ToInt32(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["BaseCode"].ToString());
                tmpOrderObj.ShipCity = "[pickup]";
                tmpOrderObj.ShipStreet = "[pickup]";
                tmpOrderObj.ShipPostalCode = "[pickup]";
                tmpOrderObj.ShipState = "NA";
                tmpOrderObj.ShipPhone = "0";
                tmpOrderObj.ShipName = "[pickup]";
                tmpOrderObj.ShipDate = Convert.ToDateTime(tmpOrderObj.StartDate);
                tmpOrderObj.ShipMethod = "OFFICE_PICKUP";


            }
            else
            {
                if (rbCusAddress.Checked)
                {
                    SessionUtility.AddValue("rbDiffAddress", false);
                    tmpOrderObj.ShipCountry = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString();
                    if (tmpOrderObj.ShipCountry.ToUpper() == "EU")
                        tmpOrderObj.ShipCountry = RadComboCountry.SelectedValue.ToString();

                    tmpOrderObj.BaseCode = Convert.ToInt32(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["BaseCode"].ToString());
                    tmpOrderObj.ShipCity = tmpOrderObj.ClientCity;
                    tmpOrderObj.ShipStreet = otherUtility.GetStringSpecialChars(tmpOrderObj.ClientStreet);
                    tmpOrderObj.ShipPostalCode = otherUtility.GetStringSpecialChars(tmpOrderObj.ClientZip);
                    tmpOrderObj.ShipState = "NA";
                    string sShipC = tmpOrderObj.ShipCountry.ToUpper();
                    //if (tmpOrderObj.ShipCountry.ToUpper() == "USA" && tmpOrderObj.ClientCity == "USA")
                    if (sShipC == "USA" || sShipC == "CANADA")
                        tmpOrderObj.ShipState = tmpOrderObj.ClientState;

                    if (sShipC == "AUSTRALIA")
                    {
                        tmpOrderObj.ShipState = "NA";
                        tmpOrderObj.ShipCity = otherUtility.GetStringSpecialChars(tmpOrderObj.ShipCity + " ," + RadComboStateAU.Text.ToString());
                    }
                    tmpOrderObj.ShipPhone = tmpOrderObj.ClientHomePhone1;
                    tmpOrderObj.ShipName = otherUtility.GetStringSpecialChars(tmpOrderObj.ClientLastName + " " + tmpOrderObj.ClientFirstName);

                    if (tmpOrderObj.ClientLastName == "")
                        tmpOrderObj.ShipName = otherUtility.GetStringSpecialChars(tmpOrderObj.UserName);

                    //tmpOrderObj.ShipDate = Convert.ToDateTime(tmpOrderObj.StartDate);

                    tmpOrderObj.ShipDate = Convert.ToDateTime(RadDeliveryDate.SelectedDate);

                    tmpOrderObj.ShipMethod = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["ShipMethod"].ToString();
                }
                else if (rbDiffAddress.Checked)
                {
                    SessionUtility.AddValue("rbDiffAddress", true);
                    tmpOrderObj.ShipCountry = otherUtility.GetStringSpecialChars(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString());
                    if (tmpOrderObj.ShipCountry.ToUpper() == "EU")
                        tmpOrderObj.ShipCountry = RadComboCountry.SelectedValue.ToString();

                    tmpOrderObj.BaseCode = Convert.ToInt32(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["BaseCode"].ToString());
                    tmpOrderObj.ShipCity = otherUtility.GetStringSpecialChars(RadShipCity.Text.Trim());
                    tmpOrderObj.ShipStreet = otherUtility.GetStringSpecialChars(RadShipAddress.Text.Trim());
                    tmpOrderObj.ShipPostalCode = otherUtility.GetStringSpecialChars(RadShipZip.Text.Trim());

                    tmpOrderObj.ShipState = "NA";
                    if (tmpOrderObj.ShipCountry.ToUpper() == "USA")
                        tmpOrderObj.ShipState = RadComboBoxStateUShip.SelectedValue.ToString();
                    if (tmpOrderObj.ShipCountry.ToUpper() == "CANADA")
                        tmpOrderObj.ShipState = RadComboBoxStateCShip.SelectedValue.ToString();
                    if (tmpOrderObj.ShipCountry.ToUpper() == "AUSTRALIA")
                    {
                        tmpOrderObj.ShipStreet = otherUtility.GetStringSpecialChars(tmpOrderObj.ShipStreet + " ," + RadComboBoxStateAUShip.Text.ToString());
                        tmpOrderObj.ShipState = "NA";
                    }
                    //if (RadShipCountry.SelectedValue.ToString().ToUpper() == "USA")
                    //    tmpOrderObj.ShipState = RadComboBoxStateUShip.SelectedValue.ToString();
                    //else if (RadComboCountry.SelectedValue.ToString().ToUpper() == "CANADA")
                    //    tmpOrderObj.ShipState = RadComboBoxStateCShip.SelectedValue.ToString();
                    //else
                    //    tmpOrderObj.ShipState = "NA";


                    tmpOrderObj.ShipPhone = RadTxtDeliveryPhone.Text.Trim() == "" ? hdDeliveryPhone.Value : RadTxtDeliveryPhone.Text.Trim();
                    tmpOrderObj.ShipName = otherUtility.GetStringSpecialChars(RadTxtDeliveryName.Text.Trim());
                    tmpOrderObj.ShipDate = Convert.ToDateTime(RadDeliveryDate.SelectedDate);
                    tmpOrderObj.ShipMethod = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["ShipMethod"].ToString(); //"SHIPPING";
                }
            }
            tmpOrderObj.PageNo = 2;

            tmpOrderObj.ShipEmail = RadTxtEmail.Text.Trim();
            if (tmpOrderObj.ShipEmail != RadCCEmail.Text.Trim())
                tmpOrderObj.ClientEmail = tmpOrderObj.ClientEmail + ";" + RadCCEmail.Text.Trim();

            tmpOrderObj.ClientFirstName = otherUtility.GetStringSpecialChars(RadClientFName.Text.Trim());
            tmpOrderObj.ClientLastName = otherUtility.GetStringSpecialChars(RadClientLName.Text.Trim());

            if (tmpOrderObj.ClientFirstName == "" || tmpOrderObj.ClientLastName == "")
            {
                int pos = tmpOrderObj.UserName.IndexOf(" ");
                if (pos > 0)
                {
                    tmpOrderObj.ClientFirstName = otherUtility.GetStringSpecialChars(tmpOrderObj.UserName.Substring(0, pos));
                    tmpOrderObj.ClientLastName = otherUtility.GetStringSpecialChars(tmpOrderObj.UserName.Substring(pos + 1, tmpOrderObj.UserName.Length - pos - 1));
                }
            }

            Promocode promoCode = new Promocode(RadPromoCode.Text);
            if (RadPromoCode.Text == "")
                promoCode = new Promocode(SessionUtility.GetValue("Upsale"));
            if (promoCode.IsVaild)
            {
                if (promoCode.IsValidPromoCodeVSPhonesRequired(promoCode, tmpOrderObj.PhonesRequired))
                    tmpOrderObj.CouponCode = promoCode.PromoCode;
                else tmpOrderObj.CouponCode = "";
            }

            if (!rbPayPal.Checked)//Regellar Payment Action 
            {
                tmpOrderObj.CCExpDate = getExpDate();
                tmpOrderObj.CCNum = RadTxtCC.Text;
                tmpOrderObj.CCTitle = RadComboCardType.SelectedValue.ToString();
                tmpOrderObj.CustomerComment = otherUtility.GetStringSpecialChars(RadTxtComments.Text.Trim());
            }
            SessionUtility.AddTmpOrder(tmpOrderObj);
            //PayPal 
            if (rbPayPal.Checked)
            {
                //PayPalBtn(tmpOrderObj);
                PayPalFunction PaypalF = new PayPalFunction();

                string redirectUrl = CallPayPalAPIFunc(tmpOrderObj, promoCode, AccessoriesCartItems, HiddenConversionRate.Value);
                Session["currentPayment"] = tmpOrderObj.SessionID.ToString();
                Session["CurrentPayPalAmount"] = tmpOrderObj.PayPalAmountCharge;
                if (redirectUrl == "")
                {
                    List<ErrorType> paypalErrors = (List<ErrorType>)Context.Items["Response_error"];
                    string paypalMsg = "";
                    foreach (ErrorType error in paypalErrors)
                    {
                        paypalMsg += error.LongMessage + "      <br/>";
                    }
                    lblMsg.InnerText = "Error message from PayPal:<br/>" + paypalMsg;// +retMsgExpress;
                    tmpOrderObj.PayPalTransactionId = "Err";
                    tmpOrderObj.PayPalAmountCharge = 0;
                    emailUtility.SendMailErr("Img PayPal btn Error : " + paypalMsg);
                    SessionUtility.AddValue("ValidErrP", paypalMsg);
                    SessionUtility.AddTmpOrder(tmpOrderObj);
                    SessionUtility.UpdateListTmpOrders(tmpOrderObj, tmpOrderObj.SessionID);
                }
                else return redirectUrl;
            }

        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("FillData \n" + ex.StackTrace + "\n" + ex.TargetSite + "\n" + ex.Message);
            return "Err";

        }
        return "";
    }



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
                if (ItemDiscount != 0)
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
                            tmpOrderObj.SimDetails[i].PaypalDiscount = Convert.ToDecimal(promoCode.getDiscount(promoCode, itemTotal1Order, ItemTotalAcc, simDetails.Count, i));
                        }
                        else tmpOrderObj.SimDetails[i].PaypalDiscount = Convert.ToDecimal(promoCode.getDiscount(promoCode, itemTotal1Order, 0, simDetails.Count, i));
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
    private void FillTexts()
    {
        RadTxtFName.Text = "Test";
        RadTxtLName.Text = "Test";
        RadTxtEmail.Text = "test@gmail.com";
        RadTxtEmail2.Text = "test@gmail.com";
        RadTxtPhone.Text = "555566";
        RadTxtCell.Text = "23126100";
        //RadTxtIMEI.Text = "353975058435070";
        RadTxtStreet.Text = "116 Parklands";
        RadTxtCity.Text = "Rochford";
        //RadTxtStreet.Text = "SS4 1SY";


        RadComboCountry.SelectedIndex = 54;
        RadComboShipping.SelectedIndex = 0;
        RadTxtZip.Text = "SS4 1SY";
        //RadComboCardType.SelectedIndex = 0;
        //RadTxtComments.Text = "test";
        rbCusAddress.Checked = true;

        RadTxtCC.Text = "1111111111111117";
        RadComboCardType.SelectedIndex = 2;
        RadComboMonth.SelectedIndex = 5;
        RadComboYear.SelectedIndex = 5;
        RadClientFName.Text = "Test";
        RadClientLName.Text = "Test";
        RadCCEmail.Text = "test@test.com";
    }



    public void Upsale()
    {
        RadWindowManager windowManager = new RadWindowManager();
        RadWindow widnow1 = new RadWindow();
        #region
        // Set the window properties   
        //widnow1.NavigateUrl = "Window1.aspx";
        //widnow1.ID = "RadWindow1";
        //widnow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code   
        //windowManager.Windows.Add(widnow1);
        //this.form1.Controls.Add(widnow1);

        /* if (UPImg.Src != "")
         {
             confirmWindow.VisibleOnPageLoad = true;
             ChooseSIMTypePerPack.InnerText = otherUtility.getResourceString("ChooseSIMTypePerPack");
             string sSelect = "";
             string sKntTitle = "";
             string sRegionName = "";
             string sRegionCode = "";
             string sCountryName = "";
             string sOption = "";
             string sCountryCode = SessionUtility.GetValue("UserCountry");
             DataTable dtRegionAreas = dbUtility.getTableBySQLTAIL("SELECT site, KNTCode,Name  FROM tblTelawayCountriesRegions where IsGeneralKNT=0 and IsActive=1 and site='" + sCountryCode + "' order by orderid");
             DataTable dtRegionGeneralAreas = dbUtility.getTableBySQLTAIL("SELECT site, KNTCode,Name  FROM tblTelawayCountriesRegions where IsGeneralKNT=1  and IsActive=1 and site='" + sCountryCode + "' order by orderid");

             SessionUtility.AddValue("DefaultKNTCode", -1);
             sSelect = "<select onChange='onSelectRegionClick(this);' id='SelectRegion'>";
             sSelect = sSelect + "<option style='font-weight:bold;' value='0'>(" + otherUtility.getResourceString("ChooseOne") + ")</option>";

             if (dtRegionAreas != null)
             {
                 foreach (DataRow dr in dtRegionAreas.Rows)
                 {
                     sRegionName = dr["Name"].ToString();
                     sRegionCode = dr["KNTCode"].ToString();
                     sCountryName = dr["site"].ToString();

                     sOption = "<option  style='font-weight:normal;' value='" + sRegionCode + "'>" + sRegionName + "</option>";
                     sSelect = sSelect + sOption;


                 }
             }

             if (dtRegionGeneralAreas != null)
             {
                 if (dtRegionGeneralAreas.Rows.Count > 0 && sCountryCode != "WW")
                 {
                     sOption = "<option style='font-weight:bold;' value='0'>(" + otherUtility.getResourceString("IfYourCity") + ")</option>";
                     sSelect = sSelect + sOption;
                 }
                 foreach (DataRow dr2 in dtRegionGeneralAreas.Rows)
                 {
                     sRegionName = dr2["Name"].ToString();
                     sRegionCode = dr2["KNTCode"].ToString();
                     sCountryName = dr2["site"].ToString();
                     sOption = "<option style='font-weight:normal;' value='" + sRegionCode + "'>" + sRegionName + "</option>";
                     sSelect = sSelect + sOption;

                 }
             }
             sSelect = sSelect + "</select>";
             DivRegion.InnerHtml = sSelect;
         }*/
        #endregion
    }



    protected void ImgPayPalBtn_Click(object sender, ImageClickEventArgs e)
    {
        PlaceOreder(false);
    }
    protected void btnCustomRadWsindowConfirm_Click(object sender, EventArgs e)
    {
        PlaceOreder(!rbPayPal.Checked);
        //simulate longer page load
        //System.Threading.Thread.Sleep(2000);
    }
    /// <summary>
    /// Place The order PayPal= 1 or regeller =0
    /// </summary>
    /// <param name="Mode"></param>
    public void PlaceOreder(bool Mode)
    {
        try
        {
            if (Mode == false) rbCC.Checked = false;/*&& !CheckBoxAgree.Checked)
            {
                lblMsg.InnerHtml = otherUtility.getResourceString("YouMustAgree");
                rbPayPal.Checked = false;
                rbCC.Checked = true;
            }
            else
            {*/
            SessionUtility.AddValue("ValidErr", "");
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            SessionUtility.AddTmpOrder(tmpOrderObj);

            if (!IsValidData())
            {
                lblMsg.InnerHtml = SessionUtility.GetValue("ValidErr");
                FillScreen();
                LoadDiv();
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "GoToErr();", true);

                return;
            }
            else
            {
                if (Upsale("Show"))//&&!SessionUtility.GetBoolValue("ShowUpsale")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(),
                        "UpsalePopup();"
                         //+"$('#ctl00_ContentPlaceHolder1_NewUpsaleAdv').load('" + SessionUtility.GetValue("Note") + " #UpsaleAdvPanel');"
                         , true);
                    SessionUtility.AddValue("ShowUpsale", true);
                }
                else
                {
                    PlaceOrderAfterUpsale(Mode);
                }
            }
            //}

        }
        catch (Exception ex)
        {
            var mode = Mode == true ? "" : "PayPal";
            var st = "StackTrace: " + ex.StackTrace + "\n TargetSite: " + ex.TargetSite;
            emailUtility.SendMailErr("PlaceOreder " + mode + "  \n " + st + "\n" + ex.Message);
        }

    }
    /// <summary>
    /// Fill the data to paypal action : Total Discount and shipping
    /// </summary>
    /// <param name="tmpOrderObj"></param>
    protected void PayPalBtn(TmpOrderObj tmpOrderObj)
    {

    }
    /// <summary>
    /// Add Sim to tmpOrderObj.SimDetails
    /// </summary>
    /// <param name="simDetails"></param>
    /// <returns></returns>
    private List<SIMDetails> UpSale(List<SIMDetails> simDetails)
    {
        string[] SimType = SessionUtility.GetValue("SecendSimType").Split('|');
        try
        {
            SIMDetails s = new SIMDetails();
            s.KNTCode = simDetails[0].KNTCode;
            s.EquipmentCode = int.Parse(SimType[0].ToString());
            s.EquipmentModel = int.Parse(SimType[0].ToString());
            s.EquipmentName = SimType[1].ToString();
            s.SimPrice = simDetails[0].SimPrice;
            s.PhoneIMEI = simDetails[0].PhoneIMEI;
            s.PaypalAmount = simDetails[0].PaypalAmount;
            s.ShipFee = simDetails[0].ShipFee;
            s.ExtendedDataPackageCode = simDetails[0].ExtendedDataPackageCode;
            simDetails.Add(s);
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("Signup Add Sim Item: " + ex.Message);
        }

        return simDetails;
    }

    protected void btnUpsaleSubmit_Click(object sender, EventArgs e)
    {
        //handel the upsale
        if (Upsale("Handle"))
            PlaceOrderAfterUpsale(!rbPayPal.Checked);

        System.Threading.Thread.Sleep(2000);
    }

    protected void btnNoUpsaleSubmit_Click(object sender, EventArgs e)
    {
        PlaceOrderAfterUpsale(!rbPayPal.Checked);

    }




    /// <summary>
    /// place order after upsale
    /// </summary>
    /// <param name="Mode">type of payment Paypal=false </param>
    private void PlaceOrderAfterUpsale(bool Mode)
    {
        FirstStep();
        string redirectURL = FillData();
        if (redirectURL == "Err") { }
        else
        {
            if (redirectURL == "" && Mode == true)
                btnNext();

            else Response.Redirect(redirectURL, false);
        }
        if (SessionUtility.GetValue("ErrSignup") == "Reload")
        {
            SessionUtility.AddValue("ErrSignup", "ReloadDone");
            PlaceOrderAfterUpsale(Mode);
        }
    }
    /// <summary>
    /// Show upsale 
    /// </summary>
    /// <param name="Mode">Show or handel </param>
    /// <returns></returns>
    public bool Upsale(string Mode)
    {
        if (Mode == "Show")
        {
            try
            {
                DataTable dt = dbUtility.getUpsaleTable(HiddenCountry.Value);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MaxSIMForDiscount"].ToString() != "")
                        if (int.Parse(HiddenSIMCount.Value) > int.Parse(dt.Rows[0]["MaxSIMForDiscount"].ToString()))
                            return false;
                    if (dt.Rows[0]["Img"].ToString() == "")
                        return false;
                    SessionUtility.AddValue("SimQnt", HiddenSIMCount.Value);
                    SessionUtility.AddValue("Note", dt.Rows[0]["Note"].ToString());
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                emailUtility.SendMailErr("upsale: " + ex.Message);
                return false;
            }

        }
        if (Mode == "Handle")
        {
            DataTable dt = dbUtility.getUpsaleTable(HiddenCountry.Value);
            if (dt != null && dt.Rows.Count > 0)
            {
                SessionUtility.AddValue("Upsale", dt.Rows[0]["CouponCode"].ToString());
                HiddenSIMCount.Value = dt.Rows[0]["UpgradeTo"].ToString();

                return true;
            }
            else return false;
        }
        return false;
    }

    /// <summary>
    /// Update the sale according to the coupon  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn2thNext_Click(object sender, EventArgs e)
    {
        RadPromoCode.Text = SessionUtility.GetValue("CouponCode");
        SessionUtility.getTmpOrder().CouponCode = SessionUtility.GetValue("CouponCode");
        SessionUtility.getTmpOrder().PlanCostUSD *= 2;
        btnNext();
    }
    /// <summary>
    /// First step of FILL DATA:  Dates, Plans, Quntity
    /// </summary>
    protected void FirstStep()
    {
        int iFlag = 0;
        try
        {
            iFlag = 1;
            SessionUtility.AddValue("ShowUpsale", false);
            SessionUtility.AddValue("SimQnt", HiddenSIMCount.Value);
            HiddenSIMCount.Value = SessionUtility.GetValue("SimQnt");
            if (SessionUtility.GetValue("UserCountry") == "")
            {
                RedirectToHomeSite();
                return;
            }
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            if (tmpOrderObj == null)
            {
                RedirectToHomeSite();
                return;
            }
            iFlag = 2;
            List<PlanDetails> planDetails = (List<PlanDetails>)SessionUtility.GetObjValue("PlanDetails");

            if (planDetails == null) RedirectToHomeSite();
            else
            {
                if (planDetails.Count > 0)
                {
                    var conversionRate = planDetails[0].conversionRate;
                    iFlag = 3;
                    tmpOrderObj.StartDate = Convert.ToDateTime(RadDateStartDate.SelectedDate);
                    tmpOrderObj.EndDate = Convert.ToDateTime(RadDateEndDate.SelectedDate);
                    TimeSpan ts = Convert.ToDateTime(tmpOrderObj.EndDate) - Convert.ToDateTime(tmpOrderObj.StartDate);
                    int differenceInDays = ts.Days + 1;
                    iFlag = 3;
                    decimal priceUSDPerItem = 0;
                    if (HiddenPlanPrice.Value != "")
                    {
                        string sPrice = HiddenPlanPrice.Value;
                        if (SessionUtility.IsFrenchLang())
                            sPrice = HiddenPlanPrice.Value.ToString().Replace(",", ".");

                        var price = Convert.ToDouble(sPrice); //differenceInDays < 11 ? 39.99 : 59.99;

                        priceUSDPerItem = Convert.ToDecimal(price) * conversionRate;
                        SessionUtility.AddValue("priceUSDPerItem", priceUSDPerItem);
                        int simQnty = int.Parse(HiddenSIMCount.Value);

                        tmpOrderObj.PricePerItem = Convert.ToDecimal(price);
                        tmpOrderObj.PlanCostUSD = Convert.ToDecimal(String.Format("{0:0.00}", priceUSDPerItem * simQnty));
                        tmpOrderObj.OpCostUSD = 0;

                        SessionUtility.AddValue("simQnty", simQnty);
                        iFlag = 4;
                        SessionUtility.AddValue("PlanCost", price * simQnty);
                        SessionUtility.AddValue("OpCost", 0);
                        SessionUtility.AddValue("DaysDiff", differenceInDays);
                    }
                    tmpOrderObj.PhonesRequired = int.Parse(HiddenSIMCount.Value);

                    iFlag = 5;
                    int iDataPackage = planDetails[0].extendedPackageCode;
                    string sDataPackage = "";

                    double itemTotal1Order = 0.0;
                    double itemTotal = 0.0;
                    double ItemTotalAcc = 0.0;

                    //Fill Accessories to Cart
                    string[] Accessories = RadAccessoriesDetails.Value.Split('|');
                    CartItems cr = new CartItems();
                    AccessoriesCartItems = cr.getObject(Accessories, planDetails[0].conversionRate);

                    var strAccessoryIdAndQuantity = "";
                    var AccIdQnt = "";
                    if (Accessories.Length > 0)
                    {
                        for (int i = 0; i < Accessories.Length - 1; i++)
                        {
                            string[] item = Accessories[i].Split(',');
                            strAccessoryIdAndQuantity += item[1] + " " + item[0] + "-" + item[2] + ",";
                            AccIdQnt += item[0] + "-" + item[2] + ",";
                            ItemTotalAcc += Convert.ToDouble(item[3]);
                            itemTotal += ItemTotalAcc;
                        }
                        tmpOrderObj.AccessoryIdAndQuantity = AccIdQnt;
                        tmpOrderObj.Tag = strAccessoryIdAndQuantity;

                    }
                    // Fill SIM info
                    List<SIMDetails> simDetails = new List<SIMDetails>();

                    //decimal amountPerItem = Convert.ToDecimal(tmpOrderObj.PlanCostUSD / tmpOrderObj.PhonesRequired);
                    iFlag = 6;
                    //KNTSimDetails.Value = SessionUtility.GetValue("KNTSimDetails");
                    //RadSimDetails.Value = SessionUtility.GetValue("RadSimDetails");


                    string[] kntSim = KNTSimDetails.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    //emailUtility.SendMailErr("KNT:" + KNTSimDetails.Value.ToString());   
                    string[] details = RadSimDetails.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (details.Length > 0)
                    {
                        for (int i = 0; i < details.Length; i++)
                        {
                            string sKNT = kntSim[i];
                            int iKnt = Convert.ToInt32(sKNT);
                            string[] dataPerSim = details[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            int simCode = Int32.Parse(dataPerSim[0]);
                            string IMEI = "353975058435070"; // ????
                            decimal amountPerItem = priceUSDPerItem;
                            iFlag = 7;
                            iDataPackage = planDetails[0].extendedPackageCode;

                            iFlag = 8;
                            amountPerItem = Convert.ToDecimal(String.Format("{0:0.00}", amountPerItem));
                            decimal simPrice = planDetails[0].simPrice;// SessionUtility.GetDecimalValue("choosenPlan.simPrice");

                            iFlag = 9;
                            AddSimItem(iKnt, simCode, IMEI, iDataPackage, amountPerItem, simPrice, 0, ref simDetails);
                            itemTotal1Order = (double)(simPrice + amountPerItem);
                            itemTotal += itemTotal1Order;

                        }
                    }
                    tmpOrderObj.SimDetails = simDetails;

                    SessionUtility.AddValue("totalAmountCharged", itemTotal);

                    PlanDetails choosenPlan = planDetails[0];
                    //for AU

                    if (!planDetails[0].countryName.Equals("CA"))
                    {
                        iFlag = 10;
                        if (differenceInDays < 11)
                            choosenPlan = planDetails[0];
                        else if (differenceInDays < 31)
                            choosenPlan = planDetails[1];
                        else if (differenceInDays < 46)
                            choosenPlan = planDetails[2];
                        else if (differenceInDays < 61)
                            choosenPlan = planDetails[3];
                    }
                    // for CA
                    if (planDetails[0].countryName.Equals("CA"))
                    {
                        if (differenceInDays <= 10)
                            choosenPlan = planDetails[0];
                        else if (differenceInDays <= 30)
                            choosenPlan = planDetails[1];
                        else if (differenceInDays < 61)
                            choosenPlan = planDetails[2];
                    }

                    iFlag = 11;
                    SessionUtility.AddValue("choosenPlan.simPrice", choosenPlan.simPrice);
                    SessionUtility.AddValue("choosenPlan.title", choosenPlan.title);
                    tmpOrderObj.PlanName = choosenPlan.callPackageName;
                    tmpOrderObj.CallPackageCode = choosenPlan.callPackageCode;

                    tmpOrderObj.CallPackageName = tmpOrderObj.PlanName;
                    iFlag = 12;
                    tmpOrderObj.SignupSourceID = "TA_" + choosenPlan.countryName;
                    tmpOrderObj.GroupName = dbUtility.ExecScalarByStrParam("select SiteUrl from [tblTelawaySitesInfo] where name = @Name", "Name", choosenPlan.countryName);

                    tmpOrderObj.EquipmentNotes = "";
                    tmpOrderObj.IsKosher = false;
                    tmpOrderObj.IsSim = true;
                    tmpOrderObj.IsEquipmentSNS = false;
                    tmpOrderObj.Insurance = false;
                    tmpOrderObj.SurfAndSave = false;
                    tmpOrderObj.Special = false;
                    tmpOrderObj.bitCallPackageOverageProtection = false;
                    tmpOrderObj.ChargeWithPayPal = false;

                    tmpOrderObj.SMSPackageCounter = (int)choosenPlan.smsPackageCode; //dbUtility.ExecIntScalar("SELECT  SmsPackageCode FROM tblUkGeneralParams");
                    tmpOrderObj.SMSPackageCode = tmpOrderObj.SMSPackageCounter;
                    tmpOrderObj.SMSPackageName = "Unlimited USA and Intl SMS - included!";

                    tmpOrderObj.PlanCode = (int)choosenPlan.planCode;// dbUtility.ExecIntScalar("SELECT PlanCode  FROM tblUkGeneralParams");
                    tmpOrderObj.ProductId = 1;

                    tmpOrderObj.DataPackageId = iDataPackage;
                    tmpOrderObj.DataPackageCode = iDataPackage;
                    tmpOrderObj.DataPackageName = sDataPackage;
                    tmpOrderObj.DataPackgeSize = "";

                    tmpOrderObj.KITD_PlanCode = (int)choosenPlan.kntCode;// dbUtility.ExecIntScalar("SELECT  KntCode FROM tblUkGeneralParams");
                    tmpOrderObj.KITD = true;
                    tmpOrderObj.KNTName = "Virtual Number";
                    tmpOrderObj.KNTRequired = 1;
                    tmpOrderObj.KITD_BLOCK_ID = false;

                    tmpOrderObj.ParentLink = choosenPlan.parentLink;
                    tmpOrderObj.SubLink = choosenPlan.subLink;

                    tmpOrderObj.UserStreet = "Telaway NET- " + SessionUtility.GetValue("UserCountry") + " USA phone";// SignupUtils.getSubLinkField("AdminComment");
                    tmpOrderObj.UserCity = "N/A";
                    tmpOrderObj.PWD = "N/A";
                    tmpOrderObj.ClientFax = "0";
                    tmpOrderObj.SessionID = SessionUtility.GetSessionId();
                    iFlag = 13;
                    var agentCode = SessionUtility.GetValue("AgentCode");
                    if (!string.IsNullOrEmpty(agentCode))
                    {
                        tmpOrderObj.AgentCode = int.Parse(agentCode);
                    }
                    if (SessionUtility.GetIntValue("SubAgentCode") > 0)
                    {
                        tmpOrderObj.SubAgentCode = SessionUtility.GetIntValue("SubAgentCode");
                    }
                    iFlag = 14;
                    tmpOrderObj.SiteSourceCountry = choosenPlan.countryName;

                    tmpOrderObj.PageNo = 1;

                    iFlag = 15;
                    SessionUtility.AddTmpOrder(tmpOrderObj);
                    SessionUtility.AddValue("SIMCount", tmpOrderObj.SimDetails.Count);

                    //Response.Redirect("SignupStep_2.aspx?c=" + SessionUtility.GetValue("UserCountry").ToUpper(), false);
                    LoadExistData();
                }
            }
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("Signup step1 Btn Next: " + ex.Message + "iFlag=" + iFlag);
        }
    }
    /// <summary>
    /// take all the information and submit it 
    /// </summary>
    protected void btnNext()
    {
        // confirmWindow.VisibleOnPageLoad = false;
        //return;
        if (submit())
        {

            HiddenStatus.Value = "SignupStep_4.aspx?c=" + SessionUtility.GetValue("UserCountry").ToUpper();
            Button2.Enabled = false;
            Button2.CssClass = "btnSubmitDisabled";
            Button2.Attributes["ClassName"] = "btnSubmitDisabled";
            SessionUtility.AddValue("newOrderSite", false);
            SessionUtility.AddValue("newSite", true);
            //this.MasterPageFile = otherUtility.getMasterName();
            Response.Redirect("SignupStep_4.aspx?c=" + SessionUtility.GetValue("UserCountry").ToUpper(), false);
            //simulate longer page load
            //System.Threading.Thread.Sleep(2000);
        }
    }
    protected void btnPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("SignupStep_1.aspx?1=2&" + SessionUtility.GetLinkNewSite(), false);
    }


    private bool submit()
    {
        //Test
        //return false;
        int i = 0;
        try
        {

            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();

            if (tmpOrderObj == null)
            {
                SessionUtility.ClearAll();
                Response.Redirect("SignupStep_1.aspx?" + SessionUtility.GetLinkNewSite(), false);
            }
            else
            {
                if (!IsValidData())
                {
                    i = 1;
                    lblMsg.InnerHtml = SessionUtility.GetValue("ValidErr");
                    FillScreen();
                    LoadDiv();
                    i = 2;
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), "GoToErr();", true);

                    return false;
                }
                //if (tmpOrderObj.CouponCode == SessionUtility.GetValue("CouponCode"))//UpSalse
                //{
                //    tmpOrderObj.PhonesRequired++;
                //    tmpOrderObj.SimDetails = UpSale(tmpOrderObj.SimDetails);
                //}
                // TODO: Load agian all the data ? 
                string redirectURL = FillData();
                i = 3;
                if (rbCC.Checked)
                    tmpOrderObj.ChargeWithPayPal = false;

                if (rbPayPal.Checked)
                {
                    tmpOrderObj.ChargeWithPayPal = true;

                    if (tmpOrderObj.PayPalTransactionId == "")
                        PayPalBtn(tmpOrderObj);
                    else
                    {
                        if ((tmpOrderObj.PayPalTransactionId == "Err" || tmpOrderObj.PayPalTransactionId == "Cancel" || tmpOrderObj.PayPalTransactionId.Trim() == "") && (tmpOrderObj.ChargeWithPayPal))
                        {
                            lblMsg.InnerHtml = otherUtility.getResourceString("PayPalProcessFail") + ". " + SessionUtility.GetValue("ValidErr") + SessionUtility.GetValue("ValidErrP");
                        }
                    }
                }

                if (!tmpOrderObj.ChargeWithPayPal)
                {
                    i = 4;
                    if (SignupUtility.FillOrderObjAndSubmit(tmpOrderObj))
                    {
                        tmpOrderObj.PageNo = 3;
                        i = 5;
                        return true;
                    }
                    else
                    {
                        i = 6;
                        lblMsg.InnerHtml = SessionUtility.GetValue("InvalidData");
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr(" Submit: i=" + i + "\n" + e.Message);
            return false;
        }
    }




    #region Load

    private void LoadText()
    {
        try
        {
            string sCountry = SessionUtility.GetValue("UserCountry").ToString();
            if (sCountry != "")
            {
                listRow1.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow1' and site=@Country", "Country", sCountry);
                listRow2.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow2' and site=@Country", "Country", sCountry);
                listRow3.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow3' and site=@Country", "Country", sCountry);

                if (listRow1.InnerHtml.Length > 2)
                {
                    if (listRow1.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow1.InnerHtml = listRow1.InnerHtml.Replace("•", "");
                }
                if (listRow2.InnerHtml.Length > 2)
                {
                    if (listRow2.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow2.InnerHtml = listRow2.InnerHtml.Replace("•", "");
                }

                if (listRow3.InnerHtml.Length > 2)
                {
                    if (listRow3.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow3.InnerHtml = listRow3.InnerHtml.Replace("•", "");
                }

                string sh = listRow3.InnerHtml;
                if (sh.IndexOf("<img") > 0)
                    sh = sh.Substring(0, sh.IndexOf("<img"));

                listRow3.InnerHtml = sh.Trim() + "!";
                listRow5.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow5' and site=@Country", "Country", sCountry);
                listRow8.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow8' and site=@Country", "Country", sCountry);
                listRow9.InnerHtml = dbUtility.ExecScalarByStrParam("SELECT Text FROM tblTAIL_NET_HomePage where Type='leftListRow9' and site=@Country", "Country", sCountry);

                if (listRow5.InnerHtml.Length > 2)
                {
                    if (listRow5.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow5.InnerHtml = listRow5.InnerHtml.Replace("•", "");
                }

                if (listRow8.InnerHtml.Length > 2)
                {
                    if (listRow8.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow8.InnerHtml = listRow8.InnerHtml.Replace("•", "");
                }
                if (listRow9.InnerHtml.Length > 2)
                {
                    if (listRow9.InnerHtml.Trim().Substring(0, 1) == "•")
                        listRow9.InnerHtml = listRow9.InnerHtml.Replace("•", "");
                }
            }
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("Signup step1 LoadText: " + ex.Message);
        }
    }
    private void LoadTextByLang()
    {


        Checkout.InnerText = otherUtility.getResourceString("Checkout");
        TermsCond.HRef = dbUtility.GetTermsURL();


        RadComboShipping.Height = 100;
        string sShipComment = dbUtility.ExecScalar("SELECT ShipComment FROM tblUKShippingOptions where ShipComment is not null and SiteName='" + SessionUtility.GetValue("UserCountry") + "'");
        if (sShipComment != "")
            shipComment.InnerText = sShipComment;

        if (!SessionUtility.IsEngLang())
        {
            RadDeliveryDate.Culture = new System.Globalization.CultureInfo(SessionUtility.GetLanguageValue("Language"), true);
        }
        string sLang = SessionUtility.GetLanguageValue("Language").ToUpper().Substring(0, 2).Trim();
        //ImgPayPalBtn.ImageUrl = "~/img/PayPalImg" + sLang + ".png";


        PlanDetailsTxt.InnerText = otherUtility.getResourceString("PlanDetails");
        Description.InnerText = otherUtility.getResourceString("Description");
        Price.InnerText = otherUtility.getResourceString("Price");

        OrderTotal.InnerText = otherUtility.getResourceString("OrderTotal");
        Shipping.InnerText = otherUtility.getResourceString("Shipping");
        Shipping2.InnerText = otherUtility.getResourceString("Shipping2");
        TotalAmount.InnerText = otherUtility.getResourceString("TotalAmount") + " ";
        before.InnerText = otherUtility.getResourceString("before") + " ";
        HiddenIncluding.Value = otherUtility.getResourceString("including") + " ";
        HiddenShipping.Value = otherUtility.getResourceString("Shipping");
        HiddenCost.Value = otherUtility.getResourceString("cost");
        HiddenSIMCount.Value = SessionUtility.GetValue("SimCount");

        //liShipping.InnerText = otherUtility.getResourceString("Shipping");
        //liCost.InnerText = otherUtility.getResourceString("cost");
        BillingInformation.InnerText = otherUtility.getResourceString("BillingInformation");
        AccessoriesSpn.InnerText = otherUtility.getResourceString("Accessories");
        Subtotal.InnerText = otherUtility.getResourceString("Subtotal");
        Total.InnerText = otherUtility.getResourceString("Total");
        ContinueShopping.InnerText = otherUtility.getResourceString("ContinueShopping");
        ApplyCouponCode.Attributes["placeholder"] = otherUtility.getResourceString("ApplyCouponCode");
        SelectPaymentMethod.InnerText = otherUtility.getResourceString("SelectPaymentMethod");


        RadTxtFName.EmptyMessage = otherUtility.getResourceString("FirstName") + ":*";
        RadTxtLName.EmptyMessage = otherUtility.getResourceString("LastName") + ":*";
        RadTxtEmail.EmptyMessage = otherUtility.getResourceString("EmailAddress") + ":*";
        RadTxtEmail2.EmptyMessage = otherUtility.getResourceString("ConfirmEmail") + ":*";
        RadTxtPhone.EmptyMessage = otherUtility.getResourceString("Telephone") + ":*";
        RadTxtCell.EmptyMessage = otherUtility.getResourceString("Mobile") + ":*";
        if (SessionUtility.GetLangParam() == "4")
            RadTxtCell.Mask = "###############";
        RadTxtStreet.EmptyMessage = otherUtility.getResourceString("Address") + ":*";
        RadTxtCity.EmptyMessage = otherUtility.getResourceString("City") + ":*";
        RadComboCountry.EmptyMessage = otherUtility.getResourceString("Country") + ":*";
        RadComboStateUSA.EmptyMessage = "Province:*";// otherUtility.getResourceString("StateProv") + ":*";
        RadComboStateAU.EmptyMessage = "Province:*";// otherUtility.getResourceString("StateProv") + ":*";
        RadComboStateC.EmptyMessage = "Province:*";// 

        HiddenProvience.Value = otherUtility.getResourceString("StateProv") + ":*";
        RadTxtZip.EmptyMessage = otherUtility.getResourceString("PostalCode");



        RequiredfieldFirstName.ErrorMessage = otherUtility.getResourceString("InvalidFirstName");
        RequiredfieldLastName.ErrorMessage = otherUtility.getResourceString("InvalidLastName");
        RegularExpressionEmailIsValid.ErrorMessage = otherUtility.getResourceString("InvalidEmail");
        RequiredfieldConfirmedemail.ErrorMessage = otherUtility.getResourceString("InvalidEmail");
        RegularExpressionValidEmail2.ErrorMessage = otherUtility.getResourceString("InvalidEmail");
        RequiredfieldEmailValid.ErrorMessage = otherUtility.getResourceString("InvalidEmail");
        CompareValidEmail.ErrorMessage = otherUtility.getResourceString("EmailMismatch");

        RequiredFieldHomeNumber.ErrorMessage = otherUtility.getResourceString("InvalidTelephone");
        RequiredFieldMobile.ErrorMessage = otherUtility.getResourceString("InvalidMobile");
        RequiredFieldStreet.ErrorMessage = otherUtility.getResourceString("InvalidAddress");
        RequiredFieldCity.ErrorMessage = otherUtility.getResourceString("InvalidCity");
        RequiredFieldCountry.ErrorMessage = otherUtility.getResourceString("InvalidCountry");

        UseTheAboveAddressForShipping.InnerText = otherUtility.getResourceString("UseTheAboveAddressForShipping");
        LikeToEnteraDifferentAddress.InnerText = otherUtility.getResourceString("LikeToEnteraDifferentAddress");
        RadDeliveryDate.DateInput.EmptyMessage = otherUtility.getResourceString("TheDateYouWillBeLeavingThisAddress");


        ShippingInformation.InnerText = otherUtility.getResourceString("ShippingInformation");
        RadComboShipping.EmptyMessage = otherUtility.getResourceString("DeliveryMethod");
        LearnMore.InnerHtml = otherUtility.getResourceString("LearnMore");
        RadTxtDeliveryName.EmptyMessage = otherUtility.getResourceString("ShippingName") + ":*";
        RadShipAddress.EmptyMessage = otherUtility.getResourceString("Address") + ":*";
        RadShipCity.EmptyMessage = otherUtility.getResourceString("City") + ":*";
        RadComboBoxStateUShip.EmptyMessage = otherUtility.getResourceString("StateProv") + ":*";
        RadTxtShipCountry.EmptyMessage = otherUtility.getResourceString("Country") + ":*";
        RadShipZip.EmptyMessage = otherUtility.getResourceString("PostalCode") + ":*";
        RadTxtDeliveryPhone.EmptyMessage = otherUtility.getResourceString("TelephoneAtDeliveryAddress") + ":*";

        YouAreCurrentlyOnASecuredSite.InnerText = otherUtility.getResourceString("YouAreCurrentlyOnASecuredSite");
        ChoosePaymentMethod.InnerText = otherUtility.getResourceString("ChoosePaymentMethod");
        PaybyPayPal.InnerText = otherUtility.getResourceString("PaybyPayPal");
        PaybyCCdetails.InnerText = otherUtility.getResourceString("PaybyCCdetails");
        RadComboCardType.EmptyMessage = otherUtility.getResourceString("CreditCardType") + ":*";
        if (SessionUtility.GetLanguageValue("Language").Contains("BR"))
        {
            CCNotice.Visible = true;
            CCNotice.InnerText = otherUtility.getResourceString("CCNotice");
        }
        RadTxtCC.EmptyMessage = otherUtility.getResourceString("CreditCardNumber") + ":*";
        ExpirationDate.InnerHtml = otherUtility.getResourceString("ExpirationDate") + ":*";
        RadClientFName.EmptyMessage = otherUtility.getResourceString("CardholderFirstName") + ":*";
        RadClientLName.EmptyMessage = otherUtility.getResourceString("CardholderLastName") + ":*";
        ThisTransWillApearTNS.InnerText = otherUtility.getResourceString("ThisTransWillApearTNS");
        AgreeTermsOfService1.InnerText = otherUtility.getResourceString("AgreeTermsOfService1");

        AgreeTermsOfService2.InnerText = " " + otherUtility.getResourceString("AgreeTermsOfService2");
        TermsCond.InnerText = otherUtility.getResourceString("TermsCond");

        RadPromoCode.EmptyMessage = otherUtility.getResourceString("CouponCode");
        RadTxtComments.EmptyMessage = otherUtility.getResourceString("Notes");
        RadCCEmail.EmptyMessage = otherUtility.getResourceString("CardholderEmail") + ":*";

        ReturnPolicy.InnerHtml = siteInfo != null ? siteInfo.ReturnPolicy : "";
        RequiredFieldValidatorCC1.ErrorMessage = otherUtility.getResourceString("InvalidCCType");
        RequiredFieldValidatorCC2.ErrorMessage = otherUtility.getResourceString("InvalidNumber");
        RequiredFieldValidatorCC3.ErrorMessage = otherUtility.getResourceString("InvalidYear");
        RequiredFieldValidatorCC4.ErrorMessage = otherUtility.getResourceString("InvalidMonth");
        RequiredFieldValidatorCC5.ErrorMessage = otherUtility.getResourceString("InvalidFirstName");
        RequiredFieldValidatorCC6.ErrorMessage = otherUtility.getResourceString("InvalidLastName");
        RequiredFieldValidatorCC7.ErrorMessage = otherUtility.getResourceString("InvalidEmail");
        RequiredFieldValidatorCC8.ErrorMessage = otherUtility.getResourceString("InvalidEmail");

        Button2.Text = otherUtility.getResourceString("PlaceYourOrder");
        //Button1.Text = otherUtility.getResourceString("Prev");





        try
        {

            if (SessionUtility.GetBoolValue("newOrderSite"))
            {
                divTitle.Visible = false;
                divTitleSpace.Visible = false;
                (this.Master as MasterPageInnerFM).UpdateTitle(otherUtility.getResourceString("OrderNow"));

            }
            else
            {//                OrderNow.InnerText = otherUtility.getResourceString("OrderNow");
            }


            if (!SessionUtility.IsEngLang())
            {
                CultureInfo CI = new System.Globalization.CultureInfo(SessionUtility.GetLanguageValue("Language"), true);

                RadDateStartDate.Culture = CI;
                RadDateEndDate.Culture = RadDateStartDate.Culture;

                RadDateStartDate.DateInput.DisplayDateFormat = CI.DateTimeFormat.ShortDatePattern;
                RadDateEndDate.DateInput.DisplayDateFormat = CI.DateTimeFormat.ShortDatePattern;
                RadDeliveryDate.DateInput.DisplayDateFormat = CI.DateTimeFormat.ShortDatePattern;
            }
            HiddenCountry.Value = SessionUtility.GetValue("UserCountry").ToUpper();
            PleaseSelectStartDate.InnerText = otherUtility.getResourceString("PleaseSelectStartDate");
            PleaseSelectStartDateHidden.Value = otherUtility.getResourceString("PleaseSelectStartDate");
            HiddenSimComment.Value = otherUtility.getResourceString("SimComment");
            HiddenKntSimTitle.Value = otherUtility.getResourceString("KNTSimTitle");
            HiddenNanoSimTxt.Value = otherUtility.getResourceString("NanoS");
            HiddenMicroSimTxt.Value = otherUtility.getResourceString("MicroS");
            HiddenSimTxt.Value = otherUtility.getResourceString("UsaR");
            HiddenDefaultKNTCode.Value = "-1";
            From.InnerText = otherUtility.getResourceString("From") + ":";
            To.InnerText = otherUtility.getResourceString("To") + ":";
            RadDateStartDate.DateInput.EmptyMessage = otherUtility.getResourceString("From");
            RadDateEndDate.DateInput.EmptyMessage = otherUtility.getResourceString("To");
            DaysCount.InnerText = otherUtility.getResourceString("DaysCount");
            YourPlanInc.InnerText = otherUtility.getResourceString("YourPlanInc");
            divQnt.InnerHtml = "<select id='cbQnty' runat='server' class='form-control' onchange='ShowPackageDatails()'><option>" + otherUtility.getResourceString("SelecQnt") + ":" + "</option> <option>1</option> <option>2</option> <option>3</option> <option>4</option> <option>5</option> <option>6</option> <option>7</option> <option>8</option> <option>9</option> <option>10</option> </select>";
            SelectQuantityHidden.Value = otherUtility.getResourceString("SelecQnt");
            PleaseSelectQuantityHidden.Value = otherUtility.getResourceString("pleaseSelectQnt");
            ChooseSIMTypePerPack.InnerText = otherUtility.getResourceString("ChooseSIMTypePerPack");
            SimDetails.InnerText = otherUtility.getResourceString("SimDetails");
            //sum.InnerText = otherUtility.getResourceString("sum") + ":";
            //Plan.InnerText = otherUtility.getResourceString("Plan");

            /*AsLowAs.InnerText = otherUtility.getResourceString("AsLowAs");
            PerDay.InnerText = otherUtility.getResourceString("PerDay");

            AsLowAs2.InnerText = otherUtility.getResourceString("AsLowAs");
            PerDay2.InnerText = otherUtility.getResourceString("PerDay");

            AsLowAs3.InnerText = otherUtility.getResourceString("AsLowAs");
            PerDay3.InnerText = otherUtility.getResourceString("PerDay");

            AsLowAs4.InnerText = otherUtility.getResourceString("AsLowAs");
            PerDay4.InnerText = otherUtility.getResourceString("PerDay");
            
            AviaPlan.InnerText = otherUtility.getResourceString("AviaPlan");
            ExtendedTrips.InnerText = otherUtility.getResourceString("ExtendedTrips");

            toAdd15D.InnerText = otherUtility.getResourceString("toAdd") + " 15 " + otherUtility.getResourceString("Days");
            toAdd30D.InnerText = otherUtility.getResourceString("toAdd") + " 30 " + otherUtility.getResourceString("Days");
            
            SelectQuantity.Text = otherUtility.getResourceString("SelecQnt");
            Button1.Value = otherUtility.getResourceString("Next");
*/
            EndDateBeforeStartDate.Value = otherUtility.getResourceString("EndDateBeforeStartDate");
            ForOrderOver60.Value = otherUtility.getResourceString("ForOrderOver60");
            Package.Value = otherUtility.getResourceString("Pack");
            LearnMoreTxt.InnerText = otherUtility.getResourceString("LearnMore");

            string sPopName = "SimInfo.html";//"SimInfo1.aspx";
            if (!SessionUtility.IsEngLang())
                sPopName = "SimInfo" + SessionUtility.GetValue("UserCountry") + ".html";


            HtmlGenericControl iframe = new HtmlGenericControl("IFRAME");
            iframe.Attributes["src"] = "PopUpText/" + sPopName;
            iframe.Style.Add("width", "100%");
            iframe.Style.Add("height", "280px");
            RadToolTip2.Height = 300;
            RadToolTip2.Controls.Clear();
            RadToolTip2.Controls.Add(iframe);
            //RadToolTip2.TargetControlID = "linkSimInfo";

            RadToolTip3.Text = otherUtility.getResourceString("CellInfo");

        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("LoadTextByLan: " + ex.Message);
        }


    }
    private void LoadDiv()
    {

        int i = 0;
        try
        {
            if (RadComboShipping.SelectedIndex == -1) return;
            RadTxtShipCountry.Text = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString();
            i = 1;
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            if (tmpOrderObj == null)
                RedirectToHomeSite();
            else
            {
                i = 2;
                decimal shipFee = 0;

                if (RadComboShipping.SelectedIndex != 0)
                    shipFee = Convert.ToDecimal(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["ShipCostGBP"].ToString());

                i = 3;
                if (tmpOrderObj != null)
                {
                    List<PlanDetails> planDetails = (List<PlanDetails>)SessionUtility.GetObjValue("PlanDetails");
                    if (planDetails == null)
                        return;

                    // true or false for dispalying also USD price
                    //HiddenDisplayDollars.Value = planDetails[0].displayUSD.ToString();
                    i = 4;
                    if (planDetails == null || planDetails.Count == 0)
                        return;

                    i = 5;
                    decimal conversionRate = planDetails[0].conversionRate;
                    HiddenCurrency.Value = planDetails[0].currencySymbol;

                    // load chart details
                    decimal totalAmount = 0;
                    i = 6;
                    StringBuilder strPlaneDetails = new StringBuilder();
                    decimal pricePerItem = Convert.ToDecimal(tmpOrderObj.PricePerItem);
                    i = 7;
                    if (tmpOrderObj.SimDetails != null)
                    {
                        foreach (SIMDetails sd in tmpOrderObj.SimDetails)
                        {
                            // totalSim += Convert.ToDecimal(sd.simPrice);
                            i = 8;
                            decimal amount = Convert.ToDecimal(pricePerItem);
                            decimal simPrice = (decimal)planDetails[0].simPrice;
                            totalAmount += amount;
                            totalAmount += simPrice;
                            if (simPrice > 0)
                            {
                                strPlaneDetails.AppendFormat(@"<tr><td>{0}</td><td class=""Right"">{1}</td></tr>"
                                   , sd.EquipmentName
                                   , otherUtility.FormatCurrencySymbolAndAmount(simPrice, planDetails[0].currencySymbol));
                            }
                            i = 9;
                            strPlaneDetails.AppendFormat(@"<tr><td>{0}</td><td class=""Right"">{1}</td></tr>"
                               , SessionUtility.GetValue("UserCountry") == "CA" ? SessionUtility.GetValue("choosenPlan.title") : sd.EquipmentName
                               , otherUtility.FormatCurrencySymbolAndAmount(amount, planDetails[0].currencySymbol));

                        }
                        i = 10;
                        spnOrderTotal.InnerHtml = otherUtility.FormatCurrencyFRSymbolAndAmount(totalAmount, planDetails[0].currencySymbol);// totalAmount.ToString() + planDetails[0].currencySymbol;

                        if (shipFee != 0)
                        {
                            totalAmount += shipFee;
                            spnShipCost.InnerHtml = shipFee.ToString("F");
                        }
                        i = 11;
                        //==== from page method
                        //PlanDetails.InnerHtml = strPlaneDetails.ToString();
                        //spnOrderTotal.InnerHtml = otherUtility.FormatCurrencyFRSymbolAndAmount(totalAmount, planDetails[0].currencySymbol);// totalAmount.ToString() + planDetails[0].currencySymbol;
                        TotalAmtPlusShipping.InnerHtml = otherUtility.FormatCurrencyFRSymbolAndAmount(totalAmount, planDetails[0].currencySymbol);// totalAmount.ToString() + planDetails[0].currencySymbol;

                        if (SessionUtility.IsFrenchLang())
                        {
                            spnOrderTotal.InnerHtml = spnOrderTotal.InnerHtml.Replace(".", ",");
                            TotalAmtPlusShipping.InnerHtml = TotalAmtPlusShipping.InnerHtml.Replace(".", ",");

                        }
                        totalAmtDollars.InnerHtml = String.Format("{0:0.00}", totalAmount * conversionRate);
                        totalAmtDollars.InnerHtml = String.Format("{0:0.00}", totalAmount * conversionRate);
                        i = 12;
                        SessionUtility.AddValue("totalAmountCharged", totalAmount * conversionRate);
                    }
                }
                if (SessionUtility.GetBoolValue("rbDiffAddress"))
                {
                    i = 13;
                    RadTxtShipCountry.Text = RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["CountryName"].ToString();
                    ShipDetails.Style.Add("display", "block");

                    RadComboBoxStateUShip.Style.Add("display", "none");
                    RadComboBoxStateUShip.Style.Add("display", "none");
                    RadComboBoxStateUShip.Style.Add("display", "none");
                    i = 14;
                    if (RadTxtShipCountry.Text.ToString().ToUpper() == "USA")
                    {
                        RadComboBoxStateUShip.Style.Add("display", "block");
                    }
                    if (RadTxtShipCountry.Text.ToString().ToUpper() == "CANADA")
                    {
                        RadComboBoxStateCShip.Style.Add("display", "block");
                    }
                    if (RadTxtShipCountry.Text.ToString().ToUpper() == "AUSTRALIA")
                    {
                        RadComboBoxStateAUShip.Style.Add("display", "block");
                    }


                }
                else
                    ShipDetails.Style.Add("display", "none");


                if (tmpOrderObj != null)
                {
                    i = 15;
                    if (!string.IsNullOrEmpty(tmpOrderObj.ShipCountry))
                        shipcountryHidden.Value = tmpOrderObj.ShipCountry.ToLower();
                }

                if (RadComboShipping.SelectedIndex > 0)
                {
                    if (RadComboShipping.SelectedValue.ToString() != "0")
                    {
                        i = 16;
                        if (!Convert.ToBoolean(RadComboShipping.Items[RadComboShipping.SelectedIndex].Attributes["optLocalPickup"].ToString()))
                        {
                            if (rbDiffAddress.Checked || rbCusAddress.Checked)
                                shipOpAddress.Style.Add("display", "block");
                        }

                        if (rbDiffAddress.Checked)
                        {
                            ShipDetails.Style.Add("display", "block");
                            StateDivShip.Style.Add("display", "none");

                            RadComboBoxStateUShip.Style.Add("display", "none");
                            RadComboBoxStateCShip.Style.Add("display", "none");
                            RadComboBoxStateAUShip.Style.Add("display", "none");
                            i = 17;
                            if (RadTxtShipCountry.Text.ToString().ToUpper() == "USA")
                            {
                                StateDivShip.Style.Add("display", "block");
                                RadComboBoxStateUShip.Style.Add("display", "block");
                            }
                            if (RadTxtShipCountry.Text.ToString().ToUpper() == "CANADA")
                            {
                                StateDivShip.Style.Add("display", "block");
                                RadComboBoxStateCShip.Style.Add("display", "block");
                            }
                            if (RadTxtShipCountry.Text.ToString().ToUpper() == "AUSTRALIA")
                            {
                                StateDivShip.Style.Add("display", "block");
                                RadComboBoxStateAUShip.Style.Add("display", "block");
                            }

                        }
                        else ShipDetails.Style.Add("display", "none");
                    }
                }

                i = 18;
                if (RadComboCountry.SelectedValue.ToString().ToUpper() == "USA")
                {
                    StateDiv.Style.Add("display", "block");
                    RadComboStateUSA.Style.Add("display", "block");
                    RadComboStateAU.Style.Add("display", "none");
                    RadComboStateC.Style.Add("display", "none");
                }
                else if (RadComboCountry.SelectedValue.ToString().ToUpper() == "CANADA")
                {
                    StateDiv.Style.Add("display", "block");
                    RadComboStateC.Style.Add("display", "block");
                    RadComboStateUSA.Style.Add("display", "none");
                    RadComboStateAU.Style.Add("display", "none");
                }
                else if (RadComboCountry.SelectedValue.ToString().ToUpper() == "AUSTRALIA")
                {
                    StateDiv.Style.Add("display", "block");
                    RadComboStateC.Style.Add("display", "none");
                    RadComboStateUSA.Style.Add("display", "none");
                    RadComboStateAU.Style.Add("display", "block");
                }
                else
                    StateDiv.Style.Add("display", "none");

                i = 19;
                //if (RadShipCountry.SelectedValue.ToString().ToUpper() == "USA")
                //{
                //    StateDivShip.Style.Add("display", "block");
                //    RadComboBoxStateUShip.Style.Add("display", "block");
                //    RadComboBoxStateCShip.Style.Add("display", "none");
                //}
                //else if (RadShipCountry.SelectedValue.ToString().ToUpper() == "CANADA")
                //{
                //    StateDivShip.Style.Add("display", "block");
                //    RadComboBoxStateCShip.Style.Add("display", "block");
                //    RadComboBoxStateUShip.Style.Add("display", "none");
                //}
                //else
                //    StateDivShip.Style.Add("display", "none");


                if (tmpOrderObj != null)
                {
                    i = 20;
                    if (tmpOrderObj.ShipCity != null)
                    {
                        if (tmpOrderObj.ShipCity.ToString() != "[pickup]")
                            shipOpAddress.Style.Add("display", "block");
                        i = 21;
                        if (tmpOrderObj.ShipStreet.ToString() != tmpOrderObj.ClientStreet.ToString())
                            rbDiffAddress.Checked = true;
                    }
                }
            }

        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("LoadDiv: " + e.Message + "i:" + i.ToString());
        }


    }
    private void LoadPlansDetails()
    {
        try
        {
            planDetails = new List<PlanDetails>();
            string country = SessionUtility.GetValue("UserCountry");

            if (string.IsNullOrEmpty(country))
                RedirectToHomeSite();
            else
            {
                //Title
                string sSqlAll = "SELECT AllPlansTopTitle,AllPlansTopText,PlanTopTitle ,PlanSubTitle,PromoLabel ,IsRightCurrency,CurrencySymbol,CurrencyLabel ,PlanPerText,ColorHighlight,PlanDetails1,PlanDetails2,PlanDetails3,PlanDetails4,PlanDetails5,PlanDetails6 "
                                 + ",PlanDetails7,PlanDetails8,PlanSignupBtnText FROM Telaway2015.dbo.tblPlans where site=@site and AllPlansTopTitle is not null and AllPlansTopText is not null";
                DataTable tblTopTextPlans = dbUtility.getTableBySQLStrParam(sSqlAll, "site", HiddenUserCountry.Value);
                if (tblTopTextPlans != null)
                {
                    if (tblTopTextPlans.Rows.Count > 0)
                    {
                        DataRow dr1 = tblTopTextPlans.Rows[0];
                        AllPlansTopTitle.InnerText = dr1["AllPlansTopTitle"].ToString();
                        AllPlansTopText.InnerText = dr1["AllPlansTopText"].ToString();
                    }
                }

                //All Plans
                string sSql = "SELECT PlanId,PlanTopTitle ,ShowRate,PlanDays,PlanSubTitle,PromoLabel ,IsRightCurrency,CurrencySymbol,CurrencyLabel ,PlanPerText,ColorHighlight,PlanDetails1,PlanDetails2,PlanDetails3,PlanDetails4,PlanDetails5,PlanDetails6 "
                    + ",PlanDetails7,PlanDetails8,PlanSignupBtnText FROM Telaway2015.dbo.tblPlans where site=@site";
                DataTable tblPlans = dbUtility.getTableBySQLStrParam(sSql, "site", HiddenUserCountry.Value);



                string sql = string.Format(@"select * from tblTelawayPlans where CountryName= @CountryName order by plandays ");
                DataTable dt = dbUtility.getTableBySQLStrParam(sql, "CountryName", country);

                // wrong country name was entered
                if (dt == null)
                    RedirectToHomeSite();

                else if (dt.Rows.Count == 0)
                    RedirectToHomeSite();

                else
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        planDetails.Add(new PlanDetails(row));
                    }

                    HiddenPlanCount.Value = dt.Rows.Count.ToString().Trim();

                    billText.InnerText = planDetails[0].billText;
                    DataRow dr = tblPlans.Rows[0];
                    if (Convert.ToInt32(dr["PlanId"].ToString()) == 1)
                    {
                        bool bColorHighlight = false;
                        if (dr["ColorHighlight"].ToString() != "")
                            bColorHighlight = Convert.ToBoolean(dr["ColorHighlight"].ToString());

                        if (bColorHighlight)
                        {
                            Plan1class.Attributes.Add("class", "pricing-box best");
                            Plan1SubTitle.InnerText = dr["PlanSubTitle"].ToString();
                        }
                        else Plan1SubTitle.InnerText = "";
                        Price1.Attributes["plan"] = planDetails[0].title;
                        Price1.Attributes["price"] = getTotalAmount(0);
                        Price1.Attributes["simPrice"] = getSimPrice(0);
                        Plan1Title.InnerText = dr["PlanTopTitle"].ToString();
                        Plan1PromoLabel.InnerHtml = dr["PromoLabel"].ToString();
                        Plan1Rate.InnerHtml = BuildPlanRate(dr);
                        Plan1CurrencyLabel.InnerText = dr["CurrencyLabel"].ToString();
                        Plan1Details1.InnerText = dbUtility.GetTextWithPlansParams(dr["PlanDetails1"].ToString());
                    }

                    if (planDetails.Count > 0)
                    {
                        HiddenCurrency.Value = planDetails[0].currencySymbol;
                        SessionUtility.AddValue("PlanDetails", planDetails);
                        SessionUtility.AddValue("CurrencySymbol", planDetails[0].currencySymbol);

                        HiddenConversionRate.Value = planDetails[0].conversionRate.ToString();
                        HiddenDisplayDollars.Value = planDetails[0].displayUSD.ToString();

                        if (HiddenDisplayDollars.Value.ToUpper() == "FALSE")
                        {
                            //usdDiv1.Style.Add("display", "none");
                            //usdDiv2.Style.Add("display", "none");
                        }
                        HiddenBBPrice.Value = planDetails[0].bbPrice.ToString();
                        HiddenCurrencyName.Value = planDetails[0].currency;

                        dr = tblPlans.Rows[1];
                        if (Convert.ToInt32(dr["PlanId"].ToString()) == 2)
                        {
                            bool bColorHighlight = false;
                            if (dr["ColorHighlight"].ToString() != "")
                                bColorHighlight = Convert.ToBoolean(dr["ColorHighlight"].ToString());

                            if (bColorHighlight)
                            {
                                Plan2class.Attributes.Add("class", "pricing-box best");
                                Plan2SubTitle.InnerText = dr["PlanSubTitle"].ToString();
                            }
                            else
                                Plan2SubTitle.InnerText = "";
                            Price2.Attributes["plan"] = planDetails[1].title;
                            Price2.Attributes["price"] = getTotalAmount(1);
                            Price2.Attributes["simPrice"] = getSimPrice(1);
                            Plan2Title.InnerText = dr["PlanTopTitle"].ToString();
                            Plan2PromoLabel.InnerHtml = dr["PromoLabel"].ToString();
                            Plan2Rate.InnerHtml = BuildPlanRate(dr);
                            Plan2CurrencyLabel.InnerText = dr["CurrencyLabel"].ToString();
                            Plan2Details1.InnerText = dbUtility.GetTextWithPlansParams(dr["PlanDetails1"].ToString());
                        }


                        dr = tblPlans.Rows[2];
                        if (Convert.ToInt32(dr["PlanId"].ToString()) == 3)
                        {
                            bool bColorHighlight = false;
                            if (dr["ColorHighlight"].ToString() != "")
                                bColorHighlight = Convert.ToBoolean(dr["ColorHighlight"].ToString());

                            if (bColorHighlight)
                            {
                                Plan3class.Attributes.Add("class", "pricing-box best");
                                Plan3SubTitle.InnerText = dr["PlanSubTitle"].ToString();
                            }
                            else Plan3SubTitle.InnerText = "";
                            Price3.Attributes["plan"] = planDetails[2].title;
                            Price3.Attributes["price"] = getTotalAmount(2);
                            Price3.Attributes["simPrice"] = getSimPrice(2);
                            Plan3Title.InnerText = dr["PlanTopTitle"].ToString();
                            Plan3PromoLabel.InnerHtml = dr["PromoLabel"].ToString();
                            Plan3Rate.InnerHtml = BuildPlanRate(dr);
                            Plan3CurrencyLabel.InnerText = dr["CurrencyLabel"].ToString();
                            Plan3Details1.InnerHtml = dbUtility.GetTextWithPlansParams(dr["PlanDetails1"].ToString());
                            Plan3.InnerHtml = dbUtility.GetTextWithPlansParams(dr["PlanDetails2"].ToString());
                            Plan3.Attributes["price"] = planDetails[2].totalAmount.ToString();
                            Plan4.InnerHtml = dbUtility.GetTextWithPlansParams(dr["PlanDetails3"].ToString());
                            if (planDetails.Count > 3)
                            {
                                Plan4.Attributes["price"] = planDetails[3].totalAmount.ToString();
                                Plan4.Attributes["plan"] = planDetails[3].title;
                                Plan4.Attributes["simPrice"] = getSimPrice(3);
                                //  Price4.Attributes["plan"] = planDetails[3].title;
                            }
                        }
                    }

                    // save site info


                    SignupUtility.CreateOrderObj();
                    TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();

                    if (SessionUtility.GetValue("UserCountry") != "")
                    {
                        var sql3 = string.Format(@"select * from tblTelawaySitesInfo where Name = @UserCountry ");

                        DataTable dtSite = dbUtility.getTableBySQLStrParam(sql3, "UserCountry", SessionUtility.GetValue("UserCountry"));

                        if (dtSite != null)
                        {
                            if (dtSite.Rows.Count > 0)
                            {
                                siteInfo = new SiteInfo(dtSite.Rows[0]);

                                SessionUtility.AddValue("SiteInfo", siteInfo);
                                tmpOrderObj.BccEmail = siteInfo.bccEmail.Trim();
                                tmpOrderObj.SalesEmail = siteInfo.salesEmail.Trim();
                                tmpOrderObj.CustomerServicEmail = siteInfo.cusServiceEmail.Trim();
                                tmpOrderObj.ConfirmationEmail = siteInfo.confirmationEmail.Trim();
                                tmpOrderObj.InfoEmail = siteInfo.infoEmail.Trim();
                                tmpOrderObj.MainEmail = siteInfo.mainEmail.Trim();
                                tmpOrderObj.TelawayPhone = siteInfo.phoneNumber;
                                tmpOrderObj.PayPalLocaleCode = siteInfo.payPalLocaleCode;

                                if (string.IsNullOrEmpty(siteInfo.LanguageCode))
                                    siteInfo.LanguageCode = "en-US";
                                tmpOrderObj.SignupLanguageCode = siteInfo.LanguageCode;
                                SessionUtility.AddValue("Language", siteInfo.LanguageCode);
                                otherUtility.ReloadLang();
                                HiddenPhone.Value = siteInfo.phoneNumber;
                            }
                        }
                    }
                    LoadAll(tmpOrderObj, country);
                    SessionUtility.AddTmpOrder(tmpOrderObj);
                }
            }
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("Signup step1 Load Plan Details: " + ex.Message);
        }
    }
    private void LoadToolTip()
    {
        try
        {
            string sCountry = "";

            if (planDetails != null)
                if (planDetails.Count > 0)
                    sCountry = planDetails[0].countryName;

            if (sCountry != "")
            {
                /* HtmlGenericControl iframe = new HtmlGenericControl("IFRAME");
                 iframe.Attributes["src"] = "PopUpText/ShipInfo.aspx?c=" + sCountry;
                 iframe.Style.Add("width", "100%");
                 iframe.Style.Add("height", "470px");

                 RadToolTip1.Height = 480;
                 RadToolTip1.Controls.Clear();
                 RadToolTip1.Controls.Add(iframe);
                 RadToolTip1.TargetControlID = "linkSeepInfo";
                 */

            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("loadToolTip " + e.Message);
        }
    }
    private void LoadAll(TmpOrderObj tmpOrderObj, string country)
    {
        if ((tmpOrderObj == null) || (country == ""))
            return;
        try
        {
            LoadStates("USA", RadComboStateUSA);
            LoadStates("Canada", RadComboStateC);
            LoadStates("Australia", RadComboStateAU);
            LoadStates("USA", RadComboBoxStateUShip);
            LoadStates("Australia", RadComboBoxStateAUShip);
            LoadStates("Canada", RadComboBoxStateCShip);
            string sCountryField;

            if (SessionUtility.IsEngLang())
                sCountryField = "CountryName";
            else
                sCountryField = "CountryName" + SessionUtility.GetValue("UserCountry").Trim().ToUpper();

            DataTable dtCountry = dbUtility.getDataTableBySQL(dbUtility.getSqlCountries());
            if (dtCountry != null)
            {
                dtCountry.DefaultView.Sort = sCountryField;
                RadComboCountry.DataSource = dtCountry;
                RadComboCountry.DataTextField = sCountryField;
                RadComboCountry.DataValueField = "CountryName";
                RadComboCountry.DataBind();
                RadComboCountry.Items.Insert(0, new RadComboBoxItem("", ""));
                RadComboCountry.SelectedIndex = -1;

                /*string sSelect = "<select  class='form-control' style='width: 90%;' onChange='onSelectCountryClick(this);' id='SelectCountry'>" +
                "<option style='font-weight:bold;' value='0'>(" + otherUtility.getResourceString("ChooseOne") + ")</option>";

                DataColumn sCountryName = new DataColumn(sCountryField);
                foreach (DataRow dr in dtCountry.Rows)
                {                    //sCountrySite = dr["isEU"].ToString();
                    sSelect = sSelect + "<option  style='font-weight:normal;' " +
                        "value='" + dr["CountryId"].ToString() + "'>"
                        + dr["CountryName" + SessionUtility.GetValue("UserCountry").Trim().ToUpper()].ToString() + "</option>";

                }
                sSelect = sSelect + "</select>";
                DivCountry.InnerHtml+= sSelect;*/
            }

            string currencySymbol = "";

            if (planDetails != null)
                if (planDetails.Count > 0)
                {
                    currencySymbol = planDetails[0].currencySymbol;
                    SessionUtility.AddValue("currencySymbol", currencySymbol);
                }

            HiddenCountry.Value = SessionUtility.GetValue("UserCountry").ToUpper();
            HiddenConversionRate.Value = planDetails[0].conversionRate.ToString();
            string sshipCostS = "convert(nvarchar(30),ShipCostGBP)";
            if (SessionUtility.IsFrenchLang())
                sshipCostS = "REPLACE(CONVERT(nvarchar(20),ShipCostGBP),'.',',') ";


            string sAgentSQL = "";
            if (SessionUtility.GetValue("AgentName") != "")
            {
                sAgentSQL = " or ( SiteName='{0}' And Activeforagent=1 and agentname='" + SessionUtility.GetValue("AgentName") + "')";
            }
            else
                sAgentSQL = " And (Activeforagent=0 )";

            string sql = "";
            if (SessionUtility.GetBoolValue("bOnlySpecialDelivery"))
            {
                sql = string.Format(@"SELECT REPLACE(CONVERT(nvarchar(20),ShipCostGBP),'.',',') as 'ShipCostGBPd',SamePriceForMultipleSims,Counter,ShipCostGBP,BaseCode,optRequireShipAddress,optLocalPickup, CountryName,ShipName as 'name',
	                                     case when(ShipCostUSA)>0 then '($'+ SUBSTRING(convert(nvarchar(30),ShipCostUSA),0,len(convert(nvarchar(30),ShipCostUSA))-1) + ')' else ' ' end as 'ShipCostDisplay',
	                                    ShipCostUSA as 'ShipCost', 
	                                    case when(ShipCostGBP)>0 then '{1}'+ {3} else '{2}' end as 'cost',
	                                    ShippingMethod  
	                                    FROM tblUKShippingOptions where (IsActive=1 AND SiteName='{0}' AND ActiveForAgent=1 AND agentname Like '%{4}%'  ) 
	                                    order by OrderId"
              // countryname,ShipCostGBP"
              , country
              , currencySymbol, otherUtility.getResourceString("Free"), sshipCostS, SessionUtility.GetValue("AgentName"));
            }
            else
            {
                sql = string.Format(@"SELECT REPLACE(CONVERT(nvarchar(20),ShipCostGBP),'.',',') as 'ShipCostGBPd',SamePriceForMultipleSims,Counter,ShipCostGBP,BaseCode,optRequireShipAddress,optLocalPickup, CountryName,ShipName as 'name',
	                                     case when(ShipCostUSA)>0 then '($'+ SUBSTRING(convert(nvarchar(30),ShipCostUSA),0,len(convert(nvarchar(30),ShipCostUSA))-1) + ')' else ' ' end as 'ShipCostDisplay',
	                                    ShipCostUSA as 'ShipCost', 
	                                    case when(ShipCostGBP)>0 then '{1}'+ {3} else '{2}' end as 'cost',
	                                    ShippingMethod  
	                                    FROM tblUKShippingOptions where ((IsActive=1 AND ActiveForAgent=0 AND SiteName='{0}') {4}) 
	                                    order by OrderId"
                    // countryname,ShipCostGBP"
                    , country
                    , currencySymbol, otherUtility.getResourceString("Free"), sshipCostS, sAgentSQL);
            }
            DataTable dtShipping = dbUtility.getTableBySQLTAIL(sql);
            if (dtShipping != null)
            {
                DataRow dr1 = dtShipping.NewRow();
                dr1["name"] = "";
                dr1["ShipCostGBPd"] = "";
                dr1["Counter"] = 0;
                //dr1["ShipCostGBPd"] = "";
                //dr1["ShipCostGBPd"] = "";
                //dr1["ShipCostGBPd"] = "";
                dtShipping.Rows.InsertAt(dr1, 0);

                RadComboShipping.DataSource = dtShipping;
                RadComboShipping.DataBind();
            }

            RadTxtDeliveryPhone.Attributes.Add("onkeypress", "return checkNumber(event);");


            //LoadDiv();
            PayPalDiv.Style.Add("display", "none");
            CCDiv.Style.Add("display", "block");
            rbCC.Checked = true;

            FillScreen();
            tmpOrderObj.PayPalAmountCharge = 0;
            tmpOrderObj.PayPalTransactionId = "";
            RadComboCardType.Items.Insert(0, new RadComboBoxItem("Amex", "Amex"));
            RadComboCardType.Items.Insert(0, new RadComboBoxItem("Discover", "Discover"));
            RadComboCardType.Items.Insert(0, new RadComboBoxItem("Mastercard", "Mastercard"));
            RadComboCardType.Items.Insert(0, new RadComboBoxItem("Visa", "Visa"));
            RadComboCardType.Items.Insert(0, new RadComboBoxItem("", ""));

            for (int i = 12; i > 0; i--)
                RadComboMonth.Items.Insert(0, new RadComboBoxItem(i.ToString(), i.ToString()));

            RadComboMonth.Items.Insert(0, new RadComboBoxItem("", ""));

            for (int iYear = DateTime.Now.Year + 12; iYear >= DateTime.Now.Year; iYear--)
                RadComboYear.Items.Insert(0, new RadComboBoxItem(iYear.ToString(), iYear.ToString()));

            RadComboYear.Items.Insert(0, new RadComboBoxItem("", ""));

            LoadToolTip();

            //KNT 
            int iCount = dbUtility.ExecIntScalar("select count(*) from tblTelawayCountriesRegions where site='" + country + "' and IsActive=1 ");
            int KNTCodeDefault = -1;
            if (iCount == 1)
                KNTCodeDefault = dbUtility.ExecIntScalar("select KNTCode from tblTelawayCountriesRegions where site='" + country + "' and IsActive=1");
            SessionUtility.AddValue("DefaultKNTCode", KNTCodeDefault);

        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("LoadAll " + e.Message);
        }
    }
    private void LoadStates(string sCountry, RadComboBox RCB)
    {
        try
        {
            string sTextField;

            if (SessionUtility.IsEngLang())
                sTextField = "StateName";
            else
                sTextField = "StateName" + SessionUtility.GetValue("UserCountry").ToUpper().Trim();

            DataTable dtState = null;
            if (sCountry == "USA")
                dtState = dbUtility.getDataTableBySQL(dbUtility.getUsaStateSql());
            else if (sCountry == "Canada")
                dtState = dbUtility.getDataTableBySQL(dbUtility.getCanadaStateSql());
            else if (sCountry == "Australia")
                dtState = dbUtility.getDataTableBySQL(dbUtility.getAusStateSql());

            if (dtState != null)
            {
                RCB.DataSource = dtState;
                RCB.DataTextField = sTextField;
                RCB.DataBind();
                RCB.Items.Insert(0, new RadComboBoxItem("", ""));
                RCB.SelectedIndex = -1;
            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("Load States:" + sCountry + " " + e.Message);
        }

    }
    private void LoadExistData()
    {
        try
        {
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            if (tmpOrderObj != null)
            {
                if (!string.IsNullOrEmpty(tmpOrderObj.ClientMobile))
                {
                    RadTxtCell.Text = tmpOrderObj.ClientMobile;
                }
                if (!string.IsNullOrEmpty(tmpOrderObj.UserName))
                {
                    int pos = tmpOrderObj.UserName.IndexOf(" ");
                    if (pos > 0)
                    {
                        RadTxtLName.Text = tmpOrderObj.UserName.Substring(0, pos);
                        RadTxtFName.Text = tmpOrderObj.UserName.Substring(pos + 1, tmpOrderObj.UserName.Length - pos - 1);
                    }
                }

                if (!string.IsNullOrEmpty(tmpOrderObj.ClientEmail))
                {
                    RadTxtEmail.Text = tmpOrderObj.ClientEmail.ToString();
                    RadTxtEmail2.Text = tmpOrderObj.ClientEmail.ToString();
                }

                if (!string.IsNullOrEmpty(tmpOrderObj.ClientHomePhone1))
                    RadTxtPhone.Text = tmpOrderObj.ClientHomePhone1.ToString();

                if (!string.IsNullOrEmpty(tmpOrderObj.ClientStreet))
                    RadTxtStreet.Text = tmpOrderObj.ClientStreet.ToString();

                if (!string.IsNullOrEmpty(tmpOrderObj.ClientCity))
                    RadTxtCity.Text = tmpOrderObj.ClientCity.ToString();

                //if (tmpOrderObj.ClientHomePhone2 != null)
                //{
                //    if (tmpOrderObj.ClientHomePhone2.ToString() != "")
                //        RadTxtIMEI.Text = tmpOrderObj.ClientHomePhone2.ToString();
                //}
                RadComboBoxItem li;
                RadComboBoxItem liUsa, liCanada, liAU;
                if (!string.IsNullOrEmpty(tmpOrderObj.ClientCountry))
                {
                    li = RadComboCountry.Items.FindItemByText(tmpOrderObj.ClientCountry.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                        RadComboCountry.Text = li.Text;
                    }

                    if (tmpOrderObj.ClientCountry.ToString().ToLower() == "usa")
                    {
                        StateDiv.Style.Add("display", "block");
                        ZipDiv.Style.Add("display", "block");
                        RadComboStateUSA.Style.Add("display", "block");
                        RadComboStateC.Style.Add("display", "none");
                        RadComboStateAU.Style.Add("display", "none");
                        if (!string.IsNullOrEmpty(tmpOrderObj.ClientState))
                        {
                            liUsa = RadComboStateUSA.Items.FindItemByValue(tmpOrderObj.ClientState.ToString());
                            if (liUsa != null)
                                liUsa.Selected = true;
                        }
                    }
                    else if (tmpOrderObj.ClientCountry.ToString().ToLower() == "australia")
                    {
                        StateDiv.Style.Add("display", "block");
                        ZipDiv.Style.Add("display", "block");
                        RadComboStateUSA.Style.Add("display", "none");
                        RadComboStateC.Style.Add("display", "none");
                        RadComboStateAU.Style.Add("display", "block");
                        if (!string.IsNullOrEmpty(tmpOrderObj.ClientState))
                        {
                            liAU = RadComboStateAU.Items.FindItemByValue(tmpOrderObj.ClientState.ToString());
                            if (liAU != null)
                                liAU.Selected = true;
                        }
                    }
                    else if (tmpOrderObj.ClientCountry.ToString().ToLower() == "canada")
                    {
                        StateDiv.Style.Add("display", "block");
                        ZipDiv.Style.Add("display", "block");
                        RadComboStateUSA.Style.Add("display", "none");
                        RadComboStateC.Style.Add("display", "block");

                        if (!string.IsNullOrEmpty(tmpOrderObj.ClientState))
                        {
                            liCanada = RadComboStateC.Items.FindItemByValue(tmpOrderObj.ClientState.ToString());
                            if (liCanada != null)
                                liCanada.Selected = true;
                        }
                    }

                    else if (tmpOrderObj.ClientCountry.ToString().ToLower() == "israel")
                    {
                        StateDiv.Style.Add("display", "none");
                        ZipDiv.Style.Add("display", "none");
                        RadComboStateUSA.Style.Add("display", "none");
                        RadComboStateAU.Style.Add("display", "none");
                        RadComboStateC.Style.Add("display", "none");
                    }
                    else
                        ZipDiv.Style.Add("display", "block");
                }

                if (!string.IsNullOrEmpty(tmpOrderObj.ClientZip))
                    RadTxtZip.Text = tmpOrderObj.ClientZip.ToString();


                if (tmpOrderObj.ShipCity != null)
                {
                    if (tmpOrderObj.ShipCity.ToString() != "[pickup]")
                        shipOpAddress.Style.Add("display", "block");

                    if (tmpOrderObj.ShipStreet.ToString() != tmpOrderObj.ClientStreet.ToString())
                        rbDiffAddress.Checked = true;
                }

                if (tmpOrderObj != null)
                {
                    if (!string.IsNullOrEmpty(tmpOrderObj.ShippingName))
                    {
                        RadComboBoxItem ri = RadComboShipping.Items.FindItemByText(tmpOrderObj.ShippingName);
                        if (ri != null)
                        {
                            ri.Selected = true;
                        }
                    }
                }//

                if (!string.IsNullOrEmpty(tmpOrderObj.ShipMethod))
                {
                    if (tmpOrderObj.ShipMethod != "OFFICE_PICKUP" && tmpOrderObj.ShipMethod != "GRP_DELIVERY")
                    {
                        if (!string.IsNullOrEmpty(tmpOrderObj.ShipName))
                        {
                            if (!(tmpOrderObj.ShipName.Contains("[pickup]")) && !(tmpOrderObj.ShipName.Contains("[group]")))
                            {

                                if (tmpOrderObj.ShipName != "")
                                {
                                    RadTxtDeliveryName.Text = tmpOrderObj.ShipName;

                                }

                                if (!string.IsNullOrEmpty(tmpOrderObj.ShipStreet))
                                    RadShipAddress.Text = tmpOrderObj.ShipStreet;

                                if (!string.IsNullOrEmpty(tmpOrderObj.ShipCity))
                                    RadTxtCity.Text = tmpOrderObj.ShipCity;

                                if (!string.IsNullOrEmpty(tmpOrderObj.ShipPhone))
                                    RadTxtDeliveryPhone.Text = tmpOrderObj.ShipPhone;
                            }

                        }// ship name
                    }//if ship method
                }
            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("LoadExistData " + e.Message);
        }
    }


    /// <summary>
    /// Load accessories acording to shipping
    /// </summary>
    private void LoadAccessories()
    {
    }

    #endregion

    private string BuildPlanRate(DataRow dr)
    {
        string sRet = "";
        Boolean IsRightCurrency = false;
        if (dr["IsRightCurrency"].ToString() != "")
            IsRightCurrency = Convert.ToBoolean(dr["IsRightCurrency"].ToString());

        //decimal dPlanRate = 0;
        if (Convert.ToBoolean(dr["ShowRate"].ToString()))
        {
            int dPlanDays = Convert.ToInt32(dr["PlanDays"].ToString());

            if (dPlanDays == 10)
                sRet = dbUtility.GetPerDayAmountByDay(10);
            if (dPlanDays == 30)
                sRet = dbUtility.GetPerDayAmountByDay(30);
            if (dPlanDays == -1 || dPlanDays == 60)
                sRet = dbUtility.GetPerDayAmountByDay(60);
        }
        string sPlanperDay = dr["PlanPerText"].ToString();
        string sPlanCurrencyLabel = dr["CurrencyLabel"].ToString();
        string sPlanCurrencySymbol = dr["CurrencySymbol"].ToString();


        // if (dPlanRate > 0)
        //   sRet = dPlanRate.ToString();

        if (IsRightCurrency)
            sRet = sRet + sPlanCurrencySymbol;
        else
            sRet = sPlanCurrencySymbol + sRet;

        sRet = sRet + sPlanperDay;
        return sRet;
    }






    protected void RadComboCountry_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Attributes["isEU"] = ((DataRowView)e.Item.DataItem)["isEU"].ToString();


    }
    protected void RadCombo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        bool displayUSDPrice = planDetails[0].displayUSD;
        string currencySymbol = planDetails[0].currencySymbol;
        string shipCostLocalPrice = ((DataRowView)e.Item.DataItem)["cost"].ToString();
        string shipCostUSDPrice = " - " + ((DataRowView)e.Item.DataItem)["ShipCostDisplay"].ToString(); ;
        string sText = ((DataRowView)e.Item.DataItem)["name"].ToString() + " " + shipCostLocalPrice + (displayUSDPrice ? shipCostUSDPrice : "");

        e.Item.Text = sText;
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Counter"].ToString();
        e.Item.Attributes["decimalCost"] = ((DataRowView)e.Item.DataItem)["ShipCost"].ToString();
        e.Item.Attributes["optRequireShipAddress"] = ((DataRowView)e.Item.DataItem)["optRequireShipAddress"].ToString();
        e.Item.Attributes["optLocalPickup"] = ((DataRowView)e.Item.DataItem)["optLocalPickup"].ToString();
        e.Item.Attributes["CountryName"] = ((DataRowView)e.Item.DataItem)["CountryName"].ToString();
        e.Item.Attributes["BaseCode"] = ((DataRowView)e.Item.DataItem)["BaseCode"].ToString();
        e.Item.Attributes["ShipCostGBP"] = ((DataRowView)e.Item.DataItem)["ShipCostGBP"].ToString();
        e.Item.Attributes["ShipMethod"] = ((DataRowView)e.Item.DataItem)["ShippingMethod"].ToString();
        e.Item.Attributes["ShipCostDisplay"] = ((DataRowView)e.Item.DataItem)["ShipCostDisplay"].ToString();
        e.Item.Attributes["DisplayUSDPrice"] = displayUSDPrice.ToString();
        e.Item.Attributes["SamePriceForMultipleSims"] = ((DataRowView)e.Item.DataItem)["SamePriceForMultipleSims"].ToString();


    }



    /// <summary>
    /// RequiredFieldValidator
    /// </summary>
    private void FillScreen()
    {
        try
        {
            TmpOrderObj tmpOrderObj = SessionUtility.getTmpOrder();
            if (tmpOrderObj != null)
            {

                tmpOrderObj.PayPalAmountCharge = 0;
                tmpOrderObj.PayPalTransactionId = "";
                //ShipDetails.Style.Add("display", "none");


                if (rbCC.Checked || (!rbCC.Checked && !rbPayPal.Checked))
                {
                    tmpOrderObj.ChargeWithPayPal = false;
                    PayPalDiv.Style.Add("display", "none");
                    CCDiv.Style.Add("display", "block");
                    RequiredFieldValidatorCC1.Enabled = true;
                    RequiredFieldValidatorCC2.Enabled = true;
                    RequiredFieldValidatorCC3.Enabled = true;
                    RequiredFieldValidatorCC4.Enabled = true;
                    RequiredFieldValidatorCC5.Enabled = true;
                    RequiredFieldValidatorCC6.Enabled = true;
                    RequiredFieldValidatorCC7.Enabled = true;
                    RequiredFieldValidatorCC8.Enabled = true;
                }
                else
                {
                    tmpOrderObj.ChargeWithPayPal = true;
                    PayPalDiv.Style.Add("display", "block");
                    CCDiv.Style.Add("display", "none");

                    RequiredFieldValidatorCC1.Enabled = false;
                    RequiredFieldValidatorCC2.Enabled = false;
                    RequiredFieldValidatorCC3.Enabled = false;
                    RequiredFieldValidatorCC4.Enabled = false;
                    RequiredFieldValidatorCC5.Enabled = false;
                    RequiredFieldValidatorCC6.Enabled = false;
                    RequiredFieldValidatorCC7.Enabled = false;
                    RequiredFieldValidatorCC8.Enabled = false;
                }

            }
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("FillScreen " + e.Message);
        }
    }

    public void RedirectToHomeSite()
    {
        var country = "";
        try
        {


            country = !string.IsNullOrEmpty(SessionUtility.GetValue("UserCountry"))
                            ? SessionUtility.GetValue("UserCountry")
                            : (planDetails != null && planDetails.Count > 0) ? planDetails[0].countryName
                            : "";



            if (string.IsNullOrEmpty(country) && SessionUtility.GetValue("HomeSiteUrl") == "")
            {
                Response.Redirect("FMPartial1.aspx?c=WW", false);
            }
            else if (string.IsNullOrEmpty(country))
            {
                Response.Redirect("http://" + SessionUtility.GetValue("HomeSiteUrl"), false);
            }

            else Response.Redirect("FMPartial1.aspx?c=" + country, false);
        }
        catch (Exception e)
        {
            emailUtility.SendMailErr("load page sent this exception\n: " + e.Message + "\n" + e.StackTrace);
        }
    }






    #region getPrice

    private DateTime getExpDate()
    {
        try
        {
            int iDays = 1;
            int iYear = Convert.ToInt32(RadComboYear.SelectedValue);
            int iMonth = Convert.ToInt32(RadComboMonth.SelectedValue);
            switch (iMonth)
            {
                case 4: iDays = 30; break;
                case 9: iDays = 30; break;
                case 6: iDays = 30; break;
                case 11: iDays = 30; break;
                case 2:
                    if ((iYear % 4 == 0) && (iYear % 100 != 0) || (iYear % 400 == 0))
                        iDays = 29;
                    else
                        iDays = 28;
                    break;

                default:
                    iDays = 31; break;
            }


            DateTime ExpDate = new DateTime(iYear, iMonth, iDays);

            return ExpDate;
        }
        catch (Exception ex)
        {
            SessionUtility.AddValue("errMsg", ex.Message);
            return new DateTime();
        }
    }

    public string FormatCurrencySymbol(int index)
    {
        if (planDetails != null)
            if (planDetails.Count > index)
                return otherUtility.FormatCurrencySymbol(planDetails[index].currencySymbol);
        return "";
    }



    public string getSimPrice(int index)
    {
        if (planDetails != null)
            if (planDetails.Count > 0)
                return planDetails[index].simPrice.ToString();

        return "";

    }

    public string getTotalAmount(int index)
    {
        if (planDetails != null)
            if (planDetails.Count > index)
                return SessionUtility.IsFrenchLang() ? planDetails[index].totalAmount.ToString() : planDetails[index].totalAmount.ToString();
        return "";
    }

    public string getPlanTitle(int index)
    {
        if (planDetails != null)
            if (planDetails.Count > index)
                return planDetails[index].title;
        return "";
    }

    #endregion


}

