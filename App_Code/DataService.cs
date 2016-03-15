using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Globalization;


/// <summary>
/// Summary description for DataService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class DataService : System.Web.Services.WebService
{
  
    [WebMethod]
    public string ValidCoupons(string CouponCode, string ItemTotal, string itemTotal1Order, string simQnt)
    {
        Promocode promoCode = new Promocode(CouponCode);
        return (promoCode.getDiscountCopunAndValid(promoCode,
            Convert.ToDouble(ItemTotal),
            Convert.ToDouble(itemTotal1Order),
            int.Parse(simQnt)));
    }

    [WebMethod]
    public string BuildPackageDiv(string UserCountry, string simQnt, string NanoSimTxt, string MicroSimTxt, string SimTxt, string sKntTitle, string CommentDiv, string WhatSimSize)
    {
        try
        {
            string newPackageDiv = "";
            SessionUtility.AddValue("SimQnt", simQnt);

            string sCountryCode = UserCountry;
            
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

            
            int iSimQnt;
            if (!Int32.TryParse(simQnt, out iSimQnt))
                return "";
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
                newPackageDiv += string.Format(@"<a href='javascript:linkSimInfo(linkSimInfo_{0});' id='linkSimInfo_{0}'>"
                            + "<img style='width: 30px; height: 30px;' src='img/Help.png' /><span>"+WhatSimSize+"</span></a>",i);
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
                    newPackageDiv = newPackageDiv + "<input type='hidden' id='DefaultKNTCod' value='" + KNTCodeDefault + "' /></div>";

                }

            }
            return newPackageDiv + CommentDiv + "~" + SessionUtility.GetIntValue("DefaultKNTCode");
        }
        catch (Exception ex)
        {
            var st = "StackTrace: " + ex.StackTrace + "\n TargetSite: " + ex.TargetSite;
            emailUtility.SendMailErr("BuildPackageDiv \n " + st + "\n" + ex.Message);
             
            return "";
        }
    }


    [WebMethod]
    public string BuildAccsessories(string ShippingId, string siteInfoName, string ConversionRate)
    {
        //   string sSql = "select a.id, a.chckbxID,a.Plan_code,a.Equipment_code,a.IsRequiredOperationSystem,0 as Insurance,a.SMSPackageCode ,a.CouponCode,a.name,a.name as 'chckBxName',a.Description,a.PriceText,a.Price,a.Img ,a.Img as 'image','' as Deposit,a.PriceText as 'rate','' as optionCode,a.CommentText,'' as 'LearnMore',0 as 'Quantity' "
        //   + " from dbor_online.dbo.tblAccessories a"
        //+ " inner join tblAccessories_Shipping ash on ash.AccessoriesID=a.id"
        //+ " where ash.shippingid =" + ShippingId;
        try
        {
        string sSql = "SELECT AccssesorID,Name,Description,PriceUSD,SiteName,PriceUSDText ,PriceText ,Img,CommentText ,Instock "
                     + " FROM tblAccessories a  inner join tblAccessories_Shipping ash on ash.AccessoriesID=a.Counter"
                     + " WHERE ash.shippingid =" + ShippingId + " AND Instock=1";
        
         DataTable dtAccessoriesByShip = dbUtility.getTableBySQLTAIL(sSql);
        string sb = "";
        if (dtAccessoriesByShip != null && dtAccessoriesByShip.Rows.Count > 0)
        for (int i = 0; i < dtAccessoriesByShip.Rows.Count; i++)
        {
            string name = dtAccessoriesByShip.Rows[i]["Name"].ToString();
            string Description = dtAccessoriesByShip.Rows[i]["Description"].ToString();
            string ID = dtAccessoriesByShip.Rows[i]["AccssesorID"].ToString();
            float rate = float.Parse(dtAccessoriesByShip.Rows[i]["PriceUSD"].ToString());
            string Rate = rate.ToString("F");// dtAccessoriesByShip.Rows[i]["rate"].ToString();
            string PriceText =siteInfoName=="WW"?dtAccessoriesByShip.Rows[i]["PriceUSDText"].ToString(): dtAccessoriesByShip.Rows[i]["PriceText"].ToString();
            string Status = "";
            //dtAccessoriesByShip.Rows[i]["Quantity"].ToString() == "0" ? "New " : "new";
            string Img = dtAccessoriesByShip.Rows[i]["Img"].ToString(); 
            sb += "<div class='col-lg-3 col-md-3 col-sm-6 col-xs-12 last'>" +
                                                                        "<div class='banner simpleCart_shelfItem'>" +
                                                                           "<div class='ImageWrapper text-center ContentWrapperHe chrome-fix effect-slide-bottom in' data-effect='slide-bottom' style='transition: all 0.7s ease-in-out;'>" +
                                                                           "<span class='onsale1'>" + Status + "</span>" +
                                                                           "<img class='img-responsive item_thumb'  src='" + Img + "' alt='' style='margin: auto;'>" +
                                                                                "<div class='ContentHe'>" +
                                                                                    "<div class='hoverimage Content'>" +
                                                                                        "<span class='accDescp'>" + Description + "</span>" +
                                                                                    "</div>" +
                                                                                "</div>" +
                                                                            "</div>" +
                                                                           " <div class='portfolio_desc'>" +
                                                                               " <div class='product_title title1'>" +
                                                                                  " <span style='display: none' class='item_itemType'>" + ID + "</span>" +
                                                                                    "<h3><span class='item_name'>" + name + "</span></h3>" +
                                                                                    "<span class='price-detail '>" + PriceText + "</span>" +
                                                                                    "<span class='price-detail item_price' style='display: none;'>" + Rate + "</span>" +
                                                                                    "<div><input type='number' step='1' value='1' min='1' class='item_quantity' />" +
                                                                                    "<span class='addtocart rocket btn_rocketPulse btn-default item_add addToCartSpan' title='Add to cart' style='cursor: pointer;'>" +
                                                                                        "<span >" +
                                                                                            "<i class='glyphicon glyphicon-shopping-cart' style='line-height: 3.5;'></i>" +
                                                                                            "<span class='cart-action-text'>ADD</span>" +
                                                                                        "</span>" +
                                                                                    "</span></div>" +
                                                                                "</div>" +
                                                                            "</div>" +
                                                                       "</div>" +
                                                                   " </div>";
        }
        return sb;
    }
        catch (Exception ex)
        {
            var st = "StackTrace: " + ex.StackTrace + "\n TargetSite: " + ex.TargetSite;
            emailUtility.SendMailErr("BuildAccsessories \n " + st + "\n" + ex.Message);

            return "";
        }

    }
}

