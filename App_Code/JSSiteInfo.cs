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
/// Summary description for JSSiteInfo
/// </summary>
public class SiteInfo
{
    public string id { get; set;}
    public string name { get; set;}
    public string siteUrl { get; set;}
    public string phoneNumber { get; set;}
    public string emailAddress { get; set;}
    public string bccEmail { get; set;}
    public string salesEmail { get; set;}
    public string cusServiceEmail { get; set;}
    public string confirmationEmail { get; set;}
    public string mainEmail { get; set;}
    public string infoEmail { get; set;}
    public string payPalLocaleCode { get; set;}
    public string LanguageCode { get; set;}
    public string ReturnPolicy { get; set; }
    

    public SiteInfo() { }
    public SiteInfo(DataRow row)
    {
        name = row["Name"].ToString();
        siteUrl = row["SiteUrl"].ToString();
        phoneNumber = row["PhoneNumber"].ToString();
        emailAddress = row["EmailAddress"].ToString();
        bccEmail = row["BccEmail"].ToString();
        salesEmail = row["SalesEmail"].ToString();
        cusServiceEmail = row["CusServiceEmail"].ToString();
        confirmationEmail = row["ConfirmationEmail"].ToString();
        mainEmail = row["MainEmail"].ToString();
        infoEmail = row["EmailAddress"].ToString();
        payPalLocaleCode = row["PayPalLocaleCode"].ToString().Trim() ;
        LanguageCode = row["LanguageCode"].ToString().Trim();
        ReturnPolicy = row["ReturnPolicy"].ToString().Trim();
    }
    public SiteInfo getSiteInfo(string UserCountry)
    {
        var sql3 = string.Format(@"select * from tblTelawaySitesInfo where Name = @UserCountry ");
        DataTable dtSite = dbUtility.getTableBySQLStrParam(sql3, "UserCountry", UserCountry);
        if (dtSite != null)
            if (dtSite.Rows.Count > 0)
                return new SiteInfo(dtSite.Rows[0]);
        return null;
    }
}
