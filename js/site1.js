$(document).ready(function() {

	//////////////////
	$('.Items').selectbox();
	
	
	///////////////
	jQuery(function(){
		jQuery('ul.sf-menu').superfish();
	});
	
	//////////////////
	//Set default open/close settings
		
		
		
		//Set default open/close settings
		$('.acc_container').hide(); //Hide/close all containers
		$('.acc_trigger:first').addClass('active').next().show(); //Add "active" class to first trigger, then show/open the immediate next container
		
		//On Click
		$('.acc_trigger').click(function(){
			if( $(this).next().is(':hidden') ) { //If immediate next container is closed...
				$('.acc_trigger').removeClass('active').next().slideUp(); //Remove all .acc_trigger classes and slide up the immediate next container
				$(this).toggleClass('active').next().slideDown(); //Add .acc_trigger class to clicked trigger and slide down the immediate next container
			}
			return false; //Prevent the browser jump to the link anchor
		});
		
		
		$("ul.tabs").tabs("div.panes");
		
		
		$('#slideshow').cycle({
			fx:     'scrollHorz',
			timeout: 4300,
			pager:  '.pager',
			cleartypeNoBg: true,
			slideExpr: '.slide'
		});
		
	
});
