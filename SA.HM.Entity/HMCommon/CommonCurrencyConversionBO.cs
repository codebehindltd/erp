using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    public class CommonCurrencyConversionBO
    {
	public int ConversionId { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyName { get; set; }
    public string CurrencyType { get; set; }
    public int FromCurrencyId { get; set; }
    public int ToCurrencyId { get; set; }
	public decimal ConversionRate { get; set; }
    public decimal BillingConversionRate { get; set; }
	public bool ActiveStat { get; set; }
	public int CreatedBy { get; set; }
	public DateTime CreatedByDate { get; set; }
	public int LastModifiedBy { get; set; }
	public DateTime LastModifiedByDate { get; set; }

    }
}
