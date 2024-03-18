namespace Core;

public class ReportModel
{
    public DateTime CloseDateTime { get;set; }
    public string Epic { get;set; }
    public string Strategy { get;set; }
    public decimal OpenPrice { get;set; }
    public decimal ClosePrice { get;set; }
    public decimal CurrentBalance { get;set; }
}
