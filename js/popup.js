$(document).ready(function () {

	// if user clicked on button, the overlay layer or the dialogbox, close the dialog	
	$('a.btn-ok, a.close ').click(function () {		
		$('#dialog-overlay, #dialog-box').hide();
		$('#dialog-overlay, #dialog-box-new').hide();
		$('#dialog-overlay, #dialog-div-box').hide();		
		return false;
	});
	
	// if user resize the window, call the same function again
	// to make sure the overlay fills the screen and dialogbox aligned to center	
	$(window).resize(function () {
		
		//only do it if the dialog box is not hidden
		if (!$('#dialog-box').is(':hidden')) popup();
		//if (!$('#dialog-box-new').is(':hidden')) popup();		
	});	
	
	
});
function closePopup(mode) {
    $('.Curtain').removeClass("visible");
    $('#dialog-overlay, #dialog-box').hide();
    $('#dialog-overlay, #dialog-box-new').hide();
    $('#dialog-overlay, #dialog-div-box').hide();
    
    return false;
}
//Popup dialog
function popup(message,title) {
	
//	if(message.length>450)	   
//	{	
//	   //$('#learn_popup').css({background:url(../img/light_box_bg_bigger.png) no-repeat left top;}).show();
//	   document.getElementById("learn_popupID").style.backgroundImage.src = "url('../img/light_box_bg_bigger.png')"; 

//	   //document.getElementById("learn_popupID").style.background= "background:url('../img/light_box_bg_bigger.png') no-repeat left top";
//	   //document.getElementById(current).style.background="background-image: url('image/arrow_sel.gif') no-repeat";
//	
// 	//.style.background= 'url(../img/light_box_bg_bigger.png) no-repeat left top';
//   }
 //else
// {
//  $('#learn_popup').css({height:266}).show();
//    //document.getElementById("learn_popupID").style.background= "background:url('../img/light_box_bg.png') no-repeat left top";
//    }
	   
	
	
	// get the screen height and width  
	var maskHeight = $(document).height();  
	var maskWidth = $(window).width();
	
	// calculate the values for center alignment
	var dialogTop =  (maskHeight/3) - ($('#dialog-box').height());  
	var dialogLeft = (maskWidth/2) - ($('#dialog-box').width()/2); 
	
	// assign values to the overlay and dialog box
	$('#dialog-overlay').css({height:maskHeight, width:maskWidth}).show();
	$('#dialog-box').css({top:dialogTop, left:dialogLeft}).show();
	
	// display the message
	//$('#dialog-message').html(message);
	$('#title').html(title);
	$('#popupMessage').html(message);
			
}


function popup1(message,title) {//for bigger
	// get the screen height and width  
	var maskHeight = $(document).height();  
	var maskWidth = $(window).width();
	
	// calculate the values for center alignment
	var dialogTop =  (maskHeight/3) - ($('#dialog-box-new').height());  
	var dialogLeft = (maskWidth/2) - ($('#dialog-box-new').width()/2); 
	
	// assign values to the overlay and dialog box
	$('#dialog-overlay').css({height:maskHeight, width:maskWidth}).show();
	$('#dialog-box-new').css({top:dialogTop, left:dialogLeft}).show();
	$('#updatePanelRates').css({"display":"block"});
	
    // display the message
	$('.Curtain').addClass("visible");
	$("#background").css({"opacity" : "0.7"}).fadeIn(6000);
	//$('#dialog-message').html(message);
	$('#title1').html(title);
	$('#popupMessage1').html(message);
			
}
function popup2(){
alert("dhgfjbhg jh bjhbjvhbjv hgbgkfkgfkv");

}