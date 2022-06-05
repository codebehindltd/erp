


select * 
into #ControlTable 
from dbo.HotelCompanyPayment

declare @TableID int


DECLARE @COUNTNUM BIGINT = 0;
while exists (select * from #ControlTable)
begin

    select top 1 @TableID = PaymentId , @COUNTNUM = @COUNTNUM+ PaymentId
    from #ControlTable
    order by PaymentId asc

	IF((SELECT AccountingPostingHeadId FROM #ControlTable where PaymentId = @TableID)!= 0)
		BEGIN
			INSERT INTO HotelCompanyPaymentTransectionDetails (	PaymentMode,	PaymentHeadId,	PaymentAmount,	PaymentDate,	CurrencyTypeId,	ConvertionRate,	ChequeDate,	ChequeNumber,	HotelCompanyPaymentId)
			select t.PaymentType, t.AccountingPostingHeadId, ((select SUM(PaymentAmount) from HotelCompanyPaymentDetails WHERE PaymentId= t.PaymentId)-(ISNULL(PaymentAdjustmentAmount, 0))-(ISNULL(AdvanceAmount, 0))), t.CreatedDate, t.CurrencyId, t.ConvertionRate, t.ChequeDate, t.ChequeNumber, t.PaymentId  from HotelCompanyPayment t WHERE PaymentId = @TableID
		END

	IF((SELECT AdvanceAmount FROM #ControlTable where PaymentId = @TableID)!= 0)
		BEGIN
			INSERT INTO HotelCompanyPaymentTransectionDetails (	PaymentMode,	PaymentHeadId,	PaymentAmount,	PaymentDate,	CurrencyTypeId,	ConvertionRate,	ChequeDate,	ChequeNumber,	HotelCompanyPaymentId)
			select t.PaymentType, t.AccountingPostingHeadId, (ISNULL(AdvanceAmount, 0)), t.CreatedDate, t.CurrencyId, t.ConvertionRate, t.ChequeDate, t.ChequeNumber, t.PaymentId  from HotelCompanyPayment t WHERE PaymentId = @TableID
		END
		
		IF((SELECT AdjustmentAccountHeadId FROM #ControlTable where PaymentId = @TableID)!= 0)
		BEGIN
			
				INSERT INTO HotelCompanyPaymentTransectionDetails (	PaymentMode,	PaymentHeadId,	PaymentAmount,	PaymentDate,	CurrencyTypeId,	ConvertionRate,	ChequeDate,	ChequeNumber,	HotelCompanyPaymentId)
				select 'Adjustment', AdjustmentAccountHeadId, PaymentAdjustmentAmount, CreatedDate, CurrencyId, ConvertionRate, null, null, PaymentId  from HotelCompanyPayment WHERE PaymentId = @TableID

		END	

    delete #ControlTable
    where PaymentId = @TableID

end

drop table #ControlTable
