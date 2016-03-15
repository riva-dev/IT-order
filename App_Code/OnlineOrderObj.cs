using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic;





public class OnlineOrderObj
{
    #region Events
    /// <UnusedEvents>
    ///public event ErrorDeletingRecordEventHandler ErrorDeletingRecord;
    ///public delegate void ErrorDeletingRecordEventHandler(string strMsg, int intKey);
    ///public event ErrorLoadingRecordEventHandler ErrorLoadingRecord;
    ///public delegate void ErrorLoadingRecordEventHandler(string strMsg, int intKey);
    ///public event ErrorUpdatingRecordEventHandler ErrorUpdatingRecord;
    ///public delegate void ErrorUpdatingRecordEventHandler(string strMsg, int intKey);
    ///public event RecordDeletedEventHandler RecordDeleted;
    ///public delegate void RecordDeletedEventHandler(int intKey);
    ///public event RecordLoadedEventHandler RecordLoaded;
    ///public delegate void RecordLoadedEventHandler(int intKey);
    ///public event RecordUpdatedEventHandler RecordUpdated;
    ///public delegate void RecordUpdatedEventHandler(int intKey);
    /// </UnusedEvents>
    //Errors
    public event ErrorAddingRecordEventHandler ErrorAddingRecord;
    public delegate void ErrorAddingRecordEventHandler(string strMsg);
    //Success 
    public event RecordAddedEventHandler RecordAdded;
    public delegate void RecordAddedEventHandler(int intKey);
    #endregion
    #region Database Variables
    private Nullable<int> m_intOnlineOrderCode;
    private Nullable<int> m_intParentOnlineOrderCode;
    private Nullable<int> m_intLinkTypeCode;
    private Nullable<int> m_intAffiliateCounter;
    private Nullable<int> m_intKNTCountryCode;

    private string m_strSignupSourceID;
    private string m_strParentLink;
    private string m_strSubLink;
    private string m_strGroupMemberID;
    private string m_strTag;
    private string m_strAccessoryIdAndQuantity;      
    private string m_strGroupName;
    private string m_strSessionID;
    private string m_strClientIP;
    private string m_strPWD;
    private string m_strHint;
    private Int32 m_intTZ;

    private Nullable<bool> m_bIsSim;
    private Nullable<bool> m_bIsKosher;
    private Nullable<bool> m_bSubmitOrder;

    private Nullable<int> m_intPlanCode;
    private Nullable<int> m_intAgentCode;
    private Nullable<int> m_intSubAgentCode;
    private Nullable<int> m_intCompanyCode;
    private Nullable<int> m_intBaseCode;
    private List<SIMDetails> m_lstSimDetails;
    private Nullable<int> m_intEquipmentCode;
    private Nullable<int> m_intEquipmentModel;
    private Nullable<int> m_intCallPackageCode;
    private Nullable<int> m_intTermsCode;
    private Nullable<int> m_intProductId;
    private Nullable<int> m_intSMSPackageCode;
    private Nullable<int> m_intSMSPackageCounter;
    private string m_strCouponCode;
    private string m_strPayPalTransactionId;
    private Nullable<decimal> m_decPayPalAmountCharge;

    private string m_strEquipmentName;
    private string m_strEquipmentNotes;
    private string m_strBaseNotes;
    private Nullable<int> m_intPhonesRequired;
    private Nullable<bool> m_IsEquipmentSNS;
    private Nullable<int> m_intKNTRequired;
    private string m_strCallPackageName;
    private string m_strDeposit;
    private string m_strKNTName;
    private string m_strTermsName;
    private string m_strSMSPackageName;
    private string m_strPlanName;
    private Nullable<bool> m_bolInsurance;
    private Nullable<bool> m_bolPurchaseEquipment;
    private Nullable<bool> m_bolKITD;
    private Nullable<bool> m_bolKITD_BLOCK_ID;
    private Nullable<int> m_intDataPackageCode;
    private Nullable<int> m_intKITD_PlanCode;
    private Nullable<bool> m_bolSurftAndSave;
    private Nullable<bool> m_bolCreditEquipmentPurchase;
    private Nullable<int> m_iDataPackageId;
    private string m_strDataPackageName;
    private string m_strDataPackgeSize;
    private string m_strShipName;
    private string m_strShippingName;
    private string m_strShipStreet;
    private string m_strShipCity;
    private string m_strShipState;
    private string m_strShipPostalCode;
    private string m_strShipPhone;
    private string m_strShipEmail;
    private string m_strShipCountry;
    private string m_strShipMethod;
    private Nullable<int> m_intShotef;
    private float m_floatForex;
    private string m_strFirstName;
    private string m_strLastName;
    private string m_strCCNumber;
    private string m_strCCType;
    private string m_strCCExpiry;
    private string m_strCCIdNumber;
    private string m_strCCOwner;
    private Nullable<int> m_intBankNumber;
    private Nullable<int> m_intBranchNumber;
    private Nullable<int> m_intBillNumber;
    private string m_strPassword;
    private string m_strCustomerType;
    private int m_intCompany;
    private string m_strSetupFeeText;
    private Nullable<bool> m_bolShipCommercial;
    private Nullable<decimal> m_decShipFee;
    private Nullable<System.DateTime> m_datShipDate;
    private Nullable<System.DateTime> m_datDepartureDate;
    private Nullable<System.DateTime> m_datStartDate;
    private Nullable<System.DateTime> m_datEndDate;
    private Nullable<System.DateTime> m_datUserEndDate;
    private string m_strUserName;
    private string m_strUserStreet;
    private string m_strUserCity;
    private string m_strClientFirstName;
    private string m_strClientLastName;
    private string m_strClientStreet;
    private string m_strClientCity;
    private string m_strClientState;
    private string m_strClientCountry;
    private string m_strClientZip;
    private string m_strClientHomePhone1;
    private string m_strClientHomePhone2;
    private string m_strClientFax;
    private string m_strClientMobile;
    private string m_strClientEmail;
    private string m_strPaymentMethod;
    private string m_strCCNum;
    private Nullable<System.DateTime> m_datCCExpDate;
    private string m_strCCTitle;
    private string m_strCCCode;
    private Nullable<int> m_intLanguageCode;
    private Nullable<bool> m_bolSpecial;
    private Nullable<bool> m_bitCallPackageOverageProtection;
    private string m_strCustomerComment;
    private Nullable<int> m_intOrderCode;
    private Nullable<DateTime> m_datOrderCodeAddedOn;
    private Nullable<int> m_intRentalCode;
    private Nullable<DateTime> m_datRentalCodeAddedOn;

    private Nullable<int> m_intSignupRepCode;

    public Nullable<int> SignupRepCode
    {
        get { return m_intSignupRepCode; }
        protected set { m_intSignupRepCode = value; }
    }

    private string m_strBccEmail;
    private string m_strSalesEmail;
    private string m_strTelawayPhone;
    private string m_strCustomerServicEmail;
    private string m_strConfirmationEmail;
    private string m_strMainEmail;
    private string m_strInfoEmail;
    private string m_strSiteSourceCountry;

    #endregion
    #region Database Properties
    public Nullable<int> ParentOnlineOrderCode
    {
        get { return m_intParentOnlineOrderCode; }
        set { m_intParentOnlineOrderCode = value; }
    }

    public Nullable<bool> bSubmitOrder
    {
        get { return m_bSubmitOrder; }
        protected set { m_bSubmitOrder = value; }
    }

    public Nullable<int> OnlineOrderCode
    {
        get { return m_intOnlineOrderCode; }
        protected set { m_intOnlineOrderCode = value; }
    }

    public string GroupMemberID
    {
        get { return m_strGroupMemberID; }
        protected set { m_strGroupMemberID = value; }
    }


    public string SessionID
    {
        get { return m_strSessionID; }
        protected set { m_strSessionID = value; }
    }
    public Nullable<int> PhonesRequired
    {
        get { return m_intPhonesRequired; }
        protected set { m_intPhonesRequired = value; }
    }

    public Nullable<int> Shotef
    {
        get { return m_intShotef; }
        protected set { m_intShotef = value; }
    }

    public int TZ
    {
        get { return m_intTZ; }
        protected set { m_intTZ = value; }
    }


    public Nullable<bool> IsEquipmentSNS
    {
        get { return m_IsEquipmentSNS; }
        protected set { m_IsEquipmentSNS = value; }
    }
    public Nullable<int> KNTRequired
    {
        get { return m_intKNTRequired; }
        protected set { m_intKNTRequired = value; }
    }
    public string SignupSourceID
    {
        get { return m_strSignupSourceID; }
        protected set { m_strSignupSourceID = value; }
    }
    public Nullable<int> LinkTypeCode
    {
        get { return m_intLinkTypeCode; }
        protected set { m_intLinkTypeCode = value; }
    }
    public Nullable<int> ProductId
    {
        get { return m_intProductId; }
        protected set { m_intProductId = value; }
    }

    public string FirstName
    {
        get { return m_strFirstName; }
        set { m_strFirstName = value; }
    }
    public string LastName
    {
        get { return m_strLastName; }
        set { m_strLastName = value; }
    }
    public float Forex
    {
        get { return m_floatForex; }
        protected set { m_floatForex = value; }
    }

    public string Password
    {
        get { return m_strPassword; }
        protected set { m_strPassword = value; }
    }
    public string CCNumber
    {
        get { return m_strCCNumber; }
        protected set { m_strCCNumber = value; }
    }
    public string CCType
    {
        get { return m_strCCType; }
        protected set { m_strCCType = value; }
    }
    public string CCExpiry
    {
        get { return m_strCCExpiry; }
        protected set { m_strCCExpiry = value; }
    }

    public string CCIdNumber
    {
        get { return m_strCCIdNumber; }
        protected set { m_strCCIdNumber = value; }
    }
    public string CCOwner
    {
        get { return m_strCCOwner; }
        protected set { m_strCCOwner = value; }
    }


    public Nullable<int> BankNumber
    {
        get { return m_intBankNumber; }
        protected set { m_intBankNumber = value; }
    }

    public Nullable<int> BranchNumber
    {
        get { return m_intBranchNumber; }
        protected set { m_intBranchNumber = value; }
    }
    public Nullable<int> BillNumber
    {
        get { return m_intBillNumber; }
        protected set { m_intBillNumber = value; }
    }

    public string CustomerType
    {
        get { return m_strCustomerType; }
        protected set { m_strCustomerType = value; }
    }


    public int Company
    {
        get { return m_intCompany; }
        protected set { m_intCompany = value; }
    }

    public string SetupFeeText
    {
        get { return m_strSetupFeeText; }
        protected set { m_strSetupFeeText = value; }
    }
    public Nullable<bool> IsSim
    {
        get { return m_bIsSim; }
        protected set { m_bIsSim = value; }
    }
    public string EquipmentName
    {
        get { return m_strEquipmentName; }
        protected set { m_strEquipmentName = value; }
    }

    public Nullable<bool> IsKosher
    {
        get { return m_bIsKosher; }
        protected set { m_bIsKosher = value; }
    }

    public string CallPackageName
    {
        get { return m_strCallPackageName; }
        protected set { m_strCallPackageName = value; }
    }

    public string Deposit
    {
        get { return m_strDeposit; }
        protected set { m_strDeposit = value; }
    }


    public string TermsName
    {
        get { return m_strTermsName; }
        protected set { m_strTermsName = value; }
    }


    public string KNTName
    {
        get { return m_strKNTName; }
        protected set { m_strKNTName = value; }
    }

    public string SMSPackageName
    {
        get { return m_strSMSPackageName; }
        protected set { m_strSMSPackageName = value; }
    }
    public string PlanName
    {
        get { return m_strPlanName; }
        protected set { m_strPlanName = value; }
    }
    public string ParentLink
    {
        get { return m_strParentLink; }
        protected set { m_strParentLink = value; }
    }
    public string SubLink
    {
        get { return m_strSubLink; }
        protected set { m_strSubLink = value; }
    }
    public Nullable<int> PlanCode
    {
        get { return m_intPlanCode; }
        protected set { m_intPlanCode = value; }
    }
    public string Tag
    {
        get { return m_strTag; }
        set { m_strTag = value; }
    }
    public string AccessoryIdAndQuantity
    {
        get { return m_strAccessoryIdAndQuantity; }
        set { m_strAccessoryIdAndQuantity = value; }
    } 
    public string GroupName
    {
        get { return m_strGroupName; }
        protected set { m_strGroupName = value; }
    }
    public string ClientIP
    {
        get { return m_strClientIP; }
        protected set { m_strClientIP = value; }
    }
    public string PWD
    {
        get { return m_strPWD; }
        protected set { m_strPWD = value; }
    }
    public string Hint
    {
        get { return m_strHint; }
        protected set { m_strHint = value; }
    }

    public Nullable<int> AgentCode
    {
        get { return m_intAgentCode; }
        protected set { m_intAgentCode = value; }
    }
    public Nullable<int> SubAgentCode
    {
        get { return m_intSubAgentCode; }
        protected set { m_intSubAgentCode = value; }
    }
    public Nullable<int> CompanyCode
    {
        get { return m_intCompanyCode; }
        protected set { m_intCompanyCode = value; }
    }
    public Nullable<int> BaseCode
    {
        get { return m_intBaseCode; }
        protected set { m_intBaseCode = value; }
    }
    public List<SIMDetails> SimDetails
    {
        get { return m_lstSimDetails; }
        set { m_lstSimDetails = value; }
    }

    public Nullable<int> EquipmentCode
    {
        get { return m_intEquipmentCode; }
        protected set { m_intEquipmentCode = value; }
    }

    public Nullable<int> EquipmentModel
    {
        get { return m_intEquipmentModel; }
        protected set { m_intEquipmentModel = value; }
    }

    public string EquipmentNotes
    {
        get { return m_strEquipmentNotes; }
        protected set { m_strEquipmentNotes = value; }
    }
    public string BaseNotes
    {
        get { return m_strBaseNotes; }
        protected set { m_strBaseNotes = value; }
    }

    public Nullable<bool> Insurance
    {
        get { return m_bolInsurance; }
        protected set { m_bolInsurance = value; }
    }
    public Nullable<bool> PurchaseEquipment
    {
        get { return m_bolPurchaseEquipment; }
        protected set { m_bolPurchaseEquipment = value; }
    }
    public Nullable<bool> KITD
    {
        get { return m_bolKITD; }
        protected set { m_bolKITD = value; }
    }
    public Nullable<bool> KITD_BLOCK_ID
    {
        get { return m_bolKITD_BLOCK_ID; }
        protected set { m_bolKITD_BLOCK_ID = value; }
    }

    public Nullable<int> DataPackageId
    {
        get { return m_iDataPackageId; }
        protected set { m_iDataPackageId = value; }
    }

    public string DataPackageName
    {
        get { return m_strDataPackageName; }
        protected set { m_strDataPackageName = value; }
    }
    public string DataPackgeSize
    {
        get { return m_strDataPackgeSize; }
        protected set { m_strDataPackgeSize = value; }
    }

    public Nullable<int> DataPackageCode
    {
        get { return m_intDataPackageCode; }
        protected set { m_intDataPackageCode = value; }
    }
    public Nullable<int> KITD_PlanCode
    {
        get { return m_intKITD_PlanCode; }
        protected set { m_intKITD_PlanCode = value; }
    }
    public Nullable<bool> SurfAndSave
    {
        get { return m_bolSurftAndSave; }
        protected set { m_bolSurftAndSave = value; }
    }
    public Nullable<bool> CreditEquipmentPurchase
    {
        get { return m_bolCreditEquipmentPurchase; }
        protected set { m_bolCreditEquipmentPurchase = value; }
    }
    public string ShipName
    {
        get { return m_strShipName; }
        protected set { m_strShipName = value; }
    }
    public string ShippingName
    {
        get { return m_strShippingName; }
        protected set { m_strShippingName = value; }
    }
    public string ShipStreet
    {
        get { return m_strShipStreet; }
        protected set { m_strShipStreet = value; }
    }
    public string ShipCity
    {
        get { return m_strShipCity; }
        protected set { m_strShipCity = value; }
    }
    public string ShipState
    {
        get { return m_strShipState; }
        set { m_strShipState = value; }
    }
    public string ShipPostalCode
    {
        get { return m_strShipPostalCode; }
        protected set { m_strShipPostalCode = value; }
    }
    public string ShipPhone
    {
        get { return m_strShipPhone; }
        protected set { m_strShipPhone = value; }
    }
    public string ShipEmail
    {
        get { return m_strShipEmail; }
        protected set { m_strShipEmail = value; }
    }
    public string ShipCountry
    {
        get { return m_strShipCountry; }
        protected set { m_strShipCountry = value; }
    }
    public string ShipMethod
    {
        get { return m_strShipMethod; }
        protected set { m_strShipMethod = value; }
    }
    public Nullable<bool> ShipCommercial
    {
        get { return m_bolShipCommercial; }
        protected set { m_bolShipCommercial = value; }
    }



    public Nullable<int> AffiliateCounter
    {
        get { return m_intAffiliateCounter; }
        protected set { m_intAffiliateCounter = value; }
    }
    public Nullable<int> KNTCountryCode
    {
        get { return m_intKNTCountryCode; }
        protected set { m_intKNTCountryCode = value; }
    }

    public Nullable<decimal> ShipFee
    {
        get { return m_decShipFee; }
        protected set { m_decShipFee = value; }
    }
    public Nullable<System.DateTime> ShipDate
    {
        get { return m_datShipDate; }
        protected set { m_datShipDate = value; }
    }
    public Nullable<System.DateTime> DepartureDate
    {
        get { return m_datDepartureDate; }
        protected set { m_datDepartureDate = value; }
    }
    public Nullable<System.DateTime> StartDate
    {
        get { return m_datStartDate; }
        protected set { m_datStartDate = value; }
    }
    public Nullable<System.DateTime> EndDate
    {
        get { return m_datEndDate; }
        protected set { m_datEndDate = value; }
    }

    public Nullable<System.DateTime> UserEndDate
    {
        get { return m_datUserEndDate; }
        protected set { m_datUserEndDate = value; }
    }
    public string UserName
    {
        get { return m_strUserName; }
        protected set { m_strUserName = value; }
    }
    public string UserStreet
    {
        get { return m_strUserStreet; }
        protected set { m_strUserStreet = value; }
    }
    public string UserCity
    {
        get { return m_strUserCity; }
        protected set { m_strUserCity = value; }
    }
    public string ClientFirstName
    {
        get { return m_strClientFirstName; }
        protected set { m_strClientFirstName = value; }
    }
    public string ClientLastName
    {
        get { return m_strClientLastName; }
        protected set { m_strClientLastName = value; }
    }
    public string ClientStreet
    {
        get { return m_strClientStreet; }
        protected set { m_strClientStreet = value; }
    }
    public string ClientCity
    {
        get { return m_strClientCity; }
        protected set { m_strClientCity = value; }
    }
    public string ClientState
    {
        get { return m_strClientState; }
        protected set { m_strClientState = value; }
    }
    public string ClientCountry
    {
        get { return m_strClientCountry; }
        protected set { m_strClientCountry = value; }
    }
    public string ClientZip
    {
        get { return m_strClientZip; }
        protected set { m_strClientZip = value; }
    }
    public string ClientHomePhone1
    {
        get { return m_strClientHomePhone1; }
        protected set { m_strClientHomePhone1 = value; }
    }


    public string PaymentMethod
    {
        get { return m_strPaymentMethod; }
        protected set { m_strPaymentMethod = value; }
    }
    public string ClientHomePhone2
    {
        get { return m_strClientHomePhone2; }
        protected set { m_strClientHomePhone2 = value; }
    }
    public string ClientFax
    {
        get { return m_strClientFax; }
        protected set { m_strClientFax = value; }
    }
    public string ClientMobile
    {
        get { return m_strClientMobile; }
        protected set { m_strClientMobile = value; }
    }
    public string ClientEmail
    {
        get { return m_strClientEmail; }
        protected set { m_strClientEmail = value; }
    }
    public string CCNum
    {
        get { return m_strCCNum; }
        protected set { m_strCCNum = value; }
    }
    public Nullable<System.DateTime> CCExpDate
    {
        get { return m_datCCExpDate; }
        protected set { m_datCCExpDate = value; }
    }
    public string CCTitle
    {
        get { return m_strCCTitle; }
        protected set { m_strCCTitle = value; }
    }
    public string CCCode
    {
        get { return m_strCCCode; }
        protected set { m_strCCCode = value; }
    }
    public Nullable<int> LanguageCode
    {
        get { return m_intLanguageCode; }
        protected set { m_intLanguageCode = value; }
    }
    public Nullable<bool> Special
    {
        get { return m_bolSpecial; }
        protected set { m_bolSpecial = value; }
    }
    public string CustomerComment
    {
        get { return m_strCustomerComment; }
        protected set { m_strCustomerComment = value; }
    }


    public Nullable<bool> bitCallPackageOverageProtection
    {
        get { return m_bitCallPackageOverageProtection; }
        protected set { m_bitCallPackageOverageProtection = value; }
    }
    public Nullable<int> CallPackageCode
    {
        get { return m_intCallPackageCode; }
        protected set { m_intCallPackageCode = value; }
    }

    public Nullable<int> TermsCode
    {
        get { return m_intTermsCode; }
        protected set { m_intTermsCode = value; }
    }
    public Nullable<int> SMSPackageCode
    {
        get { return m_intSMSPackageCode; }
        protected set { m_intSMSPackageCode = value; }
    }

    public Nullable<int> SMSPackageCounter
    {
        get { return m_intSMSPackageCounter; }
        protected set { m_intSMSPackageCounter = value; }
    }

    public string CouponCode
    {
        get { return m_strCouponCode; }
        protected set { m_strCouponCode = value; }
    }

    public string PayPalTransactionId
    {
        get { return m_strPayPalTransactionId; }
        protected set { m_strPayPalTransactionId = value; }
    }

    public Nullable<decimal> PayPalAmountCharge
    {
        get { return m_decPayPalAmountCharge; }
        protected set { m_decPayPalAmountCharge = value; }
    }

    public Nullable<int> OrderCode
    {
        get { return m_intOrderCode; }
        protected set { m_intOrderCode = value; }
    }
    public Nullable<DateTime> OrderCodeAddedOn
    {
        get { return m_datOrderCodeAddedOn; }
        protected set { m_datOrderCodeAddedOn = value; }
    }
    public Nullable<int> RentalCode
    {
        get { return m_intRentalCode; }
        protected set { m_intRentalCode = value; }
    }
    public Nullable<DateTime> RentalCodeAddedOn
    {
        get { return m_datRentalCodeAddedOn; }
        protected set { m_datRentalCodeAddedOn = value; }
    }

    public string BccEmail
    {
        get { return m_strBccEmail; }
        set { m_strBccEmail = value; }
    }
    public string SalesEmail
    {
        get { return m_strSalesEmail; }
        set { m_strSalesEmail = value; }
    }
    public string TelawayPhone
    {
        get { return m_strTelawayPhone; }
        set { m_strTelawayPhone = value; }
    }
    public string InfoEmail
    {
        get { return m_strInfoEmail; }
        set { m_strInfoEmail = value; }
    }
    public string CustomerServicEmail
    {
        get { return m_strCustomerServicEmail; }
        set { m_strCustomerServicEmail = value; }
    }
    public string ConfirmationEmail
    {
        get { return m_strConfirmationEmail; }
        set { m_strConfirmationEmail = value; }
    }

    public string MainEmail
    {
        get { return m_strMainEmail; }
        set { m_strMainEmail = value; }
    }
    public string SiteSourceCountry
    {
        get { return m_strSiteSourceCountry; }
        set { m_strSiteSourceCountry = value; }
    }
    #endregion
    #region General Properties
    private string m_strErrorMessage;
    public string ErrorMessage
    {
        get { return m_strErrorMessage; }
        protected set { m_strErrorMessage = value; }
    }
    #endregion
    #region Database Commands
    public int Add()
    {
        SqlCommand adCmd = new SqlCommand();
        SqlParameter prm = default(SqlParameter);
        int intReturn = 0;

        adCmd.CommandText = "sp_DBOR_Online_AddOrder";
        adCmd.CommandType = CommandType.StoredProcedure;

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strSignupSourceID";
            prm.SqlDbType = SqlDbType.Char;
            prm.Size = 5;
            if (string.IsNullOrEmpty(this.SignupSourceID) == false)
            {
                prm.Value = this.SignupSourceID;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intLinkTypeCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.LinkTypeCode.HasValue == true)
            {
                prm.Value = this.LinkTypeCode.Value;
            }
        }

        adCmd.Parameters.Add(prm);


        prm = new SqlParameter();
        {
            prm.ParameterName = "@intSignupRepCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.SignupRepCode.HasValue)
            {
                prm.Value = this.SignupRepCode;
            }
        }

        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strParentLink";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 15;
            if (string.IsNullOrEmpty(this.ParentLink) == false)
            {
                prm.Value = this.ParentLink;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strSubLink";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 15;
            if (string.IsNullOrEmpty(this.SubLink) == false)
            {
                prm.Value = this.SubLink;
            }
        }
        adCmd.Parameters.Add(prm);

        //strGroupMemberID
        prm = new SqlParameter();
        {
            prm.ParameterName = "@strGroupMemberID";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.GroupMemberID) == false)
            {
                prm.Value = this.GroupMemberID;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strTag";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 300;
            if (string.IsNullOrEmpty(this.Tag) == false)
            {
                prm.Value = this.Tag;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strAccessoryList";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 75;
            if (string.IsNullOrEmpty(this.AccessoryIdAndQuantity) == false)
            {
                prm.Value = this.AccessoryIdAndQuantity;
            }
        }
        adCmd.Parameters.Add(prm);
        prm = new SqlParameter();
        {
            prm.ParameterName = "@strSessionID";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 20;
            if (string.IsNullOrEmpty(this.SessionID) == false)
            {
                prm.Value = this.SessionID;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientIP";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 20;
            if (string.IsNullOrEmpty(this.ClientIP) == false)
            {
                prm.Value = this.ClientIP;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strPWD";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.PWD) == false)
            {
                prm.Value = this.PWD;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strHint";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.Hint) == false)
            {
                prm.Value = this.Hint;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intPlanCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.PlanCode.HasValue == true)
            {
                prm.Value = this.PlanCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intAgentCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.AgentCode.HasValue == true)
            {
                prm.Value = this.AgentCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intSubAgentCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.SubAgentCode.HasValue)
            {
                prm.Value = this.SubAgentCode;
            }
        }
        adCmd.Parameters.Add(prm);
        ////
        prm = new SqlParameter();
        {
            prm.ParameterName = "@intCompanyCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.CompanyCode.HasValue == true)
            {
                prm.Value = this.CompanyCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intBaseCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.BaseCode.HasValue == true)
            {
                prm.Value = this.BaseCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intEquipmentCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.EquipmentCode.HasValue == true)
            {
                prm.Value = this.EquipmentCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitInsurance";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.Insurance.HasValue == true)
            {
                prm.Value = this.Insurance.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitPurchaseEquipment";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.PurchaseEquipment.HasValue == true)
            {
                prm.Value = this.PurchaseEquipment.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipName";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.ShipName) == false)
            {
                prm.Value = this.ShipName;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipStreet";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.ShipStreet) == false)
            {
                prm.Value = this.ShipStreet;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipCity";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.ShipCity) == false)
            {
                prm.Value = this.ShipCity;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipState";
            prm.SqlDbType = SqlDbType.Char;
            prm.Size = 2;
            if (string.IsNullOrEmpty(this.ShipState) == false)
            {
                prm.Value = this.ShipState;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipPostalCode";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.ShipPostalCode) == false)
            {
                prm.Value = this.ShipPostalCode;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipPhone";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.ShipPhone) == false)
            {
                prm.Value = this.ShipPhone;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipEmail";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.ShipEmail) == false)
            {
                prm.Value = this.ShipEmail;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipCountry";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.ShipCountry) == false)
            {
                prm.Value = this.ShipCountry;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strShipMethod";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 20;
            if (string.IsNullOrEmpty(this.ShipMethod) == false)
            {
                prm.Value = this.ShipMethod;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitShipCommercial";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.ShipCommercial.HasValue == true)
            {
                prm.Value = this.ShipCommercial.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@curShipFee";
            prm.SqlDbType = SqlDbType.Money;
            if (this.ShipFee.HasValue == true)
            {
                prm.Value = this.ShipFee.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datShipDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.ShipDate.HasValue == true)
            {
                prm.Value = this.ShipDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datDepartureDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.DepartureDate.HasValue == true)
            {
                prm.Value = this.DepartureDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datStartDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.StartDate.HasValue == true)
            {
                prm.Value = this.StartDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datEndDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.EndDate.HasValue == true)
            {
                prm.Value = this.EndDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datUserEndDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.UserEndDate.HasValue == true)
            {
                prm.Value = this.UserEndDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strUserName";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.UserName) == false)
            {
                prm.Value = this.UserName;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strUserStreet";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 30;
            if (string.IsNullOrEmpty(this.UserStreet) == false)
            {
                prm.Value = this.UserStreet;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strUserCity";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 20;
            if (string.IsNullOrEmpty(this.UserCity) == false)
            {
                prm.Value = this.UserCity;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientFirstName";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.ClientFirstName) == false)
            {
                prm.Value = this.ClientFirstName;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientLastName";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.ClientLastName) == false)
            {
                prm.Value = this.ClientLastName;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientStreet";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 30;
            if (string.IsNullOrEmpty(this.ClientStreet) == false)
            {
                prm.Value = this.ClientStreet;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientCity";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 30;
            if (string.IsNullOrEmpty(this.ClientCity) == false)
            {
                prm.Value = this.ClientCity;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientState";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.ClientState) == false)
            {
                prm.Value = this.ClientState;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientCountry";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.ClientCountry) == false)
            {
                prm.Value = this.ClientCountry;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientZip";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 10;
            if (string.IsNullOrEmpty(this.ClientZip) == false)
            {
                prm.Value = this.ClientZip;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientHomePhone1";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 15;
            if (string.IsNullOrEmpty(this.ClientHomePhone1) == false)
            {
                prm.Value = this.ClientHomePhone1;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientHomePhone2";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.ClientHomePhone2) == false)
            {
                prm.Value = this.ClientHomePhone2;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientFax";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.ClientFax) == false)
            {
                prm.Value = this.ClientFax;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientMobile";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.ClientMobile) == false)
            {
                prm.Value = this.ClientMobile;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strClientEmail";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 75;
            if (string.IsNullOrEmpty(this.ClientEmail) == false)
            {
                prm.Value = this.ClientEmail;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strCCNum";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 50;
            if (string.IsNullOrEmpty(this.CCNum) == false)
            {
                prm.Value = this.CCNum;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@datCCExpDate";
            prm.SqlDbType = SqlDbType.DateTime;
            if (this.CCExpDate.HasValue == true)
            {
                prm.Value = this.CCExpDate.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strCCTitle";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.CCTitle) == false)
            {
                prm.Value = this.CCTitle;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strCCCode";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 5;
            if (string.IsNullOrEmpty(this.CCCode) == false)
            {
                prm.Value = this.CCCode;
            }
        }
        adCmd.Parameters.Add(prm);

        //new PayPalTransactionId 
        prm = new SqlParameter();
        {
            prm.ParameterName = "@strPayPalTransID";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.PayPalTransactionId) == false)
            {
                prm.Value = this.PayPalTransactionId;
            }
        }
        adCmd.Parameters.Add(prm);

        //new PayPalSeller
        prm = new SqlParameter();
        {
            prm.ParameterName = "@strPayPalSellerID";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.PayPalTransactionId) == false)
            {
                prm.Value = "TELAWAY";
            }

        }
        adCmd.Parameters.Add(prm);

        //new PayPalAmount
        prm = new SqlParameter();
        {
            prm.ParameterName = "@curPayPalAmount";
            prm.SqlDbType = SqlDbType.Money;
            if (this.PayPalAmountCharge.HasValue == true)
            {
                prm.Value = this.PayPalAmountCharge.Value;
            }
            else
                prm.Value = 0;
        }
        adCmd.Parameters.Add(prm);
        prm = new SqlParameter();
        {
            prm.ParameterName = "@intLanguageCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.LanguageCode.HasValue == true)
            {
                prm.Value = this.LanguageCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitSpecial";
            prm.SqlDbType = SqlDbType.BigInt;
            if (this.Special.HasValue == true)
            {
                prm.Value = this.Special.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitKITD";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.KITD.HasValue == true)
            {
                prm.Value = this.KITD.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitKITD_BLOCK_ID";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.KITD_BLOCK_ID.HasValue == true)
            {
                prm.Value = this.KITD_BLOCK_ID.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intKITD_PlanCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.KITD_PlanCode.HasValue == true)
            {
                prm.Value = this.KITD_PlanCode.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitSurfAndSave";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.SurfAndSave.HasValue == true)
            {
                prm.Value = this.SurfAndSave.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitCreditEquipmentPurchase";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.CreditEquipmentPurchase.HasValue == true)
            {
                prm.Value = this.CreditEquipmentPurchase.Value;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strCustomerComment";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 300;
            if (string.IsNullOrEmpty(this.CustomerComment) == false)
            {
                prm.Value = this.CustomerComment;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intParentOnlineOrderCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.ParentOnlineOrderCode.HasValue)
            {
                prm.Value = this.ParentOnlineOrderCode;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intCallPackageCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.CallPackageCode.HasValue)
            {
                prm.Value = this.CallPackageCode;
            }
        }
        adCmd.Parameters.Add(prm);

        //bitCallPackageOverageProtection

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitCallPackageOverageProtection";
            prm.SqlDbType = SqlDbType.Bit;
            if (this.bitCallPackageOverageProtection.HasValue)
            {
                prm.Value = this.bitCallPackageOverageProtection;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intSMSPackageCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.SMSPackageCode.HasValue)
            {
                prm.Value = this.SMSPackageCode;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intExtendedDataPackageCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.DataPackageCode.HasValue)
            {
                prm.Value = this.DataPackageCode;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@strCouponCode";
            prm.SqlDbType = SqlDbType.VarChar;
            prm.Size = 25;
            if (string.IsNullOrEmpty(this.CouponCode) == false)
            {
                prm.Value = this.CouponCode;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intReferrerCounter";
            prm.SqlDbType = SqlDbType.Int;
            if (this.AffiliateCounter.HasValue)
            {
                prm.Value = this.AffiliateCounter;
            }
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@bitUSA_SIM_Order";
            prm.SqlDbType = SqlDbType.Bit;
            prm.Value = true;
            //if (this.bitUSA_SIM_Order.HasValue == true)
            //{
            //   prm.Value = this.bitUSA_SIM_Order.Value;
            //}
        }
        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intKNTCountryCode";
            prm.SqlDbType = SqlDbType.Int;
            if (this.KNTCountryCode.HasValue)
            {
                prm.Value = this.KNTCountryCode;
            }
        }

        adCmd.Parameters.Add(prm);

        prm = new SqlParameter();
        {
            prm.ParameterName = "@intOnlineOrderCode";
            prm.SqlDbType = SqlDbType.Int;
            prm.Direction = ParameterDirection.Output;
        }
        adCmd.Parameters.Add(prm);

        using (SqlConnection objCnn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DBOR_Online"].ToString()))
        {
            objCnn.Open();
            SqlTransaction trn = objCnn.BeginTransaction();
            {
                adCmd.Connection = objCnn;
                adCmd.Transaction = trn;
                try
                {
                    adCmd.ExecuteNonQuery();
                    trn.Commit();
                    intReturn = (int)adCmd.Parameters["@intOnlineOrderCode"].Value;
                    this.OnlineOrderCode = intReturn;
                    if (RecordAdded != null)
                    {
                        RecordAdded(intReturn);
                    }
                }
                catch (SqlException ex)
                {
                    trn.Rollback();
                    this.ErrorMessage = ex.Errors[0].Message;
                    for (int i = 0; i <= ex.Errors.Count - 1; i++)
                    {
                        if (ErrorAddingRecord != null)
                        {
                            ErrorAddingRecord(ex.Errors[i].Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                    this.ErrorMessage = ex.ToString();
                    if (ErrorAddingRecord != null)
                    {
                        ErrorAddingRecord(ex.ToString());
                    }
                }
                finally
                {
                    adCmd.Dispose();
                }
            }
        }
        return intReturn;
    }
    #endregion
    #region Constructors
    public OnlineOrderObj()
    {

    }
    #endregion

    public void SetOrderSubmited()
    {
        this.SessionID = null;
        this.ClientIP = null;
        this.AgentCode = null;
        this.bSubmitOrder = true;
        //this.CCNum = "";
        //this.CCTitle = "";
        //this.CCExpDate = null;  
    }

    public void SetOrderFullSubmited()
    {
        this.SessionID = null;
        this.ClientIP = null;
        this.AgentCode = null;
        this.bSubmitOrder = true;
        this.CCNum = "";
        this.CCTitle = "";
        this.CCExpDate = null;
    }


    public void setNoKnt()
    {
        this.KITD_PlanCode = -1;
        this.KITD = false;

    }

    public void LoadPayPalData(string sPayPalTransactionId, decimal dPayPalAmountCharge)
    {
        this.PayPalTransactionId = sPayPalTransactionId;
        this.PayPalAmountCharge = dPayPalAmountCharge;
    }
    //fill the real object from the temp object
    public void LoadGeneralData(string sSignupSourceID, string sParentLink, string sSubLink, int iLinkTypeCode, int iAgentCode,int? iSubAgentCode, string sGroupName, int iCompanyCode, string sSiteSourceCountry)
    {
        this.AffiliateCounter = null;
        // this.PayPalTransactionId = "";
        // this.PayPalAmountCharge = 0; 

        this.KNTCountryCode = null;

        int iCountryCoode = dbUtility.GetVirtualCountryCode(sParentLink.Trim(), sSubLink.Trim());
        if (iCountryCoode > 0)
            this.KNTCountryCode = iCountryCoode;


        this.SignupSourceID = sSignupSourceID;
        this.SessionID = SessionUtility.GetSessionId();
        this.ClientIP = HttpContext.Current.Request.UserHostAddress.ToString();

        this.ParentLink = sParentLink;
        this.SubLink = sSubLink;
        this.LinkTypeCode = iLinkTypeCode;
        this.LanguageCode = 1;//English

        if (sParentLink == "ilp")
            this.LanguageCode = 2;//French

        this.AgentCode = iAgentCode;
        this.SubAgentCode = iSubAgentCode;
        this.GroupName = sGroupName;
        this.CompanyCode = iCompanyCode;

        this.SignupRepCode = -1;
        this.SiteSourceCountry = sSiteSourceCountry;
    }

    public void LoadData_Page1_1(int iPhonesRequired, List<SIMDetails> sSimDetails, string sEquipmentNotes, bool bIsKosher, bool bIsSim, bool bIsEquipmentSNS)
    {
        this.SimDetails = sSimDetails;
        this.PhonesRequired = iPhonesRequired;
        //this.EquipmentCode = iEquipmentCode;
        // this.EquipmentModel = iEquipmentModel;
        this.EquipmentNotes = sEquipmentNotes;
        //this.EquipmentName = sEquipmentName;
        this.IsKosher = bIsKosher;
        this.IsSim = bIsSim;
        this.IsEquipmentSNS = bIsEquipmentSNS;
    }

    //public void LoadData_SimInfo( int? iEquipmentCode,int? iEquipmentModel,  string sEquipmentName, string iIMEICode)

    public void LoadOrderCoupon(int packageNum)
    {
        if (SessionUtility.GetValue("Upsale") != "")
            SessionUtility.AddValue("CouponCode", dbUtility.ExecScalar_DBOR("SELECT sc.CouponCode FROM tblUpSale up inner join tblSignupCoupons sc on up.counter = sc.counter WHERE up.CouponCode ='" + SessionUtility.GetValue("Upsale") +"'"));

        if (SessionUtility.GetValue("CouponCode") != "")
        {
            bool bOnlyToSecondOrder = dbUtility.ExecBoolScalar_DBOR("SELECT OnlyToSecondOrder  FROM tblSignupCoupons where CouponCode='" + SessionUtility.GetValue("CouponCode").Trim() + "'");

            if (bOnlyToSecondOrder)
            {
                if (this.PhonesRequired > 1)
                {

                    if (bOnlyToSecondOrder)
                    {
                        if (packageNum != 2)
                            this.CouponCode = "";
                        else
                            this.CouponCode = SessionUtility.GetValue("CouponCode");
                    }
                }
                else
                    this.CouponCode = "";
            }//if bOnlyToSecondOrder


        }//if CouponCode

    }
    public void LoadData_SimInfo(SIMDetails sd, int packageNum)
    {
        try
        {
            this.EquipmentCode = sd.EquipmentCode;
            this.EquipmentModel = sd.EquipmentModel;
            this.EquipmentName = sd.EquipmentName;
            this.ClientHomePhone2 = sd.PhoneIMEI; //IMEI code

            this.DataPackageId = sd.ExtendedDataPackageCode;
            this.DataPackageCode = sd.ExtendedDataPackageCode;
            this.ShipFee = sd.ShipFee;

            this.KNTCountryCode = sd.KNTCode;

            // for multipul sims need to set order price, split the total amount per order
            if (this.PayPalAmountCharge != 0)
            {
			
			    decimal? amountCharged = packageNum == 1 ? 
                    sd.PaypalAmount + sd.ShipFee + sd.SimPrice + sd.PaypalDiscount + SessionUtility.GetDecimalValue("ItemTotalAcc") :
										sd.PaypalAmount + sd.SimPrice + sd.PaypalDiscount;
                this.PayPalAmountCharge = amountCharged;
                this.ShipFee = sd.ShipFee;
            }
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("LoadData_SimInfo:" + ex.Message);
        }
    }
    public void LoadData_SiteDetails(string bccEmail, string salesEmail, string cusServiceEmail, string confirmationEmail, string mainEmail, string infoEmail, string sPhone)
    {
        this.BccEmail = bccEmail;
        this.SalesEmail = salesEmail;
        this.CustomerServicEmail = cusServiceEmail;
        this.ConfirmationEmail = confirmationEmail;
        this.MainEmail = mainEmail;
        this.InfoEmail = infoEmail;
        this.TelawayPhone = sPhone;
    }

    public void LoadData_Page1_2(int iPlanCode, int iProductId, int iCallPackageCode, string sCallPackageName, int iSMSPackageCounter, int iSMSPackageCode, string sSMSPackageName)
    {
        this.PlanCode = iPlanCode;
        this.ProductId = iProductId;
        this.CallPackageCode = iCallPackageCode;
        this.CallPackageName = sCallPackageName;
        this.SMSPackageCode = iSMSPackageCode;
        this.SMSPackageCounter = iSMSPackageCounter;
        this.SMSPackageName = sSMSPackageName;
    }

    public void LoadData_Page1_3(int iDataPackageId, int iDataPackageCode, string sDataPackageName, string sDataPackageSize, string sKNTName, int iKITD_PlanCode, int iKNTRequired, bool bKITD)
    {
        this.DataPackageId = iDataPackageId;
        this.DataPackageCode = iDataPackageCode;
        this.DataPackageName = sDataPackageName;
        this.DataPackgeSize = sDataPackageSize;
        this.KNTName = sKNTName;
        this.KITD_PlanCode = iKITD_PlanCode;
        this.KNTRequired = iKNTRequired;
        this.KITD = bKITD;
    }

    public void LoadData_Page1_4(int iTermsCode, string sTermsName, string sTag, bool bInsurance, bool bSurfAndSave, bool bSpecial, bool bBitCallPackageOverageProtection)
    {
        this.TermsCode = iTermsCode;
        this.TermsName = sTermsName;
        this.Tag = sTag;
        this.AccessoryIdAndQuantity = "";
        this.Insurance = bInsurance;
        this.SurfAndSave = bSurfAndSave;
        this.Special = bSpecial;
        this.bitCallPackageOverageProtection = bBitCallPackageOverageProtection;
    }
    public void LoadData_Page1_4(int iTermsCode, string sTermsName, string sTag, string sAccessory, bool bInsurance, bool bSurfAndSave, bool bSpecial, bool bBitCallPackageOverageProtection)
    {
        this.TermsCode = iTermsCode;
        this.TermsName = sTermsName;
        this.Tag = sTag;
        this.AccessoryIdAndQuantity = sAccessory;
        this.Insurance = bInsurance;
        this.SurfAndSave = bSurfAndSave;
        this.Special = bSpecial;
        this.bitCallPackageOverageProtection = bBitCallPackageOverageProtection;

    }

    public void LoadData_Page1_5(int iPhonesRequired, int iKNTRequired)
    {
        this.PhonesRequired = iPhonesRequired;
        this.KNTRequired = iKNTRequired;
    }


    public void UpdateClientEmail(string sClientEmail)
    {
        this.ClientEmail = sClientEmail;
    }
    public void LoadData_Page2(string sUserName, string sUserStreet, string sUserCity, string sPWD, string sClientHomePhone1, string sClientFax, string sClientMobile, string sClientEmail, string sClientStreet, string sClientCity, string sClientCountry, string sClientState, string sClientZip, string sTag, DateTime dStartDate, DateTime dEndDate)
    {
        this.UserName = sUserName;
        this.UserStreet = sUserStreet;
        this.UserCity = sUserCity;
        this.PWD = sPWD;
        this.ClientHomePhone1 = sClientHomePhone1;
        //this.ClientHomePhone2 = sClientHomePhone2;
        this.ClientFax = sClientFax;
        this.ClientMobile = sClientMobile;
        this.ClientEmail = sClientEmail;
        this.ClientStreet = sClientStreet;
        this.ClientCity = sClientCity;
        this.ClientCountry = sClientCountry;
        this.ClientState = sClientState;
        this.ClientZip = sClientZip;
        this.Tag = sTag;
        this.StartDate = dStartDate;
        this.EndDate = dEndDate;
        this.UserEndDate = null;
    }

    public void LoadData_Page3_1(bool bPurchaseEquipment, string sShipName, string sShipStreet, string sShipCity, string sShipState, string sShipPostalCode, string sShipPhone, string sShipCountry, int iBaseCode)
    {
        this.PurchaseEquipment = bPurchaseEquipment;
        this.ShipName = sShipName;
        this.ShipStreet = sShipStreet;
        this.ShipCity = sShipCity;
        this.ShipState = sShipState;
        this.ShipPostalCode = sShipPostalCode;
        this.ShipPhone = sShipPhone;
        this.ShipCountry = sShipCountry;
        this.BaseCode = iBaseCode;
    }

    public void LoadData_Page3_2(string sShipMethod, bool bShipCommercial, decimal dShipFee, string sShippingName, DateTime dShipDate, DateTime dDepartureDate, string sBaseNotes)
    {
        this.ShipMethod = sShipMethod;
        this.ShipCommercial = bShipCommercial;
        this.ShipFee = dShipFee;
        this.ShippingName = sShippingName;
        this.ShipDate = dShipDate;
        this.DepartureDate = dDepartureDate;
        this.BaseNotes = sBaseNotes;
    }

    public void LoadData_Page4(string sCCType, int iDataPackageCode, string sDataPackageName, string sClientFirstName, string sClientLastName, string sCCCode, DateTime dCCExpDate, string sCCNum, string sShipEmail, string sCouponCode, string sCustomerComment, string sTag)
    {
        this.CCTitle = sCCType;
        this.DataPackageCode = iDataPackageCode;
        this.DataPackageName = sDataPackageName;
        this.ClientFirstName = sClientFirstName;
        this.ClientLastName = sClientLastName;
        this.CCCode = sCCCode;
        this.CCExpDate = dCCExpDate;
        this.CCNum = sCCNum;
        this.ShipEmail = sShipEmail;
        this.CouponCode = sCouponCode;
        SessionUtility.AddValue("CouponCode", this.CouponCode);
        this.CustomerComment = sCustomerComment;
        this.Tag = sTag;
    }

    public void SetCallPackageCode(int icallPackageCode)
    {
        this.CallPackageCode = icallPackageCode;
    }
    public OnlineOrderObj GetOrderFieldsForOptionals(OptionalObj aOp, OnlineOrderObj newOrder, OnlineOrderObj oldOrder)
    {
        newOrder.EquipmentModel = aOp.EquipmentCode;
        newOrder.EquipmentCode = aOp.EquipmentCode;
        newOrder.PlanCode = aOp.PlanCode;
        newOrder.Insurance = aOp.Insurance;

        //object[] ret;
        //ret= dbUtility.GetDataPackageCode(newOrder);
        //newOrder.DataPackageCode =Convert.ToInt32(ret[0]);

        int iData = -1;// dbUtility.GetDefaultDataPackageByOptional(aOp.OptionalCode);
        if (iData > -1)
            newOrder.DataPackageCode = iData;

        newOrder.CallPackageCode = -1;
        newOrder.SMSPackageCode = -1;
        newOrder.KITD_PlanCode = -1;
        newOrder.KITD = false;
        newOrder.KITD_BLOCK_ID = false;

        newOrder.AgentCode = oldOrder.AgentCode;
        newOrder.BaseCode = oldOrder.BaseCode;
        newOrder.CCCode = oldOrder.CCCode;
        newOrder.CCExpDate = oldOrder.CCExpDate;
        newOrder.CCNum = oldOrder.CCNum;
        newOrder.CCTitle = oldOrder.CCTitle;
        newOrder.ClientCity = oldOrder.ClientCity;
        newOrder.ClientCountry = oldOrder.ClientCountry;
        newOrder.ClientEmail = oldOrder.ClientEmail;
        newOrder.ClientFax = oldOrder.ClientFax;
        newOrder.ClientFirstName = oldOrder.ClientFirstName;
        newOrder.ClientHomePhone1 = oldOrder.ClientHomePhone1;
        newOrder.ClientHomePhone2 = oldOrder.ClientHomePhone2;
        newOrder.ClientIP = oldOrder.ClientIP;
        newOrder.ClientLastName = oldOrder.ClientLastName;
        newOrder.ClientMobile = oldOrder.ClientMobile;
        newOrder.ClientState = oldOrder.ClientState;
        newOrder.ClientStreet = oldOrder.ClientStreet;
        newOrder.ClientZip = oldOrder.ClientZip;
        newOrder.CompanyCode = oldOrder.CompanyCode;
        newOrder.CouponCode = oldOrder.CouponCode;
        newOrder.CreditEquipmentPurchase = oldOrder.CreditEquipmentPurchase;
        newOrder.CustomerComment = oldOrder.CustomerComment;
        newOrder.EndDate = oldOrder.EndDate;
        // newOrder.GroupName = oldOrder.GroupName;
        newOrder.LanguageCode = oldOrder.LanguageCode;
        newOrder.LinkTypeCode = oldOrder.LinkTypeCode;
        newOrder.ParentLink = oldOrder.ParentLink;
        newOrder.PurchaseEquipment = oldOrder.PurchaseEquipment;
        newOrder.PWD = oldOrder.PWD;
        newOrder.SessionID = oldOrder.SessionID;
        newOrder.ShipCity = oldOrder.ShipCity;
        //newOrder.ShipCommercial = oldOrder.ShipCommercial;
        newOrder.ShipName = oldOrder.ShipName;
        newOrder.ShipStreet = oldOrder.ShipStreet;
        newOrder.ShipState = oldOrder.ShipState;
        newOrder.ShipPostalCode = oldOrder.ShipPostalCode;
        newOrder.ShipPhone = oldOrder.ShipPhone;
        newOrder.ShipCountry = oldOrder.ShipCountry;
        newOrder.ShipEmail = oldOrder.ShipEmail;
        newOrder.BaseCode = oldOrder.BaseCode;
        newOrder.ShipMethod = oldOrder.ShipMethod;
        newOrder.ShipFee = oldOrder.ShipFee;
        newOrder.ShipDate = oldOrder.ShipDate;
        newOrder.DepartureDate = oldOrder.DepartureDate;
        newOrder.StartDate = oldOrder.StartDate;
        newOrder.EndDate = oldOrder.EndDate;
        newOrder.SignupSourceID = oldOrder.SignupSourceID;
        newOrder.Special = oldOrder.Special;
        newOrder.SubLink = oldOrder.SubLink;
        newOrder.SurfAndSave = false;
        newOrder.UserCity = oldOrder.UserCity;
        newOrder.UserName = oldOrder.UserName;
        newOrder.UserStreet = oldOrder.UserStreet;
        newOrder.Tag = oldOrder.Tag;

        return newOrder;
    }


    public void AddOrderToMyTable(int orderCode, decimal totalAmount, decimal totalAllAmount)
    {
        try
        {

            int amount = this.SimDetails.Count;
            decimal amountPerItem = totalAllAmount / amount;

            string sSimType = this.EquipmentName;

            if (sSimType == otherUtility.getResourceString("NanoS"))
                sSimType = "Nano SIM";
            else if (sSimType == otherUtility.getResourceString("MicroS"))
                sSimType = "Micro SIM";
            else if (sSimType == otherUtility.getResourceString("UsaR"))
                sSimType = "USA Regular SIM";
            string sourceSite = this.SignupSourceID; // SessionUtility.GetValue("UserCountry");
            if (!string.IsNullOrEmpty(sourceSite))
            {
                int isPaypal = this.PayPalTransactionId != "" ? 1 : 0;


                string[] sParamsArray = new string[15];
                string[] sValArray = new string[15]; ;

                sParamsArray[0] = "OnlineOrderCode";
                sValArray[0] = orderCode.ToString();

                sParamsArray[1] = "UserName";
                sValArray[1] = this.UserName;

                sParamsArray[2] = "EnteredOn";
                sValArray[2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.000");

                sParamsArray[3] = "IsPaypal";
                sValArray[3] = isPaypal == 1 ? "1" : "0";

                sParamsArray[4] = "IsCC";
                sValArray[4] = isPaypal == 1 ? "0" : "1";

                sParamsArray[5] = "SIMType";
                sValArray[5] = sSimType;

                sParamsArray[6] = "Email";
                sValArray[6] = this.ClientEmail;

                sParamsArray[7] = "OrderAmount";
                sValArray[7] = totalAmount.ToString();

                sParamsArray[8] = "OrderSource";
                sValArray[8] = sourceSite;

                sParamsArray[9] = "StartDate";
                sValArray[9] = Convert.ToDateTime(this.StartDate).ToString("yyyy-MM-dd 00:00:00.000");

                sParamsArray[10] = "EndDate";
                sValArray[10] = Convert.ToDateTime(this.EndDate).ToString("yyyy-MM-dd 00:00:00.000");

                sParamsArray[11] = "OrderAmountSingle";
                sValArray[11] = amountPerItem.ToString();

                sParamsArray[12] = "AgentName";
                sValArray[12] = dbUtility.ExecScalar_DBOR("Select AgentName from tblAgents where agentcode=" + this.AgentCode);

                sParamsArray[13] = "ParentLink";
                sValArray[13] = "Default";// dbUtility.ExecScalar_DBOR("Select * from tblAgents where agentcode=" + this.AgentCode);

                sParamsArray[14] = "Country";
                sValArray[14] = this.ClientCountry.ToString();

                if (SessionUtility.GetValue("AgentName") != "")
                    sValArray[13] = SessionUtility.GetValue("AgentName");
                //insert into log table
                string sql = string.Format(@"INSERT INTO tblOrdersTelaway (OnlineOrderCode,UserName,EnteredOn,IsPaypal,IsCC,SIMType,Email,OrderAmount,OrderSource, StartDate, EndDate,OrderAmountSingle,AgentName,ParentLink,Country) VALUES 
                                                                             (@OnlineOrderCode,@UserName ,@EnteredOn ,@IsPaypal,@IsCC ,@SIMType,@Email,@OrderAmount,@OrderSource, @StartDate, @EndDate,@OrderAmountSingle,@AgentName,@ParentLink,@Country)");

                dbUtility.ExecNoQuery_DBORParmas(sql, sParamsArray, sValArray);
            }//if
        }//try
        catch (Exception e)
        {
            emailUtility.SendMailErr("AddOrderToMyTable: " + e.Message);
        }
    }

    private string getValidChars(string sVal)
    {

        string sRet = "";
        for (int i = 0; i <= sVal.Length - 1; i++)
        {
            byte[] asciiBytes = System.Text.Encoding.ASCII.GetBytes(sVal[i].ToString());
            int iCode = Convert.ToInt32(asciiBytes[0]);

            if ((iCode >= 32 && iCode <= 57) || (iCode >= 64 && iCode <= 122))
                sRet = sRet + sVal[i];

        }
        return sRet;
    }
}


