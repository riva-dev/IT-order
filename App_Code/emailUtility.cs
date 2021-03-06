

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using MailWS;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.IO;
using System.Net;
using System.Text;
/// <summary>
/// Summary description for emailUtility
/// </summary>
public class emailUtility
{
	public emailUtility()
	{
		
	}
    
    //public static bool SendEmailConfirmation(OnlineOrder order, bool isAgent)
    //{
    //       return SendEmailConfirmation2(order, isAgent);
    //}

    public static bool SendTailEmailConfirmation(OnlineOrderObj order, bool isAgent)
    {
       int iOnlineOrderCode = Convert.ToInt32(order.OnlineOrderCode);
       string sParentOnlineOrderCode = dbUtility.ExecScalarByStrParam("SELECT ParentOnlineOrderCode FROM VIEW_ONLINE_ORDERS where OnlineOrderCode=@OnlineOrderCode","OnlineOrderCode",iOnlineOrderCode.ToString());

       if (sParentOnlineOrderCode == "")
       {
           //int iOrderCode = 0;
           //string sOrderCode = dbUtility.ExecScalar_DBOR("SELECT OrderCode  FROM VIEW_ONLINE_ORDERS where OnlineOrderCode= " + iOnlineOrderCode.ToString());
           //if (sOrderCode != "")
           //    iOrderCode = Convert.ToInt32(sOrderCode);

           //if (iOrderCode>0)
            //    return SendNewEmail(iOrderCode, isAgent,order);    
            //else
            return SendEmailConfirmation2(order, isAgent);
       }

      else 
      {
          string sSql = "SELECT OrderCode  FROM VIEW_ONLINE_ORDERS where ParentOnlineOrderCode= " + sParentOnlineOrderCode;
          DataTable dt1 = dbUtility.getDataTableBySQL(sSql);
          if (dt1 != null)
          {
              foreach (DataRow dr in dt1.Rows)
              {
                  int iOrderCode2 =0;

                  if (dr["OrderCode"]!=null)
                      iOrderCode2 = Convert.ToInt32(dr["OrderCode"].ToString());

                  //if (iOrderCode2 > 0)
                  //    SendNewEmail(iOrderCode2, isAgent, order);
                  //else
                      SendEmailConfirmation2(order, isAgent);
              }
              return true;
          }
          else return false;
      }
    }


    
    
    public static string GetHtmlFromPage()
    {
        int iOrderCode = 153042;
        const string URL = "https://www.talknsave.net/MyTnsOrderInfo3.aspx";
        string strPost = "code=" + iOrderCode.ToString();   
        string strResult = "";
        StreamWriter myWriter = default(StreamWriter);
        //create http request object 
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(URL);
        objRequest.Method ="POST";
        objRequest.ContentLength = strPost.Length;
        objRequest.ContentType ="application/x-www-form-urlencoded";
        //post request 
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream(), System.Text.Encoding.ASCII);
            myWriter.Write(strPost);
        }
        catch (Exception)
        {
        }
        finally
        {
            myWriter.Close();
            myWriter.Dispose();
        }
        //read response 
        HttpWebResponse objResponse = default(HttpWebResponse);
        StreamReader stmRdr = default(StreamReader);
        try
        {
            objResponse = (HttpWebResponse)objRequest.GetResponse();
            stmRdr = new StreamReader(objResponse.GetResponseStream());
            strResult = stmRdr.ReadToEnd();
        }
        catch (Exception)
        {
        }
        finally
        {
            stmRdr.Close();
            objResponse.Close();
            objRequest.Abort();
        }
        return strResult;

    }

    //public static bool SendNewEmail(int iOrderCode,bool isAgent,OnlineOrderObj order)
    //{
    //    bool bRet = false;
    //    try
    //    {
    //        if (SessionUtility.GetValue("Language").Contains("fr"))
    //        {
    //            return SendEmailConfirmation2(order,isAgent);
                
    //        }
    //        else
    //        {
    //            int iBaseCode =Convert.ToInt32(order.BaseCode); 
    //            string strTo = "";
    //            string strBCC = "customerservice@telaway.co.uk";
    //            string agentBcc = "";// dbUtility.getAgentEmails(Convert.ToInt32(iBaseCode));
    //            string strCCEmail = SignupUtility.getStrParentLinkField("CCEmail");

    //            //int iPhonesRequired = Convert.ToInt32(order.PhonesRequired);
    //            SessionUtility.AddValue("Language", "en-US");
    //            string sCultureInfoName = SessionUtility.GetLanguageValue("Language");
    //            ResourceManager rm = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
    //            CultureInfo ci = new CultureInfo(sCultureInfoName);
    //            SessionUtility.AddValue("myResourceManager", rm);
    //            SessionUtility.AddValue("myCultureInfo", ci);
    //            CultureInfo USACulture = CultureInfo.CreateSpecificCulture("en-US");

    //            string sFile = HttpContext.Current.Server.MapPath("TemplatesHtml\\webmail_new.htm");
    //            string strHTML = System.IO.File.ReadAllText(sFile);                
    //            string strBody = strHTML;
    //            //DataRow drOrder = GetRowByOrder(iOrderCode);
    //            strBody = GetHtmlOrder(drOrder,order,isAgent );
    //            strTo = drOrder["ClientEmail"].ToString();

    //            if (strCCEmail != "")
    //                strTo = strTo + ";" + strCCEmail;

    //            if (isAgent)
    //            {
    //                strTo = strBCC;
    //                if (agentBcc != "")
    //                    strTo = strTo + ";" + agentBcc;
    //                strBCC = "";

    //            }
    //            string strSubject = "Your TelAway Order:" + drOrder["SignupSourceKey"].ToString();
    //            strBody = strBody.Replace("%start%", "");
    //            strBody = strBody.Replace("%end%", "");
    //            SendGeneralMail(strTo, strBody, strSubject, strBCC);

    //            //bool bISim = false;
    //            //if (order.EquipmentCode == 260 || order.EquipmentCode == 760 || order.EquipmentCode == 790 || order.EquipmentCode == 910 || order.EquipmentCode == 930)
    //            //    bISim = true;

    //            //if (isAgent && bISim && (!SessionUtility.IsFrenchLang()))
    //            //    SendEmailSimCompatibility(drOrder["ClientEmail"].ToString(), drOrder["SignupSourceKey"].ToString(), drOrder["ClientLastName"].ToString(), drOrder["ClientFirstName"].ToString());


    //            bRet=true;
                
    //        }//if
    //        return bRet;
    //    }//try

    //    catch
    //    {
    //        return false;
    //    }
    //}

    public static bool SendEmailConfirmation2(OnlineOrderObj order, bool isAgent)
    {
        try
        {
            string strBCC;
            string sFile;
            string strSubject;
            string sAddressTxt;
            string sBillingAddress;
            string sComments;
            string sTel = "";



            strBCC = "";// order.BccEmail;
            strSubject =otherUtility.getResourceString("YourTelawayOrder")+": " + Convert.ToString(order.OnlineOrderCode);

            int iParentOrderCode = SessionUtility.GetIntValue("iParentOrderCode");
            if (iParentOrderCode > -1)
                strSubject = otherUtility.getResourceString("YourTelawayOrder")+": " + Convert.ToString(iParentOrderCode) + "-" + Convert.ToString(order.OnlineOrderCode);
           
            string sTempFileName = "~\\TemplatesHtml\\webmail_"+ SessionUtility.GetValue("UserCountry").Trim().ToUpper()+".htm";

            sFile = HttpContext.Current.Server.MapPath(sTempFileName);
                sAddressTxt =otherUtility.getResourceString("Address");
                sBillingAddress =otherUtility.getResourceString("BillingAddress");             
                sComments =otherUtility.getResourceString("Notes");
                sTel = "Tel";
        
            string strHTML = System.IO.File.ReadAllText(sFile);
            string strTo = order.ClientEmail;
            int iBaseCode = Convert.ToInt32(order.BaseCode);
            string agentBcc = dbUtility.ExecScalar_DBOR("SELECT EmailBcc from tblBaseCodes where BaseCode = " + iBaseCode.ToString()); 

            string strCCEmail = SignupUtility.getStrParentLinkField("CCEmail");
            if (strCCEmail != "")
                strTo = strTo + ";" + strCCEmail;

            int iPhonesRequired = Convert.ToInt32(order.PhonesRequired);
            string sSubLinkCouponCode = "", x_BAddress = "", txtBAddress = "";

            if (isAgent)
            {
                strTo = order.BccEmail;
                if (agentBcc != "")
                    strTo = strTo + ";" + agentBcc;
                strBCC = "";

            }
            else
                strBCC = "weborders@talknsave.net; confirmations@talknsave.net;"; 

            sSubLinkCouponCode = "";// SignupUtility.getSubLinkField("CouponCode");
            if (iParentOrderCode>-1)
                strHTML = strHTML.Replace("@x_OrderCode",Convert.ToString(iParentOrderCode)+"-"+ Convert.ToString(order.OnlineOrderCode));
            else
                strHTML = strHTML.Replace("@x_OrderCode", Convert.ToString(order.OnlineOrderCode));

            strHTML = strHTML.Replace("@x_OrderName", order.ClientFirstName + " " + order.ClientLastName);
            strHTML = strHTML.Replace("@x_OrderDate", DateTime.Now.ToString());
            strHTML = strHTML.Replace("@x_BillingPhone", order.ClientHomePhone1);


            if (order.PayPalAmountCharge>0)
                strHTML = strHTML.Replace("@x_ImpTransaction", "");
            else
                strHTML = strHTML.Replace("@x_ImpTransaction", otherUtility.getResourceString("ImpTransaction") );

            string x_text1 = "";
            string x_SignupOrderTxt = "";
            string x_text = "<tr><td class='td' ><strong>@title: </strong></td><td class='td'>";
            string x_text2 = "</td></tr>";


            string x_Shiptext = "<tr><td valign=top style='color:#333333;font-family:tahoma;font-size:13px;font-weight:normal;margin:0 0 16px;padding:0;'><strong>" + sAddressTxt + ":</td>";
            x_Shiptext = x_Shiptext + "<td style='color:#333333;font-family:tahoma;font-size:13px;font-weight:normal;margin:0 0 16px;padding:0;'>@x_ShippingAddress</td></tr>";

            if (SessionUtility.GetValue("SignupSpecial") == "Order.aspx")
                x_SignupOrderTxt = " (signup from order.aspx) ";

            if (isAgent)
            {

                if (order.CustomerComment!=null && (order.CustomerComment.Trim() != "" && order.CustomerComment.Trim() != "-") || (x_SignupOrderTxt != ""))
                {
                    x_text1 = x_text.Replace("@title", sComments);
                    strHTML = strHTML.Replace("@x_OrderComment", x_text1 + order.CustomerComment.Trim() + x_SignupOrderTxt + x_text2);
                }

                else
                {
                    strHTML = strHTML.Replace("@x_OrderComment", "");
                }
            }
            else
            {
                strHTML = strHTML.Replace("@x_OrderComment", "");
            }

            StringBuilder sEquipmentName = new StringBuilder();
            //string sEquipmentName ="";
            foreach (SIMDetails sd in order.SimDetails)
            {
                sEquipmentName.AppendFormat(@"{0}<br/>",sd.EquipmentName);
            }

            strHTML = strHTML.Replace("@x_Equipment", sEquipmentName.ToString());

            if (!string.IsNullOrEmpty(order.DataPackageName))
            {
                string sDataName = order.DataPackageName.Trim();
                sDataName = sDataName.Replace("<br/>", "");
                sDataName = sDataName.Replace("<br/ >", "");
                sDataName = sDataName.Replace("<br />", "");
                sDataName = sDataName.Replace("<br>", "");

                x_text1 = x_text.Replace("@title", "Data Plan");
                strHTML = strHTML.Replace("@x_Data", x_text1 + sDataName + " " + order.DataPackgeSize + x_text2);
            }
            else
                strHTML = strHTML.Replace("@x_Data", "");

            

            string sPackName = "";
            if (order.PlanName != null)
                sPackName = order.PlanName;
            else
                sPackName = order.CallPackageName.ToString();

            strHTML = strHTML.Replace("@x_PlanName", sPackName);

            strHTML = strHTML.Replace("@x_SMSPackage", "");
            strHTML = strHTML.Replace("@x_UnitsRequired", Convert.ToString(iPhonesRequired));
            strHTML = strHTML.Replace("@x_KNT", "");
            strHTML = strHTML.Replace("@x_Optionals", "");

            
            strHTML = strHTML.Replace("@x_OrderStartDate", ((DateTime)order.StartDate).ToString("MM/dd/yyyy"));
            strHTML = strHTML.Replace("@x_OrderEndDate", ((DateTime)order.EndDate).ToString("MM/dd/yyyy"));

            if (order.AccessoryIdAndQuantity != "")
            {
                
                string Accessory = "<tr><td colspan=2><hr></td></tr><tr><td colspan=2 class='td' style='color:#003882;'>"
                 + "<strong>" + otherUtility.getResourceString("Accessories") + " </strong>"
                 + "</td></tr><tr><td class='td' style='height: 16px;'><strong>Code</strong></td>"
                 + "<td class='td' style='height: 16px;'><strong>Name</strong></td>"
                 + "<td class='td' style='height: 16px;'><strong>Qnt</strong></td></tr>";


                string[] acc = order.AccessoryIdAndQuantity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in acc)
                {
                    var Aitem=item;
                    var code = Aitem.Substring(0,Aitem.IndexOf('-'));
                    Aitem = Aitem.Substring(Aitem.IndexOf('-')+1);
                    var qnt = Aitem; 
                    var name = dbUtility.ExecScalar("SELECT  Name FROM tblAccessories WHERE AccssesorID = " + code + " AND SiteName = '"+order.ClientCountry+"'");
                    //Accessory += "<tr><td>" + item + "</td></tr>";
                    Accessory += "<tr><td class='td' style='height: 16px;'>" + code + "</td>"
                 + "<td class='td' style='height: 16px;'>" + name + "</td>"
                 + "<td class='td' style='height: 16px;'>" + qnt + "</td></tr>";
                }

                strHTML = strHTML.Replace("@x_Accssesories", Accessory);
            }
            else strHTML = strHTML.Replace("@x_Accssesories", "");
            if (order.ShipMethod.Contains("PICKUP") && !(SessionUtility.GetValue("Language").Contains("fr")))
                //strHTML = strHTML.Replace("@x_ShippingName", "<a href=\"" + "http://www.talknsave.net/locations.aspx" + "\"" + ">" + order.ShippingName + "</a> ");
                strHTML = strHTML.Replace("@x_ShippingName", order.ShippingName + "<br/><br/><span>Office Address</span><br/>" + GetBaseAddress(Convert.ToInt32(order.BaseCode)));
            else
                strHTML = strHTML.Replace("@x_ShippingName", order.ShippingName);


            strHTML = strHTML.Replace("@x_DateLeaving", ((DateTime)order.DepartureDate).ToString("MM/dd/yyyy"));
           

            //shipping Address
            string x_ShippingAddress = "" ;
            if (order.ShipStreet != "[pickup]" && order.ShipStreet != "[group]")
                x_ShippingAddress = x_ShippingAddress + order.ShipName+ "<br/> "+order.ShipStreet + ",<BR>" + order.ShipCity + ", ";
            if (order.ShipState != "NA")
                x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipState;
            if (order.ShipPostalCode != "")
                x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipPostalCode + ", ";

            x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipCountry;
            x_ShippingAddress = x_ShippingAddress + "<BR>" + sTel + ": " + order.ShipPhone + "";


            if (order.ShipStreet != "[pickup]" && order.ShipStreet != "[group]")
            {
                x_Shiptext = x_Shiptext.Replace("@x_ShippingAddress", x_ShippingAddress);
                strHTML = strHTML.Replace("@x_ShippingAddress", x_Shiptext);
            }
            else
                strHTML = strHTML.Replace("@x_ShippingAddress", "");



            if (order.CouponCode != "" && sSubLinkCouponCode == "")
            {
                x_text1 = x_text.Replace("@title", "Coupon Code");
                strHTML = strHTML.Replace("@x_CouponCode", x_text1 + order.CouponCode + x_text2);
            }
            else
                strHTML = strHTML.Replace("@x_CouponCode", "");


            //Billing Address
            x_BAddress = "";
            x_BAddress = order.ClientStreet + ",<BR>" + order.ClientCity + ", ";

            if (order.ClientState != "NA")
                x_BAddress = x_BAddress + "<BR>" + order.ClientState;

            if (order.ClientZip != "")
                x_BAddress = x_BAddress + "<BR>" + order.ClientZip;

            x_BAddress = x_BAddress + "<BR>" + order.ClientCountry;
            x_BAddress = x_BAddress + "<BR>" + sTel + ": " + order.ClientHomePhone1 + "";//Téléphone
            x_BAddress = x_BAddress + "<BR>Email: " + order.ClientEmail + "</P>";

            if (isAgent)
            {
                if (x_BAddress != "")
                {
                    txtBAddress = "<tr></tr><tr><td colspan=2 class='td' style='color:#003882;'><strong>" + sBillingAddress + "</strong> </td></tr><tr><td class='td'></td><td class='td' >@x_BillingAddress</td></tr>";
                    txtBAddress = txtBAddress.Replace("@x_BillingAddress", x_BAddress);
                    strHTML = strHTML.Replace("@x_BAddress", txtBAddress);
                }
                else
                    strHTML = strHTML.Replace("@x_BAddress", "");
            }
            else
                strHTML = strHTML.Replace("@x_BAddress", "");


            // set email addresses
            strHTML = strHTML.Replace("@x_customerServiceEmail", order.CustomerServicEmail);
            strHTML = strHTML.Replace("@x_salesEmail", order.SalesEmail);
            strHTML = strHTML.Replace("@x_Phone", order.TelawayPhone);

            strHTML = strHTML.Replace("@USA_Phone",dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='WW'"));
            strHTML = strHTML.Replace("@AU_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='AU'"));
            strHTML = strHTML.Replace("@UK_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='UK'"));
            strHTML = strHTML.Replace("@BR_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='BR'"));
            strHTML = strHTML.Replace("@FR_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='FR'"));
            strHTML = strHTML.Replace("@CA_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='CA'"));
            strHTML = strHTML.Replace("@ES_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='ES'"));
            strHTML = strHTML.Replace("@AR_Phone", dbUtility.ExecScalar("SELECT PhoneNumber  FROM tblTelawaySitesInfo where Name='AR'"));

            SessionUtility.AddValue("HtmlMail", strHTML);
            bool bRet = false;


            bRet = SendGeneralMail(order.MainEmail, strTo, strHTML, strSubject, strBCC, order.SiteSourceCountry);

            return bRet;

        }
        catch (Exception ex)
        {
            SessionUtility.AddValue("errMsg", ex.Message);
            return false;
        }
    }

    //public static bool SendEmailConfirmation2(OnlineOrderObj order, bool isAgent)
    //{
    //    try
    //    {
    //        string strBCC;
    //        string sFile;
    //        string strSubject;
    //        string sAddressTxt;
    //        string sKNTTxt;
    //        string sQuantityTxt;
    //        string sInsuranceTxt;
    //        string sOptionalTxt;
    //        string sBillingAddress;
    //        string sOverageProtectionPlan;
    //        string sComments;
    //        string sTel = "";



    
  
    //        sFile = HttpContext.Current.Server.MapPath("~\\TemplatesHtml\\webmail2.htm");
    //        sAddressTxt = "Address";
    //        //sPackageTxt = "Package";
    //        sKNTTxt = "Virtual International Number";
    //        sQuantityTxt = "Quantity";
    //        sInsuranceTxt = "Insurance";
    //        sOptionalTxt = "Optional Add on";
    //        sBillingAddress = "Billing Address";
    //        sOverageProtectionPlan = "Overage Protection Plan";
    //        sComments = "Comments";
    //        sTel = "Tel";

    //        string strHTML = System.IO.File.ReadAllText(sFile);
    //        string strTo = order.ClientEmail;
    //        string agentBcc = "";// dbUtility.getAgentEmails(Convert.ToInt32(order.BaseCode));

    //        string strCCEmail = SignupUtility.getStrParentLinkField("CCEmail");
    //        if (strCCEmail != "")
    //            strTo = strTo + ";" + strCCEmail;

    //        int iPhonesRequired = Convert.ToInt32(order.PhonesRequired);
    //        string TXToptionals = "", sSubLinkCouponCode = "", x_BAddress = "", txtBAddress = "";

    //        if (isAgent)
    //        {
    //            strTo = strBCC;
    //            if (agentBcc != "")
    //                strTo = strTo + ";" + agentBcc;
    //            strBCC = "";

    //        }

    //        sSubLinkCouponCode = "";// SignupUtility.getSubLinkField("CouponCode");
    //        strHTML = strHTML.Replace("@x_OrderCode", Convert.ToString(order.OnlineOrderCode));
    //        strHTML = strHTML.Replace("@x_OrderName", order.ClientFirstName + " " + order.ClientLastName);
    //        strHTML = strHTML.Replace("@x_OrderDate", DateTime.Now.ToString());
    //        strHTML = strHTML.Replace("@x_BillingPhone", order.ClientHomePhone1);

    //        string x_text1 = "";
    //        string x_SignupOrderTxt = "";
    //        string x_text = "<tr><td class='td' ><strong>@title: </strong></td><td class='td'>";
    //        string x_text2 = "</td></tr>";


    //        string x_Shiptext = "<tr><td valign=top style='color:#333333;font-family:tahoma;font-size:13px;font-weight:normal;margin:0 0 16px;padding:0;'><strong>" + sAddressTxt + ":</td>";
    //        x_Shiptext = x_Shiptext + "<td style='color:#333333;font-family:tahoma;font-size:13px;font-weight:normal;margin:0 0 16px;padding:0;'>@x_ShippingAddress</td></tr>";

    //        if (SessionUtility.GetValue("SignupSpecial") == "Order.aspx")
    //            x_SignupOrderTxt = " (signup from order.aspx) ";

    //        if (isAgent)
    //        {

    //            if ((order.CustomerComment.Trim() != "" && order.CustomerComment.Trim() != "-") || (x_SignupOrderTxt != ""))
    //            {
    //                x_text1 = x_text.Replace("@title", sComments);
    //                strHTML = strHTML.Replace("@x_OrderComment", x_text1 + order.CustomerComment.Trim() + x_SignupOrderTxt + x_text2);
    //            }

    //            else
    //            {
    //                strHTML = strHTML.Replace("@x_OrderComment", "");
    //            }
    //        }
    //        else
    //        {
    //            strHTML = strHTML.Replace("@x_OrderComment", "");
    //        }


    //        if (!string.IsNullOrEmpty(order.DataPackageName))
    //        {
    //            string sDataName = order.DataPackageName.Trim();
    //            sDataName = sDataName.Replace("<br/>", "");
    //            sDataName = sDataName.Replace("<br/ >", "");
    //            sDataName = sDataName.Replace("<br />", "");
    //            sDataName = sDataName.Replace("<br>", "");

    //            x_text1 = x_text.Replace("@title", "Data Plan");
    //            strHTML = strHTML.Replace("@x_Data", x_text1 + sDataName + " " + order.DataPackgeSize + x_text2);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_Data", "");

    //        strHTML = strHTML.Replace("@x_Equipment", order.EquipmentName);

    //        string sPackName = "";

    //        if (order.SignupSourceID == "WEB_B")
    //        {
    //            DataRow dr = SessionUtility.GetDataRowByName("drBundle");
    //            if (dr != null)
    //            {
    //                sPackName = dr["BundleText"].ToString();
    //                sPackName = sPackName.Replace("<br/>", " ");
    //                sPackName = sPackName.Replace("<br/ >", " ");
    //                sPackName = sPackName.Replace("<br />", " ");
    //                sPackName = sPackName.Replace("<br>", " ");

    //                if (dr["BundleRate"].ToString() != "")
    //                    sPackName = sPackName + " $" + dr["BundleRate"].ToString();

    //                if (dr["BundlePeriod"].ToString() != "")
    //                    sPackName = sPackName + " " + dr["BundlePeriod"].ToString();

    //                if (dr["BundleExtra"].ToString() != "")
    //                    sPackName = sPackName + dr["BundleExtra"].ToString();


    //            }

    //        }
    //        else
    //        {
    //            if (order.PlanName != null)
    //                sPackName = order.PlanName;
    //            else
    //                sPackName = order.CallPackageName.ToString();
    //        }

    //        strHTML = strHTML.Replace("@x_PlanName", sPackName);


    //        if (order.SMSPackageCode > -1)
    //        {
    //            x_text1 = x_text.Replace("@title", "Text Plan");
    //            strHTML = strHTML.Replace("@x_SMSPackage", x_text1 + order.SMSPackageName + x_text2);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_SMSPackage", "");

    //        strHTML = strHTML.Replace("@x_UnitsRequired", Convert.ToString(iPhonesRequired));
   
    //        if ((!string.IsNullOrEmpty(order.KNTName)) && order.KNTName != "No" && order.KNTRequired > 0)
    //        {
    //            x_text1 = x_text.Replace("@title", sKNTTxt);
    //            strHTML = strHTML.Replace("@x_KNT", x_text1 + order.KNTName + " (" + order.KNTRequired + " units) " + x_text2);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_KNT", "");

    //        TXToptionals = "";
    //        OptionalObj Option;

    //        IList<OptionalObj> OptionalsList = SessionUtility.GetOnlineOptionals();
    //        if (order.bitCallPackageOverageProtection != null)
    //        {
    //            if (Convert.ToBoolean(order.bitCallPackageOverageProtection))
    //                TXToptionals = TXToptionals + " " + sOverageProtectionPlan + "<br> ";
    //        }
    //        for (int i = 0; i < OptionalsList.Count; i++)
    //        {

    //            Option = OptionalsList[i];
    //            TXToptionals = TXToptionals + Option.OptionalName;
    //            if (Option.Quantity > 1)
    //                TXToptionals = TXToptionals + " (" + sQuantityTxt + ": " + Convert.ToString(Option.Quantity) + ") ";

    //            //if (Option.OptionText != "")
    //            //  TXToptionals = TXToptionals + " - " + Option.OptionText + ".";

    //            if (Option.RequiredInsurance)
    //                TXToptionals = TXToptionals + " " + sInsuranceTxt + ": " + dbUtility.Capitalize(dbUtility.GetYesNo(Option.Insurance));

    //            TXToptionals = TXToptionals + "<br> ";
    //        }


    //        if (TXToptionals != "")
    //        {

    //            x_text1 = x_text.Replace("@title", sOptionalTxt);
    //            strHTML = strHTML.Replace("@x_Optionals", x_text1 + TXToptionals + x_text2);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_Optionals", "");


    //        strHTML = strHTML.Replace("@x_OrderStartDate", ((DateTime)order.StartDate).ToString("MM/dd/yyyy"));
    //        strHTML = strHTML.Replace("@x_OrderEndDate", ((DateTime)order.EndDate).ToString("MM/dd/yyyy"));


    //        if (order.ShipMethod.Contains("PICKUP") && !(SessionUtility.GetValue("Language").Contains("fr")))
    //            //strHTML = strHTML.Replace("@x_ShippingName", "<a href=\"" + "http://www.talknsave.net/locations.aspx" + "\"" + ">" + order.ShippingName + "</a> ");
    //            strHTML = strHTML.Replace("@x_ShippingName", order.ShippingName + "<br/><br/><span>Office Address</span><br/>" + GetBaseAddress(Convert.ToInt32(order.BaseCode)));
    //        else
    //            strHTML = strHTML.Replace("@x_ShippingName", order.ShippingName);



    //        //shipping Address
    //        string x_ShippingAddress = "";
    //        if (order.ShipStreet != "[pickup]" && order.ShipStreet != "[group]")
    //            x_ShippingAddress = x_ShippingAddress + order.ShipStreet + ",<BR>" + order.ShipCity + ", ";
    //        if (order.ShipState != "NA")
    //            x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipState;
    //        if (order.ShipPostalCode != "")
    //            x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipPostalCode + ", ";

    //        x_ShippingAddress = x_ShippingAddress + "<BR>" + order.ShipCountry;
    //        x_ShippingAddress = x_ShippingAddress + "<BR>" + sTel + ": " + order.ClientHomePhone1 + "";


    //        if (order.ShipStreet != "[pickup]" && order.ShipStreet != "[group]")
    //        {
    //            x_Shiptext = x_Shiptext.Replace("@x_ShippingAddress", x_ShippingAddress);
    //            strHTML = strHTML.Replace("@x_ShippingAddress", x_Shiptext);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_ShippingAddress", "");



    //        if (order.CouponCode != "" && sSubLinkCouponCode == "")
    //        {
    //            x_text1 = x_text.Replace("@title", "Coupon Code");
    //            strHTML = strHTML.Replace("@x_CouponCode", x_text1 + order.CouponCode + x_text2);
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_CouponCode", "");


    //        //Billing Address
    //        x_BAddress = "";
    //        x_BAddress = order.ClientStreet + ",<BR>" + order.ClientCity + ", ";

    //        if (order.ClientState != "NA")
    //            x_BAddress = x_BAddress + "<BR>" + order.ClientState;

    //        if (order.ClientZip != "")
    //            x_BAddress = x_BAddress + "<BR>" + order.ClientZip;

    //        x_BAddress = x_BAddress + "<BR>" + order.ClientCountry;
    //        x_BAddress = x_BAddress + "<BR>" + sTel + ": " + order.ClientHomePhone1 + "";//Téléphone
    //        x_BAddress = x_BAddress + "<BR>Email: " + order.ClientEmail + "</P>";

    //        if (isAgent)
    //        {
    //            if (x_BAddress != "")
    //            {
    //                txtBAddress = "<tr></tr><tr><td colspan=2 class='td' style='color:#003882;'><strong>" + sBillingAddress + "</strong> </td></tr><tr><td class='td'></td><td class='td' >@x_BillingAddress</td></tr>";
    //                txtBAddress = txtBAddress.Replace("@x_BillingAddress", x_BAddress);
    //                strHTML = strHTML.Replace("@x_BAddress", txtBAddress);
    //            }
    //            else
    //                strHTML = strHTML.Replace("@x_BAddress", "");
    //        }
    //        else
    //            strHTML = strHTML.Replace("@x_BAddress", "");


    //        // set email addresses
    //        strHTML = strHTML.Replace("@x_customerServiceEmail", order.CustomerServicEmail);
    //        strHTML = strHTML.Replace("@x_salesEmail", order.SalesEmail);


    //        SessionUtility.AddValue("HtmlMail", strHTML);
    //        bool bRet = false;

    //        //if (SessionUtility.GetValue("Language").Contains("fr"))
    //        //    bRet = SendMailFR(strTo, strHTML, strSubject, strBCC);

    //        //else
    //        //    
    //        bRet = SendGeneralMail(order.ConfirmationEmail, strTo, strHTML, strSubject, strBCC, order.SiteSourceCountry);



    //        //bool bISim = false;
    //        //if (order.EquipmentCode == 260 || order.EquipmentCode == 760 || order.EquipmentCode == 790 || order.EquipmentCode == 910 || order.EquipmentCode == 930)
    //        //    bISim = true;

    //        //if (isAgent && bISim && (!SessionUtility.IsFrenchLang()))
    //        //    SendEmailSimCompatibility( order.ClientEmail , order.OnlineOrderCode.ToString(),order.ClientLastName,order.ClientFirstName  );

    //        return bRet;



    //    }
    //    catch (Exception ex)
    //    {
    //        SessionUtility.AddValue("errMsg", ex.Message);
    //        return false;
    //    }
    //}


    public static bool SendEmailSimCompatibility(string sClientEmail, string sOnlineOrderCode, string sLastName, string sFirstName)
    {
        try
        {
            string strSubject = "Your TelAway Sim Compatibility";
            string sFile = HttpContext.Current.Server.MapPath("TemplatesHtml\\SIMcompatibilityemail.html");

            string strHTML = System.IO.File.ReadAllText(sFile);
            string strTo = sClientEmail;

            strHTML = strHTML.Replace("%signupsourcekey%", sOnlineOrderCode);
            strHTML = strHTML.Replace("%clientlastname%", sLastName);
            strHTML = strHTML.Replace("%clientfirstname%", sFirstName);


            SessionUtility.AddValue("HtmlMail", strHTML);
            return SendMail(strTo, strHTML, strSubject, "");
        }

        catch (Exception ex)
        {
            SessionUtility.AddValue("errMsg", ex.Message);
            return false;
        }
  
     }
       
    //public static bool SendEmailForChanges(Request r)
    //{
    //    try
    //    {
    //        string strBCC = "weborders@talknsave.net; confirmations@talknsave.net";
    //        string sFile = HttpContext.Current.Server.MapPath("TemplatesHtml\\changemail.html");
    //        string strHTML = System.IO.File.ReadAllText(sFile);
    //        string strSubject = "MyTalknSave Update";
    //        string strTo = r.ExistClientEmail;// r.EnteredByEmail;


    //        strHTML = strHTML.Replace("@x_RentalCode", Convert.ToString(r.RentalCode));
    //        strHTML = strHTML.Replace("@x_PhoneNumber", r.PhoneNumber );
    //        strHTML = strHTML.Replace("@x_Email", r.EnteredByEmail );


    //        string sNewChanges="";
            
    //        string sNewEndDate="<tr><td class='td'><strong>New End Date:</strong></td> <td class='td'>@x_EndDate</td></tr>";
            
    //        string sNewPlan="<tr><td class='td'><strong>New Voice Plan:</strong></td><td class='td'>@x_PlanName</td></tr>";

    //        string sNewSMS = "<tr><td class='td'><strong>New SMS Plan:</strong></td><td class='td'>@x_SMSName</td></tr>";
            
    //        //string sNewPackage="<tr><td class='td'><strong>New Call Package:</strong></td><td class='td'>@x_Callpackage</td></tr>";

    //        string sNewDataPackage = "<tr><td class='td'><strong>New Data Package:</strong></td><td class='td'>@x_DataPackage</td></tr>";

    //        string sNewCCNum = "<tr><td class='td'><strong>New Credit Card :</strong></td><td class='td'>@x_CCNum</td></tr>";

    //        string sNewEmail = "<tr><td class='td'><strong>New Email :</strong></td><td class='td'>@x_Email</td></tr>";

    //        if (r.NewEndDate.HasValue)
    //        {
    //            sNewChanges = sNewChanges + sNewEndDate;
    //            sNewChanges = sNewChanges.Replace("@x_EndDate", Convert.ToDateTime(r.NewEndDate).ToString("MM/dd/yyyy"));
    //        }
    //        if (r.NewPlanCode.HasValue || r.NewCallPackageCode.HasValue)
    //        {
    //            sNewChanges = sNewChanges + sNewPlan;
    //            sNewChanges = sNewChanges.Replace("@x_PlanName", r.NewCallPackageName);
    //        }

    //        if (r.NewSMSPackageCode.HasValue)
    //        {
    //            sNewChanges = sNewChanges + sNewSMS;
    //            sNewChanges = sNewChanges.Replace("@x_SMSName", r.NewSMSPackageName);
    //        }
    //        //if (r.NewCallPackageCode.HasValue)
    //        //{
    //        //    sNewChanges = sNewChanges + sNewPackage;
    //        //    sNewChanges = sNewChanges.Replace("@x_Callpackage", r.NewCallPackageName);//r.NewPlanCodeName
    //        //}

    //        if (r.NewExtendedDataPackageCode.HasValue)
    //        {
    //            sNewChanges = sNewChanges + sNewDataPackage;
    //            sNewChanges = sNewChanges.Replace("@x_DataPackage", r.NewExtendedDataPackageName);
    //        }

    //        if (!string.IsNullOrEmpty(r.NewCCNum))
    //        {
    //            sNewChanges = sNewChanges + sNewCCNum;
    //            sNewChanges = sNewChanges.Replace("@x_CCNum", r.NewCCTitle + " xxxx-xxxx-xxxx-" + r.NewCCNum.Substring(r.NewCCNum.Length - 4, 4) + " " + Convert.ToDateTime(r.NewCCExpDate).Month.ToString() + "/" + Convert.ToDateTime(r.NewCCExpDate).Year.ToString());
    //        }

    //        if (!string.IsNullOrEmpty(r.NewEmail))
    //        {
    //            sNewChanges = sNewChanges + sNewEmail;
    //            sNewChanges = sNewChanges.Replace("@x_Email", r.NewEmail);
    //        }
    //        strHTML = strHTML.Replace("@x_NewChanges", sNewChanges);          


    //        SessionUtils.AddValue("HtmlMail", strHTML);
    //        return SendMail(strTo, strHTML, strSubject, strBCC);

    //    }
    //    catch (Exception ex)
    //    {
    //        SessionUtils.AddValue("errMsg", ex.Message);
    //        return false;
    //    }
    //}
    
    public static void SendMailErr(string s)
    {
        string sPage="";
        string sMsg="";
        string sUrlPath = HttpContext.Current.Request.Url.ToString();
       
        string sEmailMsg =""; 
        
        if (sEmailMsg != "")
            s = sEmailMsg + s;


        if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            sPage = sPage + "<br/> IP:" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

        string sURL = HttpContext.Current.Request.Url.AbsoluteUri;
        sPage = sPage + "<br/> URL: " + sURL;

        if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]!= null)
            sPage = sPage + "<br/> REFERER: " + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString() ;

       if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
        {
            sPage = sPage + "<br/> BROWSER:";

            string sBrowser = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
            if (sBrowser.Contains("Firefox"))
                sPage = sPage + "<b><u> Firefox </u></b> ";

            if (sBrowser.Contains("Chrome"))
                sPage = sPage + "<b><u> Chrome </u></b> ";

            else if (sBrowser.Contains("Safari"))
                sPage = sPage + "<b><u> Safari </u></b> ";

            if (sBrowser.Contains("MSIE"))
                sPage = sPage + "<b><u> Internet Explorer </u></b> ";

            
            sPage=sPage+ HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();  

        }
        try
        {
            sMsg += " <br/><br/>" + SignupUtility.GetOnlineOrderSession();
        }
        catch { }
        if (s != "")
            sMsg = sMsg + " <br/>  " + s + sPage;

        string strBody = "<u>TelAway Order Site - Err </u><br/> " + sMsg;
        string strSubject;
        if (SessionUtility.GetBoolValue("newOrderSite") == true)
            strSubject = "TELAWAY NET ERROR - order site 2015";
        else
            strSubject = "TELAWAY NET ERROR - order site 2015";
        string strTo = "Web-Master@talknsave.net";

        if (!sPage.Contains("Googlebot"))
            SendMail(strTo, strBody, strSubject);
    }

    public static bool SendMailWithFile(string strTo, string strBody, string strSubject,string sAttachFile)
    {
        string strBcc = "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendFullDetailsMail("sales@talknsave.net", "website-confirmations@talknsave.net", strTo, strSubject, strBody, strBcc, "sales@talknsave.net", sAttachFile))
                    return true;
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            return false;
        }
        return false;
    }

    public static bool SendShawaraMailWithFile(string strTo, string strBody, string strSubject, string sAttachFile)
    {
        string strBcc = "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendShawarma("help@shawarmaphone.com", "help@shawarmaphone.com", strTo, strSubject, strBody, strBcc, "help@shawarmaphone.com", sAttachFile))
                    return true;
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            //throw ex; 
            return false;
        }
        return false;
    }
    public static bool SendMail(string strTo, string strBody, string strSubject,string strBcc)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";
        
        strBcc = strBcc+"Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendFullDetailsMail("sales@talknsave.net", "website-confirmations@talknsave.net", strTo, strSubject, strBody, strBcc, "sales@talknsave.net", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }


    public static bool SendGeneralMail(string strFrom,string strTo, string strBody, string strSubject, string strBcc, string countryName)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";

        strBcc = strBcc + "Web-Master@talknsave.net;";
        try
        {
            string sGmail = countryName.ToLower() + "PSW";  
            string strPsw ="";
            if (ConfigurationManager.AppSettings[sGmail]!= null)
                strPsw = ConfigurationManager.AppSettings[sGmail].ToString(); 
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                switch (countryName)
                {
                    case "UK":
                        if (mailWS1.SendUKTelAwayMail(strFrom, strFrom, strTo, strSubject, strBody, strBcc, strFrom, ""))
                            return true;
                        break;
                    case "AU":
                        if (mailWS1.SendAUTelAwayMail(strFrom, strFrom, strTo, strSubject, strBody, strBcc, strFrom, ""))
                            return true;
                        break;
                   
                    default://ca//fr //br

                        if (strPsw == "")
                            return false;
                        else if (mailWS1.SendGlobalTelAwayMail(strFrom, strFrom,strPsw, strTo, strSubject, strBody, strBcc, strFrom, ""))
                            return true;
                        break;
                }
                
            }
        }
        catch (Exception ee)
        {
            SendMailErr(ee.Message);
            return false;
        }
        return false;
    }
    public static bool SendMail(string strTo, string strBody, string strSubject)
    {
        

        string strBcc="Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendFullDetailsMail("sales@talknsave.net", "website-confirmations@talknsave.net", strTo, strSubject, strBody, strBcc, "sales@talknsave.net", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public static bool SendUnicomMail(string strTo, string strBody, string strSubject, string strBcc)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";

        strBcc = strBcc + "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendUnicomMail("support@unicom.co.il", "info@unicom.co.il", strTo, strSubject, strBody, strBcc, "info@unicom.co.il", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public static bool SendTelawayMail(string strTo, string strBody, string strSubject, string strBcc)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";

        strBcc = strBcc + "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendUnicomMail("telaway@telaway.com", "telaway@telaway.com", strTo, strSubject, strBody, strBcc, "info@telaway.co.il", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
  public static bool SendTAILMail(string strTo, string strBody, string strSubject, string strBcc)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";

        strBcc = strBcc + "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendTelAwayMail("telaway@telaway.com", "telaway@telaway.com", strTo, strSubject, strBody, strBcc, "telaway@telaway.com", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
    public static bool SendMailFR(string strTo, string strBody, string strSubject, string strBcc)
    {
        if (strBcc != "")
            strBcc = strBcc + ";";

        strBcc = strBcc + "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendFullDetailsMailFR("sales@talknsave.net", "website-confirmations@talknsave.net", strTo, strSubject, strBody, strBcc, "sales@talknsave.net", ""))
                    return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }

    public static bool SendRemindPswMail(Page thisPage, string strTo, string sPsw)
    {
        try
        {
            string sFile=HttpContext.Current.Server.MapPath("TemplatesHtml\\passwordreminder.html");
            string strHTML = System.IO.File.ReadAllText(sFile);
            string strSubject = "TalknSave Password Reminder ";
            string strBody = strHTML.Replace("%PASSWORD%", sPsw);

            //SessionUtility.AddValue("SendMail", true);
            return SendMail( strTo, strBody, strSubject);

        }
        catch
        {
            return false;
        }

    }

    public static bool SendFullDetailsMail(string strTo, string strBody, string strSubject,string strReplyTo)
    {
        string strBcc = "Web-Master@talknsave.net";
        try
        {
            using (MailWS.TNSMailService mailWS1 = new MailWS.TNSMailService())
            {
                if (mailWS1.SendFullDetailsMail("sales@talknsave.net", "Website-Confirmations@talknsave.net", strTo, strSubject, strBody, strBcc, strReplyTo, ""))
                    return true;
            }
        }
        catch (Exception e)
        {
            string s = e.Message;
            return false;
        }
        return false;
    }

    //public static bool SendMailPdf(string sEmail, int iBillId, string sPhone)
    //{
    //    using (ws_SendPDF.Service ws = new ws_SendPDF.Service())
    //    {
    //        ws.Timeout = 120000;
    //        try
    //        {
    //            if (ws.SendEmail(sEmail, iBillId, sPhone))
    //                return true;
    //        }

    //        catch
    //        {
    //            return false;
    //        }
    //    }

    //    return false;

    //}
    private static string GetAddress(DataRow dr)
    {
        string sRet = "";


        if (!string.IsNullOrEmpty(dr["ClientStreet"].ToString()))
            sRet = sRet + dr["ClientStreet"].ToString() + ", ";

        if (!string.IsNullOrEmpty(dr["ClientCity"].ToString()))
            sRet = sRet + dr["ClientCity"].ToString() + ", ";

        if (!string.IsNullOrEmpty(dr["ClientState"].ToString()))
            sRet = sRet + dr["ClientState"].ToString() + ", ";

        if (!string.IsNullOrEmpty(dr["ClientZip"].ToString()))
            sRet = sRet + dr["ClientZip"].ToString() + ", ";

        if (sRet.Contains(","))
            sRet = sRet.Substring(0, sRet.Length - 2);

        return sRet;
    }

    //private static  string GetBaseName(int iBaseCode)
    //{
    //    return dbUtility.ExecScalar("SELECT case when (BaseDisplayName is not null) then BaseDisplayName else BaseName end as 'BaseName' FROM tblBaseCodes where BaseCode=" + iBaseCode.ToString());
    //}
    private static string GetBaseAddress(int iBaseCode)
    {
        return dbUtility.ExecScalarByStrParam("SELECT case when address is not null then isnull(Address,'')+'<br/>'+isnull(phone,'')+'<br/>'+isnull(hours,'') else '' end as 'b' FROM tblBaseCodes where BaseCode=@BaseCode" ,"BaseCode",iBaseCode.ToString());
    }
    //private static string GetBaseLink(int iBaseCode)
    //{
    //    return dbUtility.ExecScalar("SELECT case countrycode when 'USA' then 'st='+isnull(USAState,'')+'&c=USA' else 'c='+countrycode end as 'locationlink' FROM tblBaseCodes where BaseCode=" + iBaseCode.ToString());
    //}
    //private static   string GetExtendedDataName(int iExtendedDataCode)
    //{
    //    return dbUtility.ExecScalar("SELECT ExtendedDataPackageName FROM tblExtendedDataPackageCodes where ExtendedDataPackageCode=" + iExtendedDataCode.ToString());
    //}

    //private static string GetSMSPackageName(int iSMSPackageCode)
    //{
    //    return dbUtility.ExecScalar("SELECT SMSPackageName FROM tblSMSPackages where SMSPackageCode=" + iSMSPackageCode.ToString());
    //}

    //private static string GetCallPackageName(int iCallPackageCode)
    //{
    //    return dbUtility.ExecScalar("SELECT CallPackageName FROM tblCallPackages where CallPackageCode=" + iCallPackageCode.ToString());
    //}

   
    //private static string GetPlanName(int iPlanCode)
    //{
    //    return dbUtility.ExecScalar("SELECT PlanName FROM tblRentalPlans where PlanCode=" + iPlanCode.ToString());
    //}

    //private static DataRow GetEquipmentRow(int iEquipmentCode)
    //{
    //    DataRow dr = null;
    //    DataTable dt = dbUtility.getDataTableBySQL("SELECT * FROM tblEquipmentModels where EquipmentCode=" + iEquipmentCode.ToString());
    //    if (dt != null)
    //        if (dt.Rows.Count > 0)
    //            dr = dt.Rows[0];

    //    return dr;
    //}
    private static DataRow GetKNTRow(int iKntCode)
    {
        DataRow dr = null;
        DataTable dt = dbUtility.getDataTableBySQL("SELECT *,case when (Price > 0) then  '$'+ CONVERT(varchar(20),Price, 1) else 'Free' end as 'KntFee' FROM tblKNTDirectPlanCodes where DirectPlanCode=" + iKntCode.ToString());
        if (dt != null)
            if (dt.Rows.Count > 0)
                dr = dt.Rows[0];

        return dr;
    }

   
    //private static string GetEquipment(DataRow dr)
    //{
    //    string sRet = "";
    //    string sFeeText = "";
    //    sRet = dr["EquipmentModel"].ToString();

    //    double dFee = 0;
    //    string sEquipmentText = "";

    //    if (dr["EquipmentCost"].ToString() != "")
    //        dFee = Convert.ToDouble(dr["EquipmentCost"].ToString());

    //    sEquipmentText = dr["EquipmentText"].ToString();

    //    if (dFee > 0 && !sEquipmentText.Contains("Euro"))
    //    {
    //        sFeeText = "-";
    //        if (dFee < 1)
    //            sFeeText = sFeeText + " " + otherUtility.getCentsByLang(dFee, true) + " ";
    //        else
    //            sFeeText = sFeeText + " " + otherUtility.getDollarByLang(dFee) + " ";
    //    }
    //    else if (dFee == 0 && !sEquipmentText.Contains("Euro"))
    //        sFeeText = " - Free";
    //    else if (dFee > -1)
    //        sFeeText = " - " + dFee;

    //    sRet = sRet + sFeeText + " " + sEquipmentText;
    //    return (sRet);
    //}
}
