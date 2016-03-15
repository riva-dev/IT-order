
    ///paste your content in js file
    var persistclose = 0 //set to 0 or 1. 1 means once the bar is manually closed, it will remain closed for browser session
    var startX; //=880// 780//1100 //set x offset of bar in pixels
    var startY = 100//110 //set y offset of bar in pixels

    var winW = 630, winH = 460;
    if (document.body && document.body.offsetWidth) {
        winW = document.body.offsetWidth;
        winH = document.body.offsetHeight;
    }
    if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
        winW = document.documentElement.offsetWidth;
        winH = document.documentElement.offsetHeight;
    }
    if (window.innerWidth && window.innerHeight) {
        winW = window.innerWidth;
        winH = window.innerHeight;
    }

  
    if (winW >= 1010)
        startX = (winW / 2) + (980/2) - 230;
    else {
        startX =  winW-50;
        startY = 50;
    }
        
    if (winW < 1010)
        startY = 50;
    
   
    var verticalpos = "fromtop" //enter "fromtop" or "frombottom"

    function iecompattest() {
        return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
    }

    function get_cookie(Name) {
        var search = Name + "="
        var returnvalue = "";
        if (document.cookie.length > 0) {
            offset = document.cookie.indexOf(search)
            if (offset != -1) {
                offset += search.length
                end = document.cookie.indexOf(";", offset);
                if (end == -1) end = document.cookie.length;
                returnvalue = unescape(document.cookie.substring(offset, end))
            }
        }
        return returnvalue;
    }

    function closebar() {
        if (persistclose)
            document.cookie = "remainclosed=1"
        document.getElementById("popupDiv").style.visibility = "hidden"
    }

    function staticbar() {
        if (document.getElementById("popupDiv") == null)
            return;
        barheight = document.getElementById("popupDiv").offsetHeight
        var ns = (navigator.appName.indexOf("Netscape") != -1) || window.opera;
        var d = document;
        function ml(id) {
            var el = d.getElementById(id);
            if (!persistclose || persistclose && get_cookie("remainclosed") == "")
                el.style.visibility = "visible"
            if (d.layers) el.style = el;
            el.sP = function(x, y) { this.style.left = x + "px"; this.style.top = y + "px"; };
            el.x = startX; //el.x
            if (verticalpos == "fromtop") {
               
                el.y = startY ;                
                //el.y = ns ? pageYOffset + innerHeight : iecompattest().scrollTop + iecompattest().clientHeight;
                //el.y += startY;
            }
            else {
                el.y = ns ? pageYOffset + innerHeight : iecompattest().scrollTop + iecompattest().clientHeight;
                el.y -= startY;
            }
            return el;
        }
        window.stayTopLeft = function() {
            if (verticalpos == "fromtop") {
                var pY = ns ? pageYOffset : iecompattest().scrollTop;
                ftlObj.y += (pY - ftlObj.y - 80 + startY) / 8;

                var y1;
                var y2;
                //var tTestimon = ml("ctl00_TestimonialsDiv");

                //if (tTestimon.y > 100)
                //  y1 = tTestimon.y - 40;
                var banner = ml("ctl00_ContentPlaceHolder1_lblLinkTextInfo")

                //y1 = document.body.offsetHeight - 900; //380; //
                y1 = document.body.offsetHeight - 980; //380; //
                y2 = 630 + banner.offsetHeight; //1400; //
               
                if (ftlObj.y <= y2)
                    ftlObj.y = y2;

                if (ftlObj.y >= y1)
                    ftlObj.y = y1;


            }
            else {
                var pY = ns ? pageYOffset + innerHeight - barheight : iecompattest().scrollTop + iecompattest().clientHeight - barheight;
                ftlObj.y += (pY - startY - ftlObj.y) / 8;
            }
            ftlObj.sP(ftlObj.x, ftlObj.y);
            setTimeout("stayTopLeft()");
        }
        ftlObj = ml("popupDiv");
        stayTopLeft();
    }

    if (window.addEventListener)
        window.addEventListener("load", staticbar, false)
    else if (window.attachEvent)
        window.attachEvent("onload", staticbar)
    else if (document.getElementById)
        window.onload = staticbar



