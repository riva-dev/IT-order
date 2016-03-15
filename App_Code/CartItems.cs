using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CartItems for the end of the order
/// </summary>
public class CartItems
{
    public CartItems() { }
    public List<CartItems> getObject(string[] Accessories, decimal conversionRate)
    {
        List<CartItems> AccessoriesList = new List<CartItems>();

        if (Accessories.Length > 0)
        {
            for (int i = 0; i < Accessories.Length - 1; i++)
            {
                string[] item = Accessories[i].Split(',');
                CartItems c = new CartItems();
                c.Id = item[0];
                c.Name = item[1];
                c.Quantity = int.Parse(item[2]);
                c.Price = Convert.ToDouble(item[3]);
                c.PriceUSA = Convert.ToDouble((Convert.ToDouble(item[3]) * (double)conversionRate).ToString("F"));
                c.Type = "AC";
                AccessoriesList.Add(c);

            }
        }
        return AccessoriesList;
    }

    /// <summary>
    /// Members
    /// </summary>

    private string name;
    private int quantity;
    private double price;
    private string type;
    private string id;
    private double priceUSA;

    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Type { get; set; }
    public string Id { get; set; }
    public double PriceUSA { get; set; }


}



/// <summary>
/// PromoCode from signup coupon code
/// </summary>
public class Promocode
{
    public Promocode(string sPromoCode)
    {

        this.IsVaild = dbUtility.IsValidCoupon(sPromoCode);
        if (this.IsVaild == false)
            return;
        DataTable dt = dbUtility.getDataTableBySQL("SELECT S.* ,D.DiscountPercent FROM tblSignupCoupons S INNER JOIN tblDiscountCodes D ON S.DiscountCode=D.DiscountCode WHERE CouponCode ='" + sPromoCode + "'");
        DataRow dr = dt.Rows[0];

        this.Id = int.Parse(dr["Counter"].ToString());
        this.PromoCode = dr["CouponCode"].ToString();
        this.PromoName = dr["CouponName"].ToString();
        this.DiscountCode = int.Parse(dr["DiscountCode"].ToString());
        this.DiscountPercent = double.Parse(dr["DiscountPercent"].ToString());
        this.CouponCredit = double.Parse(dr["CouponCredit"].ToString());
        this.OnlyToSecond = bool.Parse(dr["OnlyToSecondOrder"].ToString());
        this.OnlyToThird = bool.Parse(dr["OnlyToThirdOrder"].ToString());
        this.OnlyToGreat1NotOP = bool.Parse(dr["OnlyToGreat1NotOP"].ToString());
        this.IsIncludeAccessories = false;
    }

    /// <summary>
    /// Members
    /// </summary>
    public int Id;
    public string PromoCode;
    public string PromoName;
    public bool IsVaild;
    public int DiscountCode;
    public double DiscountPercent;
    public double CouponCredit;
    public bool OnlyToSecond;
    public bool OnlyToThird;
    public bool OnlyToGreat1NotOP;
    public bool IsIncludeAccessories;



    /// <summary>
    /// return the amount of the Promocode 
    /// </summary>
    /// <param name="promoCode"></param>
    /// <param name="itemTotal"></param>
    /// <param name="itemTotalOrder1"></param>
    /// <returns>discount amount</returns>
    /// 
    public string getDiscountCopunAndValid(Promocode promoCode, double itemTotal, double itemTotalOrder1, int SimQnt)
    {
        double discountAmount = 0;
        var NotValid = "";
        var type = "";
        if (promoCode.IsVaild)
        {
        if (promoCode.CouponCredit > 0)
        {
            discountAmount = -promoCode.CouponCredit;
            type = "Credit";

        }
        else if (promoCode.DiscountPercent > 0)
        {
            type = "Percent";
            if (promoCode.OnlyToSecond || promoCode.OnlyToThird)
            {
                if (promoCode.OnlyToSecond)
                    if (SimQnt > 1)
                        discountAmount = -((itemTotalOrder1 * (promoCode.DiscountPercent)) / 100);
                    else NotValid = "Only To Second order";
                if (promoCode.OnlyToThird)
                    if (SimQnt > 2)
                        discountAmount = -((itemTotalOrder1 * (promoCode.DiscountPercent)) / 100);
                    else NotValid = "Only To Third order";
            }
            else discountAmount = -((itemTotal * (promoCode.DiscountPercent)) / 100);
        }
        }
        else NotValid = "Not valid";
        string s = discountAmount.ToString("F");
        double d = Convert.ToDouble(s) * -1;
        return d + "|" + NotValid + "|" + type;
    }

    public double getDiscountCopun(Promocode promoCode, double itemTotal, double itemTotalOrder1, int SimQnt)
    {
        double discountAmount = 0;

        if (promoCode.CouponCredit > 0)
            discountAmount = -promoCode.CouponCredit;

        else if (promoCode.DiscountPercent > 0)
        {
            if (promoCode.OnlyToSecond || promoCode.OnlyToThird)
            {
                if (promoCode.OnlyToSecond && SimQnt > 1)
                    discountAmount = -((itemTotalOrder1 * (promoCode.DiscountPercent)) / 100);
                if (promoCode.OnlyToThird && SimQnt > 2)
                    discountAmount = -((itemTotalOrder1 * (promoCode.DiscountPercent)) / 100);

            }
            else discountAmount = -((itemTotalOrder1 * SimQnt *(promoCode.DiscountPercent)) / 100);
        }

        string s = discountAmount.ToString("F");
        double d = Convert.ToDouble(s);
        return d;
    }
    /// <summary>
    /// the amout of discount to one order with accessories
    /// </summary>
    /// <param name="promoCode"></param>
    /// <param name="itemTotalOrder1"></param>
    /// <param name="Acc"></param>
    /// <returns></returns>
    public double getDiscount(Promocode promoCode, double itemTotalOrder1, double Acc, int SimQnt, int i)
    {
        double discountAmount = 0;

        if (promoCode.CouponCredit > 0)
        {
            var v = promoCode.CouponCredit / SimQnt;
            if (v > itemTotalOrder1)
                if (Acc == 0)
                    discountAmount = -itemTotalOrder1;
                else discountAmount = -(v + (v - itemTotalOrder1) * (SimQnt - 1));
            else discountAmount = -(v + (v - itemTotalOrder1) * (SimQnt - 1));
        }
        else if (promoCode.DiscountPercent > 0)
        {
            //TO DO : 
            //CHECK ON RUN TIME WITH THE ACC.
            if (i == 0)
                if ((promoCode.OnlyToSecond || promoCode.OnlyToThird))
                {
                    if (promoCode.OnlyToSecond && SimQnt > 1)
                        discountAmount = -(((itemTotalOrder1 + Acc) * (promoCode.DiscountPercent)) / 100);

                    if (promoCode.OnlyToThird && SimQnt > 2)
                        discountAmount = -(((itemTotalOrder1 + Acc) * (promoCode.DiscountPercent)) / 100);
                }
                else discountAmount = -(((itemTotalOrder1 + Acc) * (promoCode.DiscountPercent)) / 100);

            else discountAmount = -(((itemTotalOrder1) * (promoCode.DiscountPercent)) / 100);
        }

        string s = discountAmount.ToString("F");
        double d = Convert.ToDouble(s);
        return d;
    }
    /// <summary>
    /// check if promoCode is valid with PhonesRequired
    /// </summary>
    /// <returns></returns>
    public bool IsValidPromoCodeVSPhonesRequired(Promocode promoCode, int? SimQnt)
    {
        if (SimQnt == 1 && (promoCode.OnlyToSecond == true || promoCode.OnlyToSecond == true)) return false;
        if (SimQnt == 2 && promoCode.OnlyToThird == true) return false;
        return true;
    }


}