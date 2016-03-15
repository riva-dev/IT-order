using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System.Text;

/// <summary>
/// Summary description for PayPalAPI
/// </summary>
public class PaypalAPI
{
    public bool SetExpressCheckout(SetExpressCheckoutRequestType request, ref string redirectURL)
    {
        var success = true;
        // Invoke the API
        SetExpressCheckoutReq wrapper = new SetExpressCheckoutReq();
        wrapper.SetExpressCheckoutRequest = request;
        // Create the PayPalAPIInterfaceServiceService service object to make the API call
        PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
        
        // # API call 
        // Invoke the SetExpressCheckout method in service wrapper object  
        SetExpressCheckoutResponseType setECResponse = service.SetExpressCheckout(wrapper);

        // Check for API return status
        HttpContext CurrContext = HttpContext.Current;
        CurrContext.Items.Add("paymentDetails", request.SetExpressCheckoutRequestDetails.PaymentDetails);
        //CurrContext.Items.Add("ADDROVERRIDE", 1);
        //setKeyResponseObjects(service, setECResponse);

        Dictionary<string, string> keyResponseParameters = new Dictionary<string, string>();
        string ss = "";
        foreach (ErrorType error in setECResponse.Errors)
	    {
            ss += error.LongMessage + "      " ;
	    }
        
        keyResponseParameters.Add("API Status", setECResponse.Ack.ToString());

        if (setECResponse.Ack.Equals(AckCodeType.FAILURE) ||
            (setECResponse.Errors != null && setECResponse.Errors.Count > 0))
        {
            CurrContext.Items.Add("Response_error", setECResponse.Errors);
            CurrContext.Items.Add("Response_redirectURL", null);
            success = false;
            return success;
        }
        else
        {
            CurrContext.Items.Add("Response_error", null);
            keyResponseParameters.Add("EC token", setECResponse.Token);
            CurrContext.Items.Add("Response_redirectURL", ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"].ToString()
                + "_express-checkout&token=" + setECResponse.Token);
            success = true;
        }
        CurrContext.Items.Add("Response_keyResponseObject", keyResponseParameters);
        CurrContext.Items.Add("Response_apiName", "SetExpressCheckout");
        CurrContext.Items.Add("Response_requestPayload", service.getLastRequest());
        CurrContext.Items.Add("Response_responsePayload", service.getLastResponse());

        redirectURL = ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"].ToString()
                + "_express-checkout&token=" + setECResponse.Token;

        if (success)
            return true;
        return false;
        //Response.Redirect(url);
    }

    public static bool GetDetails(string token, string payerID)
    {
        // Create request object
        GetExpressCheckoutDetailsRequestType request = new GetExpressCheckoutDetailsRequestType();
        // (Required) A timestamped token, the value of which was returned by SetExpressCheckout response.
        // Character length and limitations: 20 single-byte characters
        request.Token = token;

        // Invoke the API
        GetExpressCheckoutDetailsReq wrapper = new GetExpressCheckoutDetailsReq();
        wrapper.GetExpressCheckoutDetailsRequest = request;
        // Create the PayPalAPIInterfaceServiceService service object to make the API call
        PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
        // # API call 
        // Invoke the GetExpressCheckoutDetails method in service wrapper object
        GetExpressCheckoutDetailsResponseType response = service.GetExpressCheckoutDetails(wrapper);

        // Check for API return status
        HttpContext CurrContext = HttpContext.Current;
        CurrContext.Items.Add("Response_apiName", "GetExpressChecoutDetails");
        CurrContext.Items.Add("Response_redirectURL", null);
        CurrContext.Items.Add("Response_requestPayload", service.getLastRequest());
        CurrContext.Items.Add("Response_responsePayload", service.getLastResponse());

        Dictionary<string, string> keyResponseParameters = new Dictionary<string, string>();

        //Selenium Test Case
        keyResponseParameters.Add("PayerID", payerID);
        keyResponseParameters.Add("EC token", token);

        keyResponseParameters.Add("Correlation Id", response.CorrelationID);
        keyResponseParameters.Add("API Result", response.Ack.ToString());

        CurrContext.Items.Add("Response_keyResponseObject", keyResponseParameters);

        if (response.Ack.Equals(AckCodeType.FAILURE) ||
            (response.Errors != null && response.Errors.Count > 0))
        {
            CurrContext.Items.Add("Response_error", response.Errors);
            return false;
        }
        else
        {
            CurrContext.Items.Add("Response_error", null);
            return true;
        }
    }

    public bool DoPayment(string token, string payerId, ref Dictionary<string, string> responseParams, ref List<ErrorType> responseErrors)
    {
        try
        {
        // Create the PayPalAPIInterfaceServiceService service object to make the API call
        PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
        GetExpressCheckoutDetailsReq getECWrapper = new GetExpressCheckoutDetailsReq();
        // (Required) A timestamped token, the value of which was returned by SetExpressCheckout response.
        // Character length and limitations: 20 single-byte characters
        getECWrapper.GetExpressCheckoutDetailsRequest = new GetExpressCheckoutDetailsRequestType(token);
        // # API call 
        // Invoke the GetExpressCheckoutDetails method in service wrapper object
        GetExpressCheckoutDetailsResponseType getECResponse = service.GetExpressCheckoutDetails(getECWrapper);

        // Create request object
        DoExpressCheckoutPaymentRequestType request = new DoExpressCheckoutPaymentRequestType();
        DoExpressCheckoutPaymentRequestDetailsType requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
        request.DoExpressCheckoutPaymentRequestDetails = requestDetails;

        requestDetails.PaymentDetails = getECResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails;
        if (requestDetails.PaymentDetails.Count > 0)
        {
            if (requestDetails.PaymentDetails[0].NoteText != null)
            {
                if (requestDetails.PaymentDetails[0].NoteText.Length > 250)
                    requestDetails.PaymentDetails[0].NoteText = requestDetails.PaymentDetails[0].NoteText.Substring(0, 250) + "...";
            }
        }

        // (Required) The timestamped token value that was returned in the SetExpressCheckout response and passed in the GetExpressCheckoutDetails request.
        requestDetails.Token = token;
        // (Required) Unique PayPal buyer account identification number as returned in the GetExpressCheckoutDetails response
        requestDetails.PayerID = payerId;
        // (Required) How you want to obtain payment. It is one of the following values:
        // * Authorization – This payment is a basic authorization subject to settlement with PayPal Authorization and Capture.
        // * Order – This payment is an order authorization subject to settlement with PayPal Authorization and Capture.
        // * Sale – This is a final sale for which you are requesting payment.
        // Note: You cannot set this value to Sale in the SetExpressCheckout request and then change this value to Authorization in the DoExpressCheckoutPayment request.
        requestDetails.PaymentAction = (PaymentActionCodeType)
            Enum.Parse(typeof(PaymentActionCodeType), "2"); //TO FIX

        // Invoke the API
        DoExpressCheckoutPaymentReq wrapper = new DoExpressCheckoutPaymentReq();
        wrapper.DoExpressCheckoutPaymentRequest = request;
        // # API call 
        // Invoke the DoExpressCheckoutPayment method in service wrapper object
        DoExpressCheckoutPaymentResponseType doECResponse = service.DoExpressCheckoutPayment(wrapper);

        // Check for API return status
        var success = true;
        //Dictionary<string, string> responseParams = new Dictionary<string, string>();
        responseParams.Add("Correlation Id", doECResponse.CorrelationID);
        responseParams.Add("API Result", doECResponse.Ack.ToString());
        HttpContext CurrContext = HttpContext.Current;
        if (doECResponse.Ack.Equals(AckCodeType.FAILURE) ||
            (doECResponse.Errors != null && doECResponse.Errors.Count > 0))
        {
            responseErrors = doECResponse.Errors;
            success = false;
        }
        else
        {
            responseErrors = null;
            responseParams.Add("EC Token", doECResponse.DoExpressCheckoutPaymentResponseDetails.Token);
            responseParams.Add("Transaction Id", doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].TransactionID);
            responseParams.Add("Payment status", doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].PaymentStatus.ToString());
            if (doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].PendingReason != null)
            {
                responseParams.Add("Pending reason", doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].PendingReason.ToString());
            }
            if (doECResponse.DoExpressCheckoutPaymentResponseDetails.BillingAgreementID != null)
                responseParams.Add("Billing Agreement Id", doECResponse.DoExpressCheckoutPaymentResponseDetails.BillingAgreementID);

            success = true;
        }

        if (success)
            return true;
        return false;

    }
        catch(Exception ex)
        {
            emailUtility.SendMailErr("DoPayments " + ex.Message);
            return false;
        }

    }

}