<%@ Page Language="C#" MasterPageFile="~/MasterPageInnerFM.master" AutoEventWireup="true"
    CodeFile="FMPartial1.aspx.cs" Inherits="FMPartial" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="css/rocket.min.css">
    <link rel="stylesheet" href="css/shoppingCart.css">

    <script src="js/rocket.min.js"></script>
    <script src="js/popup.js"></script>
    <script>








        var firstTime = true;




        function pageLoad() {

            // $find('RadDeliveryDate').get_dateInput().set_textBoxValue('Package Date');
        }
        function hideDollarPrice() {
            if (document.getElementById("<%= HiddenDisplayDollars.ClientID %>").value == "False") {
                $('#usdPrice').hide();
            }
            var HiddenShipping = document.getElementById('ctl00_ContentPlaceHolder1_HiddenShipping').value;
            var HiddenCost = document.getElementById('ctl00_ContentPlaceHolder1_HiddenCost').value;
            /*var liShipping= document.getElementById('ctl00_ContentPlaceHolder1_RadComboShipping_Header_liShipping'); 
            liShipping.innerHTML=HiddenShipping;
            var liCost= document.getElementById('ctl00_ContentPlaceHolder1_RadComboShipping_Header_liCost'); 
            liCost.innerHTML=HiddenCost;*/
        }

        function ValidateData() {
            var endDatePicker = $find("<%= RadDateEndDate.ClientID %>").get_selectedDate();
            var startDatePicker = $find("<%= RadDateStartDate.ClientID %>").get_selectedDate();
            var qntselection = document.getElementById("cbQnty").value;
            var PleaseSelectStartDate = document.getElementById("<%= PleaseSelectStartDateHidden.ClientID %>").value;
            var SelectQuantity = document.getElementById("<%= PleaseSelectQuantityHidden.ClientID %>").value;
            var divQnt = document.getElementById("<%= divQnt.ClientID %>");
            divQnt.style.border = "none";
            if (startDatePicker == null) {
                $('#lblErr').html(PleaseSelectStartDate);
                //window.scrollTo(0,0);
                return false;
            }
            if (endDatePicker == null) {
                $('#lblErr').html(PleaseSelectStartDate);
                //window.scrollTo(0,0);
                return false;
            }
            //       if (startDatePicker!=null && endDatePicker!=null)
            //       {          
            //          // var datediff = daysBetween(startDatePicker,endDatePicker); 
            //           PageMethods.GetDaysBetween(startDatePicker,endDatePicker,fValid);   
            //       }       
            var valMsg = "";
            if (qntselection == 0) {
                $('#lblErr').html(SelectQuantity);
                divQnt.style.border = "2px solid red";
                PageMethods.UpdateErrHtml('InValidQuantity', true, updateErrHtmlFunc);
                return false;
            }
            var isCheckedSimType = true;
            $('.packageDetails3').each(function () {
                var pkgNum = $(this).attr('id').replace('packageDiv', '');
                if ($('input[name=xSimtype_' + pkgNum + ']:checked').length > 0) {
                    var divSimType = document.getElementById("divSimType_" + pkgNum + '');
                    if (divSimType != null)
                        divSimType.style.border = "0px solid red";
                }
                else {
                    var divSimType = document.getElementById("divSimType_" + pkgNum + '');
                    if (divSimType != null) {
                        isCheckedSimType = false;
                        divSimType.style.border = "2px solid red";
                        valMsg = '<%=otherUtility.getResourceString("ValChooseSimType") %>';

                    }
                }
            });
            var isCheckedKNTCode = true;
            $('.packageDetails3').each(function () {
                var pkgNum = $(this).attr('id').replace('packageDiv', '');
                kntCode = $("#SelectRegion" + pkgNum).val();
                var divKntCode = document.getElementById("SelectRegion" + pkgNum + '');
                if (kntCode == '0') {
                    isCheckedKNTCode = false;
                    valMsg = '<%=otherUtility.getResourceString("ValChooseKntCode") %>';
                    divKntCode.style.border = "2px solid red";
                }
            });
            if (isCheckedSimType == false) {
                PageMethods.UpdateErrHtml('InValidSimType', true, updateErrHtmlFunc);

                return false;
            }
            else PageMethods.UpdateErrHtml('InValidSimType', false, updateErrHtmlFunc);
            if (isCheckedKNTCode == false) {
                PageMethods.UpdateErrHtml('InValidKntCode', true, updateErrHtmlFunc);
                return false;
            }
            else PageMethods.UpdateErrHtml('InValidKntCode', false, updateErrHtmlFunc);
            if (valMsg != "") {
                $('#lblErr').html(valMsg);
                //window.scrollTo(0,0);
                return false;
            }

            var IMEINumbers = "";
            $('.IMEINum').each(function () {
                var pkgNum = $(this).attr('phoneNum');
                var IMEI = $(this).val();
                IMEINumbers += IMEI + ',';
            });
            //PageMethods.ValidateIMEI(IMEINumbers, ValidateIMEIDone);
            SaveSimData();
            return true;
        }

        function keyPress(sender, eventArgs) {
            var value = sender.get_value();
            if (value == "")
                sender.set_caretPosition(0);


        }


        function ResponseEnd() {
            $(".smallanimationload").fadeOut("slow");
            $('.Curtain').removeClass("visible");
            $('.warpper').removeClass("warpper-visable");
            var HiddenStatus = document.getElementById("<%= HiddenStatus.ClientID %>").value;
            var url = "SignupStep_4.aspx?c=" + document.getElementById("ctl00_ContentPlaceHolder1_HiddenCountry").value;
            //var url= "FMOrderConfirmation.aspx?c="+document.getElementById("ctl00_ContentPlaceHolder1_HiddenCountry").value;
            if (HiddenStatus != "0") {
                window.location.replace(url);
            }
            else {
            }
        }

        //save data if choose already   
        function ShowDataIfChooseAlready() {
            if ($('#ctl00_ContentPlaceHolder1_rbDiffAddress').is(':checked'))
                $('#Step5').show('slow');
            if (document.getElementById("<%= HiddenDays.ClientID %>").value != "0")
                document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = document.getElementById("<%= HiddenDays.ClientID %>").value;
            if (document.getElementById("ctl00_ContentPlaceHolder1_HiddenSIMCount").value > "0") {
                document.getElementById("cbQnty").value = document.getElementById("ctl00_ContentPlaceHolder1_HiddenSIMCount").value;
                ShowPackageDatails();
            }
            preventJumping();
        }

        function GoToErr() {
            $('html, body').animate({ scrollTop: $('#ctl00_ContentPlaceHolder1_lblMsg').offset().top - 150 });
            $('#collapseOne').removeClass('in').addClass('collapse');
            ResponseEnd();
            ShowDataIfChooseAlready();

        }
        function preventJumping() {
            /*scrollBillingInformationHeader = 250 + $('#ctl00_ContentPlaceHolder1_lblMsg').height();
            scrollPaymentMethodHeader = 350 + $('#ctl00_ContentPlaceHolder1_lblMsg').height() * 6;
            */
            document.getElementsByClassName('BillingInformationHeader')[0].addEventListener("click", printMousePosb);
            document.getElementsByClassName('ChoosePaymentMethodHeader')[0].addEventListener("click", printMousePosp);
        }

        function printMousePosb(e) {
            var cursorX = e.clientX; var cursorY = e.clientY;
            scrollBillingInformationHeader = $('#ctl00_ContentPlaceHolder1_lblMsg').height() + 67 * 3; //cursorY - $('#collapse2').height();
            $("html, body").animate({ scrollTop: scrollBillingInformationHeader }, 0);


        }
        function printMousePosp(e) {
            var cursorX = e.clientX; var cursorY = e.clientY;
            scrollPaymentMethodHeader = $('#ctl00_ContentPlaceHolder1_lblMsg').height() + 67 * 4;
            $("html, body").animate({ scrollTop: scrollPaymentMethodHeader }, 0);
        }


        function ViewStateAccessories() {
            sender = $find("<%= RadComboShipping.ClientID %>");
            args = sender.findItemByValue(sender.get_value());
            if (args != null) {
                ship_selected(sender, null);
                for (key in simpleCartItem)
                    simpleCart.add(simpleCartItem[key]);
            }
            else $('#Step8').hide();
        }

        function ViewState(simQnt) {

            var simDetails = document.getElementById("<%= RadSimDetails.ClientID %>").value.split('|');
            var kntDetails = document.getElementById("<%= KNTSimDetails.ClientID %>").value.split('|');
            if (simDetails[0] == "" && kntDetails[0] == "") return;
            var i = 1;

            for (i; i <= simQnt; i++) {
                try {
                    var SelectdValueSIM = simDetails[i - 1].split(',');
                    if (SelectdValueSIM == "null" || SelectdValueSIM == "") {
                        onSimTypeClick($('#divSimType_' + i)[0]);
                    }
                    else {
                        var name = 'xSimtype_' + i;
                        $('input[name=' + name + '][value=' + SelectdValueSIM[0] + ']').prop('checked', true);
                    }

                    var SelectdValueKNT = kntDetails[i - 1];
                    if (SelectdValueKNT == "null" || SelectdValueKNT == "") {
                        SelectdValueKNT = "0";
                        onSelectRegionClick($('#SelectRegion' + i)[0]);
                    }

                    $("#SelectRegion" + i).val(SelectdValueKNT);






                }
                catch (err) {

                    return;
                }

            }
        }
        function RequestStart() {
            //close the Cart
            $("aside").removeClass("visible");
            $('.Curtain').addClass("visible");
            $('.warpper').addClass("warpper-visable");

            $(".smallanimationload").fadeIn("slow");
            isEndOfOrder = true;
        }

        //place the order 
        function clickOK(b) {
            isEndOfOrder = true;
            firstTime = false;
            document.getElementById('ctl00_ContentPlaceHolder1_lblMsg').innerHTML = '';
            if (b == null)
                b = document.getElementById('<%= Button2.ClientID %>');
            b.disabled = true;
            b.setAttribute('class', 'btnSubmitDisabled');
            b.setAttribute('className', 'btnSubmitDisabled');

            saveAccessoriesData();
            if (ValidateData() == true) {
                RequestStart();
                document.getElementById('<%=Button2.ClientID %>').click();
            }
            return true;
        }
        function updateErrHtmlFunc(result) {
            if (firstTime == true)
                return;
            document.getElementById('ctl00_ContentPlaceHolder1_lblMsg').innerHTML = result;
            b = document.getElementById('<%= Button2.ClientID %>');
            b.disabled = false;
            b.setAttribute('class', 'btnSubmit');
            b.setAttribute('className', 'btnSubmit');
        }
        var simpleCartItem = [];
        function saveAccessoriesData() {
            var array = simpleCart.allItems();
            var AccessoriesDetails = "";
            simpleCartItem = [];
            for (key in array) {
                if (array.hasOwnProperty(key)) {
                    var value = array[key];
                    if (value.itemtype != null) {//=="AC")
                        AccessoriesDetails += value.itemtype + ',' + value.name + ',' + value.quantity + ',' + value.price + '|';
                        simpleCartItem.push(array[key]);
                    }
                }
            }
            document.getElementById("<%= RadAccessoriesDetails.ClientID %>").value = AccessoriesDetails;
        }

        var Mode = "";
        var done = false;
        function SaveSimData() {
            var SimQnt = document.getElementById("cbQnty").value;
            var simDetails = "";
            var kntDetails = "";
            var IMEINumbers = "";
            var pricePerItem = parseFloat(document.getElementById("<%= HiddenPlanPrice.ClientID %>").value);
            document.getElementById("<%= HiddenDefaultKNTCode.ClientID %>").value = parseInt($('#DefaultKNTCod').val());
            var defaultKNTCode = parseInt(document.getElementById("<%= HiddenDefaultKNTCode.ClientID %>").value);
            $('.packageDetails3').each(function () {
                var pkgNum = $(this).attr('id').replace('packageDiv', '');
                simDetails += $('input[name=xSimtype_' + pkgNum + ']:checked').val() + ',';
                simDetails += '0 ,';
                simDetails += "0";
                IMEINumbers += "0";
                simDetails += '|';
            });
            $('.packageDetails3').each(function () {
                var pkgNum = $(this).attr('id').replace('packageDiv', '');
                var divKntCode = $("#SelectRegion" + pkgNum);
                if (defaultKNTCode > 0)
                    kntDetails += defaultKNTCode;
                else kntDetails += $("#SelectRegion" + pkgNum).val();

                kntDetails += '|';
            });
            if (Mode == "Upsale") {
                if (done == false) {
                    done = true;
                    document.getElementById("<%= RadSimDetails.ClientID %>").value += simDetails;
                    simDetails = document.getElementById("<%= RadSimDetails.ClientID %>").value;
                    document.getElementById("<%= strIMEIDetails.ClientID %>").value += IMEINumbers.substring(0, IMEINumbers.length - 1);
                    document.getElementById("<%= KNTSimDetails.ClientID %>").value += kntDetails;
                }
            }
            else {
                document.getElementById("<%= RadSimDetails.ClientID %>").value = simDetails;
                document.getElementById("<%= strIMEIDetails.ClientID %>").value = IMEINumbers.substring(0, IMEINumbers.length - 1);
                document.getElementById("<%= KNTSimDetails.ClientID %>").value = kntDetails;
            }

            //Show(simDetails,pricePerItem,SimQnt);
            var res = simDetails.split("|");
            res.sort();
            var sum = {}, i, counter = 0;
            for (i = 1; i < res.length; i++) {
                var name = res[i].replace(",0 ,0", '')
                var oldSum = sum[name];
                if (typeof oldSum === 'undefined')
                    counter = 1;
                else counter++;
                sum[name] = counter;
            }

            var price = parseFloat(document.getElementById("<%= HiddenPlanPrice.ClientID %>").value);
            var simPrice = parseFloat(document.getElementById("<%= HiddenSIMPrice.ClientID %>").value);
            //PageMethods.SetChart(sum,price,simPrice,SimQnt,kntDetails,simDetails,tbodyShopping); 
            if (parseFloat(document.getElementById("<%= HiddenSIMPrice.ClientID %>").value) > 0)
                SetCart(sum, simPrice, SimQnt);
            AddToTatal((price * SimQnt) + (simPrice * SimQnt));

        }

        function Show(simDetails, pricePerItem, SimQnt) {
            var res = simDetails.split("|");
            res.sort();
            var sum = {}, i, counter = 0;
            for (i = 1; i < res.length; i++) {
                var name = res[i].replace(",0 ,0", '')
                var oldSum = sum[name];
                if (typeof oldSum === 'undefined')
                    counter = 1;
                else counter++;
                sum[name] = counter;
            }

            var price = parseFloat(document.getElementById("<%= HiddenPlanPrice.ClientID %>").value);
            var simPrice = parseFloat(document.getElementById("<%= HiddenSIMPrice.ClientID %>").value);
            //PageMethods.SetChart(sum,price,simPrice,SimQnt,tbodyShopping); 
            //SetCart(sum,price,simPrice,SimQnt);
            AddToTatal((price * SimQnt) + (simPrice * SimQnt));




        }

        function GetSIMName(simCode) {
            var NanoSimTxt = document.getElementById("<%= HiddenNanoSimTxt.ClientID %>").value;
            var MicroSimTxt = document.getElementById("<%= HiddenMicroSimTxt.ClientID %>").value;
            var SimTxt = document.getElementById("<%= HiddenSimTxt.ClientID %>").value;
            if (simCode == "1250") return SimTxt;// "USA Regular SIM";
            if (simCode == "1430") return MicroSimTxt; //"USA Micro SIM";
            if (simCode == "1440") return NanoSimTxt;// "USA Nano SIM"; 
            return SimTxt;// "USA Regular SIM";
        }

        //update the cart --insted of using PageMethods.SetChart
        var simNumbersQnt = [0, 0, 0];
        function SetCart(sum, simPrice, SimQnt) {
            var myItem;
            var arr = $.map(sum, function (value, index) { return [index, value]; });
            var simNumbers = [1250, 1430, 1440];

            for (var i = 0; i < 3; i++) {
                if (sum[simNumbers[i]] == null) {
                    myItem = simpleCart.add({ name: GetSIMName(simNumbers[i]), price: simPrice, quantity: 1, thumb: 'img/telawaySim.jpg' });
                    //myItem.decrement(simNumbersQnt[i]+1);
                    myItem.set("quantity", 0);
                    myItem.remove();
                    simpleCart.update();
                    simNumbersQnt[i] = 0;
                }
                else {
                    myItem = simpleCart.add({ name: GetSIMName(simNumbers[i]), price: simPrice, quantity: sum[simNumbers[i]], thumb: 'img/telawaySim.jpg' });

                    myItem.set("quantity", sum[simNumbers[i]]);
                    simpleCart.update();
                    simNumbersQnt[i] = sum[simNumbers[i]];
                }
            }

        }
        //result from PageMethods.SetChart
        function tbodyShopping(result) {
            ctl00_ContentPlaceHolder1_PlanDetails.innerHTML = result;
        }

        function ValidateIMEIDone(msg) {
            if (msg != "") {
                $('#lblErr').html(msg);
                window.scrollTo(0, 0);
                return false;
            }
            else {

                return true;
            }
        }
        Function.prototype.curry = function () {
            var method = this, args = Array.prototype.slice.call(arguments);
            return function () {
                return method.apply(this, Array.prototype.slice.call(arguments).concat([args]));
            };
        };

        function ClearComboSelection(country) {
            if (country == "USA")
                ComboState = $find("<%= RadComboStateUSA.ClientID %>");
            else if (country == "Canada")
                ComboState = $find("<%= RadComboStateC.ClientID %>");
            else if (country == "Australia")
                ComboState = $find("<%= RadComboStateAU.ClientID %>");

    if (ComboState != null) {
        var item = ComboState.findItemByText("");
        if (item != null)
            item.select();

    }

}

function OnAgreeChecked() {
    var cb = "ctl00_ContentPlaceHolder1_CheckBoxAgree";
    var cbE = document.getElementById(cb);

    if (cbE.checked) {
        var s = "ctl00_ContentPlaceHolder1_DivAgree";
        var txtDiv = document.getElementById(cb);
        txtDiv = $('.agree')[0];
        txtDiv.style.border = "0px solid red";
        PageMethods.UpdateErrHtml('InValidAgree', false, updateErrHtmlFunc);
        var b = document.getElementById('<%= Button2.ClientID %>');
        b.disabled = false;
        b.setAttribute('class', 'btnSubmit');
        b.setAttribute('className', 'btnSubmit');
    }
    else PageMethods.UpdateErrHtml('InValidAgree', true, updateErrHtmlFunc);
}




function ShowShipDetails() {
    $('#Step5').show('slow');
    txtChanged1('shipOpAddress', 'InValidshipOpAddress');
    var RadComboStateUSShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateUShip');
    var RadComboStateAUShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateAUShip');
    var RadComboStateCShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateCShip');
    var StateDivShip = document.getElementById('ctl00_ContentPlaceHolder1_StateDivShip');

    var Country = $find("<%= RadComboShipping.ClientID %>");
    var item = Country.get_selectedItem();
    var CountryName = "";
    var isPickup = item.get_attributes().getAttribute("optLocalPickup");
    if (item != null) CountryName = item.get_attributes().getAttribute("CountryName");

    var shipCountryDv = document.getElementById('ctl00_ContentPlaceHolder1_RadTxtShipCountry');

    $('#ctl00_ContentPlaceHolder1_ShipDetails').show('slow');
    //document.getElementById('ctl00_ContentPlaceHolder1_ShipDetails').style.display = "block";    
    //document.getElementById('ctl00_ContentPlaceHolder1_ShipDetails').style.borderColor = '#c1c1c1'; 
    //document.getElementById('ctl00_ContentPlaceHolder1_divShipDate').style.display = "none";  

    var hiddenCountryDiv = document.getElementById("<%= shipcountryHidden.ClientID %>");
    var hiddenCountry = hiddenCountryDiv.value;

    if (hiddenCountry == "usa")
        document.getElementById('ctl00_ContentPlaceHolder1_StateDivShip').style.display = "block";
    else document.getElementById('ctl00_ContentPlaceHolder1_StateDivShip').style.display = "none";

    RadComboStateUSShip.style.display = 'none';
    RadComboStateCShip.style.display = 'none';
    RadComboStateAUShip.style.display = 'none';
    StateDivShip.style.display = 'none';

    if (CountryName.toUpperCase() == "USA") {
        StateDivShip.style.display = 'block';
        RadComboStateUSShip.style.display = 'block';
    }

    if (CountryName.toUpperCase() == "AUSTRALIA") {
        StateDivShip.style.display = 'block';
        RadComboStateAUShip.style.display = 'block';
    }

    if (CountryName.toUpperCase() == "CANADA") {
        StateDivShip.style.display = 'block';
        RadComboStateCShip.style.display = 'block';
    }

    shipCountryDv.value = CountryName;

    if (hiddenCountry != "israel") {
        document.getElementById('ctl00_ContentPlaceHolder1_ZipDivShip').style.display = "block";
        if (isPickup == "True") document.getElementById('ctl00_ContentPlaceHolder1_DateLeaveDiv').style.display = "none";
        else document.getElementById('ctl00_ContentPlaceHolder1_DateLeaveDiv').style.display = "block";

    }


}

function HideShipDetails(sts) {
    if (sts != 'NO')
        txtChanged1('shipOpAddress', 'InValidshipOpAddress');
    $('#ctl00_ContentPlaceHolder1_ShipDetails').hide('slow');
    $('#Step5').hide('slow');
    // document.getElementById('ctl00_ContentPlaceHolder1_ShipDetails').style.display = "none";    

}

function OnClientItemsRequestedHandler(sender, eventArgs) {
    //set the max allowed height of the combo   
    var MAX_ALLOWED_HEIGHT = 120;
    //this is the single item's height   
    var SINGLE_ITEM_HEIGHT = 12;
    var calculatedHeight = sender.get_items().get_count() * SINGLE_ITEM_HEIGHT;
    var dropDownDiv = sender.get_dropDownElement();
    if (calculatedHeight > MAX_ALLOWED_HEIGHT) {
        setTimeout(
            function () {
                dropDownDiv.firstChild.style.height = MAX_ALLOWED_HEIGHT + "px";
            }, 20
        );
    }
    else {
        setTimeout(
            function () {
                dropDownDiv.firstChild.style.height = calculatedHeight + "px";
            }, 20
        );
    }
}



function txtChanged(sender, eventArgs, params) {

    var maxLen = '';
    if (params.length > 2)
        maxLen = params[2];

    var name = params[1];
    var div = params[0];
    var s = "ctl00_ContentPlaceHolder1_" + div;
    var txtDiv = document.getElementById(s);

    var value = eventArgs.get_newValue();
        switch (sender.get_id()) {
            case "ctl00_ContentPlaceHolder1_RadTxtCell":
                $("#<%=hdnCell.ClientID %>").val(value);
                        break;
                    case "ctl00_ContentPlaceHolder1_RadTxtDeliveryPhone":
                        $("#<%=hdDeliveryPhone.ClientID %>").val(value);
                        break;
                    case "ctl00_ContentPlaceHolder1_RadTxtPhone":
                        $("#<%=hdPhone.ClientID %>").val(value);
                        break;
                    default: "";
            }


            if (firstTime == true) return;
            if (eventArgs.get_newValue() != "") {
                if (maxLen != '') {
                    if (eventArgs.get_newValue().length < maxLen) {
                        txtDiv.style.border = "2px solid red";
                        PageMethods.UpdateErrHtml(name, true, updateErrHtmlFunc);
                    }
                    else {
                        txtDiv.style.border = "0px solid red";
                        PageMethods.UpdateErrHtml(name, false, updateErrHtmlFunc);
                    }
                }
                else {
                    txtDiv.style.border = "0px solid red";
                    PageMethods.UpdateErrHtml(name, false, updateErrHtmlFunc);
                }
            }
            else {
                txtDiv.style.border = "0px solid red";
                PageMethods.UpdateErrHtml(name, false, updateErrHtmlFunc);
            }


        }

        function checkNumber(e) {
            var k = e.which;
            if (e.keyCode > 47 && e.keyCode < 58) {//numbers            
            }
            else return false;

        }

        function ComboCountrySelected(sender, eventArgs) {
            txtChanged1('DivCountry', 'InValidCountry');
            var value = sender.get_value();
            var comboStateUsa = document.getElementById("ctl00_ContentPlaceHolder1_RadComboStateUSA");
            var comboStateC = document.getElementById("ctl00_ContentPlaceHolder1_RadComboStateC");
            var comboStateAU = document.getElementById("ctl00_ContentPlaceHolder1_RadComboStateAU");

            var ZipDiv = document.getElementById("ctl00_ContentPlaceHolder1_ZipDiv");
            var StateDiv = document.getElementById("ctl00_ContentPlaceHolder1_StateDiv");
            var hiddenCountryDiv = document.getElementById("<%= shipcountryHidden.ClientID %>");

    //hiddenCountryDiv.value=value;

    if (value == "Israel") ZipDiv.style.display = "none";
    else ZipDiv.style.display = "block";

    if ((value != "USA") && (value != "Canada") && (value != "Australia"))
        StateDiv.style.display = "none";

    else {
        StateDiv.style.display = "block";
        var Province = document.getElementById("<%= HiddenProvience.ClientID %>").value;
        if (value == "USA") {
            comboStateUsa.style.display = "block";
            comboStateC.style.display = "none";
            comboStateAU.style.display = "none";
            //hiddenCountryDiv.value="USA";                   
            $("#ctl00_ContentPlaceHolder1_RadComboStateUSA_Input").val(Province);
            ClearComboSelection(value);
        }
        if (value == "Canada") {
            comboStateUsa.style.display = "none";
            comboStateC.style.display = "block";
            comboStateAU.style.display = "none";
            ClearComboSelection(value);
            $("#ctl00_ContentPlaceHolder1_RadComboStateC_Input").val(Province);
        }
        if (value == "Australia") {
            comboStateUsa.style.display = "none";
            comboStateC.style.display = "none";
            comboStateAU.style.display = "block";
            ClearComboSelection(value);
            $("#ctl00_ContentPlaceHolder1_RadComboStateAU_Input").val(Province);
        }

        IfNeedState();





    }
}
function IfNeedState() {

    $('.StateDiv').find('br').remove();
    $('.StateDiv').append("<br>");
}
function onSelectRegionClick(divKNT) {
    divKNT.style.border = "1px solid Gray";

    var isCheckedKNTCode = true;
    var counter = 0;
    document.getElementById("<%= KNTSimDetails.ClientID %>").value = "";
    $('.packageDetails3').each(function () {
        var pkgNum = $(this).attr('id').replace('packageDiv', '');
        kntCode = $("#SelectRegion" + pkgNum).val();
        var divKntCode = document.getElementById("SelectRegion" + pkgNum + '');
        if (kntCode == '0') {
            isCheckedKNTCode = false;
            divKntCode.style.border = "2px solid red";
        }
        else {
            counter++;
            document.getElementById("<%= KNTSimDetails.ClientID %>").value += kntCode + '|';
            }
        });
        if (counter == document.getElementById("cbQnty").value) {
            openAndcloseCart();
        }
        PageMethods.UpdateErrHtml('InValidKntCode', !isCheckedKNTCode, updateErrHtmlFunc);

    }



    function onSimTypeClick(divSim) {
        divSim.style.border = "none";//0px solid red";
        var isCheckedSimType = true;
        var pkgNum = divSim.id.replace('divSimType_', '');
        document.getElementById("<%= RadSimDetails.ClientID %>").value = "";

        $('.packageDetails3').each(function () {
            pkgNum = $(this).attr('id').replace('packageDiv', '');
            if ($('input[name=xSimtype_' + pkgNum + ']:checked').length > 0)
                document.getElementById("<%= RadSimDetails.ClientID %>").value += $('input[name=xSimtype_' + pkgNum + ']:checked').val() + '|';
            else {
                var divSimType = document.getElementById("divSimType_" + pkgNum + '');
                if (divSimType != null)
                    isCheckedSimType = false;
            }
        });
        openAndcloseCart();
        PageMethods.UpdateErrHtml('InValidSimType', !isCheckedSimType, updateErrHtmlFunc);

    }

    function OnClientBlurHandler(sender, eventArgs) {
        var textInTheCombo = sender.get_text();
        var item = sender.findItemByText(textInTheCombo);
        //if there is no item with that text
        if (!item) {
            sender.set_text("");
            setTimeout(function () {
                var inputElement = sender.get_inputDomElement();
                inputElement.focus();
                //inputElement.style.backgroundColor = "red";
            }, 20);
        }
    }
    function StateChanged() {
        var s = "ctl00_ContentPlaceHolder1_StateDiv";
        var txtDiv = document.getElementById(s);
        txtDiv.style.border = "0px solid red";
        PageMethods.UpdateErrHtml('InValidState', false, updateErrHtmlFunc);
    }

    function ShipStateChanged() {
        var s = "ctl00_ContentPlaceHolder1_StateDivShip";
        var txtDiv = document.getElementById(s);
        txtDiv.style.border = "0px solid red";
        PageMethods.UpdateErrHtml('InValidShipState', false, updateErrHtmlFunc);
    }




    function DropDownOpened(sender, args) {
        $('.rcbSlide').css('z-index', '1001');
        //$(sender._animationContainer).attr("style","z-index:1000") 
    }

    function ship_selected(sender, args) {
        $('#Step6').show();

        var DivComboShipping = document.getElementById('ctl00_ContentPlaceHolder1_DivComboShipping');
        var shipCountryDv = document.getElementById('ctl00_ContentPlaceHolder1_RadTxtShipCountry');
        var shipOpAddress = document.getElementById('ctl00_ContentPlaceHolder1_shipOpAddress');
        var ShipDetails = document.getElementById('ctl00_ContentPlaceHolder1_ShipDetails');
        var StateDivShip = document.getElementById('ctl00_ContentPlaceHolder1_StateDivShip');
        var RadComboStateUSShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateUShip');
        var RadComboStateAUShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateAUShip');
        var RadComboStateCShip = document.getElementById('ctl00_ContentPlaceHolder1_RadComboBoxStateCShip');
        var item = args == null ? sender.findItemByValue(sender.get_value()) : args.get_item();
        var val = item == "" ? "" : item.get_value();
        var txt = item == "" ? "" : item.get_text();
        var desc = "";

        // add shipping to total amount
        var spnshipCost = document.getElementById('ctl00_ContentPlaceHolder1_spnShipCost');
        var currency = document.getElementById('ctl00_ContentPlaceHolder1_HiddenCurrency').value;
        var spnTotalAmount = document.getElementById('ctl00_ContentPlaceHolder1_TotalAmtPlusShipping');
        var spnOrderTotal = document.getElementById('ctl00_ContentPlaceHolder1_spnOrderTotal');
        var spntotalAmtDollars = document.getElementById('ctl00_ContentPlaceHolder1_totalAmtDollars');
        var shipCost = item == "" ? 0 : item.get_attributes().getAttribute("ShipCostGBP");
        var CountryName = item == "" ? "" : item.get_attributes().getAttribute("CountryName");
        var HiddenIncluding = document.getElementById('ctl00_ContentPlaceHolder1_HiddenIncluding').value;
        var conversionRate = document.getElementById("<%= HiddenConversionRate.ClientID %>").value;

        // change text 
        if (args != null)
            $('#spnBeforeAfterShipping').html(HiddenIncluding);

        // update shipping price and total amount
        var currAmount = parseFloat(spnOrderTotal.innerHTML) + parseFloat(shipCost);
        //spnTotalAmount.innerHTML = parseFloat(currAmount).toFixed(2);

        spntotalAmtDollars.innerHTML = parseFloat(currAmount * conversionRate).toFixed(2);



        var country = document.getElementById("<%= HiddenCountry.ClientID %>").value;

        simpleCart({ shippingFlatRate: shipCost });
        simpleCart.update();
        /*
        if (args!=null)    
            spnshipCost.innerHTML = shipCost;
        if (country=="FR")
        {
          spnTotalAmount.innerHTML=spnTotalAmount.innerHTML.replace(".",",")+currency;
          spnshipCost.innerHTML = spnshipCost.innerHTML.replace(".",",")+currency;
        }
      */
        if (val == "") return;
        else {
            if (val != "-1") {
                desc = item.get_attributes().getAttribute("optLocalPickup");
                ShipCountry = item.get_attributes().getAttribute("CountryName");
            }
            if (desc == "True") {
                shipOpAddress.style.display = 'none';
                ShipDetails.style.display = 'none';
                document.getElementById('ctl00_ContentPlaceHolder1_DateLeaveDiv').style.display = "none";
            }
            else {
                shipOpAddress.style.display = 'block';
                document.getElementById('ctl00_ContentPlaceHolder1_DateLeaveDiv').style.display = "block";
            }
            RadComboStateUSShip.style.display = 'none';
            RadComboStateAUShip.style.display = 'none';
            RadComboStateCShip.style.display = 'none';
            StateDivShip.style.display = 'none';

            if (CountryName.toUpperCase() == "USA") {
                StateDivShip.style.display = 'block';
                RadComboStateUSShip.style.display = 'block';
            }
            if (CountryName.toUpperCase() == "AUSTRALIA") {
                StateDivShip.style.display = 'block';
                RadComboStateAUShip.style.display = 'block';
            }
            if (CountryName.toUpperCase() == "CANADA") {
                StateDivShip.style.display = 'block';
                RadComboStateCShip.style.display = 'block';
            }
            shipCountryDv.value = CountryName;

            if (val == "0") {
                DivComboShipping.style.border = "2px solid red";
                PageMethods.UpdateErrHtml('InValidshipOp', true, updateErrHtmlFunc);
            }
            else {
                DivComboShipping.style.border = "0px solid red";
                PageMethods.SetShipCode(val, '', fSetShip);


                $.callWebService({
                    serviceName: 'BuildAccsessories',
                    data: {
                        ShippingId: val,
                        siteInfoName: country,
                        ConversionRate: conversionRate
                    },
                    success: function (data) {
                        if (data != "") {
                            $('.AccessoriesListC').html("");
                            $('.AccessoriesListC').append(data);
                            $('#Step8').show();
                            ShowAccessories = true;


                        }
                        else $('#Step8').hide();
                    }
                });
            }
        }
    }
    //result from SetShipCode
    function fSetShip() { }
    function CCTypeChanged() {
        var s = "ctl00_ContentPlaceHolder1_DivCCType";
        var txtDiv = document.getElementById(s);
        txtDiv.style.border = "0px solid red";
        PageMethods.UpdateErrHtml('InValidCCType', false, updateErrHtmlFunc);
    }

    function CCEXpChanged(sender, args) {
        var comboM = $find('<%=RadComboMonth.ClientID %>');
        var comboMVal = comboM.get_selectedItem() == null ? '' : comboM.get_selectedItem().get_value();
        var comboY = $find('<%=RadComboYear.ClientID %>');
        var comboYVal = comboY.get_selectedItem() == null ? '' : comboY.get_selectedItem().get_value();
        if (comboYVal != '' && comboMVal != '') {
            var s = "ctl00_ContentPlaceHolder1_DivCCExpDate";
            var txtDiv = document.getElementById(s);
            txtDiv.style.border = "0px solid red";
            PageMethods.UpdateErrHtml('InValidCCExpDate', false, updateErrHtmlFunc);
        }
        else PageMethods.UpdateErrHtml('InValidCCExpDate', true, updateErrHtmlFunc);
    }

    function txtChanged1(div, name) {
        if (firstTime == true) return;
        var s = "ctl00_ContentPlaceHolder1_" + div;
        var txtDiv = document.getElementById(s);
        txtDiv.style.border = "0px solid red";
        PageMethods.UpdateErrHtml(name, false, updateErrHtmlFunc);
    }

    function ShowCCDetails() {
        document.getElementById('ctl00_ContentPlaceHolder1_CCDiv').style.display = "block";
        document.getElementById('ctl00_ContentPlaceHolder1_PayPalDiv').style.display = "none";
        document.getElementById('ctl00_ContentPlaceHolder1_ThisTransWillApearTNS').style.display = "block";
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC1.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC2.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC3.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC4.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC5.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC6.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC7.ClientID %>'), true);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC8.ClientID %>'), true);
    }

    function HideCCDetails() {
        txtChanged.curry('PayPalDiv', 'InValidPayment');
        document.getElementById('ctl00_ContentPlaceHolder1_CCDiv').style.display = "none";
        document.getElementById('ctl00_ContentPlaceHolder1_PayPalDiv').style.display = "block";
        document.getElementById('ctl00_ContentPlaceHolder1_ThisTransWillApearTNS').style.display = "none";
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC1.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC2.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC3.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC4.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC5.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC6.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC7.ClientID %>'), false);
        ValidatorEnable(document.getElementById('<%= RequiredFieldValidatorCC8.ClientID %>'), false);
    }


    function ShowDatePopup(sender, args) {
        if (sender._clientID == "ctl00_ContentPlaceHolder1_RadDateStartDate_dateInput") Popup_Side("SpStartDate");
        else if (sender._clientID == "ctl00_ContentPlaceHolder1_RadDateEndDate_dateInput") Popup_Side("SpEndDate");
        else Popup_Side("SpDeliveryDate");
    }

    //open the calander     
    function Popup_Side(Span) {
        if (Span == "SpStartDate") {
            var datePickerD = $find("<%=RadDateStartDate.ClientID %>");
            datePickerD.showPopup();
        }
        if (Span == "SpEndDate") {
            var datePickerD = $find("<%=RadDateEndDate.ClientID %>");
            datePickerD.showPopup();
        }
        if (Span == "SpDeliveryDate") {
            var datePickerD = $find("<%=RadDeliveryDate.ClientID %>");
                    datePickerD.showPopup();
                }
                $('.RadCalendarPopup').css('z-index', '1000');

            }

            var hash = window.location.hash;
            hash = function () {
                $('html, body').animate({
                    scrollTop: $(hash).offset().top
                });
            };
            var scrollBillingInformationHeader;
            var scrollPaymentMethodHeader;
            var ShowAccessories = false;
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function (e) {
                // re-bind jQuery events 
                DoAlljQuery(e);
            });
            $(document).ready(function (e) {
                // bind jQuery events 
                simpleCart.empty();
                DoAlljQuery(e);
            });

            function DoAlljQuery(e) {



                document.getElementById("ctl00_ContentPlaceHolder1_RadDateEndDate_wrapper").style.removeProperty("display");
                document.getElementById("ctl00_ContentPlaceHolder1_RadDateStartDate_wrapper").style.removeProperty("display");
                $('.RadCalendarPopup').css('z-index', '1000');
                $('#SpStartDate').click(function (e) {
                    var posX = $(this).offset().left, posY = $(this).offset().top;
                    var datePickerD = $find("<%=RadDateStartDate.ClientID %>");
                    datePickerD.showPopup(e.pageX, e.pageY);


                });
                $('#SpEndDate').click(function (e) {
                    var posX = $(this).offset().left, posY = $(this).offset().top;
                    var datePickerD = $find("<%=RadDateEndDate.ClientID %>");
                    datePickerD.showPopup(e.pageX, e.pageY);

                });

                $('#SpDeliveryDate').click(function (e) {
                    var posX = $(this).offset().left, posY = $(this).offset().top;
                    var datePickerD = $find("<%=RadDeliveryDate.ClientID %>");
                    datePickerD.showPopup(e.pageX, e.pageY);

                });


                simpleCart({ currency: document.getElementById('ctl00_ContentPlaceHolder1_HiddenCurrencyName').value });
                simpleCart({ conversionRate: document.getElementById('ctl00_ContentPlaceHolder1_HiddenConversionRate').value });
                hideDollarPrice();
                if (firstTime == true) {
                    $('#Step2').hide();
                    $('#Step3').hide();
                    //$('#Step4').hide();
                    $('#Step5').hide();
                    $('#Step6').hide();
                    $('#Step8').hide();
                }




                $('.addtocart').on('click', function () {
                    rocketcss(this, '.target', 'rocketPulse');
                    //simpleCart.add('quantity=1','name=3 USA-to-Europe Adapters','price=6.97','thumb="TAImages/01_shop.jpg');            
                    $('.target').addClass('targetPulse');
                    updateItems(-1);
                });
                $('.cart-form').on('click', function () { openAndcloseCart('close'); });
                $('.btn-coupon').on('click', function () {
                    var coupon = $('.coupon-input').val();
                    if (coupon != '') {
                        var SimQnt = document.getElementById("cbQnty").value;
                        var itemT1Order = parseFloat(choosenPlanPrice) + parseFloat(choosenSIMPrice);
                        var ItemT = itemT1Order * SimQnt;//simpleCart.grandTotalWithDollar();            
                        $.callWebService({
                            serviceName: 'ValidCoupons',
                            data: { CouponCode: coupon, ItemTotal: ItemT, itemTotal1Order: itemT1Order, simQnt: SimQnt },
                            success: function (data) {
                                var promoCpde = data.split('|');
                                if (data == "" || data == "0|") { $('.coupon-form').append("<div class='coupon-error'>Not valid</div>"); return; }
                                if (promoCpde[1] != "") { $('.coupon-form').append("<div class='coupon-error'>" + promoCpde[1] + "</div>"); return; }
								if (promoCpde[0] == "0") { $('.coupon-form').append("<div class='coupon-error'>" + promoCpde[1] + "</div>"); return; }
                                if (promoCpde[2] != "")
                                    if (promoCpde[2] == "Percent") {}
                                    else if (promoCpde[2] == "Credit") {
                                        promoCpde[0] = (promoCpde[0] / document.getElementById("ctl00_ContentPlaceHolder1_HiddenConversionRate").value);
                                    }
                                simpleCart({ promoCode: promoCpde[0] });
                                simpleCart.grandTotalPromoCode();
                                $(".coupon-error").remove();
                                $(".cart-applied-promo").empty();
                                $('.cart-applied-promo').append("<dl class='calculation adjustments'><dt>Promo: " + coupon + "</dt><dd class='simpleCart_promoCode'>- " + simpleCart.toCurrency(promoCpde[0]) + "</dd></dl>");

                                var radPromoCode = $find("<%= RadPromoCode.ClientID %>");
                                radPromoCode.set_value(coupon);


                            }
                        });
                    }
                    else $('.coupon-form').append("<div class='coupon-error'>Not valid</div>");
                });

                //prevent from Billing informayion to jump         
                $('.BillingInformationHeader').on('click', (function () {
                    $("html, body").animate({ scrollTop: 250 + $('#ctl00_ContentPlaceHolder1_lblMsg').height() }, 0);
                }));
                $('.ChoosePaymentMethodHeader').on('click', (function () {
                    $("html, body").animate({ scrollTop: 350 + $('#ctl00_ContentPlaceHolder1_lblMsg').height() }, 0);
                    if (ShowAccessories == true)
                        setTimeout(function () {
                            $('#collapseSeven').addClass('in').removeClass('collapse');
                        }, 20);

                }));


                $('.target').on('click', (function () { isEndOfOrder = false; openAndcloseCart() }));
                $('.cart-close-btn').on('click', (function () {
                    $("aside").removeClass("visible");
                    $('.Curtain').removeClass("visible");
                    $('.warpper').removeClass("warpper-visable");
                }));
                $('.Curtain').on('click', (function () {
                    if (!isEndOfOrder) {
                        $("aside").removeClass("visible");
                        $('.Curtain').removeClass("visible");
                        $('.warpper').removeClass("warpper-visable");
                        isEndOfOrder = false;
                    }
                }));
            }

            var isEndOfOrder = false;
            function openAndcloseCart(mode) {
                var res = ValidateData();
                if (Mode != "Upsale") {
                    $("aside").removeClass("visible");
                    $('.Curtain').removeClass("visible");
                    $('.warpper').removeClass("warpper-visable");
                    if ((res == true && mode != 'close') || (mode != 'close' && simpleCart.quantity() == 0)) {
                        $("aside").addClass("visible");
                        $('.Curtain').addClass("visible");
                        $('.warpper').addClass("warpper-visable");
                    }
                }
            }
            function DeliveryDateSelected(sender, eventArgs) { }
            //Start date selected
            function StartDateSelected(sender, eventArgs) {
                var endDatePicker = $find("<%= RadDateEndDate.ClientID %>");
                var startDatePicker = $find("<%= RadDateStartDate.ClientID %>");
                var EndDate1 = endDatePicker.get_selectedDate();
                if (EndDate1 != null)
                    DateSelected();
            }

            //End date selected
            function EndDateSelected(sender, eventArgs) {
                DateSelected();
            }

            function DateSelected(sender, eventArgs) {
                var endDatePicker = $find("<%= RadDateEndDate.ClientID %>");
                var startDatePicker = $find("<%= RadDateStartDate.ClientID %>");
                var EndDate1 = endDatePicker.get_selectedDate();
                var NewEndDate = EndDate1.format("MM/dd/yyyy");
                var strNewEndDate = NewEndDate.split("/");
                var eY = strNewEndDate[2];
                var eM = strNewEndDate[0] - 1 + 1;
                var eD = strNewEndDate[1];
                var StartDate1 = startDatePicker.get_selectedDate();
                var NewStartDate = StartDate1.format("MM/dd/yyyy");
                var strNewStartDate = NewStartDate.split("/");
                var sY = strNewStartDate[2];
                var sM = strNewStartDate[0] - 1 + 1;
                var sD = strNewStartDate[1];
                if (StartDate1 != null && EndDate1 != null) {
                    //PageMethods.GetDaysBetween(sD, sM, sY, eD, eM, eY, PMGetDays);
                    PMGetDays(GetDaysBetween(EndDate1, StartDate1));
                }
            }

            function GetDaysBetween(endDate, startDate) {
                //Difference in days
                var differenceInDays = Math.round((endDate - startDate) / 1000 / 60 / 60 / 24);
                return differenceInDays;
            }

            var choosenPlanTitel = 0;
            var choosenPlanPrice = 0;
            var choosenSIMPrice = 0;
            //result from PageMethods.GetDaysBetween

            function PMGetDays(result) {
                var country = document.getElementById("<%= HiddenCountry.ClientID %>").value;
                var PlanCount = document.getElementById("<%= HiddenPlanCount.ClientID %>").value;
                var endDatePicker = $find("<%= RadDateEndDate.ClientID %>");
                var startDatePicker = $find("<%= RadDateStartDate.ClientID %>");
                var EndDateBeforeStartDate = document.getElementById("<%= EndDateBeforeStartDate.ClientID %>").value;
                var ForOrderOver60 = document.getElementById("<%= ForOrderOver60.ClientID %>").value;
                datediff = result;

                choosenSIMPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('simPrice');
                choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                if (datediff < 0) {
                    alert(EndDateBeforeStartDate);
                    //endDatePicker.clear(); 
                    datediff = 1;
                    endDatePicker.set_selectedDate(startDatePicker.get_selectedDate());
                    document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = 1;
                    choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
                    choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                }
                else if (datediff >= 0) {
                    datediff += 1;
                    country = document.getElementById("<%= HiddenUserCountry.ClientID %>").value;
                    phone = document.getElementById("<%= HiddenPhone.ClientID %>").value;
                    choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
                    choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                    if (PlanCount == "2") {
                        if (datediff < 31) {
                            choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
                            choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                        }
                        else if (datediff < 61) {
                            choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price2').attr('price');
                            choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price2').attr('plan');
                        }
                        else if (datediff > 60) {
                            alert(ForOrderOver60 + ": " + phone);
                            datediff = 1;
                            choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price2').attr('price');
                            choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price2').attr('plan');
                            //endDatePicker.clear(); 
                            endDatePicker.set_selectedDate(startDatePicker.get_selectedDate());
                            document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = 1;
                        }
            }
            else if (PlanCount == "3") {
                if (datediff < 11) {
                    choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
                    choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                }
                else if (datediff <= 30 && datediff >= 11) {
                    choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price2').attr('price');
                    choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price2').attr('plan');
                }
                else if (datediff > 30 && datediff <= 60) {
                    if ((country == "CA") || (country == "FR")) {
                        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price3').attr('price');
                        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price3').attr('plan');
                    }
                    else {
                        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price3').attr('price');
                        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price3').attr('plan');
                    }
                }
                else if (datediff > 60) {
                    alert(ForOrderOver60 + ": " + phone);
                    datediff = 1;
                    choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
                    choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
                    //endDatePicker.clear(); 
                    endDatePicker.set_selectedDate(startDatePicker.get_selectedDate());
                    document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = 1;
                }
}
else {
    if (datediff < 11) {
        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
    }
    if (datediff <= 30 && datediff >= 11) {
        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price2').attr('price');
        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price2').attr('plan');
    }

    if (datediff > 30 && datediff <= 45) {
        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price3').attr('price');
        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price3').attr('plan');
    }
    if (datediff > 45 && datediff <= 60) {
        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Plan4').attr('price');
        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Plan4').attr('plan');
    }
    else if (datediff > 60) {
        alert(ForOrderOver60 + " :" + phone);
        datediff = 1;
        //endDatePicker.clear();
        endDatePicker.set_selectedDate(startDatePicker.get_selectedDate());
        choosenPlanPrice = $('#ctl00_ContentPlaceHolder1_Price1').attr('price');
        choosenPlanTitel = $('#ctl00_ContentPlaceHolder1_Price1').attr('plan');
    }
}
}
    document.getElementById("<%= HiddenPlanPrice.ClientID %>").value = choosenPlanPrice;
                document.getElementById("<%= HiddenSIMPrice.ClientID %>").value = choosenSIMPrice;
                document.getElementById("<%= HiddenPlanTitel.ClientID %>").value = choosenPlanTitel;

                //$('#Step3').slideDown('slow'); 
                //choosenPlanPrice=choosenPlanPrice.replace(",",".");
                //alert("choosenPlanPrice: "+choosenPlanPrice);
                var item = document.getElementById("cbQnty").value;
                if (item > 0) {
                    AddToTatal(choosenPlanPrice, false);
                    AddSIMToTatal(choosenSIMPrice);
                }
                if (datediff < 0) document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = 1;
                else {
                    document.getElementById("<%= totalDaysDis.ClientID %>").innerHTML = datediff;
                    var SimQnt = document.getElementById("cbQnty").value;
                    if (SimQnt > 0)
                        updateItems(SimQnt);
                }
                //var combo = $find("ctl00_ContentPlaceHolder1_cbQnty");
                //var item = combo.findItemByValue(0);
                //renderPackageDetails(1);

                document.getElementById("<%= HiddenDays.ClientID %>").value = datediff;
                $('#Step2').show();
                //$('#collapse2').slideDown('slow');
                //document.getElementById("step2").style.display = "block";
                //$('#spaceEnd').hide();
            }


            //count the items in the cart
            var Accsesoreies = 0;
            var NumOfSim = 0;
            function updateItems(SimQnt) {

                var myItem;
                if (isNaN(SimQnt)) {
                    saveAccessoriesData();
                    simpleCart.each(function () { simpleCart.empty(); });
                    simpleCart.empty();
                }
                else {
                    if (SimQnt > -1) {

                        if (Accsesoreies == 0) {
                            simpleCart.each(function () { simpleCart.empty(); });
                            simpleCart.empty();
                            itemsQty.innerHTML = parseInt(SimQnt);
                            myItem = simpleCart.add({ name: choosenPlanTitel, price: choosenPlanPrice, quantity: SimQnt, thumb: 'images/Plan.png' });
                            NumOfSim = parseInt(SimQnt);
                        }
                        else {
                            myItem = simpleCart.add({ name: choosenPlanTitel, price: choosenPlanPrice, quantity: SimQnt, thumb: 'images/Plan.png' });
                            myItem.decrement(Math.abs(parseInt(SimQnt) - NumOfSim));
                            myItem.quantity(SimQnt);
                            itemsQty.innerHTML = parseInt(SimQnt) + Accsesoreies;
                        }

                        simpleCart.update();
                    }
                    else {

                        itemsQty.innerHTML = parseInt(itemsQty.innerText) + 1;
                        Accsesoreies++;


                    }
                }
                $('.target').removeClass('targetPulse');

                /*var currency= document.getElementById('ctl00_ContentPlaceHolder1_HiddenCurrency').value;
            var spnOrderAmount=document.getElementById('ctl00_ContentPlaceHolder1_spnOrderTotal'); 
            var spnTotalPlsShipAmount = document.getElementById('ctl00_ContentPlaceHolder1_TotalAmtPlusShipping');
            $.callWebService({
                serviceName: 'CalcSum', 
                data: {Price: price, SimPrices:simPrice, SimQnt:SimQnt,CurrencySymbol:currency},
                success: function (data) {
                    $.each(data, function (i, item) {
                        //spnOrderAmount.innerHTML =item.strTotalCharge;
                        //spnTotalPlsShipAmount.innerHTML =item.strTotalCharge;
                   });
               }
             });*/


                simpleCart.update();
                $('.target').addClass('targetPulse');
                itemsQty.innerHTML = itemsQty.innerHTML;
                $('.target').removeClass('targetPulse');

            }

            function ShowPackageDatails() //sender, eventArgs
            {
                var divQnt = document.getElementById("<%= divQnt.ClientID %>");
                divQnt.style.border = "none";
                //var SimQnt = $find("ctl00_ContentPlaceHolder1_cbQnty").get_selectedItem().get_value();
                var SimQnt = document.getElementById("cbQnty").value;
                updateItems(SimQnt);
                renderPackageDetails(SimQnt);
                document.getElementById("ctl00_ContentPlaceHolder1_HiddenSIMCount").value = SimQnt;
                $('#Step3').show();

                PageMethods.UpdateErrHtml('InValidQuantity', false, updateErrHtmlFunc);
            }

            function AddToTatal(amount, isBB) {

                var currency = document.getElementById('ctl00_ContentPlaceHolder1_HiddenCurrency').value;
                var country = document.getElementById("<%= HiddenCountry.ClientID %>").value;
                var conversionRate = document.getElementById("<%= HiddenConversionRate.ClientID %>").value;
                var spnOrderTotal = document.getElementById('ctl00_ContentPlaceHolder1_spnOrderTotal');
                var spnTotalAmtPlusShipping = document.getElementById('ctl00_ContentPlaceHolder1_TotalAmtPlusShipping');
                var spnShipCost = document.getElementById('ctl00_ContentPlaceHolder1_spnShipCost');
                var spnTotalAmtDollars = document.getElementById('ctl00_ContentPlaceHolder1_totalAmtDollars');
                if (isBB) {
                    var curAmount = parseFloat($('#spnOrderTotal').html());
                    var newAmount = parseFloat(curAmount + amount).toFixed(2);
                    $('#spnOrderTotal').html(parseFloat(newAmount).toFixed(2));
                    if (country == "FR" || country == "ES")
                        spnAmount.innerHTML = spnAmount.innerHTML.replace(".", ",");
                    $('#totalAmtDollars').html(parseFloat(newAmount * conversionRate).toFixed(2));

                }
                else {
                    if (country == "FR" || country == "ES") {
                        //$('#spnOrderTotal').html(parseFloat(amount).toFixed(2));
                        //$('#totalAmtDollars').html(parseFloat(amount * conversionRate).toFixed(2));               
                        //$('#spnTotalAmtPlusShipping').html(parseFloat(amount * conversionRate).toFixed(2));       

                        spnOrderTotal.innerHTML = parseFloat(amount).toFixed(2);
                        spnOrderTotal.innerHTML = spnOrderTotal.innerHTML.replace(".", ",") + currency;
                        //spnTotalAmtDollars.innerHTML=parseFloat(amount * conversionRate).toFixed(2);
                        //spnTotalAmtPlusShipping.innerHTML=parseFloat(amount +spnShipCost.innerText).toFixed(2)+currency;
                    }
                    else {
                        /*$('#spnOrderTotal').html(parseFloat(amount).toFixed(2));
                        $('#totalAmtDollars').html(parseFloat(amount * conversionRate).toFixed(2)); 
                        $('#spnTotalAmtPlusShipping').html(parseFloat(amount * conversionRate).toFixed(2));
                        */


                        spnOrderTotal.innerHTML = parseFloat(amount).toFixed(2);
                        //spnTotalAmtDollars.innerHTML=parseFloat(amount * conversionRate).toFixed(2);
                        //spnTotalAmtPlusShipping.innerHTML=parseFloat(amount +spnShipCost.innerText).toFixed(2);
                    }

                    //ship_selected(null,null);						

                }
            }

            function AddSIMToTatal(amount) {
                var conversionRate = document.getElementById("<%= HiddenConversionRate.ClientID %>").value;
                var curAmount = parseFloat($('#totalSIM').html());
                $('#totalSIM').html(parseFloat(amount).toFixed(2));

            }

            var nanoSimTxt, microSimTxt, simTxt, SimComment, KntSimTitle;
            var price, simPrice;
            function renderPackageDetails(SimQnt) {
                nanoSimTxt = document.getElementById("<%= HiddenNanoSimTxt.ClientID %>").value;
                microSimTxt = document.getElementById("<%= HiddenMicroSimTxt.ClientID %>").value;
                simTxt = document.getElementById("<%= HiddenSimTxt.ClientID %>").value;
                SimComment = document.getElementById("<%= HiddenSimComment.ClientID %>").value;
                price = parseFloat(document.getElementById("<%= HiddenPlanPrice.ClientID %>").value);
                simPrice = parseFloat(document.getElementById("<%= HiddenSIMPrice.ClientID %>").value);
                var Package = document.getElementById("<%= Package.ClientID %>").value;

                KntSimTitle = document.getElementById("<%= HiddenKntSimTitle.ClientID %>").value;

                $('#packageDetails').html("");
                if (SimQnt == 0) {
                    AddToTatal(price);
                    AddSIMToTatal(simPrice);
                    $('.collapseThree').hide('slow');
                    return;
                }

                AddToTatal((SimQnt * price) + (SimQnt * simPrice));
                var Country = document.getElementById("ctl00_ContentPlaceHolder1_HiddenCountry").value;
                var commentDiv = '<br><div style="clear:both;"><span style="font-size:20px;">' + SimComment + '</span></div>';
                //PageMethods.BuildPackageDiv(SimQnt, NanoSimTxt, MicroSimTxt, SimTxt, KntSimTitle, commentDiv, funcUpdatePackageDetails);

                $.callWebService({
                    serviceName: 'BuildPackageDiv',
                    data: { UserCountry: Country, simQnt: SimQnt, NanoSimTxt: nanoSimTxt, MicroSimTxt: microSimTxt, SimTxt: simTxt, sKntTitle: KntSimTitle, CommentDiv: commentDiv, WhatSimSize: ctl00_ContentPlaceHolder1_ChooseSIMTypePerPack.textContent },
                    success: function (data) {
                        if (data != "")
                            funcUpdatePackageDetails(data);
                        else ViewStateAccessories();
                    }
                });

                $('.collapseThree').slideDown("slow");
            }
            // result from PageMethods.BuildPackageDiv
            function funcUpdatePackageDetails(result) {
                var array = result.split("~");
                var packageDetails = array[0];
                var DefaultKNTCode = array[1];

                $('#packageDetails').html("");
                $('#packageDetails').append(packageDetails);
                document.getElementById("<%=HiddenDefaultKNTCode.ClientID %>").value = DefaultKNTCode;
                //document.getElementById("<%=HiddenpackageDetails1.ClientID %>").value =result;
                // document.getElementById('ctl00_ContentPlaceHolder1_HiddenpackageDetails1').value=result;

                ViewState(document.getElementById("ctl00_ContentPlaceHolder1_HiddenSIMCount").value);
                ViewStateAccessories();
            }

            function onFocus(sender, eventArgs) {
                var tooltip = $find('<%=RadToolTip3.ClientID %>');
                tooltip.set_animation(Telerik.Web.UI.ToolTipAnimation.Fade);
                tooltip.set_targetControlID('<%=DivCell.ClientID %>');
                if (tooltip.get_text() != "") {
                    setTimeout(function () {
                        tooltip.show();
                    }, 20);
                }
            }

            function onBlur(sender, eventArgs) {
                var tooltip = $find('<%=RadToolTip3.ClientID %>');
                setTimeout(function () { tooltip.hide(); }, 5);
            }

            function linkSimInfo(linkSimInfo) {
                var tooltip = $find('<%=RadToolTip2.ClientID %>');
                tooltip.set_animation(Telerik.Web.UI.ToolTipAnimation.Fade);
                tooltip.set_targetControlID(linkSimInfo.id);
                setTimeout(function () { tooltip.show(); }, 20);
            }
            function linkShipInfo() {
                var tooltip = $find('<%=RadToolTip1.ClientID %>');
                tooltip.set_animation(Telerik.Web.UI.ToolTipAnimation.Fade);
                tooltip.set_targetControlID("linkShipInfo");
                setTimeout(function () { tooltip.show(); }, 20);
                var c = document.getElementById("ctl00_ContentPlaceHolder1_HiddenCountry").value;
                $("#ctl00_ContentPlaceHolder1_UpDateToolTip").load("./PopUpText/ShipInfo.aspx?c=" + c);

            }
    </script>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="Button2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel2" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ImgPayPalBtn">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Panel2" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <ClientEvents OnResponseEnd="ResponseEnd" OnRequestStart="RequestStart" />
    </telerik:RadAjaxManager>
    <div id="divTitle" runat="server">
        <h1 id="Checkout" runat="server"></h1>
    </div>
    <div class="wrapper">
        <div class="postwrapper clearfix">
            <div class="container" style="padding-left: 0px;">
                <div class="row">
                    <div class="half-width clearfix">
                        <div id="content" class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <input type="hidden" id="HiddenCurrency" value="0" runat="server" />
                                        <input type="hidden" id="HiddenConversionRate" value="0" runat="server" />
                                        <input type="hidden" id="HiddenCountry" value="0" runat="server" />
                                        <input type="hidden" id="HiddenSimComment" value="0" runat="server" />
                                        <input type="hidden" id="HiddenKntSimTitle" value="0" runat="server" />
                                        <input type="hidden" id="EndDateBeforeStartDate" value="0" runat="server" />
                                        <input type="hidden" id="ForOrderOver60" value="0" runat="server" />
                                        <input type="hidden" id="Package" value="0" runat="server" />
                                        <input type="hidden" id="SelectQuantityHidden" value="0" runat="server" />
                                        <input type="hidden" id="PleaseSelectQuantityHidden" value="0" runat="server" />
                                        <input type="hidden" id="PleaseSelectStartDateHidden" value="0" runat="server" />
                                        <input type="hidden" id="HiddenPlanCount" value="0" runat="server" />
                                        <input type="hidden" id="HiddenUserCountry" value="0" runat="server" />
                                        <input type="hidden" id="HiddenPhone" value="0" runat="server" />
                                        <input type="hidden" id="HiddenPlanTitel" value="0" runat="server" />
                                        <input type="hidden" id="HiddenPlanPrice" value="0" runat="server" />
                                        <input type="hidden" id="HiddenSIMPrice" value="0" runat="server" />
                                        <input type="hidden" id="HiddenBBPrice" value="0" runat="server" />
                                        <input type="hidden" id="HiddenCurrencyName" value="0" runat="server" />
                                        <input type="hidden" id="HiddenDisplayDollars" value="" runat="server" />
                                        <input type="hidden" id="HiddenNanoSimTxt" value="0" runat="server" />
                                        <input type="hidden" id="HiddenMicroSimTxt" value="0" runat="server" />
                                        <input type="hidden" id="HiddenSimTxt" value="0" runat="server" />
                                        <input type="hidden" id="HiddenDefaultKNTCode" value="0" runat="server" />
                                        <input type="hidden" id="HiddenStatus" runat="server" value="0" />
                                        <input type="hidden" id="HiddenDays" runat="server" value="0" />
                                        <input type="hidden" id="HiddenpackageDetails" runat="server" value="0" />
                                        <input type="hidden" id="HiddenProvience" runat="server" value="0" />
                                        <asp:HiddenField ID="HiddenpackageDetails1" runat="server" Value="0" />
                                        <asp:HiddenField ID="shipcountryHidden" Value="0" runat="server" />
                                        <asp:HiddenField ID="HiddenIncluding" Value="0" runat="server" />
                                        <asp:HiddenField ID="HiddenShipping" Value="0" runat="server" />
                                        <asp:HiddenField ID="HiddenCost" Value="0" runat="server" />
                                        <asp:HiddenField ID="HiddenSIMCount" Value="0" runat="server" />
                                    </div>
                                    <div id="divTitleSpace" runat="server" class="tospacer">
                                    </div>
                                    <span id="lblMsg" runat="server" style="color: Red; font-weight: bold; line-height: 17px;"></span>
                                    <div class="panel-group" id="accordion2" style="margin: 0 50px 0 0;">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne">
                                                        <span id="PleaseSelectStartDate" runat="server">Date</span></a>
                                                </h4>
                                            </div>
                                            <div id="collapseOne" class="panel-collapse collapse in">
                                                <div id="Step1" class="panel-body">
                                                    <div id="LeftStep1" class="col-sm-6 col-lg-6 col-md-12">
                                                        <div class="title">
                                                            <h3>
                                                                <span id="From" runat="server"></span>
                                                            </h3>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="input-group" style="display: flex; cursor: pointer;">
                                                                <span id="SpStartDate" class="input-group-addon" style="width: 50px; height: 34px;"
                                                                    onclick="Popup_Side('SpStartDate');"><i class="fa fa-calendar"></i></span>
                                                                <telerik:RadDatePicker ID="RadDateStartDate" runat="server" Culture="English (United States)"
                                                                    Font-Size="10pt" CssClass="form-control" PopupDirection="BottomRight" Width="90%"
                                                                    ShowPopupOnFocus="true">
                                                                    <Calendar ID="Calendar3" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                        ViewSelectorText="x" ShowRowHeaders="False" Skin="Bootstrap">
                                                                    </Calendar>
                                                                    <DatePopupButton Visible="false" />
                                                                    <DateInput ID="DateInput2" runat="server" DateFormat="M/dd/yyyy" DisplayDateFormat="M/dd/yyyy"
                                                                        LabelWidth="40%" EmptyMessage="From">
                                                                        <ClientEvents OnValueChanged="StartDateSelected" OnFocus="ShowDatePopup" />
                                                                        <EnabledStyle BorderStyle="None" />
                                                                        <EmptyMessageStyle ForeColor="black" />
                                                                        <FocusedStyle BorderStyle="None" />
                                                                    </DateInput>
                                                                    <ShowAnimation Type="Slide" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="RightStep1" class="col-sm-6 col-lg-6 col-md-12">
                                                        <div class="title">
                                                            <h3>
                                                                <span id="To" runat="server"></span>
                                                            </h3>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="input-group" style="display: flex; cursor: pointer;">
                                                                <span id="SpEndDate" class="input-group-addon" onclick="Popup_Side('SpEndDate');"
                                                                    style="width: 50px; height: 34px;"><i class="fa fa-calendar"></i></span>
                                                                <telerik:RadDatePicker ID="RadDateEndDate" runat="server" Culture="English (United States)"
                                                                    Font-Size="10pt" CssClass="form-control" PopupDirection="BottomRight" Width="90%"
                                                                    ShowPopupOnFocus="true">
                                                                    <Calendar ID="Calendar2" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                        ViewSelectorText="x" ShowRowHeaders="False">
                                                                    </Calendar>
                                                                    <DatePopupButton Visible="false" />
                                                                    <DateInput ID="DateInput1" runat="server" DateFormat="M/dd/yyyy" DisplayDateFormat="M/dd/yyyy"
                                                                        LabelWidth="40%" EmptyMessage="To">
                                                                        <ClientEvents OnValueChanged="EndDateSelected" />
                                                                        <EnabledStyle BorderStyle="None" />
                                                                        <EmptyMessageStyle ForeColor="black" />
                                                                        <FocusedStyle BorderStyle="None" />
                                                                    </DateInput>
                                                                    <ShowAnimation Type="Slide" />
                                                                </telerik:RadDatePicker>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6 col-lg-6 col-md-12">
                                                        <span id="DaysCount" runat="server"># Days :</span> <span runat="server" id="totalDaysDis"></span>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapse2">
                                                        <span id="SimDetails" runat="server">Sim Details</span></a>
                                                </h4>
                                            </div>
                                            <div id="collapse2" class="panel-collapse collapse">
                                                <div id="Step2" class="panel-body">
                                                    <div class="planDescr" style="font-weight: bold;">
                                                        <div style="line-height: 28px; font-weight: bold; font-size: 26px; color: #023467;">
                                                            <span id="YourPlanInc" runat="server"></span>
                                                        </div>
                                                        <br />
                                                        <ul>
                                                            <li id="listRow1" runat="server" class="Test">Unlimited USA calling!</li>
                                                            <li id="listRow2" runat="server" class="Test">Unlimited UK calling!</li>
                                                            <li id="listRow3" runat="server" class="Test">Unlimited data - email, apps and more!</li>
                                                            <li id="listRow5" runat="server" class="Test">UK local number (free!)</li>
                                                            <li id="listRow8" runat="server" class="Test">Easy installation just Plug-and-Play</li>
                                                            <li id="listRow9" runat="server" class="Test">Shipped from UK</li>
                                                        </ul>
                                                    </div>
                                                    <br />
                                                    <div class="title1" style="display: none">
                                                        <b><span id="ChooseSIMTypePerPack" runat="server"></span></b>
                                                        <br />
                                                        <br />
                                                        <hr />
                                                        <a href="" id="linkSimInfo"><span id="LearnMoreTxt" runat="server"></span>
                                                            <img style="width: 30px; height: 30px;" src="img/Help.png" />
                                                        </a>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div id="divQnt" runat="server" class="simQnt">
                                                    </div>
                                                    <!-- end divQnt -->
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <div id="packageDetails">
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="HiddenSimQNTY" Value="0"></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="RadSimDetails" Value=""></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="KNTSimDetails" Value=""></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="strIMEIDetails" Value=""></asp:HiddenField>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <div class="panel panel-default BillingInformation">
                                            <div class="panel-heading BillingInformationHeader">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapse3">
                                                        <span id="BillingInformation" runat="server">Billing Information</span></a>
                                                </h4>
                                            </div>
                                            <div id="collapse3" class="panel-collapse collapse">
                                                <div id="Step3" class="panel-body">
                                                    <div id="left" class="col-sm-6 col-lg-6 col-md-12">
                                                        <div id="DivFirstName" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtFName" runat="server" Width="90%" CssClass="form-control"
                                                                CausesValidation="True">
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivFirstName','InValidFName')" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredfieldFirstName" runat="server"
                                                                ControlToValidate="RadTxtFName" ErrorMessage="" ValidationGroup="1" Display="Dynamic" />
                                                        </div>
                                                        <br />
                                                        <div id="DivLastName" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtLName" runat="server" EmptyMessage="* Last Name" Width="90%"
                                                                CssClass="form-control" CausesValidation="True">
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivLastName','InValidLName')" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredfieldLastName" CssClass="reqField" runat="server"
                                                                Display="Dynamic" ControlToValidate="RadTxtLName" ErrorMessage="" ValidationGroup="1" />
                                                        </div>
                                                        <br />
                                                        <div id="DivEmail" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtEmail" runat="server" CssClass="form-control" EmptyMessage="* Email"
                                                                Width="90%" CausesValidation="True">
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivEmail','InValidEmail')" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                            <br />
                                                            <asp:RegularExpressionValidator CssClass="reqField" ID="RegularExpressionEmailIsValid"
                                                                runat="server" ControlToValidate="RadTxtEmail" ErrorMessage="" Display="Dynamic"
                                                                ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*"
                                                                SetFocusOnError="True" ValidationGroup="1"></asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredfieldConfirmedemail" CssClass="reqField"
                                                                runat="server" Display="Dynamic" ControlToValidate="RadTxtEmail" ErrorMessage=""
                                                                ValidationGroup="1" />
                                                            <asp:CompareValidator ID="CompareValidEmail" CssClass="reqField" Display="Dynamic"
                                                                runat="server" ErrorMessage="" ControlToCompare="RadTxtEmail" ControlToValidate="RadTxtEmail2"
                                                                SetFocusOnError="True" ValidationGroup="1"></asp:CompareValidator>
                                                        </div>
                                                        <br />
                                                        <div id="DivEmail2" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtEmail2" runat="server" Width="90%" CssClass="form-control"
                                                                EmptyMessage="* Confirm Email">
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivEmail2','InValidEmail2')" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                            <asp:RegularExpressionValidator CssClass="reqField" ID="RegularExpressionValidEmail2"
                                                                runat="server" ControlToValidate="RadTxtEmail2" ErrorMessage="" Display="Dynamic"
                                                                ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*"
                                                                ValidationGroup="1"></asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredfieldEmailValid" CssClass="reqField" runat="server"
                                                                Display="Dynamic" ControlToValidate="RadTxtEmail2" ErrorMessage="" ValidationGroup="1" />
                                                        </div>
                                                        <br />
                                                        <div id="DivPhone" runat="server">
                                                            <asp:HiddenField ID="hdPhone" runat="server" />
                                                            <telerik:RadMaskedTextBox ID="RadTxtPhone" runat="server" PromptChar="" EmptyMessage="* Telephone"
                                                                Width="90%" CssClass="form-control" Mask="############" DisplayPromptChar=""
                                                                ResetCaretOnFocus="True" RoundNumericRanges="False" SelectionOnFocus="SelectAll"
                                                                ZeroPadNumericRanges="False">
                                                                <ClientEvents OnKeyPress="keyPress" OnValueChanged="txtChanged.curry('DivPhone','InValidPhone',9)" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadMaskedTextBox>
                                                            <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldHomeNumber" runat="server"
                                                                Display="Dynamic" ControlToValidate="RadTxtPhone" ErrorMessage="" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <br>
                                                        <div id="DivCell" runat="server">
                                                            <asp:HiddenField ID="hdnCell" runat="server" />
                                                            <telerik:RadMaskedTextBox ID="RadTxtCell" runat="server" PromptChar="" Width="90%"
                                                                CssClass="form-control" Mask="############" DisplayPromptChar="" ResetCaretOnFocus="True"
                                                                RoundNumericRanges="False" SelectionOnFocus="SelectAll" EmptyMessage="* Mobile"
                                                                ZeroPadNumericRanges="False">
                                                                <ClientEvents OnKeyPress="keyPress" OnValueChanged="txtChanged.curry('DivCell','InValidCell',10)"
                                                                    OnFocus="onFocus" OnBlur="onBlur" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadMaskedTextBox>
                                                            <br />
                                                            <asp:RequiredFieldValidator CssClass="reqFieldH" ID="RequiredFieldMobile" runat="server"
                                                                ControlToValidate="RadTxtCell" Display="Dynamic" ErrorMessage="" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div id="right" class="col-sm-6 col-lg-6 col-md-12">
                                                        <div id="DivAddress" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtStreet" Width="90%" CssClass="form-control" runat="server"
                                                                EmptyMessage="* Address">
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivAddress','InValidAddress')" />
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldStreet" runat="server"
                                                                ControlToValidate="RadTxtStreet" ErrorMessage="" ValidationGroup="1" Display="Dynamic">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <br />
                                                        <div id="DivCity" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtCity" Width="90%" runat="server" EmptyMessage="* City"
                                                                CssClass="form-control">
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                                <ClientEvents OnValueChanged="txtChanged.curry('DivCity','InValidCity')" />
                                                            </telerik:RadTextBox>
                                                            <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldCity" runat="server"
                                                                ControlToValidate="RadTxtCity" ErrorMessage="" Display="Dynamic" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <br />
                                                        <div id="DivCountry" runat="server">
                                                            <telerik:RadComboBox OnClientSelectedIndexChanged="ComboCountrySelected" OnItemDataBound="RadComboCountry_ItemDataBound"
                                                                EmptyMessage="Country" ID="RadComboCountry" runat="server" OnClientBlur="OnClientBlurHandler"
                                                                DataMember="CountryId" DataTextField="CountryName" BorderColor="black" Width="90%"
                                                                OnClientDropDownOpened="DropDownOpened"
                                                                CssClass="form-control no-combo-image" HighlightTemplatedItems="true" MarkFirstMatch="true"
                                                                EnableLoadOnDemand="True" DataValueField="CountryId" MaxHeight="300px" Filter="StartsWith">
                                                                <CollapseAnimation Duration="50" Type="InExpo" />
                                                            </telerik:RadComboBox>
                                                            <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldCountry" runat="server"
                                                                Display="Dynamic" ControlToValidate="RadComboCountry" ErrorMessage="" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <br />
                                                        <div id="StateDiv" class="StateDiv" runat="server" style="display: none;">
                                                            <telerik:RadComboBox ID="RadComboStateUSA" OnClientSelectedIndexChanged="StateChanged"
                                                                CssClass="form-control no-combo-image" EmptyMessage="state" runat="server" DataTextField="StateName"
                                                                DataValueField="StateCode" DataMember="StateCode" BorderColor="black" Width="90%"
                                                                MaxHeight="300px" Filter="StartsWith" HighlightTemplatedItems="true" MarkFirstMatch="true"
                                                                EnableLoadOnDemand="True" OnClientDropDownOpened="DropDownOpened">
                                                            </telerik:RadComboBox>
                                                            <telerik:RadComboBox ID="RadComboStateC" OnClientSelectedIndexChanged="StateChanged"
                                                                runat="server" DataMember="StateCode" BorderColor="black" CssClass="form-control no-combo-image"
                                                                DataTextField="StateName" DataValueField="StateCode" Width="90%" EmptyMessage="state"
                                                                MaxHeight="300px" Filter="StartsWith" HighlightTemplatedItems="true" MarkFirstMatch="true"
                                                                EnableLoadOnDemand="True" OnClientDropDownOpened="DropDownOpened">
                                                                <CollapseAnimation Duration="50" Type="InExpo" />
                                                            </telerik:RadComboBox>
                                                            <telerik:RadComboBox ID="RadComboStateAU" OnClientSelectedIndexChanged="StateChanged"
                                                                CssClass="form-control no-combo-image" runat="server" DataMember="StateCode"
                                                                BorderColor="black" DataTextField="StateName" DataValueField="StateCode" Width="90%"
                                                                EmptyMessage="State" MaxHeight="300px" Filter="StartsWith" HighlightTemplatedItems="true"
                                                                MarkFirstMatch="true" EnableLoadOnDemand="True" OnClientDropDownOpened="DropDownOpened">
                                                            </telerik:RadComboBox>
                                                        </div>
                                                        <div id="ZipDiv" runat="server">
                                                            <telerik:RadTextBox ID="RadTxtZip" runat="server" EmptyMessage="Zip" Width="90%"
                                                                CssClass="form-control">
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                        </div>
                                                        <br />
                                                        <div id="DivShip" runat="server">
                                                            <div id="DivComboShipping" runat="server">
                                                                <telerik:RadComboBox ID="RadComboShipping" runat="server" OnItemDataBound="RadCombo_ItemDataBound"
                                                                    MarkFirstMatch="True" EmptyMessage="Delivery Method" HighlightTemplatedItems="True"
                                                                    Width="90%" MaxHeight="300px" CssClass="form-control no-combo-image inner5_drop_down2"
                                                                    BorderColor="black" OnClientSelectedIndexChanged="ship_selected" OnClientDropDownOpened="DropDownOpened">
                                                                </telerik:RadComboBox>
                                                            </div>
                                                            <br />
                                                             <a id="linkShipInfo"  onclick="javascript:linkShipInfo();"><span id="LearnMore" runat="server"></span></a>
                                                            <div id="shipComment" runat="server">
                                                            </div>
                                                            <div id="shipOpAddress" style="display: none;" runat="server">
                                                                <asp:RadioButton ID="rbCusAddress" GroupName="rbShip" OnClick="HideShipDetails();"
                                                                    runat="server" Text="" /><span style="color: #303030;" id="UseTheAboveAddressForShipping"
                                                                        runat="server"></span>
                                                                <br />
                                                                <asp:RadioButton ID="rbDiffAddress" GroupName="rbShip" OnClick="ShowShipDetails();"
                                                                    runat="server" Text="" /><span style="color: #303030;" id="LikeToEnteraDifferentAddress"
                                                                        runat="server"></span>
                                                            </div>
                                                            <div runat="server" id="DateLeaveDiv">
                                                                <div class="form-group">
                                                                    <div class="input-group" style="display: flex; cursor: pointer; width: 90%;">
                                                                        <span id="SpDeliveryDate" class="input-group-addon" style="width: 50px; height: 34px;"
                                                                            onclick="Popup_Side('SpDeliveryDate');"><i class="fa fa-calendar"></i></span>
                                                                        <telerik:RadDatePicker ID="RadDeliveryDate" runat="server" Culture="English (United States)"
                                                                            Font-Size="10pt" CssClass="form-control zindex" PopupDirection="TopRight" Width="90%" ShowPopupOnFocus="true">
                                                                            <Calendar ID="Calendar4" runat="server" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                                                ViewSelectorText="x" ShowRowHeaders="False">
                                                                            </Calendar>
                                                                            <DatePopupButton Visible="false" />
                                                                            <DateInput ID="DateInput3" runat="server" DateFormat="M/dd/yyyy" DisplayDateFormat="M/dd/yyyy"
                                                                                LabelWidth="40%" EmptyMessage="The Date You Will Be Leaving This Address">
                                                                                <ClientEvents OnValueChanged="txtChanged.curry('DateLeaveDiv','InValidDateLeave')" />
                                                                                <EmptyMessageStyle ForeColor="black" />
                                                                                <EnabledStyle BorderStyle="None" />
                                                                                <FocusedStyle />
                                                                                <EnabledStyle BorderColor="black" />
                                                                                <HoveredStyle BorderColor="#FF8000" />
                                                                            </DateInput>
                                                                            <ShowAnimation Type="Slide" />
                                                                        </telerik:RadDatePicker>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                           
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <br />
                                                        <span id="ReturnPolicy" runat="server"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <div class="panel panel-default" id="Step5" style="display: none;">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseFive">
                                                        <span id="ShippingInformation" runat="server">Shipping Information</span></a>
                                                </h4>
                                            </div>
                                            <div id="collapseFive" class="panel-collapse collapse">
                                                <div id="Step4" class="panel-body">
                                                    <div id="ShipDetails" runat="server">
                                                        <div class="col-sm-6 col-lg-6 col-md-12">
                                                            <div id="DivShipName" runat="server">
                                                                <telerik:RadTextBox ID="RadTxtDeliveryName" CssClass="form-control" runat="server"
                                                                    Width="90%">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivShipName','InValidShipName')" />
                                                                </telerik:RadTextBox>
                                                            </div>
                                                            <br />
                                                            <div id="DivShipAddress" runat="server">
                                                                <telerik:RadTextBox ID="RadShipAddress" runat="server" CssClass="form-control" Width="90%">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivShipAddress','InValidShipAddress')" />
                                                                </telerik:RadTextBox>
                                                            </div>
                                                            <br />
                                                            <div runat="server" id="DivShipCity">
                                                                <telerik:RadTextBox ID="RadShipCity" runat="server" CssClass="form-control" Width="90%">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivShipCity','InValidShipCity')" />
                                                                </telerik:RadTextBox>
                                                            </div>
                                                            <br />
                                                            <div id="StateDivShip" runat="server" style="display: none;">
                                                                <span style="display: inline-table;">
                                                                    <telerik:RadComboBox ID="RadComboBoxStateUShip" OnClientDropDownOpened="DropDownOpened" OnClientSelectedIndexChanged="ShipStateChanged"
                                                                        DataTextField="StateName" DataValueField="StateCode" DataMember="StateCode" runat="server"
                                                                        Height="200px" Width="90%" CssClass="form-control no-combo-image" Filter="StartsWith"
                                                                        HighlightTemplatedItems="true" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                                                    </telerik:RadComboBox>
                                                                </span><span style="display: inline-table;">
                                                                    <telerik:RadComboBox ID="RadComboBoxStateAUShip" OnClientDropDownOpened="DropDownOpened" OnClientSelectedIndexChanged="ShipStateChanged"
                                                                        DataTextField="StateName" DataValueField="StateCode" DataMember="StateCode" runat="server"
                                                                        Height="200px" Width="90%" CssClass="form-control" Filter="StartsWith" HighlightTemplatedItems="true"
                                                                        MarkFirstMatch="true" EnableLoadOnDemand="True">
                                                                    </telerik:RadComboBox>
                                                                </span><span style="display: inline-table;">
                                                                    <telerik:RadComboBox ID="RadComboBoxStateCShip" OnClientDropDownOpened="DropDownOpened" OnClientSelectedIndexChanged="ShipStateChanged"
                                                                        DataTextField="StateName" DataValueField="StateCode" DataMember="StateCode" runat="server"
                                                                        Height="200px" Width="90%" CssClass="form-control" Filter="StartsWith" HighlightTemplatedItems="true"
                                                                        MarkFirstMatch="true" EnableLoadOnDemand="True">
                                                                    </telerik:RadComboBox>
                                                                </span>
                                                                <div class="clrflt">
                                                                </div>
                                                            </div>
                                                            <br />
                                                            <div id="DivShipCountry" runat="server">
                                                                <telerik:RadTextBox ID="RadTxtShipCountry" Enabled="false" runat="server" CssClass="form-control"
                                                                    Width="90%">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivShipCountry','InValidShipCountry')" />
                                                                </telerik:RadTextBox>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-sm-6 col-lg-6 col-md-12">
                                                            <div id="ZipDivShip" runat="server">
                                                                <telerik:RadTextBox ID="RadShipZip" runat="server" CssClass="form-control" Width="90%">
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('ZipDivShip','InValidShipZip')" />
                                                                </telerik:RadTextBox>
                                                            </div>
                                                            <br />
                                                            <div runat="server" id="DivShipPhone">
                                                                <asp:HiddenField ID="hdDeliveryPhone" runat="server" />
                                                                <telerik:RadMaskedTextBox ID="RadTxtDeliveryPhone" CssClass="form-control" runat="server"
                                                                    Width="90%" Text="" PromptChar="" Mask="################" DisplayPromptChar=""
                                                                    ResetCaretOnFocus="True" RoundNumericRanges="False" SelectionOnFocus="SelectAll"
                                                                    ZeroPadNumericRanges="False">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnKeyPress="keyPress" OnValueChanged="txtChanged.curry('DivShipPhone','InValidShipPhone',9)" />
                                                                </telerik:RadMaskedTextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <div class="panel panel-default ChoosePaymentMethod">
                                            <div class="panel-heading ChoosePaymentMethodHeader">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapse6">
                                                        <span id="ChoosePaymentMethod" runat="server">Choose Payment Method</span></a>
                                                </h4>
                                            </div>
                                            <div id="collapse6" class="panel-collapse collapse">
                                                <div id="Step6" class="panel-body">
                                                    <div class="title">
                                                        <img height="52px" src="img/secure.jpg"><span id="YouAreCurrentlyOnASecuredSite"
                                                            runat="server"></span>
                                                        <p id="SelectPaymentMethod" runat="server">
                                                            Please select the preferred payment method to use on this order.
                                                        </p>
                                                        <b>
                                                            <br />
                                                            <div id="PayPalRB" visible="true" runat="server">
                                                                <div class="col-sm-6 col-lg-6 col-md-12">
                                                                    <asp:RadioButton ID="rbCC" GroupName="rbPay" Checked="true" onclick="ShowCCDetails();"
                                                                        runat="server" Text="" />
                                                                    <span style="color: #303030;" id="PaybyCCdetails" runat="server"></span>

                                                                </div>
                                                                <div class="col-sm-6 col-lg-6 col-md-12">
                                                                    <asp:RadioButton ID="rbPayPal" GroupName="rbPay" onclick="HideCCDetails();" runat="server"
                                                                        Text="" />
                                                                    <span style="color: #303030;" id="PaybyPayPal" runat="server"></span>
                                                                </div>

                                                            </div>
                                                            <br />
                                                            <label id="CCNotice" runat="server" style="color: Red; width: 100%; height: 60px;"
                                                                visible="false">
                                                            </label>
                                                        </b>
                                                    </div>
                                                    <br />
                                                    <div id="CCDiv" runat="server">
                                                        <div class="col-sm-6 col-lg-6 col-md-12">
                                                            <div id="DivCCType" runat="server">
                                                                <telerik:RadComboBox ID="RadComboCardType" OnClientSelectedIndexChanged="CCTypeChanged" OnClientDropDownOpened="DropDownOpened"
                                                                    runat="server" Width="90%" Filter="Contains" BorderColor="black" CssClass="form-control no-combo-image">
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC1" runat="server"
                                                                    Display="Dynamic" ControlToValidate="RadComboCardType" ErrorMessage="Invalid CC Type"
                                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <br />
                                                            <div id="DivCCNum" runat="server">
                                                                <telerik:RadMaskedTextBox ID="RadTxtCC" runat="server" CssClass="form-control" PromptChar=""
                                                                    Mask="####-####-####-####" EmptyMessage="Credit Card Number" Width="90%" DisplayPromptChar=""
                                                                    ResetCaretOnFocus="True" RoundNumericRanges="False" SelectionOnFocus="SelectAll"
                                                                    ZeroPadNumericRanges="False">
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivCCNum','InValidCCNum')" OnKeyPress="keyPress" />
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                </telerik:RadMaskedTextBox>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC2" runat="server"
                                                                    Display="dynamic" ControlToValidate="RadTxtCC" ErrorMessage="Invalid CC" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <br />
                                                            <div id="DivCCExpDate" runat="server" style="width: 90%">
                                                                <div id="ExpirationDate" runat="server" class="left30">
                                                                </div>
                                                                <div class="left30">

                                                                    <telerik:RadComboBox ID="RadComboMonth" OnClientSelectedIndexChanged="CCEXpChanged" OnClientDropDownOpened="DropDownOpened"
                                                                        runat="server" EmptyMessage="Month" BorderColor="black" CssClass="form-control no-combo-image"
                                                                        Width="100%" Filter="Contains">
                                                                    </telerik:RadComboBox>
                                                                </div>
                                                                <div class="right30">
                                                                    <telerik:RadComboBox ID="RadComboYear" OnClientSelectedIndexChanged="CCEXpChanged" OnClientDropDownOpened="DropDownOpened"
                                                                        runat="server" EmptyMessage="Year" BorderColor="black" CssClass="form-control no-combo-image"
                                                                        Width="100%" Filter="Contains">
                                                                    </telerik:RadComboBox>
                                                                </div>

                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC3" runat="server"
                                                                    Display="Dynamic" ControlToValidate="RadComboYear" ErrorMessage="Invalid Year"
                                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC4" runat="server"
                                                                    Display="Dynamic" ControlToValidate="RadComboMonth" ErrorMessage="Invalid Month"
                                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                <br />
                                                                <br />
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-sm-6 col-lg-6 col-md-12">
                                                            <div id="DivCCFirstName" runat="server">
                                                                <telerik:RadTextBox ID="RadClientFName" CssClass="form-control" runat="server" Width="90%">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivCCFirstName','InValidCCFirstName')" />
                                                                </telerik:RadTextBox>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC5" runat="server"
                                                                    Display="Dynamic" ControlToValidate="RadClientFName" ErrorMessage="Invalid Name"
                                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <br />
                                                            <div id="DivCCLastName" runat="server">
                                                                <telerik:RadTextBox ID="RadClientLName" Width="90%" runat="server" CssClass="form-control">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivCCLastName','InValidCCLastName')" />
                                                                </telerik:RadTextBox>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC6" runat="server"
                                                                    Display="Dynamic" ControlToValidate="RadClientLName" ErrorMessage="Invalid Name"
                                                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <br />
                                                            <div id="DivCCEmail" runat="server">
                                                                <telerik:RadTextBox ID="RadCCEmail" CssClass="form-control" Width="90%" runat="server"
                                                                    EnableViewState="False">
                                                                    <EmptyMessageStyle ForeColor="black" />
                                                                    <FocusedStyle BorderColor="#FF8000" />
                                                                    <EnabledStyle BorderColor="black" />
                                                                    <HoveredStyle BorderColor="#FF8000" />
                                                                    <ClientEvents OnValueChanged="txtChanged.curry('DivCCEmail','InValidCCEmail')" />
                                                                </telerik:RadTextBox>
                                                                <asp:RequiredFieldValidator CssClass="reqField" ID="RequiredFieldValidatorCC7" runat="server"
                                                                    ControlToValidate="RadCCEmail" Display="Dynamic" ErrorMessage="" ValidationGroup="1"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator CssClass="reqField" ID="RequiredFieldValidatorCC8"
                                                                    runat="server" ControlToValidate="RadCCEmail" ErrorMessage="" Display="Dynamic"
                                                                    ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*"
                                                                    SetFocusOnError="True" ValidationGroup="1"></asp:RegularExpressionValidator>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-6 col-lg-6 col-md-12 clearfix">
                                                        <div id="DivCouponCode">
                                                            <telerik:RadTextBox ID="RadPromoCode" EmptyMessage="Coupon Code" CssClass="form-control"
                                                                runat="server" Width="90%" TextMode="MultiLine">
                                                                <EmptyMessageStyle ForeColor="black" />
                                                                <FocusedStyle BorderColor="#FF8000" />
                                                                <EnabledStyle BorderColor="black" />
                                                                <HoveredStyle BorderColor="#FF8000" />
                                                            </telerik:RadTextBox>
                                                        </div>
                                                        <br />
                                                    </div>
                                                    <div class="col-sm-6 col-lg-6 col-md-12 clearfix">
                                                        <telerik:RadTextBox ID="RadTxtComments" EmptyMessage="Comments" runat="server" CssClass="form-control"
                                                            TextMode="MultiLine" MaxLength="300" Width="90%">
                                                            <EmptyMessageStyle ForeColor="black" />
                                                            <FocusedStyle BorderColor="#FF8000" />
                                                            <EnabledStyle BorderColor="black" />
                                                            <HoveredStyle BorderColor="#FF8000" />
                                                        </telerik:RadTextBox>
                                                        <br />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <br />
                                                    <div id="DivAgree" runat="server">
                                                        <asp:CheckBox Checked="false" onchange="OnAgreeChecked()" ID="CheckBoxAgree" runat="server" CssClass="agree" />
                                                        <strong id="AgreeTxt" runat="server"><span id="AgreeTermsOfService1" runat="server"></span><a style="cursor: pointer;" runat="server" id="TermsCond" target="_blank"></a>
                                                            <span id="AgreeTermsOfService2" runat="server"></span>. </strong>
                                                    </div>
                                                    <br />
                                                    <span id="billText" runat="server"></span>
                                                    <br />
                                                    <br />
                                                    <span id="ThisTransWillApearTNS" runat="server"></span>
                                                    <br />
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <div class="panel panel-default accsesoreis" id="Step8" style="display: none;">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseSeven">
                                                        <span id="AccessoriesSpn" runat="server">Accessories</span> </a>
                                                </h4>
                                            </div>
                                            <div id="collapseSeven" class="panel-collapse collapse">
                                                <div id="Step7">
                                                    <div class="whitewrapper bordertop clearfix">
                                                        <asp:HiddenField runat="server" ID="RadAccessoriesDetails" Value=""></asp:HiddenField>
                                                    </div>
                                                    <!-- end whitewrapper -->
                                                    <div id="AccessoriesList" runat="server" class="row clearfix text-center AccessoriesListC">
                                                    </div>

                                                    <!-- end row -->
                                                </div>
                                            </div>
                                        </div>
                                        <!-- end panel-default" -->
                                        <br />
                                        <br />
                                        <div>
                                            <asp:Button ID="Button2" CssClass="btnSubmit" runat="server" OnClientClick="clickOK(this);"
                                                Text="" UseSubmitBehavior="false" OnClick="btnCustomRadWsindowConfirm_Click" />

                                            <div id="PayPalDiv" runat="server" style="display: block;">
                                                <asp:ImageButton ID="ImgPayPalBtn" runat="server"
                                                    ImageUrl="https://www.paypalobjects.com/webstatic/en_US/i/buttons/checkout-logo-large.png"
                                                    alt="PayPal" CssClass="paypal"
                                                    OnClientClick="clickOK(this);" OnClick="ImgPayPalBtn_Click" />

                                                <br />
                                                <%-- <asp:ImageButton ID="ImgPayPalBtn1" runat="server" ImageUrl="" OnClick="ImgPayPalBtn_Click"
                                                            OnClientClick="clickOK(null)" ValidationGroup="1" CausesValidation="true" />--%>
                                                <br />
                                                <br />
                                                <%-- <span style="color: red">Coupons are not active when using PayPal</span>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- end panel-group-->
                                    </a>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Button2" />
                                    <asp:AsyncPostBackTrigger ControlID="btnNoUpsale" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUpsale" />
                                    <asp:AsyncPostBackTrigger ControlID="ImgPayPalBtn" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </div>
                        <!-- end col-lg-9 -->
                        <div id="sidebar" runat="server" class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                            <div>
                                <div class="row">
                                    <div>
                                        <div class="title text-center">
                                            <h1 id="AllPlansTopTitle" runat="server">AllPlansTopTitle</h1>
                                            <hr>
                                            <p class="lead LightBlueText" id="AllPlansTopText" runat="server">
                                                AllPlansTopText
                                            </p>
                                        </div>
                                        <div class="pricing_details clearfix">
                                            <div data-effect="helix">
                                                <div id="Plan1class" runat="server" class="pricing-box">
                                                    <div class="title HeightTitle">
                                                        <h3 id="Plan1Title" runat="server">Plan1Title</h3>
                                                        <h4 id="Plan1SubTitle" runat="server">Plan1SubTitle</h4>
                                                    </div>
                                                    <hr>
                                                    <div class="price" id="Price1" runat="server">
                                                        <span id="Plan1PromoLabel" class="pricePromoLabel" runat="server">Plan1PromoLabel</span><br />
                                                        <span id="Plan1Rate" class="priceText" runat="server">Plan1Rate </span>
                                                        <br />
                                                        <span id="Plan1CurrencyLabel" class="pricePromoLabel2" runat="server"></span>
                                                    </div>
                                                    <hr>
                                                    <span id="Plan1Details1" style="font-weight: bold;" runat="server">Plan1Details1</span>
                                                </div>
                                                <!-- end custombox -->
                                            </div>
                                            <!-- enc col-3 -->
                                        </div>
                                        <!--end pricing_details -->
                                        <div class="pricing_details clearfix">
                                            <div data-effect="helix">
                                                <div id="Plan2class" runat="server" class="pricing-box">
                                                    <div class="title HeightTitle">
                                                        <h3 id="Plan2Title" runat="server">Plan2Title</h3>
                                                        <h4 id="Plan2SubTitle" runat="server">Plan2SubTitle</h4>
                                                    </div>
                                                    <hr>
                                                    <div class="price" id="Price2" runat="server">
                                                        <span id="Plan2PromoLabel" class="pricePromoLabel" runat="server">Plan2PromoLabel </span>
                                                        <br />
                                                        <span id="Plan2Rate" class="priceText" runat="server">Plan2Rate </span>
                                                        <br />
                                                        <span id="Plan2CurrencyLabel" class="pricePromoLabel2" runat="server"></span>
                                                    </div>
                                                    <hr>
                                                    <span id="Plan2Details1" style="font-weight: bold;" runat="server">Plan2Details1</span>
                                                </div>
                                                <!-- end custombox -->
                                            </div>
                                            <!-- enc col-3 -->
                                        </div>
                                        <!--end pricing_details -->
                                        <div class="pricing_details clearfix">
                                            <div class="" data-effect="helix">
                                                <div id="Plan3class" runat="server" class="pricing-box">
                                                    <div class="title HeightTitle">
                                                        <h3 id="Plan3Title" runat="server">Plan3Title</h3>
                                                        <h4 id="Plan3SubTitle" runat="server">Plan3SubTitle</h4>
                                                    </div>
                                                    <hr>
                                                    <div class="price" id="Price3" runat="server">
                                                        <p>
                                                            <span id="Plan3PromoLabel" class="pricePromoLabel" runat="server">Plan3PromoLabel </span>
                                                            <br />
                                                            <span id="Plan3Rate" class="priceText" runat="server">Plan3Rate </span>
                                                            <br />
                                                            <span id="Plan3CurrencyLabel" class="pricePromoLabel2" runat="server"></span>
                                                        </p>
                                                    </div>
                                                    <hr>
                                                    <span id="Plan3Details1" style="font-weight: bold;" runat="server">PlanDetails1</span>
                                                    <span id="Plan3" style="font-weight: bold;" runat="server">Plan3Details1</span>
                                                    <span id="Plan4" style="font-weight: bold;" runat="server">Plan4Details1</span>
                                                </div>
                                                <!-- end custombox -->
                                            </div>
                                            <!-- enc col-3 -->
                                        </div>
                                        <!--end pricing_details -->
                                    </div>
                                    <!-- end container -->
                                </div>
                            </div>
                            <!-- end whitewrapper -->
                        </div>
                        <!-- end sidebar -->
                    </div>
                    <!-- end half-width -->
                    <aside class="Right-drawer">

                        <script src="js/cart.js?1"></script>

                        <div class="Cart clearfix text-center">
                            <header class="cart-header">
                                <div>
                                    <div class="orderIconheader" id="PlanDetailsTxt" runat="server">
                                    </div>
                                    <button class="cart-close-btn" type="button">
                                        <svg version="1.1" x="0px" y="0px" width="20px" height="33px" viewbox="0 0 20 33"
                                            enable-background="new 0 0 20 33" xml:space="preserve">
                                            <polyline fill="none" stroke="#FFFFFF" stroke-width="2.4"
                                                stroke-linecap="square" points="17.071,30.642 2.929,16.5 17.071,2.358 
	                                                        ">
                                            </polyline>
                                        </svg>
                                    </button>
                                </div>
                            </header>
                            <!-- header -->
                            <!-- show the cart -->
                            <div class="cart-line-items">
                                <div class="simpleCart_items">
                                </div>
                            </div>
                            <div id="ShoppingChart" class="clearfix">
                                <div class="orderDetails" style="display: none;">
                                    <table class="tblOrderDetails">
                                        <thead>
                                            <tr>
                                                <td style="height: 16px">
                                                    <span id="Description" runat="server"></span>
                                                </td>
                                                <td style="height: 16px">
                                                    <span id="Span1" runat="server">Quantity</span>
                                                </td>
                                                <td class="Right" style="height: 16px">
                                                    <span id="Price" runat="server"></span>
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody runat="server" id="PlanDetails">
                                        </tbody>
                                    </table>
                                    <div style="width: 100%; text-align: right; margin-top: 10px;">
                                        <div>
                                            <span id="OrderTotal" runat="server"></span>: <b><span id="spnOrderTotalCurrencySymbol"
                                                runat="server"></span><span runat="server" id="spnOrderTotal" class="simpleCart_total">0.00</span> </b>
                                        </div>
                                        <div>
                                            <span id="Shipping" runat="server"></span>: <span id="spnShipCostCurrencySymbol"
                                                runat="server"></span><span id="spnShipCost" runat="server" class="simpleCart_shipping">0.00</span>
                                        </div>
                                        <div class="divTotalAmount">
                                            <span id="TotalAmount" runat="server"></span><span id="spnBeforeAfterShipping"><span
                                                id="before" runat="server"></span></span><span id="Shipping2" runat="server"></span>
                                            <span id="TotalAmtPlusShippingCurrencySymbol" runat="server"></span><span id="TotalAmtPlusShipping"
                                                runat="server" class="simpleCart_grandTotal">0.00</span>
                                        </div>
                                        <div id="usdPrice1" style="text-align: right; color: #be2a31; padding-right: 15px; font: bold  20px arial;">
                                            ($ <span id="totalAmtDollars" runat="server"></span>)
                                        </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <!-- end ShoppingChart -->
                            <%--<!-- button to empty the cart -->
                                                <a href="javascript:;" class="simpleCart_empty">empty</a>
                                                <div class="simpleCart_shelfItem">
                    <img src="" class="item_thumb" />
                    <h2 class="item_name"> Awesome T-shirt </h2>
                    <input type="text" value="1" class="item_Quantity">
                    <span class="item_price">$35.99</span>
                    <a class="item_add" href="javascript:;"> Add to Cart </a>
                </div>--%>
                            <!-- create a checkout button 
                                                <a href="javascript:;" class="simpleCart_checkout">checkout</a>-->
                            <footer class="cart-footer">
                                <div class="cart-price-breakdown calculations order-summary">
                                    <div class="cart-subtotal">
                                        <dl class="calculation">
                                            <dt id="Subtotal" runat="server">Subtotal</dt>
                                            <dd class="simpleCart_total"></dd>
                                        </dl>
                                        <dl class="calculation">
                                            <dt>Shipping</dt>
                                            <dd class="simpleCart_shipping"></dd>
                                        </dl>
                                    </div>
                                    <div class="cart-promo-form">
                                        <div class="coupon-form">
                                            <div class="coupon-input-wrapper">
                                                <input id="ApplyCouponCode" runat="server" class="coupon-input" name="coupon_code" type="text" value="" placeholder="Apply a coupon code">
                                            </div>
                                            <div class="btn-coupon-wrapper">
                                                <span class="btn-coupon" name="apply">APPLY</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cart-applied-promo">
                                    </div>
                                    <div class="cart-total">
                                        <dl class="calculation total">
                                            <dt id="Total" runat="server">Total</dt>
                                            <dd class="simpleCart_grandTotal"></dd>
                                        </dl>
                                        <div id="usdPrice" style="text-align: right; padding-right: 15px; font: bold  20px arial;">
                                            (<span class="simpleCart_grandTotalWithDollar"></span>)
                                        </div>
                                    </div>
                                </div>
                                <div class="cart-actions">
                                    <div class="cart-form">
                                        <span class="checkout" data-track="Clicked Checkout from Cart">
                                            <span id="ContinueShopping" runat="server" class="cart-action-text">Continue Shopping > </span>
                                        </span>
                                    </div>
                                </div>
                                <%--<!-- cart quantity (ex. 3) -->
                                                    quantity
                                                    <div class="simpleCart_quantity">
                                                    </div>
                                                    <!-- tax cost (ex. $1.38) -->
                                                    tax
                                                    <div class="simpleCart_tax">
                                                    </div>
                                                    <!-- tax rate (ex. %0.6) -->
                                                    taxRate
                                                    <div class="simpleCart_taxRate">
                                                    </div>
                                                    <!-- grand total, including tax and shipping (ex. $28.49) -->
                                                    grandTotal --%>
                            </footer>
                            <!--footer -->
                        </div>
                    </aside>
                    <!--aside-->
                </div>
                <!-- end row -->
            </div>
            <!-- end container -->
        </div>
    </div>

    <div class="Curtain" id="main-curtain">
    </div>
    <telerik:RadToolTip runat="server" ID="RadToolTip1" RelativeTo="Element" IsClientID="True" AutoCloseDelay="10000"
        Position="TopCenter" ShowEvent="OnClick" HideEvent="LeaveTargetAndToolTip"
        ManualCloseButtonText="close" Height="480">
        <asp:UpdatePanel runat="server" ID="UpDateToolTip"></asp:UpdatePanel>
    </telerik:RadToolTip>
    <telerik:RadToolTip runat="server" ID="RadToolTip2" RelativeTo="Element" IsClientID="true" AutoCloseDelay="10000"
        Position="TopCenter" ShowEvent="OnClick" HideEvent="LeaveTargetAndToolTip" TargetControlID=""
        ManualCloseButtonText="close">
    </telerik:RadToolTip>
    <telerik:RadToolTip runat="server" ID="RadToolTip3" RelativeTo="Element" IsClientID="true"
        TargetControlID="" Position="BottomCenter" ShowEvent="FromCode" HideEvent="FromCode" Width="250px">
    </telerik:RadToolTip>


    <script type="text/javascript">
        var TakeTheOrder = 0;


        function UpsalePopup() {
            try {
                popup1('Upgarde', 'New');
                //$('#ctl00_ContentPlaceHolder1_NewUpsaleAdv').load('PopUpText/UpsaleAdv.aspx #UpsaleAdvPanel');
                //if browser close and the order didnt download
                window.clearTimeout(TakeTheOrder);
                TakeTheOrder = window.setTimeout(function () { EndUpsale() }, 600000);
            }
            catch (err) { EndUpsale(); }
        }

        function f1(result) {
            if (result == true)
                window.location.assign("SignupStep_4.aspx?c=" + document.getElementById("ctl00_ContentPlaceHolder1_HiddenCountry").value);
        }

        var currentLoadingPanel = null;
        var currentUpdatedControl = null;
        function requestStart() {
            currentLoadingPanel = $find("<%= RadAjaxLoadingPanel1.ClientID%>");
            btn1 = $find("<%= Button2.ClientID%>");
            //btn1.set_enabled(false);
            currentUpdatedControl = "<%= Button2.ClientID %>";
            //show the loading panel over the updated control   
            currentLoadingPanel.show(currentUpdatedControl);

        }
        function Upgrade() { }
        function ShowLoading(mode) {
            if (status != "done") {
                clearTimeout(TakeTheOrder);
                closePopup();
                if (mode == 'UpsaleAdv') {


                }
                else {
                    RequestStart();
                    if (mode != "Upsale") {
                        status = "done";
                        document.getElementById('<%=btnNoUpsale.ClientID %>').click();
                    }
                }

            }

        }
        function ChangeImage(imgURL) {
            //make sure the ID of the image is set correctly
            document.getElementById('ctl00_ContentPlaceHolder1_imgUpsale').src = imgURL;
        }
    </script>

    <div id="dialog-box-new">
        <div class="learn_popup_big" id="learn_popupID">

            <ajaxToolkit:UpdatePanel ID="NewUpsaleAdv" runat="server">
                <ContentTemplate></ContentTemplate>
            </ajaxToolkit:UpdatePanel>
            <div id="dbtnUpsale" style="display: block">
                <asp:Button ID="btnUpsale" runat="server" OnClientClick="Upgrade();" OnClick="btnUpsaleSubmit_Click"
                    CssClass="btnSubmit" Text="Add to My order" /><br />
                <asp:Button ID="btnNoUpsale" runat="server" OnClientClick="ShowLoading();" OnClick="btnNoUpsaleSubmit_Click"
                    CssClass="" Text="" /><br />
            </div>
            <a href="#" class="close" onclick="javascript:ShowLoading();"></a>
        </div>
    </div>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="30">
        <div class="loading">
        </div>
    </telerik:RadAjaxLoadingPanel>



</asp:Content>
