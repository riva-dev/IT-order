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
/// Summary description for SessionUtility
/// </summary>
public class SessionUtility
{
    public SessionUtility()
    {

    }

    public static void ClearAll()
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        if (thisSession != null)
        {
            thisSession.Clear();
           
        }

        if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
        {
            HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
            myCookie.Expires = DateTime.Now.AddYears(-30);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        // Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-30);
        HttpRuntime.Cache.Remove("TmpOrdersList");

    }
    public static void ClearValue(string sName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        if (thisSession != null)
        {
            sName = getName(sName); 

            if (thisSession[sName] != null) 
                thisSession[sName] = "";
        }
    }

    public static string GetLinkNewSite()
    {
        if (GetBoolValue("newSite") == true)
            return "n=1";
        else
            return "n=0";
    }

    public static void AddValue(string sName, object value)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;


        if (thisSession != null)
        {
            sName = getName(sName); 

            thisSession[sName] = value;

        }
    }

    public static void AddValuePlus(string sName, object value)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        string sOldName = "";

        if (thisSession != null)
        {
            if (thisSession[sName] != null)
                sOldName = thisSession[sName].ToString();

            thisSession[sName] = sOldName + "<br/><span style='top:-5px;position:relative;font-size:18px;'>.</span>" + value.ToString();

        }
    }


    public static bool GetBoolValue(string sName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        try
        {
            if (thisSession != null)
            {
                sName = getName(sName); 


                if (thisSession[sName] != null)
                    return Convert.ToBoolean(thisSession[sName].ToString());
                else
                    return false;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    public static bool? GetInsBoolValue(string sName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        try
        {
            if (thisSession != null)
            {
                sName = getName(sName); 


                if (thisSession[sName] != null)
                    return Convert.ToBoolean(thisSession[sName].ToString());
                else

                    return null;
            }
            else
                return null;
        }
        catch
        {
            return null;
        }
    }


    public static void AddOnlineOptionals(IList<OptionalObj> objOptionals)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
           thisSession["OnlineOptionals"] = objOptionals;  
        }
    }


    public static void ClearListTmpOrder()
    {
        HttpRuntime.Cache.Remove("TmpOrdersList");
    }

    public static void  AddListTmpOrder(TmpOrderObj tmpOrderObj)
    {        
        IList<TmpOrderObj> TmpOrderList = SessionUtility.GetListTmpOrders();
        if (TmpOrderList == null)
        {
            TmpOrderList = new System.Collections.Generic.List<TmpOrderObj>();
        }
        TmpOrderList.Add(tmpOrderObj);
        HttpRuntime.Cache.Insert("TmpOrdersList", TmpOrderList, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);

    }

    public static void UpdateListTmpOrders(TmpOrderObj tmp, string sSessionId)
    {
        IList<TmpOrderObj> listTmpOrders=GetListTmpOrders();
        if (listTmpOrders != null)
        {
            for (int i = 0; i < listTmpOrders.Count; i++)
            {
                TmpOrderObj t = listTmpOrders[i];
                if (t.SessionID.ToString() == sSessionId)
                {
                    listTmpOrders.Remove(t);
                }
            }
            listTmpOrders.Add(tmp);
        }
        HttpRuntime.Cache.Remove("TmpOrdersList");
        HttpRuntime.Cache.Insert("TmpOrdersList", listTmpOrders, null, DateTime.Now.AddMinutes(30), TimeSpan.Zero);

        //return listTmpOrders;
    }
    
    public static IList<TmpOrderObj> GetListTmpOrders()
    {
        if (HttpRuntime.Cache["TmpOrdersList"] != null)
            return (IList<TmpOrderObj>)HttpRuntime.Cache["TmpOrdersList"];
        else
            return null;
        
    }
    public static IList<OptionalObj> GetOnlineOptionals()
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            if (thisSession["OnlineOptionals"] != null)
                return (IList<OptionalObj>)thisSession["OnlineOptionals"];
            else
                return null;
        }
        else
            return null;
    }

    public static OptionalObj AddOptional(string sOptName, int iOptCode, int iPlanCode, int OptEquipmentCode, bool bOptInsurance, int iQuantity, string sOptionalText, bool bRequiredInsurance, string sOptionalType)
    {
        

        OptionalObj o = new OptionalObj();
        o.OptionalCode = iOptCode;
        o.OptionalName = sOptName;
        o.PlanCode = iPlanCode;
        o.Insurance = bOptInsurance;
        o.EquipmentCode = OptEquipmentCode;
        o.Quantity = iQuantity;
        o.OptionText = sOptionalText;
        o.RequiredInsurance = bRequiredInsurance;
        o.OptionalType = sOptionalType;
        IList<OptionalObj> obj = GetOnlineOptionals();   
        obj.Add(o);
        return o;

    }
    public static TmpOrderObj getTmpOrder()
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {


            string sName = getName("TmpOrderObj"); 

            if (thisSession[sName] != null)
                return (TmpOrderObj)thisSession[sName];
            else
                return null;
        }
        else
            return null;
    }


    public static List<PlanDetails> getPlanDetails()
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {


            string sName = getName("PlanDetails");

            if (thisSession[sName] != null)
                return (List<PlanDetails>)thisSession[sName];
            else
                return null;
        }
        else
            return null;
    }


    public static void AddTmpOrder(TmpOrderObj t)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            string sName = getName("TmpOrderObj"); 

            thisSession[sName] = t;            
        }
        
    }

    public static object GetObjValue(string sName)
    {

        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            sName = getName(sName); 

            if (thisSession[sName] != null)
                return thisSession[sName];
            else
                return null;
        }
        else
            return null;
    }
    public static string GetValue(string sName)
    {

        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            sName = getName(sName); 
            
            if (thisSession[sName] != null)
                return thisSession[sName].ToString();
            else

                return "";
        }
        else
        {
            //HttpContext.Current.Response.Redirect("OfflinePage.aspx?msg=3");
            return "";
        }
    }

   
    public static DataRow getAllowButtons(string sStatus)
    {
        //HttpSessionState thisSession = HttpContext.Current.Session;
        //if (thisSession != null)
        //{

        //    string sRowName = "AllowButtons";

        //    if (thisSession[sRowName] != null)
        //        return (DataRow)thisSession[sRowName];
        //    else
        //    {
        //        DataRow dr = dbUtility.GetRowAllowButtonsByStatus(sStatus); 
        //        if (dr != null)
        //        {
        //            thisSession[sRowName] = dr;
        //            return (DataRow)thisSession[sRowName];
        //        }
        //        else
        //            return null;
        //    }
        //}
        //else
        //    return null;
        return null;//
    }



    
    public static string getName(string s)
    {
        //HttpSessionState thisSession = HttpContext.Current.Session;

        //if (s != "PageId" && s != "errMsg" & thisSession["PageId"] != null)
         //   s = s + thisSession["PageId"].ToString();
       

        return s;
    }


    public static void SetLOB(string sLOB)
    {
        AddValue("LOB", sLOB);  
    }

    public static bool IsLOBWW()
    {
        if (GetValue("LOB").ToString() == "WW")
            return true;
        else
            return false;
    }


    public static bool IsLOBRABBIS()
    {
        if (GetValue("LOB").ToString() == "RABBIS")
            return true;
        else
            return false;
    }

    public static bool IsLOBTAIL()
    {
        if (GetValue("LOB").ToString() == "TAIL")
            return true;
        else
            return false;
    }

    public static int GetLOBCode()
    {
        if (GetValue("LOB").ToString() == "TAIL")
            return 20;
        else
            return 10;
    }



    public static string GetLanguageValue(string sName)
    {
        string sLang = GetValue(sName);
        if (sLang == "")
        {
            sLang = "en-US";
            AddValue(sName, sLang);
        }
        return sLang;
    }

    public static bool IsFrenchLang()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "fr-FR")
            return true;
        else if (sLang == "es-ES")
            return true;
        else return false;
    }

    public static bool IsPortagLang()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "pt-BR")
            return true;
        else
            return false;
    }
    
    public static bool IsHebLang()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "he-IL")
            return true;
        else
            return false;
    }

    public static bool IsPortLang()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "pt-BR")
            return true;
        else
            return false;
    }

    public static bool IsEngLang()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "en-US")
            return true;
        else
            return false;
    }


    public static string GetLangParam()
    {
        string sLang = GetLanguageValue("Language");
        if (sLang == "fr-FR")
            return "2";
        else if (sLang == "en-US")
            return "1";
        else if (sLang == "he-IL")
            return "3";
        if (sLang == "es-ES")
            return "4";
        else
            return "1";
    }
    public static Nullable<DateTime> GetDateTimeValue(string sName)
    {

        HttpSessionState thisSession = HttpContext.Current.Session;
        DateTime d;
        if (thisSession != null)
        {
            sName = getName(sName); 

            if (thisSession[sName] != null)
            {
                try
                {
                    d=Convert.ToDateTime(thisSession[sName].ToString());
                    return d;
                }
                catch 
                {
                    return null;
                }
                
            }
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    public static string GetSessionId()
    {

        HttpSessionState thisSession = HttpContext.Current.Session;
        if (thisSession != null)
        {
            return thisSession.SessionID;
        }
        return "";
    }

    public static Int32 GetIntValue(string sName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        try
        {
            if (thisSession != null)
            {

                sName = getName(sName); 

                if (thisSession[sName] != null)
                    return Convert.ToInt32(thisSession[sName].ToString());
                else
                    return 0;
            }
            else
                return 0;
        }
        catch
        {
            return 0;
        }
    }

    public static decimal  GetDecimalValue(string sName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;
        try
        {
            if (thisSession != null)
            {

                sName = getName(sName);

                if (thisSession[sName] != null)
                    return Convert.ToDecimal(thisSession[sName].ToString());
                else
                    return 0;
            }
            else
                return 0;
        }
        catch
        {
            return 0;
        }
    }

    public static DataTable GetDTValue(string sTableName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            sTableName = getName(sTableName); 

            if (thisSession[sTableName] != null)
                return (DataTable)thisSession[sTableName];
            else
                return null;
        }
        else
            return null;
    }

    public static DataSet GetDSValue(string sTableName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {

            sTableName = getName(sTableName); 

            if (thisSession[sTableName] != null)
                return (DataSet)thisSession[sTableName];
            else
                return null;
        }
        else
            return null;
    }

    public static DataRow GetDTFirstRow(string sTableName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            sTableName = getName(sTableName); 

            if (thisSession[sTableName] != null)
            {
                DataTable dt = (DataTable)thisSession[sTableName];
                DataRow dr = null;
                if (dt.Rows.Count > 0)
                    dr = dt.Rows[0];

                return dr;
            }
            else
                return null;
        }
        else
            return null;
    }

    public static DataRow GetDataRowByName(string sRowName)
    {
        HttpSessionState thisSession = HttpContext.Current.Session;

        if (thisSession != null)
        {
            sRowName = getName(sRowName);

            if (thisSession[sRowName] != null)
            {
                DataRow dr = (DataRow)thisSession[sRowName];
                return dr;
            }
            else
                return null;
        }
        else
            return null;
    }



}
