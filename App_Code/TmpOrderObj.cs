using System;
using System.Collections.Generic;

public class SIMDetails
{
    private Nullable<int> m_intEquipmentCode;
    private Nullable<int> m_intEquipmentModel;
    private Nullable<int> m_intExtendedDataPackageCode;
    private Nullable<int> m_intKNTCode;
    private string m_strPhoneIMEI;
    private string m_strEquipmentName;
    private Nullable<decimal> m_decPaypalAmount;
    private Nullable<decimal> m_decPaypalDiscount;
    private Nullable<decimal> m_decShipFee;
    private Nullable<decimal> m_decEquipmentPrice;
    private Nullable<decimal> m_decSimPrice;
    private string m_strSIMPlanName;
    private Nullable<int> m_intOnlineOrderCode;

    public Nullable<int> KNTCode
    {
        get { return m_intKNTCode; }
        set { m_intKNTCode = value; }
    }
    public Nullable<decimal> SimPrice
    {
        get { return m_decSimPrice; }
        set { m_decSimPrice = value; }
    }
    public Nullable<int> EquipmentCode
    {
        get { return m_intEquipmentCode; }
        set { m_intEquipmentCode = value; }
    }

    public Nullable<int> OnlineOrderCode
    {
        get { return m_intOnlineOrderCode; }
        set { m_intOnlineOrderCode = value; }
    }

    public Nullable<int> EquipmentModel
    {
        get { return m_intEquipmentModel; }
        set { m_intEquipmentModel = value; }
    }
    public Nullable<int> ExtendedDataPackageCode
    {
        get { return m_intExtendedDataPackageCode; }
        set { m_intExtendedDataPackageCode = value; }
    }

    public string PhoneIMEI
    {
        get { return m_strPhoneIMEI; }
        set { m_strPhoneIMEI = value; }
    }

    public string EquipmentName
    {
        get { return m_strEquipmentName; }
        set { m_strEquipmentName = value; }
    }

    public Nullable<decimal> PaypalAmount
    {
        get { return m_decPaypalAmount; }
        set { m_decPaypalAmount = value; }
    }
    public Nullable<decimal> PaypalDiscount
    {
        get { return m_decPaypalDiscount; }
        set { m_decPaypalDiscount = value; }
    }
    public Nullable<decimal> ShipFee
    {
        get { return m_decShipFee; }
        set { m_decShipFee = value; }
    }
    public Nullable<decimal> EquipmentPrice
    {
        get { return m_decEquipmentPrice; }
        set { m_decEquipmentPrice = value; }
    }

    public string SIMPlanName
    {
        get { return m_strSIMPlanName; }
        set { m_strSIMPlanName = value; }
    }

}



public class TmpOrderObj
{
    private Nullable<int> m_intPageNo;
    private Nullable<int> m_intOnlineOrderCode;
    private Nullable<int> m_intParentOnlineOrderCode;
    private Nullable<int> m_intLinkTypeCode;
    private string m_strSignupSourceID;
    private string m_strParentLink;
    private string m_strSubLink;
    private string m_strGroupName;
    private string m_strTag;
    private string m_strAccessoryIdAndQuantity;
    private string m_strSessionID;
    private string m_strClientIP;
    private string m_strPWD;
    private string m_strHint;
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
    private bool bChargeWithPayPal;
    private string m_strPayPalTransactionId;
    private Nullable<decimal> m_decPayPalAmountCharge;
    private Nullable<decimal> m_decPayPalAllAmountCharge;
    private int i_PayPalLogCounter;


    private string m_strEquipmentName;
    private string m_strEquipmentNotes;
    private string m_strBaseNotes;
    private Nullable<int> m_intPhonesRequired;
    private Nullable<bool> m_IsEquipmentSNS;
    private Nullable<bool> m_IsRequierdOperationSys;
    private Nullable<int> m_intKNTRequired;
    private string m_strCallPackageName;
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
    private Nullable<bool> m_bolShipCommercial;
    private Nullable<decimal> m_decShipFee;
    private Nullable<decimal> m_decShipGBPFee;
    private Nullable<System.DateTime> m_datShipDate;
    private Nullable<System.DateTime> m_datDepartureDate;
    private Nullable<System.DateTime> m_datStartDate;
    private Nullable<System.DateTime> m_datEndDate;
    private string m_strUserName;
    private string m_strUserNamePayPal;
    private string m_strUserStreet;
    private string m_strUserCity;
    private string m_strClientFirstName;
    private string m_strClientLastName;
    private string m_strClientStreet;
    private string m_strClientCity;
    private string m_strClientState;
    private string m_strClientCountry;
    private string m_strClientZip;
    private string m_strDeposit;
    private string m_strSetupFeeText;
    private string m_strClientHomePhone1;
    private string m_strClientHomePhone2;
    private string m_strClientFax;
    private string m_strClientMobile;
    private string m_strClientEmail;
    private string m_strCCNum;
    private Nullable<System.DateTime> m_datCCExpDate;
    private string m_strCCTitle;
    private string m_strCCCode;
    private Nullable<int> m_intLanguageCode;
    private Nullable<bool> m_bolSpecial;
    private Nullable<bool> m_bitCallPackageOverageProtection;
    private string m_strCustomerComment;
    private Nullable<System.DateTime> m_datRecordCreationDate;
    private Nullable<int> m_intOrderCode;
    private Nullable<DateTime> m_datOrderCodeAddedOn;
    private Nullable<int> m_intRentalCode;
    private Nullable<DateTime> m_datRentalCodeAddedOn;
    private decimal m_intPlanCostUSD;
    private decimal m_PricePerItem;
    private int m_intOpCostUSD;
    private string m_strBccEmail;
    private string m_strSalesEmail;
    private string m_strMainEmail;
    private string m_strCustomerServicEmail;
    private string m_strConfirmationEmail;
    private string m_strSiteSourceCountry;
    private string m_strInfoEmail;
    private string m_strTelawayPhone;
    private string m_strPayPalLocaleCode;
    private string m_strSignupLanguageCode;

    public Nullable<int> PageNo
    {
        get { return m_intPageNo; }
        set { m_intPageNo = value; }
    }

    public decimal PlanCostUSD
    {
        get { return m_intPlanCostUSD; }
        set { m_intPlanCostUSD = value; }
    }
    public decimal PricePerItem
    {
        get { return m_PricePerItem; }
        set { m_PricePerItem = value; }
    }

    public int OpCostUSD
    {
        get { return m_intOpCostUSD; }
        set { m_intOpCostUSD = value; }
    }

    public Nullable<int> OnlineOrderCode
    {
        get { return m_intOnlineOrderCode; }
        protected set { m_intOnlineOrderCode = value; }
    }
    public Nullable<int> ParentOnlineOrderCode
    {
        get { return m_intParentOnlineOrderCode; }
        set { m_intParentOnlineOrderCode = value; }
    }

    public Nullable<int> PhonesRequired
    {
        get { return m_intPhonesRequired; }
        set { m_intPhonesRequired = value; }
    }



    public Nullable<bool> bSubmitOrder
    {
        get { return m_bSubmitOrder; }
        set { m_bSubmitOrder = value; }
    }
    public Nullable<bool> IsEquipmentSNS
    {
        get { return m_IsEquipmentSNS; }
        set { m_IsEquipmentSNS = value; }
    }
    public Nullable<bool> IsRequierdOperationSys
    {
        get { return m_IsRequierdOperationSys; }
        set { m_IsRequierdOperationSys = value; }
    }
    public Nullable<int> KNTRequired
    {
        get { return m_intKNTRequired; }
        set { m_intKNTRequired = value; }
    }
    public string SignupSourceID
    {
        get { return m_strSignupSourceID; }
        set { m_strSignupSourceID = value; }
    }
    public Nullable<int> LinkTypeCode
    {
        get { return m_intLinkTypeCode; }
        set { m_intLinkTypeCode = value; }
    }
    public Nullable<int> ProductId
    {
        get { return m_intProductId; }
        set { m_intProductId = value; }
    }

    public string SetupFeeText
    {
        get { return m_strSetupFeeText; }
        set { m_strSetupFeeText = value; }
    }

    public Nullable<bool> IsSim
    {
        get { return m_bIsSim; }
        set { m_bIsSim = value; }
    }
    public string EquipmentName
    {
        get { return m_strEquipmentName; }
        set { m_strEquipmentName = value; }
    }

    public Nullable<bool> IsKosher
    {
        get { return m_bIsKosher; }
        set { m_bIsKosher = value; }
    }

    public string CallPackageName
    {
        get { return m_strCallPackageName; }
        set { m_strCallPackageName = value; }
    }

    public string Deposit
    {
        get { return m_strDeposit; }
        set { m_strDeposit = value; }
    }


    public string TermsName
    {
        get { return m_strTermsName; }
        set { m_strTermsName = value; }
    }


    public string KNTName
    {
        get { return m_strKNTName; }
        set { m_strKNTName = value; }
    }

    public string SMSPackageName
    {
        get { return m_strSMSPackageName; }
        set { m_strSMSPackageName = value; }
    }
    public string PlanName
    {
        get { return m_strPlanName; }
        set { m_strPlanName = value; }
    }
    public string ParentLink
    {
        get { return m_strParentLink; }
        set { m_strParentLink = value; }
    }
    public string SubLink
    {
        get { return m_strSubLink; }
        set { m_strSubLink = value; }
    }
    public string GroupName
    {
        get { return m_strGroupName; }
        set { m_strGroupName = value; }
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
    public string SessionID
    {
        get { return m_strSessionID; }
        set { m_strSessionID = value; }
    }
    public string ClientIP
    {
        get { return m_strClientIP; }
        set { m_strClientIP = value; }
    }
    public string PWD
    {
        get { return m_strPWD; }
        set { m_strPWD = value; }
    }
    public string Hint
    {
        get { return m_strHint; }
        set { m_strHint = value; }
    }
    public Nullable<int> PlanCode
    {
        get { return m_intPlanCode; }
        set { m_intPlanCode = value; }
    }
    public Nullable<int> AgentCode
    {
        get { return m_intAgentCode; }
        set { m_intAgentCode = value; }
    }
    public Nullable<int> SubAgentCode
    {
        get { return m_intSubAgentCode; }
        set { m_intSubAgentCode = value; }
    }
    public Nullable<int> CompanyCode
    {
        get { return m_intCompanyCode; }
        set { m_intCompanyCode = value; }
    }
    public Nullable<int> BaseCode
    {
        get { return m_intBaseCode; }
        set { m_intBaseCode = value; }
    }

    public List<SIMDetails> SimDetails
    {
        get { return m_lstSimDetails; }
        set { m_lstSimDetails = value; }
    }

    public Nullable<int> EquipmentCode
    {
        get { return m_intEquipmentCode; }
        set { m_intEquipmentCode = value; }
    }

    public Nullable<int> EquipmentModel
    {
        get { return m_intEquipmentModel; }
        set { m_intEquipmentModel = value; }
    }

    public string EquipmentNotes
    {
        get { return m_strEquipmentNotes; }
        set { m_strEquipmentNotes = value; }
    }
    public string BaseNotes
    {
        get { return m_strBaseNotes; }
        set { m_strBaseNotes = value; }
    }

    public Nullable<bool> Insurance
    {
        get { return m_bolInsurance; }
        set { m_bolInsurance = value; }
    }
    public Nullable<bool> PurchaseEquipment
    {
        get { return m_bolPurchaseEquipment; }
        set { m_bolPurchaseEquipment = value; }
    }
    public Nullable<bool> KITD
    {
        get { return m_bolKITD; }
        set { m_bolKITD = value; }
    }
    public Nullable<bool> KITD_BLOCK_ID
    {
        get { return m_bolKITD_BLOCK_ID; }
        set { m_bolKITD_BLOCK_ID = value; }
    }

    public Nullable<int> DataPackageId
    {
        get { return m_iDataPackageId; }
        set { m_iDataPackageId = value; }
    }

    public string DataPackageName
    {
        get { return m_strDataPackageName; }
        set { m_strDataPackageName = value; }
    }
    public string DataPackgeSize
    {
        get { return m_strDataPackgeSize; }
        set { m_strDataPackgeSize = value; }
    }

    public Nullable<int> DataPackageCode
    {
        get { return m_intDataPackageCode; }
        set { m_intDataPackageCode = value; }
    }
    public Nullable<int> KITD_PlanCode
    {
        get { return m_intKITD_PlanCode; }
        set { m_intKITD_PlanCode = value; }
    }
    public Nullable<bool> SurfAndSave
    {
        get { return m_bolSurftAndSave; }
        set { m_bolSurftAndSave = value; }
    }
    public Nullable<bool> CreditEquipmentPurchase
    {
        get { return m_bolCreditEquipmentPurchase; }
        set { m_bolCreditEquipmentPurchase = value; }
    }
    public string ShipName
    {
        get { return m_strShipName; }
        set { m_strShipName = value; }
    }
    public string ShippingName
    {
        get { return m_strShippingName; }
        set { m_strShippingName = value; }
    }
    public string ShipStreet
    {
        get { return m_strShipStreet; }
        set { m_strShipStreet = value; }
    }
    public string ShipCity
    {
        get { return m_strShipCity; }
        set { m_strShipCity = value; }
    }
    public string ShipState
    {
        get { return m_strShipState; }
        set { m_strShipState = value; }
    }
    public string ShipPostalCode
    {
        get { return m_strShipPostalCode; }
        set { m_strShipPostalCode = value; }
    }
    public string ShipPhone
    {
        get { return m_strShipPhone; }
        set { m_strShipPhone = value; }
    }
    public string ShipEmail
    {
        get { return m_strShipEmail; }
        set { m_strShipEmail = value; }
    }
    public string ShipCountry
    {
        get { return m_strShipCountry; }
        set { m_strShipCountry = value; }
    }
    public string ShipMethod
    {
        get { return m_strShipMethod; }
        set { m_strShipMethod = value; }
    }
    public Nullable<bool> ShipCommercial
    {
        get { return m_bolShipCommercial; }
        set { m_bolShipCommercial = value; }
    }
    public Nullable<decimal> ShipFee
    {
        get { return m_decShipFee; }
        set { m_decShipFee = value; }
    }


    public Nullable<decimal> ShipGBPFee
    {
        get { return m_decShipGBPFee; }
        set { m_decShipGBPFee = value; }
    }
    public Nullable<System.DateTime> ShipDate
    {
        get { return m_datShipDate; }
        set { m_datShipDate = value; }
    }
    public Nullable<System.DateTime> DepartureDate
    {
        get { return m_datDepartureDate; }
        set { m_datDepartureDate = value; }
    }
    public Nullable<System.DateTime> StartDate
    {
        get { return m_datStartDate; }
        set { m_datStartDate = value; }
    }
    public Nullable<System.DateTime> EndDate
    {
        get { return m_datEndDate; }
        set { m_datEndDate = value; }
    }
    public string UserName
    {
        get { return m_strUserName; }
        set { m_strUserName = value; }
    }
    public string UserNamePayPal
    {
        get { return m_strUserNamePayPal; }
        set { m_strUserNamePayPal = value; }
    }
    public string UserStreet
    {
        get { return m_strUserStreet; }
        set { m_strUserStreet = value; }
    }

    public string PayPalTransactionId
    {
        get { return m_strPayPalTransactionId; }
        set { m_strPayPalTransactionId = value; }
    }


    public bool ChargeWithPayPal
    {
        get { return bChargeWithPayPal; }
        set { bChargeWithPayPal = value; }
    }

    public Nullable<decimal> PayPalAmountCharge
    {
        get { return m_decPayPalAmountCharge; }
        set { m_decPayPalAmountCharge = value; }
    }

    public Nullable<decimal> PayPalAllAmountCharge
    {
        get { return m_decPayPalAllAmountCharge; }
        set { m_decPayPalAllAmountCharge = value; }
    }

    public int PayPalLogCounter
    {
        get { return i_PayPalLogCounter; }
        set { i_PayPalLogCounter = value; }
    }





    public string UserCity
    {
        get { return m_strUserCity; }
        set { m_strUserCity = value; }
    }
    public string ClientFirstName
    {
        get { return m_strClientFirstName; }
        set { m_strClientFirstName = value; }
    }
    public string ClientLastName
    {
        get { return m_strClientLastName; }
        set { m_strClientLastName = value; }
    }
    public string ClientStreet
    {
        get { return m_strClientStreet; }
        set { m_strClientStreet = value; }
    }
    public string ClientCity
    {
        get { return m_strClientCity; }
        set { m_strClientCity = value; }
    }
    public string ClientState
    {
        get { return m_strClientState; }
        set { m_strClientState = value; }
    }
    public string ClientCountry
    {
        get { return m_strClientCountry; }
        set { m_strClientCountry = value; }
    }
    public string ClientZip
    {
        get { return m_strClientZip; }
        set { m_strClientZip = value; }
    }
    public string ClientHomePhone1
    {
        get { return m_strClientHomePhone1; }
        set { m_strClientHomePhone1 = value; }
    }
    public string ClientHomePhone2
    {
        get { return m_strClientHomePhone2; }
        set { m_strClientHomePhone2 = value; }
    }
    public string ClientFax
    {
        get { return m_strClientFax; }
        set { m_strClientFax = value; }
    }
    public string ClientMobile
    {
        get { return m_strClientMobile; }
        set { m_strClientMobile = value; }
    }
    public string ClientEmail
    {
        get { return m_strClientEmail; }
        set { m_strClientEmail = value; }
    }
    public string CCNum
    {
        get { return m_strCCNum; }
        set { m_strCCNum = value; }
    }
    public Nullable<System.DateTime> CCExpDate
    {
        get { return m_datCCExpDate; }
        set { m_datCCExpDate = value; }
    }
    public string CCTitle
    {
        get { return m_strCCTitle; }
        set { m_strCCTitle = value; }
    }
    public string CCCode
    {
        get { return m_strCCCode; }
        set { m_strCCCode = value; }
    }
    public Nullable<int> LanguageCode
    {
        get { return m_intLanguageCode; }
        set { m_intLanguageCode = value; }
    }
    public Nullable<bool> Special
    {
        get { return m_bolSpecial; }
        set { m_bolSpecial = value; }
    }
    public string CustomerComment
    {
        get { return m_strCustomerComment; }
        set { m_strCustomerComment = value; }
    }


    public Nullable<bool> bitCallPackageOverageProtection
    {
        get { return m_bitCallPackageOverageProtection; }
        set { m_bitCallPackageOverageProtection = value; }
    }
    public Nullable<int> CallPackageCode
    {
        get { return m_intCallPackageCode; }
        set { m_intCallPackageCode = value; }
    }

    public Nullable<int> TermsCode
    {
        get { return m_intTermsCode; }
        set { m_intTermsCode = value; }
    }
    public Nullable<int> SMSPackageCode
    {
        get { return m_intSMSPackageCode; }
        set { m_intSMSPackageCode = value; }
    }

    public Nullable<int> SMSPackageCounter
    {
        get { return m_intSMSPackageCounter; }
        set { m_intSMSPackageCounter = value; }
    }

    public string CouponCode
    {
        get { return m_strCouponCode; }
        set { m_strCouponCode = value; }
    }
    public Nullable<System.DateTime> RecordCreationDate
    {
        get { return m_datRecordCreationDate; }
        protected set { m_datRecordCreationDate = value; }
    }
    public Nullable<int> OrderCode
    {
        get { return m_intOrderCode; }
        set { m_intOrderCode = value; }
    }
    public Nullable<DateTime> OrderCodeAddedOn
    {
        get { return m_datOrderCodeAddedOn; }
        set { m_datOrderCodeAddedOn = value; }
    }
    public Nullable<int> RentalCode
    {
        get { return m_intRentalCode; }
        set { m_intRentalCode = value; }
    }
    public Nullable<DateTime> RentalCodeAddedOn
    {
        get { return m_datRentalCodeAddedOn; }
        set { m_datRentalCodeAddedOn = value; }
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
    public string MainEmail
    {
        get { return m_strMainEmail; }
        set { m_strMainEmail = value; }
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
    public string InfoEmail
    {
        get { return m_strInfoEmail; }
        set { m_strInfoEmail = value; }
    }
    public string TelawayPhone
    {
        get { return m_strTelawayPhone; }
        set { m_strTelawayPhone = value; }
    }
    public string PayPalLocaleCode
    {
        get { return m_strPayPalLocaleCode; }
        set { m_strPayPalLocaleCode = value; }
    }
    public string SignupLanguageCode
    {
        get { return m_strSignupLanguageCode; }
        set { m_strSignupLanguageCode = value; }
    }
    public string SiteSourceCountry
    {
        get { return m_strSiteSourceCountry; }
        set { m_strSiteSourceCountry = value; }
    }





}


