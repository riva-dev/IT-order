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
/// Summary description for wsUtility
/// </summary>
public class wsUtility
{
	public wsUtility()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bool DownloadOrder()
    {
        bool bRet = false;
        using (BillInfoWS.BillingInfo ws = new BillInfoWS.BillingInfo())
        {
            ws.Timeout = 60000;
            try
            {
                int i = ws.DownloadNewOnlineOrders(20);
                bRet = true;

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The request failed with HTTP status"))
                {
                    SessionUtility.AddValue("errServer", "True");
                    SessionUtility.AddValue("errMsg", "DownloadOrder " + ex.Message);
                }
                bRet = false;
            }
        }

        return bRet;
    }

}
