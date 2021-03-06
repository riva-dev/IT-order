
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Text;
/// <summary>
/// Summary description for otherUtility
/// </summary>
public class otherUtility
{
    public otherUtility()
	{
		
	}

   

    public static string getMasterName()
    {
        if (SessionUtility.GetBoolValue("newOrderSite"))
            return ("~/MasterPageInnerFM.master");
        if (SessionUtility.GetBoolValue("newSite"))
            return("~/MasterPageInner.master");
        else
            return("~/MasterPage.master");
    }
    public static void SSL()
    {
        string url_path_current = HttpContext.Current.Request.Url.ToString();
        url_path_current = url_path_current.ToLower();
        int pos = url_path_current.LastIndexOf("/");
        if (pos > -1 & !url_path_current.Contains("localhost"))
        {
            if (url_path_current.StartsWith("http:") == true)
            {
                HttpContext.Current.Response.Redirect("https://www.telaway.net/order" + url_path_current.Remove(0, pos), false);
            }
        }
    }


    public static void NotSSL()
    {
        string url_path_current = HttpContext.Current.Request.Url.ToString();
        url_path_current = url_path_current.ToLower();
        int pos = url_path_current.LastIndexOf("/");
        if (pos > -1)
        {
            if (url_path_current.StartsWith("https:") == true)
            {
                HttpContext.Current.Response.Redirect("http://www.telaway.net/order" + url_path_current.Remove(0, pos), false);
            }
        }
    }
    public static string FormatCurrencySymbol(string symbol)
    {
        if (SessionUtility.IsFrenchLang ())
            return "";
        else
        {
        string[] splitSymbol = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (splitSymbol.Length > 0)
            if (splitSymbol[0].Equals("US"))
                return @"<span style=""font-size:9px;"">US </span>$";
        return symbol;
        }
    }

    public static string FormatCurrencySymbolAndAmount(decimal amt,string symbol)
    {
        if (SessionUtility.IsFrenchLang())
            return amt.ToString().Replace(".", ",") + symbol;
        else
        {
            string[] splitSymbol = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitSymbol.Length > 0)
                if (splitSymbol[0].Equals("US"))
                    return @"<span style=""font-size:9px"">US </span>$" + amt.ToString();
            return  symbol + amt.ToString();
        }
    }

    public static string FormatCurrencyFRSymbolAndAmount(decimal amt, string symbol)
    {
        if (SessionUtility.IsFrenchLang())
            return amt.ToString().Replace(".", ",") + symbol;
        else
            return amt.ToString(); 
        
    }
   
    public  static void writeLog(string s)
    {
        string sFileName = "C:\\Inetpub\\wwwroot\\TNS1\\log.txt";
      
        try
        {
            int tics = System.Environment.TickCount & Int32.MaxValue; 
            TextWriter tw = new StreamWriter(sFileName, true);
            long t = 0;
            if (SessionUtility.GetValue("LastTics")!="")
                t=Convert.ToInt64(  SessionUtility.GetValue("LastTics"));
            //long tt = DateTime.Now.Ticks;
            long tt2 = 0;
            if (t > 0 )
            {
                tt2 = tics - t;
            }
            //tw.WriteLine(s.PadRight(30) +" " + DateTime.Now+"  "+ DateTime.Now.Ticks +" "+ tt2.ToString());
            tw.WriteLine(s.PadRight(30) + " " + DateTime.Now + "  " + tics+ " "+ tt2.ToString());
            
            tw.Close();
            SessionUtility.AddValue("LastTics", tics); 
        }
        catch
        {

        }
    }


    public static int? GetIntOrNull(Nullable<int> val)
    {
        if (val.HasValue)
        {
            if (val == -1)
                return null;
            else
                return Convert.ToInt32(val);
        }
        else
            return null;   
    }
    public static int GetIntVal(Nullable<int> val, string sFieldName)
    {
        if (val.HasValue)
            return Convert.ToInt32(val);
        else
        {
            emailUtility.SendMailErr(sFieldName + " Is null");
            throw (new Exception(sFieldName + " have null data "));   
        }
    }
    public static int GetValidIntVal(Nullable<int> val, string sFieldName)
    {
        int ret = -1;

        if (val.HasValue)
        {
            ret=Convert.ToInt32(val);
            if (ret > 0)
                return ret;
            else
            {
                emailUtility.SendMailErr(sFieldName + " <=0 ");
                throw (new Exception(sFieldName + " is invalid. Please go back to select it again. "));
            }
        }
        else
        {
            emailUtility.SendMailErr(sFieldName + " Is null");
            throw (new Exception(sFieldName + " have null data "));
        }
    }
    
    public static string GetStrVal(string  val, string sFieldName)
    {
        if (val.Trim()!="")
            return val;
        else
        {
            emailUtility.SendMailErr(sFieldName + " Is empty");
            throw (new Exception(sFieldName + " have empty data "));            
        }
    }

    public static DateTime GetDateTimeVal(Nullable<DateTime> val, string sFieldName)
    {
        if (val.HasValue)
            return Convert.ToDateTime(val);
        else
        {
            emailUtility.SendMailErr(sFieldName + " no date ");
            throw (new Exception(sFieldName + " have null data "));   
        }
    }

    public static bool GetBoolVal(Nullable<bool> val , string sFieldName)
    {
        if (val.HasValue)
            return Convert.ToBoolean(val);
        else
        {
            emailUtility.SendMailErr(sFieldName + " Is null");
            throw (new Exception(sFieldName + " have null data "));   
        }
    }


    public static decimal GetDecimalVal(Nullable<double> val, string sFieldName)
    {
        if (val.HasValue)
            return Convert.ToDecimal(val);
        else
        {
            emailUtility.SendMailErr(sFieldName + " Is null");
            throw (new Exception(sFieldName + " have null data "));   
        }
    }

    public static  string getDecimalPoint()
    {
        if (SessionUtility.IsFrenchLang())
            return (",");
        else
            return (".");

    }

    public static string getPadString(int len)
    {
        //"&nbsp;&nbsp;"

        string spaceChar = "";
        for (int i = 0; i < len; i++)
            spaceChar = spaceChar + "&nbsp;";
    
        return spaceChar;
    }



    public static string getDateFormat()
    {
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aCI.ToString() == "en-US")
            return "MM/dd/yyyy";
        else
            return "dd/MM/yyyy";
    }

    public static string getResourceString(string sName)
    {
        string sRet = "";
        ResourceManager aRM = SessionUtility.GetObjValue("myResourceManager") as ResourceManager;
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aRM == null || aCI == null)
        {
            string sCultureInfoName = SessionUtility.GetLanguageValue("Language");
            aRM = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
            aCI = new CultureInfo(sCultureInfoName);
            SessionUtility.AddValue("myResourceManager", aRM);
            SessionUtility.AddValue("myCultureInfo", aCI);
            sRet = aRM.GetString(sName, aCI);

        }
        else
            sRet = aRM.GetString(sName, aCI);

        return sRet;

    }


    public static void LoadLangParameter(Page p)
    {
        string sCurrentLang = SessionUtility.GetLanguageValue("Language");
        string sNewLang = "";
        string sParam = "";

        if (p.Request.QueryString["l"] != null)
        {
            sParam = p.Request.QueryString["l"].ToString();
            sNewLang = GetLanNameById(sParam);
            SessionUtility.AddValue("Language", sNewLang);
            if (sNewLang != sCurrentLang)
                ReloadLang();
        }

    }

    public static string GetLanNameById(string sId)
    {
        string sLang="en-US";

        if (sId == "1")
           sLang = "en-US";
        else if (sId == "2")
           sLang = "fr-FR";
        else if (sId == "3")
            sLang = "he-IL";

        return sLang;
    }

    public static void ReloadLang()
    {        
        ResourceManager aRM = SessionUtility.GetObjValue("myResourceManager") as ResourceManager;
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        
        string sCultureInfoName = SessionUtility.GetLanguageValue("Language");
        aRM = new ResourceManager("Resources.Strings", System.Reflection.Assembly.Load("App_GlobalResources"));
        aCI = new CultureInfo(sCultureInfoName);
        SessionUtility.AddValue("myResourceManager", aRM);
        SessionUtility.AddValue("myCultureInfo", aCI);
          

    }

   

    public static string getFormatNumber()
    {
        string sRet = "";
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aCI.ToString() == "en-US")
            sRet = "{0:$0.00}";
        else
            sRet = "{0:0.00$}";   
        

        return sRet;

    }

    public static string getCentsByLang(decimal cents,Boolean  bWithPoints)
    {
        string sRet = "";
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aCI.ToString() == "en-US")
        {
            if (bWithPoints)
                sRet = (cents * 100).ToString("0.#") + "¢ ";
            else              
                sRet = (cents * 100).ToString("0") + "¢ ";

        }
        else
            sRet = string.Format(aCI, "{0:0.###$}", cents);


        return sRet;

    }
    public static string getCentsByLang(double cents, Boolean bWithPoints)
    {
        string sRet = "";
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aCI.ToString() == "en-US")
        {
            if (bWithPoints)
                sRet = (cents * 100).ToString("0.#") + "¢ ";
            else
                sRet = (cents * 100).ToString("0") + "¢ ";

        }
        else
            sRet = string.Format(aCI, "{0:0.###$}", cents);


        return sRet;

    }

    public static string getDollarByLang(double  dNum)
    {
        string sRet = "";
        CultureInfo aCI = SessionUtility.GetObjValue("myCultureInfo") as CultureInfo;
        if (aCI.ToString() == "en-US")
            sRet = string.Format(aCI, "{0:$0.#0}", dNum);
        else
            sRet = string.Format(aCI, "{0:0.##$}", dNum);


        return sRet;

    }

    public static string FormatDate(DateTime dDate)
    {
        return dDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
    }


    public static string GetStringSpecialChars(string sVal)
    {
        string sRet = "";       
        string normalized = sVal.Normalize(NormalizationForm.FormKD);
        StringBuilder builder = new StringBuilder();
        foreach (char c in normalized)
        {
            if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                builder.Append(c);

        }
        sRet = builder.ToString().Replace("Æ", "AE").Replace("æ", "ae").Replace("Œ", "OE").Replace("œ", "oe");

        return sRet;
    }

}
