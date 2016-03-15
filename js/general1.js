function VisbileRB_Ship(obj, rb, t) {
    var el = document.getElementById(rb);
    if (el != null) {
        if (el.checked)
            toggle(obj, 'show');
        else
            toggle(obj, 'hide');
    }

    var oShip = "ctl00_ContentPlaceHolder1_shipDetails";
    if (t=='1')
        toggle(oShip, 'show');
    else
        toggle(oShip, 'hide');
    
}
function toggle(obj, s) {
    var el = document.getElementById(obj);
    if (el != null) {
        if (s == '') {
            if (el.style.display != 'none') {
                el.style.display = 'none';
            }
            else {
                el.style.display = 'block';
            }
        } else if (s == 'show') {
            el.style.display = 'block';
        } else if (s == 'hide') {
            el.style.display = 'none';
        }
    }
}

function getScrollXY() {
    var scrOfX = 0, scrOfY = 0;
    if (typeof (window.pageYOffset) == 'number') {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
    }
    return [scrOfX, scrOfY];
}

function HideLeftLinks()
{
    $('.inner_container').css('left','0px');
    $('.inner_container').css('width','950px');
//    $('.main_container').css('top','0px');
//    $('.heading').css('left','0px');
}