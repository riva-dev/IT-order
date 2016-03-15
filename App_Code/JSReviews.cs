using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for JSReviews
/// </summary>
public class JSReview
{
    public string id { get; set;}
    public string name {get; set;}
	public string homeTown {get; set;}
	public string homepageQuote{get; set;}
	public string question1 {get; set;}
	public string answer1 {get; set;}
	public string question2 {get; set;}
    public string answer2 { get; set;}
    public string reviewDate { get; set;}
    public string shortDate { get; set;}

    public JSReview() { }
    public JSReview(DataRow row)
    {
        id = row[0].ToString();
        //name = row[1].ToString();
        name = row["Name"].ToString();
        homeTown = row[2].ToString();
        homepageQuote = row[3].ToString();
        question1 = row[4].ToString();
        answer1 = row[5].ToString();
        question2 = row[6].ToString();
        answer2 = row[7].ToString();
        DateTime date = DateTime.Parse( row[8].ToString());
        reviewDate = date.ToString();
        shortDate = date.ToString("MMM") +" " + date.Year.ToString();
    }
}
