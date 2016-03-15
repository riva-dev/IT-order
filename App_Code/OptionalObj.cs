using System;




public class OptionalObj
{
    private string m_strOptionalName;
    private int m_intOptionalCode;
    private int m_intPlanCode;
    private int m_intEquipmentCode;
    private bool m_bitInsurance;
    private bool m_bitRequiredInsurance;
    private int m_intQuantity;
    private string m_strOptionText;
    private string m_strOptionalType;

    
    public int OptionalCode
    {
        get { return m_intOptionalCode; }
        set { m_intOptionalCode = value; }
    }

    public string OptionalType
    {
        get { return m_strOptionalType; }
        set { m_strOptionalType = value; }
    }
    public string OptionalName
    {
        get { return m_strOptionalName; }
        set { m_strOptionalName = value; }
    }
    public int Quantity
    {
        get { return m_intQuantity; }
        set { m_intQuantity = value; }
    }
    public string OptionText
    {
        get { return m_strOptionText; }
        set { m_strOptionText = value; }
    }
    public int PlanCode
    {
        get { return m_intPlanCode; }
        set { m_intPlanCode = value; }
    }
    public int EquipmentCode
    {
        get { return m_intEquipmentCode; }
        set { m_intEquipmentCode = value; }
    }
    public bool Insurance
    {
        get { return m_bitInsurance; }
        set { m_bitInsurance = value; }
    }

    public bool RequiredInsurance
    {
        get { return m_bitRequiredInsurance; }
        set { m_bitRequiredInsurance = value; }
    }
  
}

       
 
