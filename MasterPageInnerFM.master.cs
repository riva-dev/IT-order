using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPageInnerFM : System.Web.UI.MasterPage
{
    public string HomeSiteURL
    {
        get
        {
            return HiddinHomeSiteURL.Value;
        }
        set
        {
            HiddinHomeSiteURL.Value = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Page.Header.DataBind();
            LoadTexts();
            LoadHeaderFooter();
            LoadGeneralInfo();
            LoadTextByLang();
            LoadSocialLinks();
            LoadTweets();
            HiddinHomeSiteURL.Value = GetSourceURL();
            HomeSiteURL = HiddinHomeSiteURL.Value;

        }
    }

    private void LoadSocialLinks()
    {
        DataTable dtSocails = dbUtility.getTableBySQLStrParam2015("SELECT ItemName,Link  FROM tblSocialLinks", "", "");
        if (dtSocails != null)
            if (dtSocails.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSocails.Rows)
                {
                    if (dr["ItemName"].ToString().ToLower().Contains("facebook"))
                    {
                        linkFacebookFooter.HRef = dr["Link"].ToString();
                        linkFacebookHeader.HRef = dr["Link"].ToString();
                    }

                    if (dr["ItemName"].ToString().ToLower().Contains("google"))
                    {
                        linkGooglePlusFooter.HRef = dr["Link"].ToString();
                        linkGooglePlusHeader.HRef = dr["Link"].ToString();
                    }

                    if (dr["ItemName"].ToString().ToLower().Contains("youtube"))
                    {
                        linkYoutubeFooter.HRef = dr["Link"].ToString();
                        linkYoutubeHeader.HRef = dr["Link"].ToString();
                    }

                    if (dr["ItemName"].ToString().ToLower().Contains("twitter"))
                    {
                        linkTwitterFooter.HRef = dr["Link"].ToString();
                        linkTwitterHeader.HRef = dr["Link"].ToString();
                    }
                }
            }
    }
    private void LoadTextByLang()
    {
        //try
        //{
        //    if (this.Request.Params["language"] != null)
        //        SessionUtility.AddValue("Language", this.Request.Params["language"].ToString().Trim());
        //    else
        //        SessionUtility.AddValue("Language", "en-US");     
        //}
        //catch
        //{
        //    SessionUtility.AddValue("Language", "en-US");          
        //}
       
        string country = SessionUtility.GetValue("UserCountry");
        //if (country == "CA")
        //    headerId.Style.Add("background", "#003366 url(img/banner-ca.jpg) repeat-x 0px 0px");
        //else
        //    headerId.Style.Add("background", "#003366 url(img/flagBanner.jpg) repeat-x 0px 0px");


        //if (country == "FR")
        //    ImageTelAWAY.ImageUrl = "~/img/logoFR.png";

        //if (country == "BR")
        //    ImageTelAWAY.ImageUrl = "~/img/logoBR.png";

        //Copyright.InnerText = otherUtility.getResourceString("Copyright");
        //TermsCond.InnerText = otherUtility.getResourceString("TermsCond");
        //ContactUs.InnerText = otherUtility.getResourceString("ContactUs");
    }

    private void LoadHeaderFooter()
    {
        Cart.InnerHtml = otherUtility.getResourceString("Cart");
        string sSql = "SELECT HeaderLogoPath,FooterLogoPath,AboutText,ContactInfoText,TwitterText,TwitterFeed,DropALine,Name,EmailAddress,[Message],"
        + " SendBtnText,Phone,Email,CorporateLinkText,CorporateLink,CopyrightText,PrivacyText,PrivacyLink,TermsText,TermsLink"
        + " FROM tblHeaderFooter where site=@site";
        DataTable dtHeaderFooter = dbUtility.getTableBySQLStrParam2015(sSql, "site", SessionUtility.GetValue("UserCountry").ToUpper());
        if (dtHeaderFooter != null)
        {
            if (dtHeaderFooter.Rows.Count > 0)
            {
                DataRow dr = dtHeaderFooter.Rows[0];
                EmailText.InnerText = dr["Email"].ToString();
                PhoneText.InnerText = dr["Phone"].ToString();
                PhoneTextFooter.InnerText = dr["Phone"].ToString() + ":";
                //CorporateTravelText.InnerText = dr["CorporateLinkText"].ToString();
                //CorporateTravelText.HRef = DBUtils.GetFullPath(sSite) + dr["CorporateLink"].ToString();
                ContactInfoText.InnerText = dr["ContactInfoText"].ToString();
                AboutText.InnerHtml = dr["AboutText"].ToString();
                RecentTweets.InnerText = dr["TwitterText"].ToString();
                // DropUsALine.InnerText = dr["DropALine"].ToString();
                //
                //    nameInput.Attributes["placeholder"] = dr["Name"].ToString();
                //    emailInput.Attributes["placeholder"] = dr["EmailAddress"].ToString();
                //   messageInput.Attributes["placeholder"] = dr["Message"].ToString();
                //  btnSendText.Text = dr["SendBtnText"].ToString();
                CopyrightText.InnerText = dr["CopyrightText"].ToString();

                //linkPrivacy.InnerText = dr["PrivacyText"].ToString();
                //linkPrivacy.HRef = DBUtils.GetFullPath(sSite) + dr["PrivacyLink"].ToString();

                linkTerms.InnerText = dr["TermsText"].ToString();
                linkTerms.HRef = dbUtility.GetTermsLink(SessionUtility.GetValue("UserCountry").ToUpper());

                logoImgFooter.Src = "TAImages/" + dr["FooterLogoPath"].ToString();
                logoImgHeader.Src = "TAImages/" + dr["HeaderLogoPath"].ToString();

            }
        }
    }
    private void LoadGeneralInfo()
    {
        string sSql = "SELECT SiteUrl ,PhoneNumber,EmailAddress,BccEmail,BillingEmail"
        + ",SalesEmail,CusServiceEmail,ConfirmationEmail,MainEmail,FreeShippingImg,DefRateHomePage"
        + " ,DefRateHomePageCents,LanguageCode,PayPalLocaleCode,Currency,ShowUsForCurrency,Street,City,Country,[Hours],Title,TermsUrl"
        + " FROM tblSitesGeneralInfo where site=@site";
        DataTable dtGeneralInfoSite = dbUtility.getTableBySQLStrParam2015(sSql, "site", SessionUtility.GetValue("UserCountry").ToUpper());
        if (dtGeneralInfoSite != null)
        {
            if (dtGeneralInfoSite.Rows.Count > 0)
            {
                DataRow dr = dtGeneralInfoSite.Rows[0];

                if (SessionUtility.GetValue("AgentName") != "")
                {
                    dr["EmailAddress"] = SessionUtility.GetValue("AgentEmail") != "" ? SessionUtility.GetValue("AgentEmail") : dr["EmailAddress"].ToString();
                    dr["PhoneNumber"] = SessionUtility.GetValue("AgentPhone") != "" ? SessionUtility.GetValue("AgentPhone") : dr["PhoneNumber"].ToString();
                }

                SiteEmail.InnerHtml = "<a style='display:inline;' class='HeaderIcon' href='mailto:" + dr["EmailAddress"].ToString() + "'>" + dr["EmailAddress"].ToString() + "</a>";//mailto:" dr["EmailAddress"].ToString();

                SitePhone.InnerText = dr["PhoneNumber"].ToString();

                SiteEmailFooter.InnerHtml = "<a style='display:inline;' class='HeaderIcon' href='mailto:" + dr["EmailAddress"].ToString() + "'>" + dr["EmailAddress"].ToString() + "</a>";//mailto:" dr["EmailAddress"].ToString();
                SitePhoneFooter.InnerText = dr["PhoneNumber"].ToString();

                SiteAddressFooter.InnerHtml = dr["Street"].ToString() + " <br/>" + dr["City"].ToString() + " " + dr["Country"].ToString();


            }
        }
    }
    public static string GetAppRootUrl(bool endSlash)
    {
        string host = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        string appRootUrl = HttpContext.Current.Request.ApplicationPath;
        if (!appRootUrl.EndsWith("/")) //a virtual
        {
            appRootUrl += "/";
        }
        if (!endSlash)
        {
            appRootUrl = appRootUrl.Substring(0, appRootUrl.Length - 1);
        }
        return host + appRootUrl;
    }

    public static string GetSourceURL()
    {

        var country = SessionUtility.GetValue("UserCountry");

        string sSiteUrl = dbUtility.ExecScalarByStrParam("select SiteUrl from [tblTelawaySitesInfo] where name =@Country", "Country", country);
        SessionUtility.AddValue("HomeSiteUrl", sSiteUrl);
        return sSiteUrl;
    }

    public static string GetPhoneNumber()
    {
        var country = SessionUtility.GetValue("UserCountry");

        return dbUtility.ExecScalarByStrParam("select PhoneNumber from [tblTelawaySitesInfo] where name = @Country", "Country", country);
    }

    public static string GetEmail()
    {
        var country = SessionUtility.GetValue("UserCountry");

        return dbUtility.ExecScalarByStrParam("select EmailAddress from [tblTelawaySitesInfo] where name =@Country", "Country", country);
    }

    private void LoadTexts()
    {


        if (SessionUtility.GetValue("AgentName") != "" && SessionUtility.GetBoolValue("bDisplayImage"))
        {
            imgAgent.Src = "img/" + SessionUtility.GetValue("AgentName") + ".jpg";
            imgAgent.Visible = true;
        }

    }
    private static string[] GetTweets()
    {
        string[] sAll = new string[6];
        try
        {

            string[] sTweetArray = new string[3];
            string[] sTweetTime = new string[3];

            sTweetArray[0] = "";
            sTweetArray[1] = "";
            sTweetArray[2] = "";

            sTweetTime[0] = "";
            sTweetTime[1] = "";
            sTweetTime[2] = "";

            string sRows = dbUtility.ExecScalarByStrParam2015("SELECT count(*)  FROM tblTweets where  datediff(day,LastUpdate,getdate())<1", "", "");
            int iRows = 0;
            if (sRows != "")
                iRows = Convert.ToInt32(sRows);
            bool bLoadTweets = false;
            if (iRows > 1)
                bLoadTweets = false;
            else
                bLoadTweets = true;


            if (bLoadTweets)
            {

                System.Net.WebClient client = new System.Net.WebClient();
                string AllTweetString = client.DownloadString("http://twitter.com/search?f=realtime&q=telawayUSA");

                //<p class="js-tweet-text tweet-text" 
                //<small class="time">
                int iTweetText11 = -1;
                int iTweetText22 = -1;
                int iTweetText33 = -1;

                int iTweetText1 = AllTweetString.IndexOf("js-tweet-text tweet-text");
                if (iTweetText1 > -1)
                {
                    string sTweetS = AllTweetString.Substring(iTweetText1, AllTweetString.Length - iTweetText1);
                    int iSofTweetText1 = sTweetS.IndexOf("</p>");
                    string sTweet1 = sTweetS.Substring(sTweetS.IndexOf(">") + 1, iSofTweetText1 - sTweetS.IndexOf(">") - 1);

                    while (sTweet1.Contains("twitter-atreply pretty-link") && iTweetText1 > -1)
                    {
                        iTweetText1 = AllTweetString.IndexOf("js-tweet-text tweet-text", iTweetText1 + 1);
                        if (iTweetText1 > -1)
                        {
                            iTweetText11 = iTweetText1;
                            sTweetS = AllTweetString.Substring(iTweetText1, AllTweetString.Length - iTweetText1);
                            iSofTweetText1 = sTweetS.IndexOf("</p>");
                            sTweet1 = sTweetS.Substring(sTweetS.IndexOf(">") + 1, iSofTweetText1 - sTweetS.IndexOf(">") - 1);
                        }
                    }

                    string ssT1 = sTweet1;
                    while (ssT1.Contains("<a href"))
                        ssT1 = GetTextFromTweet(ssT1);

                    sTweetArray[0] = ssT1;
                }
                int iTweetText2 = AllTweetString.IndexOf("js-tweet-text tweet-text", iTweetText11 + 1);
                if (iTweetText2 > -1)
                {
                    iTweetText22 = -1;
                    string sTweetS2 = AllTweetString.Substring(iTweetText2, AllTweetString.Length - iTweetText2);
                    int iSofTweetText2 = sTweetS2.IndexOf("</p>");
                    string sTweet2 = sTweetS2.Substring(sTweetS2.IndexOf(">") + 1, iSofTweetText2 - sTweetS2.IndexOf(">") - 1);
                    while (sTweet2.Contains("twitter-atreply pretty-link") && iTweetText2 > -1)
                    {
                        iTweetText2 = AllTweetString.IndexOf("js-tweet-text tweet-text", iTweetText2 + 1);
                        if (iTweetText2 > -1)
                        {
                            iTweetText22 = iTweetText2;
                            sTweetS2 = AllTweetString.Substring(iTweetText2, AllTweetString.Length - iTweetText2);
                            iSofTweetText2 = sTweetS2.IndexOf("</p>");
                            sTweet2 = sTweetS2.Substring(sTweetS2.IndexOf(">") + 1, iSofTweetText2 - sTweetS2.IndexOf(">") - 1);
                        }
                    }

                    string ssT2 = sTweet2;
                    while (ssT2.Contains("<a href"))
                        ssT2 = GetTextFromTweet(ssT2);
                    sTweetArray[1] = ssT2;
                }
                iTweetText33 = -1;
                int iTweetText3 = AllTweetString.IndexOf("js-tweet-text tweet-text", iTweetText22 + 1);
                if (iTweetText3 > -1)
                {
                    string sTweetS3 = AllTweetString.Substring(iTweetText3, AllTweetString.Length - iTweetText3);
                    int iSofTweetText3 = sTweetS3.IndexOf("</p>");
                    string sTweet3 = sTweetS3.Substring(sTweetS3.IndexOf(">") + 1, iSofTweetText3 - sTweetS3.IndexOf(">") - 1);
                    while (sTweet3.Contains("twitter-atreply pretty-link") && iTweetText3 > -1)
                    {
                        iTweetText3 = AllTweetString.IndexOf("js-tweet-text tweet-text", iTweetText3 + 1);
                        if (iTweetText3 > -1)
                        {
                            iTweetText33 = iTweetText3;
                            sTweetS3 = AllTweetString.Substring(iTweetText3, AllTweetString.Length - iTweetText3);
                            iSofTweetText3 = sTweetS3.IndexOf("</p>");
                            sTweet3 = sTweetS3.Substring(sTweetS3.IndexOf(">") + 1, iSofTweetText3 - sTweetS3.IndexOf(">") - 1);
                        }
                    }

                    string ssT3 = sTweet3;
                    while (ssT3.Contains("<a href"))
                        ssT3 = GetTextFromTweet(ssT3);
                    sTweetArray[2] = ssT3;
                }
                if (iTweetText11 > -1)
                {
                    int iTime1 = AllTweetString.LastIndexOf("<small class=" + "\"" + "time" + "\"" + ">", iTweetText11);
                    string sAllTime1 = AllTweetString.Substring(iTime1, AllTweetString.Length - iTime1);
                    int iSofTime1 = sAllTime1.IndexOf("</small>");
                    string sTimeA1 = sAllTime1.Substring(sAllTime1.IndexOf(">") + 1, iSofTime1 - sAllTime1.IndexOf(">") - 1);
                    string s = sTimeA1.Substring(0, sTimeA1.IndexOf("</span></a>"));
                    string s1Time = s.Substring(s.LastIndexOf(">") + 1, s.Length - s.LastIndexOf(">") - 1);
                    sTweetTime[0] = s1Time;
                }

                if (iTweetText22 > -1)
                {
                    int iTime2 = AllTweetString.LastIndexOf("<small class=" + "\"" + "time" + "\"" + ">", iTweetText22);
                    string sAllTime2 = AllTweetString.Substring(iTime2, AllTweetString.Length - iTime2);
                    int iSofTime2 = sAllTime2.IndexOf("</small>");
                    string sTimeA2 = sAllTime2.Substring(sAllTime2.IndexOf(">") + 1, iSofTime2 - sAllTime2.IndexOf(">") - 1);
                    string s2 = sTimeA2.Substring(0, sTimeA2.IndexOf("</span></a>"));
                    string s2Time = s2.Substring(s2.LastIndexOf(">") + 1, s2.Length - s2.LastIndexOf(">") - 1);
                    sTweetTime[1] = s2Time;
                }

                if (iTweetText33 > -1)
                {
                    int iTime3 = AllTweetString.LastIndexOf("<small class=" + "\"" + "time" + "\"" + ">", iTweetText33);
                    string sAllTime3 = AllTweetString.Substring(iTime3, AllTweetString.Length - iTime3);
                    int iSofTime3 = sAllTime3.IndexOf("</small>");
                    string sTimeA3 = sAllTime3.Substring(sAllTime3.IndexOf(">") + 1, iSofTime3 - sAllTime3.IndexOf(">") - 1);
                    string s3 = sTimeA3.Substring(0, sTimeA3.IndexOf("</span></a>"));
                    string s3Time = s3.Substring(s3.LastIndexOf(">") + 1, s3.Length - s3.LastIndexOf(">") - 1);
                    sTweetTime[2] = s3Time;
                }

                sTweetArray[0] = sTweetArray[0].Replace("<a href", "");
                sTweetArray[0] = sTweetArray[0].Replace("</a>", "");

                sTweetArray[1] = sTweetArray[1].Replace("<a href", "");
                sTweetArray[1] = sTweetArray[1].Replace("</a>", "");

                sTweetArray[2] = sTweetArray[2].Replace("<a href", "");
                sTweetArray[2] = sTweetArray[2].Replace("</a>", "");

                if (sTweetArray[0].Length > 60)
                    sTweetArray[0] = sTweetArray[0].Substring(0, 60);

                if (sTweetArray[1].Length > 60)
                    sTweetArray[1] = sTweetArray[1].Substring(0, 60);

                if (sTweetArray[2].Length > 60)
                    sTweetArray[2] = sTweetArray[2].Substring(0, 60);


                sAll[0] = sTweetArray[0];
                sAll[1] = sTweetArray[1];
                sAll[2] = sTweetArray[2];

                sAll[3] = sTweetTime[0];
                sAll[4] = sTweetTime[1];
                sAll[5] = sTweetTime[2];


                string sSql1 = "delete from tblTweets";
                dbUtility.ExecuteNonQuery2015(sSql1);
                string sSql2 = "";
                if (sTweetArray[0] != "")
                {
                    sSql2 = "INSERT INTO tblTweets(TweetText,TweetTime,LastUpdate) VALUES ('" + sTweetArray[0].Replace("'", "''") + "','" + sTweetTime[0].Replace("'", "''") + "',getdate())";
                    dbUtility.ExecuteNonQuery2015(sSql2);
                }
                if (sTweetArray[1] != "")
                {
                    string sSql3 = "INSERT INTO tblTweets(TweetText,TweetTime ,LastUpdate) VALUES ('" + sTweetArray[1].Replace("'", "''") + "','" + sTweetTime[1].Replace("'", "''") + "',getdate())";
                    dbUtility.ExecuteNonQuery2015(sSql3);
                }

                if (sTweetArray[2] != "")
                {
                    string sSql4 = "INSERT INTO tblTweets(TweetText,TweetTime ,LastUpdate) VALUES ('" + sTweetArray[2].Replace("'", "''") + "','" + sTweetTime[2].Replace("'", "''") + "',getdate())";
                    dbUtility.ExecuteNonQuery2015(sSql4);
                }
            }
            else
            {
                DataTable dt = dbUtility.getTableBySQLStrParam2015("SELECT *  FROM tblTweets order by LastUpdate", "", "");
                if (dt != null)
                {
                    if (dt.Rows.Count > 2)
                    {
                        sAll[0] = dt.Rows[0]["TweetText"].ToString();
                        sAll[1] = dt.Rows[1]["TweetText"].ToString();
                        sAll[2] = dt.Rows[2]["TweetText"].ToString();

                        sAll[3] = dt.Rows[0]["TweetTime"].ToString();
                        sAll[4] = dt.Rows[1]["TweetTime"].ToString();
                        sAll[5] = dt.Rows[2]["TweetTime"].ToString();
                    }
                }

            }
            return sAll;
        }
        catch (Exception ex)
        {
            //emailUtility.SendMailErr("GetTweets:" + ex.Message);
            DataTable dt = dbUtility.getTableBySQLStrParam2015("SELECT *  FROM tblTweets", "", "");
            if (dt != null)
            {
                if (dt.Rows.Count > 2)
                {
                    sAll[0] = dt.Rows[0]["TweetText"].ToString();
                    sAll[1] = dt.Rows[1]["TweetText"].ToString();
                    sAll[2] = dt.Rows[2]["TweetText"].ToString();

                    sAll[3] = dt.Rows[0]["TweetTime"].ToString();
                    sAll[4] = dt.Rows[1]["TweetTime"].ToString();
                    sAll[5] = dt.Rows[2]["TweetTime"].ToString();
                }
            }
            return sAll;
        }
    }

    private static string GetTextFromTweet(string sTweet)
    {
        string sRetString = "";
        int iiT = sTweet.IndexOf("<a href");
        int iiTHref = sTweet.IndexOf("</a>", iiT + 1);
        string ssT = sTweet;
        if (iiT > -1 && sTweet.Length > 10)
            sRetString = sTweet.Substring(0, iiT);
        if (iiT < 5 && iiT >= 0)
            // sRetString = sTweet.Substring(iiTHref + 4, sTweet.Length - iiTHref - 4);
            sRetString = sTweet.Substring(iiTHref + 4, sTweet.Length - iiTHref - 4);
        return sRetString;

    }
    private void LoadTweets()
    {

        try
        {
            string[] sTweets = GetTweets();
            twitts1Text.InnerHtml = sTweets[0];
            TweetTime1.InnerText = sTweets[3];

            twitts2Text.InnerHtml = sTweets[1];
            TweetTime2.InnerText = sTweets[4];

            twitts3Text.InnerHtml = sTweets[2];
            TweetTime3.InnerText = sTweets[5];

            //string sSql = "SELECT TweetTime,TweetText  FROM tblTweets";
            //DataTable dtTweets = DBUtils.getTableBySQLStrParam(sSql, "", "");
            //int iIx = 0;
            //if (dtTweets != null)
            //{
            //    if (dtTweets.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dtTweets.Rows)
            //        {
            //            iIx++;
            //            if (iIx == 1)
            //            {
            //                twitts1Text.InnerText = dr["TweetText"].ToString()+" ";
            //                TweetTime1.InnerText = dr["TweetTime"].ToString();
            //            }
            //            if (iIx == 2)
            //            {
            //                twitts2Text.InnerText = dr["TweetText"].ToString() + " ";
            //                TweetTime2.InnerText = dr["TweetTime"].ToString();
            //            }
            //            if (iIx == 3)
            //            {
            //                twitts3Text.InnerText = dr["TweetText"].ToString() + " ";
            //                TweetTime3.InnerText = dr["TweetTime"].ToString();
            //            }
            //        }
            //    }
            //}
            //
        }
        catch (Exception ex)
        {
            emailUtility.SendMailErr("LoadTweets:" + ex.Message);
        }
    }
    public void UpdateTitle(string sPageTitle)
    {
        PageTitleDiv.InnerText = sPageTitle;
        PageTitleLi.InnerText = sPageTitle;

        LinkHomePage.HRef = dbUtility.GetFullPath(SessionUtility.GetValue("UserCountry").ToUpper());
        HomePageLink.HRef = LinkHomePage.HRef;
    }
}
