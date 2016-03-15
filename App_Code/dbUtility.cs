


using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;  
using System.Globalization;
using System.Resources;
using System.Reflection;

/// <summary>
/// Summary description for dbUtility
/// </summary>
public class dbUtility
{

    public dbUtility()
	{
		
	}

    

  
    public static string Capitalize(string s)
    {
        s = s.Trim();

        string retS = "";
        if (s.Trim() == "")
            return s;



        string[] sArray = s.Split(' ');
        string sEzer = "";

        if (sArray.Length > 1)
        {
            for (int i = 0; i < sArray.Length; i++)
            {
                sEzer = sArray[i];
                if (sEzer.Trim() != "")
                {
                    sArray[i] = sEzer.Substring(0, 1).ToUpper() + sEzer.Substring(1, sEzer.Length - 1);
                    retS = retS + sArray[i];
                    if (i < sArray.Length - 1)
                        retS = retS + " ";
                }
            }
            return retS;
        }
        else
        {
            if (s.Length > 1)
            {
                retS = s.Substring(0, 1).ToUpper();
                retS = retS + s.Substring(1, s.Length - 1);
                return retS;

            }
            else
                return s.ToUpper();
        }
    }

    public static bool isValidPlan(int iPlanCode)
    {
        int lobCode = SessionUtility.GetLOBCode();
        string s = "SELECT PlanCode  FROM tblPlans where plancode="+iPlanCode.ToString()+" and company="+lobCode;
        bool b=ExecBoolScalar(s); 
        return b;
    }


    public static DataTable getPlanInfo(int iPlanCode)
    {
        DataTable dt;
        string s = "SELECT * FROM tblPlans where plancode='" + iPlanCode.ToString()+"'";
        dt = getTableBySQL(s);        
        return dt;
    }


    public static DataTable getShipDetails(int shipId)
    {

        string s = " SELECT s.shippingID,s.shippingName,s.shippingDesc ,s.optRequireShipAddress ,s.optLocalPickup ,s.shippingCost as 'cost',s.shippingCurrency ,c.CountryName,"
        + " case when s.optLocalPickup=1 then 'OFFICE_PICKUP' else 'SHIPPING' end as 'shippingMethodNew' "
        + " FROM tblShippingOptions s inner join tblCountries c on c.countryID=s.Country "
        + " where s.shippingID=" + shipId.ToString() + " and s.ShowInSite=1 ";
        return getTableBySQL(s);
    }
    public static bool IsValidCCNum(string strCCNum)
    {

        bool ret = false;
        string SQL = "SELECT dbo.udf_Bank_IsLuhn(" + "'" + strCCNum + "'" + ") AS Result";
        using (SqlConnection aCon = new SqlConnection(ConfigurationManager.AppSettings["DBOR_Online"].ToString()))
        {
            aCon.Open();
            SqlCommand aCmd = new SqlCommand();
            aCmd.CommandText = SQL;
            aCmd.CommandType = CommandType.Text;
            aCmd.Connection = aCon;
            ret = (bool)aCmd.ExecuteScalar();
            aCon.Close();
        }
        return ret;
    }
    public static DataTable getShippingByLob(int iLOB)
    {
        string s=" SELECT s.shippingID,s.shippingName,s.shippingDesc ,s.optRequireShipAddress ,s.optLocalPickup ,s.shippingCost as 'cost',s.shippingCurrency ,c.CountryName,"
        + " case when s.optLocalPickup=1 then 'OFFICE_PICKUP' else 'SHIPPING' end as 'shippingMethodNew' "
        + " FROM tblShippingOptions s inner join tblCountries c on c.countryID=s.Country " 
        + " where s.company="+ iLOB.ToString() +" and s.ShowInSite=1 ";
        return getTableBySQL(s);
    }

    


  
    public static string ReplaceField(string sFieldName, string sFieldValue, string sHtml)
    {
        if (sFieldValue == null)
            sFieldValue = "";

        if (sFieldValue.Trim() != "")
            sHtml = sHtml.Replace(sFieldName, sFieldValue);
        else
            sHtml = sHtml.Replace(sFieldName, "");

        return sHtml;
    }

    public static string getHtmlErrMsg1()
    {
        SessionUtility.AddValue("ValidErr", "");

        if (SessionUtility.GetBoolValue("InValidQuantity"))
            SessionUtility.AddValue("ValidErr", otherUtility.getResourceString("pleaseSelectQnt"));

       
        if (SessionUtility.GetBoolValue("InValidKntCode"))
            SessionUtility.AddValue("ValidErr", otherUtility.getResourceString("ValChooseKntCode"));
        
        if (SessionUtility.GetBoolValue("InValidSimType"))
                    SessionUtility.AddValue("ValidErr", otherUtility.getResourceString("ValChooseSimType"));

        return SessionUtility.GetValue("ValidErr");
    }

    public static string getHtmlErrMsg()
    {
        SessionUtility.AddValue("ValidErr", "");

        if (SessionUtility.GetBoolValue("InValidDates"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseSelectStartDate"));

        if (SessionUtility.GetBoolValue("InValidQuantity"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("pleaseSelectQnt"));


        if (SessionUtility.GetBoolValue("InValidKntCode"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseKntCode"));

        if (SessionUtility.GetBoolValue("InValidSimType"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ValChooseSimType"));


        /*if (SessionUtility.GetBoolValue("InValidFName"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterFirstName"));

        if (SessionUtility.GetBoolValue("InValidLName"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterLastName"));

        if (SessionUtility.GetBoolValue("InValidEmail"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));

        if (SessionUtility.GetBoolValue("InValidEmail2"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterValidEmail"));
                
        if (SessionUtility.GetBoolValue("InValidPhone"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterPhone"));
                
        if (SessionUtility.GetBoolValue("InValidCell"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCell"));

        if (SessionUtility.GetBoolValue("InValidAddress"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterStreet"));
              
        if (SessionUtility.GetBoolValue("InValidCity"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCity"));
              
        if (SessionUtility.GetBoolValue("InValidCountry"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseContry"));
                
        if (SessionUtility.GetBoolValue("InValidState"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseState"));
        */
        //if (SessionUtility.GetBoolValue("InValidZip"))

        if (SessionUtility.GetBoolValue("InValidshipOp"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PlaeseChooseShippingOptions"));
           
        if (SessionUtility.GetBoolValue("InValidshipOpAddress"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PlaeseChooseShippingOptions"));

        if (SessionUtility.GetBoolValue("InValidshipOpAddress2"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("WeAreSorryDeliveryMethod"));

        if (SessionUtility.GetBoolValue("InValidDateLeave"))
            SessionUtility.AddValuePlus("ValidErr", SessionUtility.GetValue("DateLeave") == "" ? otherUtility.getResourceString("PleaseEnterDeliveryDate") : SessionUtility.GetValue("DateLeave"));

        if (SessionUtility.GetBoolValue("InValidShipName"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipName"));

        if (SessionUtility.GetBoolValue("InValidShipAddress"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipAddress"));

        if (SessionUtility.GetBoolValue("InValidShipCity"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipCity"));

        if (SessionUtility.GetBoolValue("InValidShipState"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseShipState"));

        //if (SessionUtility.GetBoolValue("InValidShipZip"))

        //if (SessionUtility.GetBoolValue("InValidShipPhone"))
         //   SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterShipPhone"));

        //if (SessionUtility.GetBoolValue("InValidShipCountry"))
        /*if (SessionUtility.GetBoolValue("InValidPayment"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ChoosePaymentMethod"));*/

        if (SessionUtility.GetBoolValue("InValidCCType"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseChooseCCType"));
        
        if (SessionUtility.GetBoolValue("InValidCCNum"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("IncorrectCreditCardNumber"));

       /* if (SessionUtility.GetBoolValue("InValidCCFirstName"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCFName"));

        if (SessionUtility.GetBoolValue("InValidCCLastName"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnterCCLName"));

        if (SessionUtility.GetBoolValue("InValidCCEmail"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("PleaseEnteraCCEmail"));
        */
        if (SessionUtility.GetBoolValue("InValidCCExpDate"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("ItSeemsCCExpired"));

        if (SessionUtility.GetBoolValue("InValidAgree"))
            SessionUtility.AddValuePlus("ValidErr", otherUtility.getResourceString("YouMustAgree"));

        return SessionUtility.GetValue("ValidErr");
    }
    public static bool IsValidCoupon(string sCoupon)
    {
        bool ret = false;
        string SQL = "SELECT dbo.udf_IsTACouponValid(@strCouponCode,@datCheckDate) AS Result";
        using (SqlConnection aCon = new SqlConnection(ConfigurationManager.AppSettings["DBOR_Online"].ToString()))
        {
            aCon.Open();
            SqlCommand aCmd = new SqlCommand();
            aCmd.CommandText = SQL;
            aCmd.Parameters.Add("@strCouponCode", SqlDbType.NVarChar);
            aCmd.Parameters.Add("@datCheckDate", SqlDbType.DateTime);
            aCmd.Parameters["@strCouponCode"].Value = sCoupon;
            aCmd.Parameters["@datCheckDate"].Value = DateTime.Now;
            aCmd.CommandType = CommandType.Text;
            aCmd.Connection = aCon;
            ret = (bool)aCmd.ExecuteScalar();
            aCon.Close();
        }
        return ret;
    }
    private static string ReplaceImgField(string sFieldName, string sFieldValue, string sHtml)
    {
        string sImg = "<img src='img/checkbox1.jpg'   />";
        string sNoImg = "<img src='img/box.jpg'  />";

        if (sFieldValue == null)
            sFieldValue = "";

        if (sFieldValue != "")
            sHtml = sHtml.Replace(sFieldName, sImg);
        else
            sHtml = sHtml.Replace(sFieldName, sNoImg);

        return sHtml;
    }
    public static string ExecScalarByStrParams(string sSql,string sPar, string sVal)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.Parameters.AddWithValue("@" + sPar, sVal);
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sRetVal = "";
                    sRetVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sRetVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }


     public static string ExecScalarByStrParam(string sSql,string sPar, string sVal)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.Parameters.AddWithValue("@" + sPar, sVal);
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sRetVal = "";
                    sRetVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sRetVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }
    public static string ExecScalarByStrParam2013(string sSql, string sPar, string sVal)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.Parameters.AddWithValue("@" + sPar, sVal);
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sRetVal = "";
                    sRetVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sRetVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }

    public static string ExecScalar(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sVal = "";
                    sVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }


    public static int ExecIntScalar(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    int iVal = -1;
                    string sVal = Convert.ToString(aCmd.ExecuteScalar());
                    iVal = Convert.ToInt32(sVal);  
                    return iVal;
                }
                catch
                {
                    return -1;
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return -1;
        }

    }
    public static int ExecIntScalar_DBOR(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    int iVal = -1;
                    string sVal = Convert.ToString(aCmd.ExecuteScalar());
                    iVal = Convert.ToInt32(sVal);
                    return iVal;
                }
                catch
                {
                    return -1;
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return -1;
        }

    }
    public static string ExecScalar_DBOR(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sVal = "";
                    sVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }

    public static bool UpdatePayPalLogOrder(int iPayPalLogCounter, int iOnlineOrderCode)
    {
        string sSql = "Update tblPaypalLog set OnlineOrderCode=@OnlineOrderCode where Counter=@Counter " ;

        string[] sParamsArray = new string[2];
        string[] sValArray = new string[2]; 

        sParamsArray[0] = "OnlineOrderCode";
        sValArray[0] = iOnlineOrderCode.ToString();

        sParamsArray[1] = "Counter";
        sValArray[1] = iPayPalLogCounter.ToString();

        return ExecNoQuery_DBORParmas(sSql, sParamsArray, sValArray);

    }

    public static bool UpdatePayPalTransaction(int iPayPalLogCounter, string sPaypalTransID)
    {
        string sSql = "Update tblPaypalLog set Comment=@Comment, PaymentSuccess=@PaymentSuccess,PaypalTransID=@PaypalTransID where Counter=@Counter";
        string[] sParamsArray = new string[4];
        string[] sValArray = new string[4]; ;

        sParamsArray[0] = "Comment";
        sValArray[0] = "Success";

        sParamsArray[1] = "PaymentSuccess";
        sValArray[1] = "1";

        sParamsArray[2] = "PaypalTransID";
        sValArray[2] = sPaypalTransID;

        sParamsArray[3] = "Counter";
        sValArray[3] = iPayPalLogCounter.ToString();

        return ExecNoQuery_DBORParmas(sSql, sParamsArray, sValArray);
    }
    public static bool UpdatePayPalTransactionCanceled(int iPayPalLogCounter)
    {
        string sSql = "Update tblPaypalLog set Comment=@Comment where Counter=@Counter";


        string[] sParamsArray = new string[2];
        string[] sValArray = new string[2]; ;

        sParamsArray[0] = "Comment";
        sValArray[0] = "Canceled" ;

        sParamsArray[1] = "Counter";
        sValArray[1] = iPayPalLogCounter.ToString();

        return ExecNoQuery_DBORParmas(sSql, sParamsArray,sValArray );
    }
    

    public static bool UpdatePayPalTransactionFailed(int iPayPalLogCounter,string sErr)
    {
        if (sErr.Length > 40)
            sErr = sErr.Substring(0, 40);  

        string sSql = "Update tblPaypalLog set Comment=@Comment where Counter=@Counter";

        string[] sParamsArray= new string[2];
        string[] sValArray = new string[2]; ;

        sParamsArray[0] = "Comment";
        sValArray[0] = "Failed:"+sErr;

        sParamsArray[1] = "Counter";
        sValArray[1] = iPayPalLogCounter.ToString();

        return ExecNoQuery_DBORParmas(sSql,sParamsArray,sValArray);
    }
    public static int AddPayPalLog(string sPayPalTransactionId, decimal dPayPalAmountCharge, bool bSucess, string sUserName, string sClientEmail, string sSessionId, string sComments)
    {
        int iResult = -1;

        string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            dbAdapter.InsertCommand = new SqlCommand();
            dbAdapter.InsertCommand.CommandText = "AY_DBOR_Online_AddPayPalLog";
            dbAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            dbAdapter.InsertCommand.Parameters.AddWithValue("@intOnlineOrderCode", null );
            dbAdapter.InsertCommand.Parameters.AddWithValue("@strClientName", sUserName);
            
            dbAdapter.InsertCommand.Parameters.AddWithValue("@strClientEmail", sClientEmail);
             dbAdapter.InsertCommand.Parameters.AddWithValue("@strSessionId", sSessionId);
            
            dbAdapter.InsertCommand.Parameters.AddWithValue("@decAmount", dPayPalAmountCharge );
            dbAdapter.InsertCommand.Parameters.AddWithValue("@strPaypalTransID", sPayPalTransactionId );
            dbAdapter.InsertCommand.Parameters.AddWithValue("@bPaymentSuccess", bSucess );
            dbAdapter.InsertCommand.Parameters.AddWithValue("@strComment", sComments);
         
            SqlParameter prm = new SqlParameter();
            prm.ParameterName = "@intNewCounter";
            prm.SqlDbType = SqlDbType.Int;
            prm.Direction = ParameterDirection.Output;
            dbAdapter.InsertCommand.Parameters.Add(prm);

            dbAdapter.InsertCommand.Connection = aConn;
            dbAdapter.InsertCommand.Connection.Open();
            try
            {
                dbAdapter.InsertCommand.ExecuteNonQuery();
                iResult = (int)dbAdapter.InsertCommand.Parameters["@intNewCounter"].Value;
            }
            catch (Exception e)
            {
                SessionUtility.AddValue("errMsg", e.Message);
                return iResult;
            }
            finally
            {
                dbAdapter.InsertCommand.Connection.Close();
            }
            return iResult;
        }
    }


    public static bool ExecBoolScalar(string sSql)
    {
        string s=ExecScalar(sSql);
        if (s == "1")
            return true;
        else
            return false;
    }

    public static bool ExecBoolScalar_DBOR(string sSql)
    {
        string s = ExecScalar_DBOR(sSql);
        if (s.ToUpper()  == "TRUE")
            return true;
        else
            return false;
    }
    public static bool ExecNoQuery(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    aCmd.ExecuteNonQuery();
                    return true;

                }
                catch
                {
                    return false;
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return false;
        }

    }


    //public static bool ExecNoQuery_DBOR(string sSql)
    //{
    //    try
    //    {
    //        string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
    //        using (SqlConnection aConn = new SqlConnection(conStr))
    //        {
    //            aConn.Open();
    //            SqlCommand aCmd = new SqlCommand();
    //            aCmd.CommandText = sSql;
    //            aCmd.CommandType = CommandType.Text;
    //            aCmd.Connection = aConn;
    //            try
    //            {
    //                aCmd.ExecuteNonQuery();
    //                return true;

    //            }
    //            catch
    //            {
    //                return false;
    //            }

    //            finally
    //            {
    //                aConn.Close();
    //            }
    //        }
    //    }
    //    catch(Exception ee) 
    //    {
    //        emailUtility.SendMailErr("EXEC NO Query " + sSql + " " + ee.Message); 
    //        return false;
    //    }

    //}
    public static string GetTermsURL()
    {
        string  country = SessionUtility.GetValue("UserCountry");

        return dbUtility.ExecScalarByStrParam("select TermsURL from [tblTelawaySitesInfo] where name =@Country", "Country", country);
    }

    public static bool ExecNoQuery_DBORParmas(string sSql, string[] sParamsArray,string[] sValArray)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                for (int i = 0; i < sParamsArray.Length; i++)
                {
                    if (sParamsArray[i].Contains("OrderAmount"))
                        aCmd.Parameters.AddWithValue("@" + sParamsArray[i],Convert.ToDecimal(sValArray[i]));
                    else
                        aCmd.Parameters.AddWithValue("@" + sParamsArray[i], sValArray[i]);
                }
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    aCmd.ExecuteNonQuery();
                    return true;

                }
                catch(Exception ex)
                {
                    emailUtility.SendMailErr("ExecNoQuery_DBORParmas  :" + ex.Message); 
                    return false;
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch (Exception ee)
        {
            emailUtility.SendMailErr("EXEC NO Query " + sSql + " " + ee.Message);
            return false;
        }

    }
    public static string getSqlCountries()
    {
        string s = "SELECT DISTINCT ltrim(CountryName) as 'CountryName', ltrim(CountryNameBR) as 'CountryNameBR', ltrim(CountryNameFR) as 'CountryNameFR', ltrim(CountryNameES) as 'CountryNameES', ltrim(CountryNameAR) as 'CountryNameAR',ltrim(CountryNameMX) as 'CountryNameMX', CountryID,isEU FROM tblCountries WHERE activeBilling = 1 ORDER BY CountryName ";
        return s;

    }
    public static string getUsaStateSql()
    {
        string s = "SELECT StateCode ,StateName,StateNameBR,StateNameFR,StateNameES,StateNameAR,StateNameMX FROM tblStates order by StateName";
        return s;
    }

    public static string getCanadaStateSql()
    {
        string s = "select  CanadaStateCode as 'StateCode',CanadaStateName as 'StateName',CanadaStateNameFR as 'StateNameFR' ,CanadaStateNameBR as 'StateNameBR',CanadaStateNameES as 'StateNameES',CanadaStateNameAR as 'StateNameAR' ,CanadaStateNameMX as 'StateNameMX' from dbo.tblCanadaStates";
        return s;
    }
    public static string getAusStateSql()
    {
        string s = "SELECT AusStateCode as 'StateCode',AusStateName as 'StateName',AusStateNameBR as 'StateNameBR' ,AusStateNameFR as 'StateNameFR',AusStateNameES as 'StateNameES',AusStateNameAR as 'StateNameAR' ,AusStateNameMX as 'StateNameMX' FROM tblAustraliaStates ";
        return s;
    }
    
    public static string getUsaCanadaStateSql()
    {
        string s = "SELECT StateCode ,StateName FROM tblStates  union all  select  CanadaStateCode as 'StateCode',CanadaStateName as 'StateName' from dbo.tblCanadaStates";
        return s;
    }
    public static DataTable getTableBySQL(string sSql)
    {
        DataTable dt = new DataTable();
        string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            try
            {
                dbAdapter.SelectCommand = new SqlCommand();
                dbAdapter.SelectCommand.CommandText = sSql;
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.SelectCommand.CommandTimeout = 5;
                dbAdapter.SelectCommand.Connection = aConn;
                dbAdapter.SelectCommand.Connection.Open();
                dbAdapter.Fill(dt);
                dbAdapter.SelectCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                SessionUtility.AddValue("errMsg", ex.Message);
                return null;
            }
        }
        return dt;
    }
    public static string GetFullPath(string sSite)
    {
        return ExecScalarByStrParam2015("SELECT FullUrl  FROM tblSitesGeneralInfo where site=@site", "site", sSite);
    }

    public static string GetTermsLink(string sSite)
    {
        //return ExecScalarByStrParam2015("SELECT TermsLink  FROM tblLinks where site=@site", "site", sSite);
        return ExecScalarByStrParam2015("SELECT TermsUrl  FROM tblSitesGeneralInfo  where site=@site", "site", sSite);
    }

    public static string GetHomePageLink(string sSite)
    {
        return (ExecScalarByStrParam2015("SELECT FolderName  FROM tblFolders where site=@site", "site", sSite) + "/HomePage.aspx");
    }
    public static string ExecScalarByStrParam2015(string sSql, string sPar, string sVal)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["Telaway2015"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.Parameters.AddWithValue("@" + sPar, sVal);
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    string sRetVal = "";
                    sRetVal = Convert.ToString(aCmd.ExecuteScalar());
                    return sRetVal;
                }
                catch
                {
                    return "";
                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {
            return "";
        }

    }

    public static DataTable getTableBySQLStrParam(string sSql, string sPar, string sVal)
    {
        DataTable dt = new DataTable();
        string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            try
            {
                SqlCommand command = new SqlCommand(sSql, aConn);
                command.Parameters.AddWithValue("@" + sPar, sVal);
             
                dbAdapter.SelectCommand = command;
                dbAdapter.SelectCommand.CommandText = sSql;
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.SelectCommand.CommandTimeout = 5;
                dbAdapter.SelectCommand.Connection = aConn;
                dbAdapter.SelectCommand.Connection.Open();
                dbAdapter.Fill(dt);
                dbAdapter.SelectCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                SessionUtility.AddValue("errMsg", ex.Message);
                return null;
            }
        }
        return dt;
    }
    public static DataTable getTableBySQLStrParam2015(string sSql, string sPar, string sVal)
    {
        DataTable dt = new DataTable();
        string conStr = System.Configuration.ConfigurationManager.AppSettings["Telaway2015"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            try
            {
                SqlCommand command = new SqlCommand(sSql, aConn);
                command.Parameters.AddWithValue("@" + sPar, sVal);

                dbAdapter.SelectCommand = command;
                dbAdapter.SelectCommand.CommandText = sSql;
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.SelectCommand.CommandTimeout = 5;
                dbAdapter.SelectCommand.Connection = aConn;
                dbAdapter.SelectCommand.Connection.Open();
                dbAdapter.Fill(dt);
                dbAdapter.SelectCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                SessionUtility.AddValue("errMsg", ex.Message);
                return null;
            }
        }
        return dt;
    }

    public static void ExecuteNonQuery2015(string sSql)
    {
        try
        {
            string conStr = System.Configuration.ConfigurationManager.AppSettings["Telaway2015"].ToString();
            using (SqlConnection aConn = new SqlConnection(conStr))
            {
                aConn.Open();
                SqlCommand aCmd = new SqlCommand();
                aCmd.CommandText = sSql;
                aCmd.CommandType = CommandType.Text;
                aCmd.Connection = aConn;
                try
                {
                    aCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    emailUtility.SendMailErr("ExecuteNonQuery:" + ex.Message);

                }

                finally
                {
                    aConn.Close();
                }
            }
        }
        catch
        {

        }

    }
    public static int GetVirtualCountryCode(string sParentLink,string sSubLink)
    {
        int iCountryCode = -1;
        iCountryCode = ExecIntScalar_DBOR("select sv.CountryCode from tblSubLinks_VirtualCountryCodes sv inner join dbo.tblSubLinks s on sv.sublinkid=s.counter where s.parentlink='" + sParentLink + "' and s.sublink='" + sSubLink + "'");

        return iCountryCode;
    }

    public static DataTable getDataTableBySQL(string sSql)
    {
        DataTable dt = new DataTable();
        string conStr = System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            try
            {
                dbAdapter.SelectCommand = new SqlCommand();
                dbAdapter.SelectCommand.CommandText = sSql;
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.SelectCommand.CommandTimeout = 5;
                dbAdapter.SelectCommand.Connection = aConn;
                dbAdapter.SelectCommand.Connection.Open();
                dbAdapter.Fill(dt);
                dbAdapter.SelectCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                SessionUtility.AddValue("errMsg", ex.Message);
                return null;
            }
        }
        return dt;
    }

    
    public static DataTable getTableBySQLTAIL(string sSql)
    {
        DataTable dt = new DataTable();
        string conStr = System.Configuration.ConfigurationManager.AppSettings["TAIL2013"].ToString();
        using (SqlConnection aConn = new SqlConnection(conStr))
        {
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            try
            {
                dbAdapter.SelectCommand = new SqlCommand();
                dbAdapter.SelectCommand.CommandText = sSql;
                dbAdapter.SelectCommand.CommandType = CommandType.Text;
                dbAdapter.SelectCommand.CommandTimeout = 5;
                dbAdapter.SelectCommand.Connection = aConn;
                dbAdapter.SelectCommand.Connection.Open();
                dbAdapter.Fill(dt);
                dbAdapter.SelectCommand.Connection.Close();
            }
            catch (Exception ex)
            {
                SessionUtility.AddValue("errMsg", ex.Message);
                return null;
            }
        }
        return dt;
    }

    public static string GetTextWithPlansParams(string s)
    {
        if (s.Contains("@Plan10D@"))
            s = s.Replace("@Plan10D@", GetPlanAmountByDays(10));

        if (s.Contains("@Plan30D@"))
            s = s.Replace("@Plan30D@", GetPlanAmountByDays(30));

        if (s.Contains("@Plan45D@"))
            s = s.Replace("@Plan45D@", GetPlanAmountByDays(45));

        if (s.Contains("@Plan60D@"))
            s = s.Replace("@Plan60D@", GetPlanAmountByDays(60));

        if (s.Contains("@MinmumPerDay@"))
            s = s.Replace("@MinmumPerDay@", GetMinmumPerDay());

        if (s.Contains("@Plan45DDisplay@"))
            s = s.Replace("@Plan45DDisplay@", GetAmountDispaly(45));

        if (s.Contains("@Plan60DDisplay@"))
            s = s.Replace("@Plan60DDisplay@", GetAmountDispaly(60));

        if (s.Contains("@PerDayForPlan10D@"))
            s = s.Replace("@PerDayForPlan10D@", GetPerDayAmountByDay(10));

        if (s.Contains("@PerDayForPlan30D@"))
            s = s.Replace("@PerDayForPlan30D@", GetPerDayAmountByDay(30));

        if (s.Contains("@PerDayForPlan60D@"))
            s = s.Replace("@PerDayForPlan60D@", GetPerDayAmountByDay(60));

        return s;
    }

    public static string GetAmountDispaly(int iDays)
    {
        string sRet = "";
        string sSite = SessionUtility.GetValue("UserCountry");
        string sSql = "SELECT case when(IsRightCurrency=1) then convert(nvarchar(30),AmountForDisplay)+CurrencySymbol else CurrencySymbol+convert(nvarchar(30),AmountForDisplay) end FROM tblTelawayPlans where countryname=@site and planDays=" + iDays.ToString();
        sRet = ExecScalarByStrParam2013(sSql, "site", sSite);
        return sRet;
    }

    public static string GetDecimalPoint()
    {
        string sRet = "";
        string sSite = SessionUtility.GetValue("UserCountry");
        string sSql = "SELECT  DecimalPoint FROM tblPlans where site=@site and DecimalPoint is not null";
        sRet = ExecScalarByStrParam2015(sSql, "site", sSite);
        return sRet;
    }

    public static string GetPlanAmountByDays(int iDays)
    {
        string sRet = "";
        string sSite = SessionUtility.GetValue("UserCountry");
        string sSql = "SELECT  case when(IsRightCurrency=1) then convert(nvarchar(30),TotalAmount)+''+CurrencySymbol else CurrencySymbol+convert(nvarchar(30),TotalAmount) end  FROM tblTelawayPlans where countryname=@site and planDays=" + iDays.ToString();
        sRet = ExecScalarByStrParam2013(sSql, "site", sSite);
        sRet = sRet.Replace(".", GetDecimalPoint());
        return sRet;
    }

    public static string GetPerDayAmountByDay(int iDays)
    {
        string sRet = "";
        string sSite = SessionUtility.GetValue("UserCountry");
        string sSql = "SELECT  TotalAmount/" + iDays + " FROM tblTelawayPlans where countryname=@site and planDays=" + iDays.ToString();
        sRet = ExecScalarByStrParam2013(sSql, "site", sSite);
        decimal d = Convert.ToDecimal(sRet);
        sRet = d.ToString();
        sRet = sRet.Substring(0, 4);
        //decimal aTruncated = Math.Truncate(d * 100) / 100;
        //sRet=d.ToString("F");
        //sRet = aTruncated.ToString();  
        sRet = sRet.Replace(".", GetDecimalPoint());
        return sRet;
    }

    public static string GetMinmumPerDay()
    {
        string sRet = "";
        string sSite = SessionUtility.GetValue("UserCountry");
        string sSql = "SELECT case when(IsRightCurrency=1) then  convert(nvarchar(30), AsLowPerDay)+CurrencySymbol else CurrencySymbol+convert(nvarchar(30), AsLowPerDay)end  FROM tblTelawayPlans where countryname=@site and planDays=60";
        sRet = ExecScalarByStrParam2013(sSql, "site", sSite);
        sRet = sRet.Replace(".", GetDecimalPoint());
        return sRet;
    }

    public static DataTable getUpsaleTable(string site)
    {
        return getDataTableBySQL("SELECT * FROM tblUpSale WHERE Active=1 AND ForTelaway=1 AND Site='" + site + "'");
    }
}
