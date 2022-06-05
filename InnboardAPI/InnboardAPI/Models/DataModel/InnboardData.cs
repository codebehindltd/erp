namespace InnboardAPI.Models.DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class InnboardData : DbContext
    {
        public InnboardData()
            : base("name=InnboardData")
        {
        }

        public virtual DbSet<BanquetBillPayment> BanquetBillPayment { get; set; }
        public virtual DbSet<BanquetInformation> BanquetInformation { get; set; }
        public virtual DbSet<BanquetOccessionType> BanquetOccessionType { get; set; }
        public virtual DbSet<BanquetRefference> BanquetRefference { get; set; }
        public virtual DbSet<BanquetRequisites> BanquetRequisites { get; set; }
        public virtual DbSet<BanquetReservation> BanquetReservation { get; set; }
        public virtual DbSet<BanquetReservationBillPayment> BanquetReservationBillPayment { get; set; }
        public virtual DbSet<BanquetReservationClassificationDiscount> BanquetReservationClassificationDiscount { get; set; }
        public virtual DbSet<BanquetReservationDetail> BanquetReservationDetail { get; set; }
        public virtual DbSet<BanquetSeatingPlan> BanquetSeatingPlan { get; set; }
        public virtual DbSet<CgsMonthlyTransaction> CgsMonthlyTransaction { get; set; }
        public virtual DbSet<CgsTransactionHead> CgsTransactionHead { get; set; }
        public virtual DbSet<CommonBank> CommonBank { get; set; }
        public virtual DbSet<CommonBusinessPromotion> CommonBusinessPromotion { get; set; }
        public virtual DbSet<CommonBusinessPromotionDetail> CommonBusinessPromotionDetail { get; set; }
        public virtual DbSet<CommonCheckedByApprovedBy> CommonCheckedByApprovedBy { get; set; }
        public virtual DbSet<CommonCity> CommonCity { get; set; }
        public virtual DbSet<CommonCompanyBank> CommonCompanyBank { get; set; }
        public virtual DbSet<CommonCompanyProfile> CommonCompanyProfile { get; set; }
        public virtual DbSet<CommonCostCenter> CommonCostCenter { get; set; }
        public virtual DbSet<CommonCountries> CommonCountries { get; set; }
        public virtual DbSet<CommonCurrency> CommonCurrency { get; set; }
        public virtual DbSet<CommonCurrencyConversion> CommonCurrencyConversion { get; set; }
        public virtual DbSet<CommonCurrencyTransaction> CommonCurrencyTransaction { get; set; }
        public virtual DbSet<CommonCustomFieldData> CommonCustomFieldData { get; set; }
        public virtual DbSet<CommonDocuments> CommonDocuments { get; set; }
        public virtual DbSet<CommonIndustry> CommonIndustry { get; set; }
        public virtual DbSet<CommonLocation> CommonLocation { get; set; }
        public virtual DbSet<CommonMessage> CommonMessage { get; set; }
        public virtual DbSet<CommonMessageDetails> CommonMessageDetails { get; set; }
        public virtual DbSet<CommonModuleName> CommonModuleName { get; set; }
        public virtual DbSet<CommonModuleType> CommonModuleType { get; set; }
        public virtual DbSet<CommonPaymentMode> CommonPaymentMode { get; set; }
        public virtual DbSet<CommonPrinterInfo> CommonPrinterInfo { get; set; }
        public virtual DbSet<CommonProfession> CommonProfession { get; set; }
        public virtual DbSet<CommonReportConfigDetails> CommonReportConfigDetails { get; set; }
        public virtual DbSet<CommonReportConfigMaster> CommonReportConfigMaster { get; set; }
        public virtual DbSet<CommonSetup> CommonSetup { get; set; }
        public virtual DbSet<DashboardItem> DashboardItem { get; set; }
        public virtual DbSet<DashboardManagement> DashboardManagement { get; set; }
        public virtual DbSet<GatePass> GatePass { get; set; }
        public virtual DbSet<GatePassDetails> GatePassDetails { get; set; }
        public virtual DbSet<GLAccountConfiguration> GLAccountConfiguration { get; set; }
        public virtual DbSet<GLAccountsMapping> GLAccountsMapping { get; set; }
        public virtual DbSet<GLAccountTypeSetup> GLAccountTypeSetup { get; set; }
        public virtual DbSet<GLBudget> GLBudget { get; set; }
        public virtual DbSet<GLBudgetDetails> GLBudgetDetails { get; set; }
        public virtual DbSet<GLCashFlowGroupHead> GLCashFlowGroupHead { get; set; }
        public virtual DbSet<GLCashFlowHead> GLCashFlowHead { get; set; }
        public virtual DbSet<GLCashFlowSetup> GLCashFlowSetup { get; set; }
        public virtual DbSet<GLCommonSetup> GLCommonSetup { get; set; }
        public virtual DbSet<GLCompany> GLCompany { get; set; }
        public virtual DbSet<GLDealMaster> GLDealMaster { get; set; }
        public virtual DbSet<GLDonor> GLDonor { get; set; }
        public virtual DbSet<GLFiscalYear> GLFiscalYear { get; set; }
        public virtual DbSet<GLFiscalYearClosingDetails> GLFiscalYearClosingDetails { get; set; }
        public virtual DbSet<GLFiscalYearClosingMaster> GLFiscalYearClosingMaster { get; set; }
        public virtual DbSet<GLFixedAssets> GLFixedAssets { get; set; }
        public virtual DbSet<GLGeneralLedger> GLGeneralLedger { get; set; }
        public virtual DbSet<GLLedger> GLLedger { get; set; }
        public virtual DbSet<GLLedgerDetails> GLLedgerDetails { get; set; }
        public virtual DbSet<GLLedgerMaster> GLLedgerMaster { get; set; }
        public virtual DbSet<GLNodeMatrix> GLNodeMatrix { get; set; }
        public virtual DbSet<GLNotesConfiguration> GLNotesConfiguration { get; set; }
        public virtual DbSet<GLProfitLossGroupHead> GLProfitLossGroupHead { get; set; }
        public virtual DbSet<GLProfitLossHead> GLProfitLossHead { get; set; }
        public virtual DbSet<GLProfitLossSetup> GLProfitLossSetup { get; set; }
        public virtual DbSet<GLProject> GLProject { get; set; }
        public virtual DbSet<GLReportConfiguration> GLReportConfiguration { get; set; }
        public virtual DbSet<GLReportConfigurationDetail> GLReportConfigurationDetail { get; set; }
        public virtual DbSet<GLRuleBreak> GLRuleBreak { get; set; }
        public virtual DbSet<GLVoucherApprovedInfo> GLVoucherApprovedInfo { get; set; }
        public virtual DbSet<HotelAirlineInformation> HotelAirlineInformation { get; set; }
        public virtual DbSet<HotelCompanyBillGeneration> HotelCompanyBillGeneration { get; set; }
        public virtual DbSet<HotelCompanyBillGenerationDetails> HotelCompanyBillGenerationDetails { get; set; }
        public virtual DbSet<HotelCompanyPayment> HotelCompanyPayment { get; set; }
        public virtual DbSet<HotelCompanyPaymentDetails> HotelCompanyPaymentDetails { get; set; }
        public virtual DbSet<HotelCompanyPaymentLedger> HotelCompanyPaymentLedger { get; set; }
        public virtual DbSet<HotelCompanyPaymentLedgerClosingDetails> HotelCompanyPaymentLedgerClosingDetails { get; set; }
        public virtual DbSet<HotelCompanyPaymentLedgerClosingMaster> HotelCompanyPaymentLedgerClosingMaster { get; set; }
        public virtual DbSet<HotelCompanyWiseDiscountPolicy> HotelCompanyWiseDiscountPolicy { get; set; }
        public virtual DbSet<HotelComplementaryItem> HotelComplementaryItem { get; set; }
        public virtual DbSet<HotelCurrencyConversion> HotelCurrencyConversion { get; set; }
        public virtual DbSet<HotelDailyRoomCondition> HotelDailyRoomCondition { get; set; }
        public virtual DbSet<HotelDayClose> HotelDayClose { get; set; }
        public virtual DbSet<HotelDayProcessing> HotelDayProcessing { get; set; }
        public virtual DbSet<HotelEmpTaskAssignment> HotelEmpTaskAssignment { get; set; }
        public virtual DbSet<HotelFloor> HotelFloor { get; set; }
        public virtual DbSet<HotelFloorBlock> HotelFloorBlock { get; set; }
        public virtual DbSet<HotelFloorManagement> HotelFloorManagement { get; set; }
        public virtual DbSet<HotelGuestBillApproved> HotelGuestBillApproved { get; set; }
        public virtual DbSet<HotelGuestBillPayment> HotelGuestBillPayment { get; set; }
        public virtual DbSet<HotelGuestBirthdayNotification> HotelGuestBirthdayNotification { get; set; }
        public virtual DbSet<HotelGuestCompany> HotelGuestCompany { get; set; }
        public virtual DbSet<HotelGuestDayLetCheckOut> HotelGuestDayLetCheckOut { get; set; }
        public virtual DbSet<HotelGuestDocuments> HotelGuestDocuments { get; set; }
        public virtual DbSet<HotelGuestExtraServiceBillApproved> HotelGuestExtraServiceBillApproved { get; set; }
        public virtual DbSet<HotelGuestHouseCheckOut> HotelGuestHouseCheckOut { get; set; }
        public virtual DbSet<HotelGuestInformation> HotelGuestInformation { get; set; }
        public virtual DbSet<HotelGuestInformationOnline> HotelGuestInformationOnline { get; set; }
        public virtual DbSet<HotelGuestPreference> HotelGuestPreference { get; set; }
        public virtual DbSet<HotelGuestPreferenceMapping> HotelGuestPreferenceMapping { get; set; }
        public virtual DbSet<HotelGuestReference> HotelGuestReference { get; set; }
        public virtual DbSet<HotelGuestRegistration> HotelGuestRegistration { get; set; }
        public virtual DbSet<HotelGuestReservation> HotelGuestReservation { get; set; }
        public virtual DbSet<HotelGuestReservationOnline> HotelGuestReservationOnline { get; set; }
        public virtual DbSet<HotelGuestRoomShiftInfo> HotelGuestRoomShiftInfo { get; set; }
        public virtual DbSet<HotelGuestServiceBill> HotelGuestServiceBill { get; set; }
        public virtual DbSet<HotelGuestServiceBillApproved> HotelGuestServiceBillApproved { get; set; }
        public virtual DbSet<HotelGuestServiceInfo> HotelGuestServiceInfo { get; set; }
        public virtual DbSet<HotelHKRoomStatus> HotelHKRoomStatus { get; set; }
        public virtual DbSet<HotelLinkedRoomDetails> HotelLinkedRoomDetails { get; set; }
        public virtual DbSet<HotelLinkedRoomMaster> HotelLinkedRoomMaster { get; set; }
        public virtual DbSet<HotelMonthToDateInfo> HotelMonthToDateInfo { get; set; }
        public virtual DbSet<HotelOnlineRoomReservation> HotelOnlineRoomReservation { get; set; }
        public virtual DbSet<HotelOnlineRoomReservationDetail> HotelOnlineRoomReservationDetail { get; set; }
        public virtual DbSet<HotelPaymentSummary> HotelPaymentSummary { get; set; }
        public virtual DbSet<HotelRegistrationAireportPickupDrop> HotelRegistrationAireportPickupDrop { get; set; }
        public virtual DbSet<HotelRegistrationComplementaryItem> HotelRegistrationComplementaryItem { get; set; }
        public virtual DbSet<HotelRegistrationServiceInfo> HotelRegistrationServiceInfo { get; set; }
        public virtual DbSet<HotelReservationAireportPickupDrop> HotelReservationAireportPickupDrop { get; set; }
        public virtual DbSet<HotelReservationBillPayment> HotelReservationBillPayment { get; set; }
        public virtual DbSet<HotelReservationComplementaryItem> HotelReservationComplementaryItem { get; set; }
        public virtual DbSet<HotelReservationNoShowProcess> HotelReservationNoShowProcess { get; set; }
        public virtual DbSet<HotelReservationServiceInfo> HotelReservationServiceInfo { get; set; }
        public virtual DbSet<HotelRoomCondition> HotelRoomCondition { get; set; }
        public virtual DbSet<HotelRoomDiscrepancy> HotelRoomDiscrepancy { get; set; }
        public virtual DbSet<HotelRoomFeatures> HotelRoomFeatures { get; set; }
        public virtual DbSet<HotelRoomFeaturesInfo> HotelRoomFeaturesInfo { get; set; }
        public virtual DbSet<HotelRoomInventory> HotelRoomInventory { get; set; }
        public virtual DbSet<HotelRoomInventoryDetails> HotelRoomInventoryDetails { get; set; }
        public virtual DbSet<HotelRoomLogFile> HotelRoomLogFile { get; set; }
        public virtual DbSet<HotelRoomNumber> HotelRoomNumber { get; set; }
        public virtual DbSet<HotelRoomOwner> HotelRoomOwner { get; set; }
        public virtual DbSet<HotelRoomOwnerDetail> HotelRoomOwnerDetail { get; set; }
        public virtual DbSet<HotelRoomRegistration> HotelRoomRegistration { get; set; }
        public virtual DbSet<HotelRoomRegistrationDetail> HotelRoomRegistrationDetail { get; set; }
        public virtual DbSet<HotelRoomReservation> HotelRoomReservation { get; set; }
        public virtual DbSet<HotelRoomReservationDetail> HotelRoomReservationDetail { get; set; }
        public virtual DbSet<HotelRoomReservationDetailOnline> HotelRoomReservationDetailOnline { get; set; }
        public virtual DbSet<HotelRoomReservationOnline> HotelRoomReservationOnline { get; set; }
        public virtual DbSet<HotelRoomStatus> HotelRoomStatus { get; set; }
        public virtual DbSet<HotelRoomStatusPossiblePath> HotelRoomStatusPossiblePath { get; set; }
        public virtual DbSet<HotelRoomStatusPossiblePathHead> HotelRoomStatusPossiblePathHead { get; set; }
        public virtual DbSet<HotelRoomStopChargePosting> HotelRoomStopChargePosting { get; set; }
        public virtual DbSet<HotelRoomType> HotelRoomType { get; set; }
        public virtual DbSet<HotelSalesSummary> HotelSalesSummary { get; set; }
        public virtual DbSet<HotelSegmentHead> HotelSegmentHead { get; set; }
        public virtual DbSet<HotelSegmentRateChart> HotelSegmentRateChart { get; set; }
        public virtual DbSet<HotelServiceBillTransfered> HotelServiceBillTransfered { get; set; }
        public virtual DbSet<HotelStock> HotelStock { get; set; }
        public virtual DbSet<HotelTaskAssignmentRoomWise> HotelTaskAssignmentRoomWise { get; set; }
        public virtual DbSet<HotelTaskAssignmentToEmployee> HotelTaskAssignmentToEmployee { get; set; }
        public virtual DbSet<InvCategory> InvCategory { get; set; }
        public virtual DbSet<InvCategoryCostCenterMapping> InvCategoryCostCenterMapping { get; set; }
        public virtual DbSet<InvCogsAccountVsItemCategoryMappping> InvCogsAccountVsItemCategoryMappping { get; set; }
        public virtual DbSet<InvCogsClosing> InvCogsClosing { get; set; }
        public virtual DbSet<InvCostCenterNDineTimeWiseItemTransaction> InvCostCenterNDineTimeWiseItemTransaction { get; set; }
        public virtual DbSet<InvDefaultClassificationConfiguration> InvDefaultClassificationConfiguration { get; set; }
        public virtual DbSet<InvDineTimeWiseItemTransaction> InvDineTimeWiseItemTransaction { get; set; }
        public virtual DbSet<InvDineTimeWiseItemTransactionDetails> InvDineTimeWiseItemTransactionDetails { get; set; }
        public virtual DbSet<InvDineTimeWisePaymentDetails> InvDineTimeWisePaymentDetails { get; set; }
        public virtual DbSet<InvInventoryAccountVsItemCategoryMappping> InvInventoryAccountVsItemCategoryMappping { get; set; }
        public virtual DbSet<InvItem> InvItem { get; set; }
        public virtual DbSet<InvItemClassification> InvItemClassification { get; set; }
        public virtual DbSet<InvItemClassificationCostCenterMapping> InvItemClassificationCostCenterMapping { get; set; }
        public virtual DbSet<InvItemCostCenterMapping> InvItemCostCenterMapping { get; set; }
        public virtual DbSet<InvItemDetails> InvItemDetails { get; set; }
        public virtual DbSet<InvItemSpecialRemarks> InvItemSpecialRemarks { get; set; }
        public virtual DbSet<InvItemStockAdjustment> InvItemStockAdjustment { get; set; }
        public virtual DbSet<InvItemStockAdjustmentDetails> InvItemStockAdjustmentDetails { get; set; }
        public virtual DbSet<InvItemStockInformation> InvItemStockInformation { get; set; }
        public virtual DbSet<InvItemStockSerialInformation> InvItemStockSerialInformation { get; set; }
        public virtual DbSet<InvItemStockVariance> InvItemStockVariance { get; set; }
        public virtual DbSet<InvItemStockVarianceDetails> InvItemStockVarianceDetails { get; set; }
        public virtual DbSet<InvItemSupplierMapping> InvItemSupplierMapping { get; set; }
        public virtual DbSet<InvItemTransaction> InvItemTransaction { get; set; }
        public virtual DbSet<InvItemTransactionDetails> InvItemTransactionDetails { get; set; }
        public virtual DbSet<InvItemTransactionLog> InvItemTransactionLog { get; set; }
        public virtual DbSet<InvItemTransactionLogSummary> InvItemTransactionLogSummary { get; set; }
        public virtual DbSet<InvItemTransactionPaymentDetails> InvItemTransactionPaymentDetails { get; set; }
        public virtual DbSet<InvLocation> InvLocation { get; set; }
        public virtual DbSet<InvLocationCostCenterMapping> InvLocationCostCenterMapping { get; set; }
        public virtual DbSet<InvManufacturer> InvManufacturer { get; set; }
        public virtual DbSet<InvRecipeDeductionDetails> InvRecipeDeductionDetails { get; set; }
        public virtual DbSet<InvServiceBandwidth> InvServiceBandwidth { get; set; }
        public virtual DbSet<InvServicePackage> InvServicePackage { get; set; }
        public virtual DbSet<InvServicePriceMatrix> InvServicePriceMatrix { get; set; }
        public virtual DbSet<InvTransactionMode> InvTransactionMode { get; set; }
        public virtual DbSet<InvUnitConversion> InvUnitConversion { get; set; }
        public virtual DbSet<InvUnitHead> InvUnitHead { get; set; }
        public virtual DbSet<InvUserGroupCostCenterMapping> InvUserGroupCostCenterMapping { get; set; }
        public virtual DbSet<LCInformation> LCInformation { get; set; }
        public virtual DbSet<LCInformationDetail> LCInformationDetail { get; set; }
        public virtual DbSet<LCOverHeadExpense> LCOverHeadExpense { get; set; }
        public virtual DbSet<LCOverHeadName> LCOverHeadName { get; set; }
        public virtual DbSet<LCPayment> LCPayment { get; set; }
        public virtual DbSet<LCPaymentLedger> LCPaymentLedger { get; set; }
        public virtual DbSet<MemberPaymentConfiguration> MemberPaymentConfiguration { get; set; }
        public virtual DbSet<MemMemberBasics> MemMemberBasics { get; set; }
        public virtual DbSet<MemMemberFamilyMember> MemMemberFamilyMember { get; set; }
        public virtual DbSet<MemMemberReference> MemMemberReference { get; set; }
        public virtual DbSet<MemMemberType> MemMemberType { get; set; }
        public virtual DbSet<MenuGroupNLinkIcon> MenuGroupNLinkIcon { get; set; }
        public virtual DbSet<PayrollAllowanceDeductionHead> PayrollAllowanceDeductionHead { get; set; }
        public virtual DbSet<PayrollApplicantResult> PayrollApplicantResult { get; set; }
        public virtual DbSet<PayrollAppraisalEvalutionBy> PayrollAppraisalEvalutionBy { get; set; }
        public virtual DbSet<PayrollAppraisalEvalutionRatingFactorDetails> PayrollAppraisalEvalutionRatingFactorDetails { get; set; }
        public virtual DbSet<PayrollAppraisalMarksIndicator> PayrollAppraisalMarksIndicator { get; set; }
        public virtual DbSet<PayrollAppraisalRatingFactor> PayrollAppraisalRatingFactor { get; set; }
        public virtual DbSet<PayrollAppraisalRatingScale> PayrollAppraisalRatingScale { get; set; }
        public virtual DbSet<PayrollAttendanceDevice> PayrollAttendanceDevice { get; set; }
        public virtual DbSet<PayrollAttendanceEventHead> PayrollAttendanceEventHead { get; set; }
        public virtual DbSet<PayrollBenefitHead> PayrollBenefitHead { get; set; }
        public virtual DbSet<PayrollBestEmployeeNomination> PayrollBestEmployeeNomination { get; set; }
        public virtual DbSet<PayrollBestEmployeeNominationDetails> PayrollBestEmployeeNominationDetails { get; set; }
        public virtual DbSet<PayrollBonusSetting> PayrollBonusSetting { get; set; }
        public virtual DbSet<PayrollDepartment> PayrollDepartment { get; set; }
        public virtual DbSet<PayrollDesignation> PayrollDesignation { get; set; }
        public virtual DbSet<PayrollDisciplinaryAction> PayrollDisciplinaryAction { get; set; }
        public virtual DbSet<PayrollDisciplinaryActionReason> PayrollDisciplinaryActionReason { get; set; }
        public virtual DbSet<PayrollDisciplinaryActionType> PayrollDisciplinaryActionType { get; set; }
        public virtual DbSet<PayrollDonor> PayrollDonor { get; set; }
        public virtual DbSet<PayrollEducationLevel> PayrollEducationLevel { get; set; }
        public virtual DbSet<PayrollEmpAdvanceTaken> PayrollEmpAdvanceTaken { get; set; }
        public virtual DbSet<PayrollEmpAllowanceDeduction> PayrollEmpAllowanceDeduction { get; set; }
        public virtual DbSet<PayrollEmpAppraisalEvalution> PayrollEmpAppraisalEvalution { get; set; }
        public virtual DbSet<PayrollEmpAttendance> PayrollEmpAttendance { get; set; }
        public virtual DbSet<PayrollEmpBenefit> PayrollEmpBenefit { get; set; }
        public virtual DbSet<PayrollEmpCareerInfo> PayrollEmpCareerInfo { get; set; }
        public virtual DbSet<PayrollEmpCareerTraining> PayrollEmpCareerTraining { get; set; }
        public virtual DbSet<PayrollEmpDependent> PayrollEmpDependent { get; set; }
        public virtual DbSet<PayrollEmpDistrict> PayrollEmpDistrict { get; set; }
        public virtual DbSet<PayrollEmpDivision> PayrollEmpDivision { get; set; }
        public virtual DbSet<PayrollEmpEducation> PayrollEmpEducation { get; set; }
        public virtual DbSet<PayrollEmpExperience> PayrollEmpExperience { get; set; }
        public virtual DbSet<PayrollEmpGrade> PayrollEmpGrade { get; set; }
        public virtual DbSet<PayrollEmpGratuity> PayrollEmpGratuity { get; set; }
        public virtual DbSet<PayrollEmpIncrement> PayrollEmpIncrement { get; set; }
        public virtual DbSet<PayrollEmpLanguage> PayrollEmpLanguage { get; set; }
        public virtual DbSet<PayrollEmpLastMonthBenifitsPayment> PayrollEmpLastMonthBenifitsPayment { get; set; }
        public virtual DbSet<PayrollEmpLeaveInformation> PayrollEmpLeaveInformation { get; set; }
        public virtual DbSet<PayrollEmpLoan> PayrollEmpLoan { get; set; }
        public virtual DbSet<PayrollEmployee> PayrollEmployee { get; set; }
        public virtual DbSet<PayrollEmployeeBillGeneration> PayrollEmployeeBillGeneration { get; set; }
        public virtual DbSet<PayrollEmployeeBillGenerationDetails> PayrollEmployeeBillGenerationDetails { get; set; }
        public virtual DbSet<PayrollEmployeePayment> PayrollEmployeePayment { get; set; }
        public virtual DbSet<PayrollEmployeePaymentDetails> PayrollEmployeePaymentDetails { get; set; }
        public virtual DbSet<PayrollEmployeePaymentLedger> PayrollEmployeePaymentLedger { get; set; }
        public virtual DbSet<PayrollEmployeePaymentLedgerClosingDetails> PayrollEmployeePaymentLedgerClosingDetails { get; set; }
        public virtual DbSet<PayrollEmployeePaymentLedgerClosingMaster> PayrollEmployeePaymentLedgerClosingMaster { get; set; }
        public virtual DbSet<PayrollEmployeeStatus> PayrollEmployeeStatus { get; set; }
        public virtual DbSet<PayrollEmpOverTime> PayrollEmpOverTime { get; set; }
        public virtual DbSet<PayrollEmpOverTimeSetup> PayrollEmpOverTimeSetup { get; set; }
        public virtual DbSet<PayrollEmpPayScale> PayrollEmpPayScale { get; set; }
        public virtual DbSet<PayrollEmpPF> PayrollEmpPF { get; set; }
        public virtual DbSet<PayrollEmpPromotion> PayrollEmpPromotion { get; set; }
        public virtual DbSet<PayrollEmpReference> PayrollEmpReference { get; set; }
        public virtual DbSet<PayrollEmpRoster> PayrollEmpRoster { get; set; }
        public virtual DbSet<PayrollEmpSalaryProcess> PayrollEmpSalaryProcess { get; set; }
        public virtual DbSet<PayrollEmpSalaryProcessDetail> PayrollEmpSalaryProcessDetail { get; set; }
        public virtual DbSet<PayrollEmpSalaryProcessDetailTemp> PayrollEmpSalaryProcessDetailTemp { get; set; }
        public virtual DbSet<PayrollEmpSalaryProcessTemp> PayrollEmpSalaryProcessTemp { get; set; }
        public virtual DbSet<PayrollEmpTax> PayrollEmpTax { get; set; }
        public virtual DbSet<PayrollEmpTaxDeduction> PayrollEmpTaxDeduction { get; set; }
        public virtual DbSet<PayrollEmpTaxDeductionSetting> PayrollEmpTaxDeductionSetting { get; set; }
        public virtual DbSet<PayrollEmpTermination> PayrollEmpTermination { get; set; }
        public virtual DbSet<PayrollEmpThana> PayrollEmpThana { get; set; }
        public virtual DbSet<PayrollEmpTimeSlab> PayrollEmpTimeSlab { get; set; }
        public virtual DbSet<PayrollEmpTimeSlabRoster> PayrollEmpTimeSlabRoster { get; set; }
        public virtual DbSet<PayrollEmpTraining> PayrollEmpTraining { get; set; }
        public virtual DbSet<PayrollEmpTrainingDetail> PayrollEmpTrainingDetail { get; set; }
        public virtual DbSet<PayrollEmpTrainingOrganizer> PayrollEmpTrainingOrganizer { get; set; }
        public virtual DbSet<PayrollEmpTrainingType> PayrollEmpTrainingType { get; set; }
        public virtual DbSet<PayrollEmpTransfer> PayrollEmpTransfer { get; set; }
        public virtual DbSet<PayrollEmpType> PayrollEmpType { get; set; }
        public virtual DbSet<PayrollEmpWorkStation> PayrollEmpWorkStation { get; set; }
        public virtual DbSet<PayrollEmpYearlyLeave> PayrollEmpYearlyLeave { get; set; }
        public virtual DbSet<PayrollGratutitySettings> PayrollGratutitySettings { get; set; }
        public virtual DbSet<PayrollHoliday> PayrollHoliday { get; set; }
        public virtual DbSet<PayrollInterviewType> PayrollInterviewType { get; set; }
        public virtual DbSet<PayrollJobCircular> PayrollJobCircular { get; set; }
        public virtual DbSet<PayrollJobCircularApplicantMapping> PayrollJobCircularApplicantMapping { get; set; }
        public virtual DbSet<PayrollLeaveBalanceClosing> PayrollLeaveBalanceClosing { get; set; }
        public virtual DbSet<PayrollLeaveType> PayrollLeaveType { get; set; }
        public virtual DbSet<PayrollLoanCollection> PayrollLoanCollection { get; set; }
        public virtual DbSet<PayrollLoanHoldup> PayrollLoanHoldup { get; set; }
        public virtual DbSet<PayrollLoanSetting> PayrollLoanSetting { get; set; }
        public virtual DbSet<PayrollPFSetting> PayrollPFSetting { get; set; }
        public virtual DbSet<PayrollRosterHead> PayrollRosterHead { get; set; }
        public virtual DbSet<PayrollSalaryFormula> PayrollSalaryFormula { get; set; }
        public virtual DbSet<PayrollSalaryHead> PayrollSalaryHead { get; set; }
        public virtual DbSet<PayrollServiceChargeConfiguration> PayrollServiceChargeConfiguration { get; set; }
        public virtual DbSet<PayrollServiceChargeConfigurationDetails> PayrollServiceChargeConfigurationDetails { get; set; }
        public virtual DbSet<PayrollServiceChargeDistribution> PayrollServiceChargeDistribution { get; set; }
        public virtual DbSet<PayrollServiceChargeDistributionDetails> PayrollServiceChargeDistributionDetails { get; set; }
        public virtual DbSet<PayrollStaffingBudget> PayrollStaffingBudget { get; set; }
        public virtual DbSet<PayrollStaffingBudgetDetails> PayrollStaffingBudgetDetails { get; set; }
        public virtual DbSet<PayrollStaffRequisition> PayrollStaffRequisition { get; set; }
        public virtual DbSet<PayrollStaffRequisitionDetails> PayrollStaffRequisitionDetails { get; set; }
        public virtual DbSet<PayrollTaxSetting> PayrollTaxSetting { get; set; }
        public virtual DbSet<PayrollTimeSlabHead> PayrollTimeSlabHead { get; set; }
        public virtual DbSet<PayrollWorkingDay> PayrollWorkingDay { get; set; }
        public virtual DbSet<PMFinishedProduct> PMFinishedProduct { get; set; }
        public virtual DbSet<PMFinishedProductDetails> PMFinishedProductDetails { get; set; }
        public virtual DbSet<PMMemberPaymentLedger> PMMemberPaymentLedger { get; set; }
        public virtual DbSet<PMMemberPaymentLedgerClosingDetails> PMMemberPaymentLedgerClosingDetails { get; set; }
        public virtual DbSet<PMMemberPaymentLedgerClosingMaster> PMMemberPaymentLedgerClosingMaster { get; set; }
        public virtual DbSet<PMProductOut> PMProductOut { get; set; }
        public virtual DbSet<PMProductOutDetails> PMProductOutDetails { get; set; }
        public virtual DbSet<PMProductReceived> PMProductReceived { get; set; }
        public virtual DbSet<PMProductReceivedBillPayment> PMProductReceivedBillPayment { get; set; }
        public virtual DbSet<PMProductReceivedDetails> PMProductReceivedDetails { get; set; }
        public virtual DbSet<PMProductReturn> PMProductReturn { get; set; }
        public virtual DbSet<PMProductSerialInfo> PMProductSerialInfo { get; set; }
        public virtual DbSet<PMPurchaseOrder> PMPurchaseOrder { get; set; }
        public virtual DbSet<PMPurchaseOrderDetails> PMPurchaseOrderDetails { get; set; }
        public virtual DbSet<PMRequisition> PMRequisition { get; set; }
        public virtual DbSet<PMRequisitionDetails> PMRequisitionDetails { get; set; }
        public virtual DbSet<PMSales> PMSales { get; set; }
        public virtual DbSet<PMSalesBillingInfo> PMSalesBillingInfo { get; set; }
        public virtual DbSet<PMSalesBillPayment> PMSalesBillPayment { get; set; }
        public virtual DbSet<PMSalesDetail> PMSalesDetail { get; set; }
        public virtual DbSet<PMSalesInvoice> PMSalesInvoice { get; set; }
        public virtual DbSet<PMSalesSiteInfo> PMSalesSiteInfo { get; set; }
        public virtual DbSet<PMSalesTechnicalInfo> PMSalesTechnicalInfo { get; set; }
        public virtual DbSet<PMSupplier> PMSupplier { get; set; }
        public virtual DbSet<PMSupplierPayment> PMSupplierPayment { get; set; }
        public virtual DbSet<PMSupplierPaymentDetails> PMSupplierPaymentDetails { get; set; }
        public virtual DbSet<PMSupplierPaymentLedger> PMSupplierPaymentLedger { get; set; }
        public virtual DbSet<PMSupplierPaymentLedgerClosingDetails> PMSupplierPaymentLedgerClosingDetails { get; set; }
        public virtual DbSet<PMSupplierPaymentLedgerClosingMaster> PMSupplierPaymentLedgerClosingMaster { get; set; }
        public virtual DbSet<PMSupplierProductReturn> PMSupplierProductReturn { get; set; }
        public virtual DbSet<PMSupplierProductReturnDetails> PMSupplierProductReturnDetails { get; set; }
        public virtual DbSet<PPumpMachineTest> PPumpMachineTest { get; set; }
        public virtual DbSet<PRPOUserPermission> PRPOUserPermission { get; set; }
        public virtual DbSet<RestaurantBearer> RestaurantBearer { get; set; }
        public virtual DbSet<RestaurantBill> RestaurantBill { get; set; }
        public virtual DbSet<RestaurantBillClassificationDiscount> RestaurantBillClassificationDiscount { get; set; }
        public virtual DbSet<RestaurantBillDetail> RestaurantBillDetail { get; set; }
        public virtual DbSet<RestaurantBillPayment> RestaurantBillPayment { get; set; }
        public virtual DbSet<RestaurantBuffet> RestaurantBuffet { get; set; }
        public virtual DbSet<RestaurantBuffetCostCenterMapping> RestaurantBuffetCostCenterMapping { get; set; }
        public virtual DbSet<RestaurantBuffetDetail> RestaurantBuffetDetail { get; set; }
        public virtual DbSet<RestaurantCombo> RestaurantCombo { get; set; }
        public virtual DbSet<RestaurantComboCostCenterMapping> RestaurantComboCostCenterMapping { get; set; }
        public virtual DbSet<RestaurantComboDetail> RestaurantComboDetail { get; set; }
        public virtual DbSet<RestaurantCostCenterTableMapping> RestaurantCostCenterTableMapping { get; set; }
        public virtual DbSet<RestaurantDailySalesStatementConfiguration> RestaurantDailySalesStatementConfiguration { get; set; }
        public virtual DbSet<RestaurantEmpKotBillDetail> RestaurantEmpKotBillDetail { get; set; }
        public virtual DbSet<RestaurantKitchen> RestaurantKitchen { get; set; }
        public virtual DbSet<RestaurantKitchenCostCenterMapping> RestaurantKitchenCostCenterMapping { get; set; }
        public virtual DbSet<RestaurantKotBillDetail> RestaurantKotBillDetail { get; set; }
        public virtual DbSet<RestaurantKotBillMaster> RestaurantKotBillMaster { get; set; }
        public virtual DbSet<RestaurantKotPendingList> RestaurantKotPendingList { get; set; }
        public virtual DbSet<RestaurantKotSpecialRemarksDetail> RestaurantKotSpecialRemarksDetail { get; set; }
        public virtual DbSet<RestaurantRecipeDetail> RestaurantRecipeDetail { get; set; }
        public virtual DbSet<RestaurantReservation> RestaurantReservation { get; set; }
        public virtual DbSet<RestaurantReservationItemDetail> RestaurantReservationItemDetail { get; set; }
        public virtual DbSet<RestaurantReservationTableDetail> RestaurantReservationTableDetail { get; set; }
        public virtual DbSet<RestaurantTable> RestaurantTable { get; set; }
        public virtual DbSet<RestaurantTableManagement> RestaurantTableManagement { get; set; }
        public virtual DbSet<RestaurantTableReservationDetail> RestaurantTableReservationDetail { get; set; }
        public virtual DbSet<RestaurantTableStatus> RestaurantTableStatus { get; set; }
        public virtual DbSet<RestaurantToken> RestaurantToken { get; set; }
        public virtual DbSet<SalesBandwidthInfo> SalesBandwidthInfo { get; set; }
        public virtual DbSet<SalesContractDetails> SalesContractDetails { get; set; }
        public virtual DbSet<SalesCustomer> SalesCustomer { get; set; }
        public virtual DbSet<SalesServiceBundle> SalesServiceBundle { get; set; }
        public virtual DbSet<SalesServiceBundleDetails> SalesServiceBundleDetails { get; set; }
        public virtual DbSet<SecurityActivityLogs> SecurityActivityLogs { get; set; }
        public virtual DbSet<SecurityLogError> SecurityLogError { get; set; }
        public virtual DbSet<SecurityMenuGroup> SecurityMenuGroup { get; set; }
        public virtual DbSet<SecurityMenuLinks> SecurityMenuLinks { get; set; }
        public virtual DbSet<SecurityMenuWiseLinks> SecurityMenuWiseLinks { get; set; }
        public virtual DbSet<SecurityObjectPermission> SecurityObjectPermission { get; set; }
        public virtual DbSet<SecurityObjectTab> SecurityObjectTab { get; set; }
        public virtual DbSet<SecurityUserCostCenterMapping> SecurityUserCostCenterMapping { get; set; }
        public virtual DbSet<SecurityUserGroup> SecurityUserGroup { get; set; }
        public virtual DbSet<SecurityUserGroupCostCenterMapping> SecurityUserGroupCostCenterMapping { get; set; }
        public virtual DbSet<SecurityUserInformation> SecurityUserInformation { get; set; }
        public virtual DbSet<SMAccountManager> SMAccountManager { get; set; }
        public virtual DbSet<SMBillingPeriod> SMBillingPeriod { get; set; }
        public virtual DbSet<SMCompanySalesCall> SMCompanySalesCall { get; set; }
        public virtual DbSet<SMCompanySalesCallDetail> SMCompanySalesCallDetail { get; set; }
        public virtual DbSet<SMCompanySite> SMCompanySite { get; set; }
        public virtual DbSet<SMContractPeriod> SMContractPeriod { get; set; }
        public virtual DbSet<SMCurrentVendor> SMCurrentVendor { get; set; }
        public virtual DbSet<SMIndustry> SMIndustry { get; set; }
        public virtual DbSet<SMItemOrServiceDelivery> SMItemOrServiceDelivery { get; set; }
        public virtual DbSet<SMQuotation> SMQuotation { get; set; }
        public virtual DbSet<SMQuotationDetails> SMQuotationDetails { get; set; }
        public virtual DbSet<SMSalesOrder> SMSalesOrder { get; set; }
        public virtual DbSet<SMSalesOrderDetails> SMSalesOrderDetails { get; set; }
        public virtual DbSet<SMServiceType> SMServiceType { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserDashboardItemMapping> UserDashboardItemMapping { get; set; }
        public virtual DbSet<PayrollEmpAttendanceLogSuprima> PayrollEmpAttendanceLogSuprima { get; set; }
        public virtual DbSet<PayrollEmpBankInfo> PayrollEmpBankInfo { get; set; }
        public virtual DbSet<PayrollEmpNomineeInfo> PayrollEmpNomineeInfo { get; set; }
        public virtual DbSet<PMProductOutSerialInfo> PMProductOutSerialInfo { get; set; }
        public virtual DbSet<SalesService> SalesService { get; set; }
        public virtual DbSet<SecurityLogFile> SecurityLogFile { get; set; }
        public virtual DbSet<viewChkFromLdg> viewChkFromLdg { get; set; }
        public virtual DbSet<viewComEmployeeInfo> viewComEmployeeInfo { get; set; }
        public virtual DbSet<viewCommonVoucher> viewCommonVoucher { get; set; }
        public virtual DbSet<ViewLedgerDetailAmountSum> ViewLedgerDetailAmountSum { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BanquetBillPayment>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<BanquetInformation>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetInformation>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetInformation>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetInformation)
                .HasForeignKey(e => e.BanquetId);

            modelBuilder.Entity<BanquetOccessionType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetOccessionType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetOccessionType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetOccessionType>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetOccessionType)
                .HasForeignKey(e => e.OccessionTypeId);

            modelBuilder.Entity<BanquetRefference>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetRefference>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetRefference>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetRefference)
                .HasForeignKey(e => e.RefferenceId);

            modelBuilder.Entity<BanquetRequisites>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetRequisites>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetRequisites>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ReservationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ReservationMode)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.BookingFor)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.InvoiceServiceCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.AdditionalChargeType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.CancellationReason)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.SpecialInstructions)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.RebateRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.BillStatus)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetBillPayment)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetReservationBillPayment)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetReservationClassificationDiscount)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservation>()
                .HasMany(e => e.BanquetReservationDetail)
                .WithOptional(e => e.BanquetReservation)
                .HasForeignKey(e => e.ReservationId);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationDetail>()
                .Property(e => e.ItemType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationDetail>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetReservationDetail>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetSeatingPlan>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetSeatingPlan>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetSeatingPlan>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetSeatingPlan>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<BanquetSeatingPlan>()
                .HasMany(e => e.BanquetReservation)
                .WithOptional(e => e.BanquetSeatingPlan)
                .HasForeignKey(e => e.SeatingId);

            modelBuilder.Entity<CgsMonthlyTransaction>()
                .Property(e => e.CreatedDate)
                .IsUnicode(false);

            modelBuilder.Entity<CgsMonthlyTransaction>()
                .Property(e => e.LastModifiedDate)
                .IsUnicode(false);

            modelBuilder.Entity<CgsTransactionHead>()
                .Property(e => e.CreatedDate)
                .IsUnicode(false);

            modelBuilder.Entity<CgsTransactionHead>()
                .Property(e => e.LastModifiedDate)
                .IsUnicode(false);

            modelBuilder.Entity<CommonBank>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonBusinessPromotion>()
                .Property(e => e.BPHead)
                .IsUnicode(false);

            modelBuilder.Entity<CommonBusinessPromotion>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonBusinessPromotion>()
                .Property(e => e.PercentAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CommonBusinessPromotionDetail>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCity>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.SwiftCode)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.AccountNo1)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyBank>()
                .Property(e => e.AccountNo2)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.CompanyCode)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.CompanyAddress)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.WebAddress)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.VatRegistrationNo)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCompanyProfile>()
                .Property(e => e.CompanyType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.CostCenter)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.CostCenterLogo)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.BillNumberPrefix)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.AdditionalChargeType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.CostCenterType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCostCenter>()
                .Property(e => e.DefaultView)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCountries>()
                .Property(e => e.Code2Digit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CommonCountries>()
                .Property(e => e.Code3Digit)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CommonCountries>()
                .Property(e => e.CodeNumeric)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CommonCountries>()
                .Property(e => e.SBCode)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCurrency>()
                .Property(e => e.CurrencyName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCurrency>()
                .Property(e => e.CurrencyType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCurrencyTransaction>()
                .Property(e => e.TransactionNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCustomFieldData>()
                .Property(e => e.FieldType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonCustomFieldData>()
                .Property(e => e.FieldValue)
                .IsUnicode(false);

            modelBuilder.Entity<CommonDocuments>()
                .Property(e => e.DocumentCategory)
                .IsUnicode(false);

            modelBuilder.Entity<CommonDocuments>()
                .Property(e => e.DocumentType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonDocuments>()
                .Property(e => e.Extention)
                .IsUnicode(false);

            modelBuilder.Entity<CommonDocuments>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CommonDocuments>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<CommonIndustry>()
                .Property(e => e.IndustryName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonLocation>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonMessage>()
                .Property(e => e.Importance)
                .IsUnicode(false);

            modelBuilder.Entity<CommonMessage>()
                .Property(e => e.Subjects)
                .IsUnicode(false);

            modelBuilder.Entity<CommonMessage>()
                .Property(e => e.MessageBody)
                .IsUnicode(false);

            modelBuilder.Entity<CommonMessageDetails>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<CommonModuleName>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonModuleName>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonModuleName>()
                .Property(e => e.ModulePath)
                .IsUnicode(false);

            modelBuilder.Entity<CommonPaymentMode>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<CommonPaymentMode>()
                .Property(e => e.HierarchyIndex)
                .IsUnicode(false);

            modelBuilder.Entity<CommonPrinterInfo>()
                .Property(e => e.StockType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonPrinterInfo>()
                .Property(e => e.PrinterName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonProfession>()
                .Property(e => e.ProfessionName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonProfession>()
                .Property(e => e.ProfessionCode)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReportConfigMaster>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<CommonReportConfigMaster>()
                .Property(e => e.NodeType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.SetupName)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.SetupValue)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<DashboardManagement>()
                .Property(e => e.DivName)
                .IsUnicode(false);

            modelBuilder.Entity<GatePass>()
                .Property(e => e.GatePassNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GatePass>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<GatePassDetails>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<GatePassDetails>()
                .Property(e => e.ApprovedQuantity)
                .HasPrecision(18, 0);

            modelBuilder.Entity<GLAccountConfiguration>()
                .Property(e => e.AccountType)
                .IsUnicode(false);

            modelBuilder.Entity<GLAccountsMapping>()
                .Property(e => e.MappingKey)
                .IsUnicode(false);

            modelBuilder.Entity<GLAccountsMapping>()
                .Property(e => e.MappingValue)
                .IsUnicode(false);

            modelBuilder.Entity<GLAccountTypeSetup>()
                .Property(e => e.AccountType)
                .IsUnicode(false);

            modelBuilder.Entity<GLBudgetDetails>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLCashFlowGroupHead>()
                .Property(e => e.GroupHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLCashFlowHead>()
                .Property(e => e.CashFlowHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLCashFlowHead>()
                .Property(e => e.NotesNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLCommonSetup>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<GLCommonSetup>()
                .Property(e => e.SetupName)
                .IsUnicode(false);

            modelBuilder.Entity<GLCommonSetup>()
                .Property(e => e.SetupValue)
                .IsUnicode(false);

            modelBuilder.Entity<GLCompany>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<GLCompany>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<GLCompany>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<GLCompany>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<GLDealMaster>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<GLDealMaster>()
                .Property(e => e.Narration)
                .IsUnicode(false);

            modelBuilder.Entity<GLDealMaster>()
                .Property(e => e.PayerOrPayee)
                .IsUnicode(false);

            modelBuilder.Entity<GLDealMaster>()
                .Property(e => e.GLStatus)
                .IsUnicode(false);

            modelBuilder.Entity<GLDonor>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<GLDonor>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<GLDonor>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<GLDonor>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<GLFiscalYear>()
                .Property(e => e.FiscalYearName)
                .IsUnicode(false);

            modelBuilder.Entity<GLFiscalYear>()
                .Property(e => e.IncomeTaxPercentage)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLFiscalYearClosingDetails>()
                .Property(e => e.ClosingDRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLFiscalYearClosingDetails>()
                .Property(e => e.ClosingCRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLFiscalYearClosingDetails>()
                .Property(e => e.ClosingBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLFiscalYearClosingMaster>()
                .Property(e => e.ProfitLossClosing)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.CostCentreHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.NodeHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.HierarchyIndex)
                .IsUnicode(false);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.PriorBalance)
                .HasPrecision(15, 2);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.ReceivedAmount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.PaidAmount)
                .HasPrecision(15, 2);

            modelBuilder.Entity<GLGeneralLedger>()
                .Property(e => e.NodeNarration)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedger>()
                .Property(e => e.ChequeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedger>()
                .Property(e => e.NodeNarration)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerDetails>()
                .Property(e => e.ChequeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerDetails>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLLedgerDetails>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLLedgerDetails>()
                .Property(e => e.NodeNarration)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerDetails>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.Narration)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.PayerOrPayee)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.GLStatus)
                .IsUnicode(false);

            modelBuilder.Entity<GLLedgerMaster>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .Property(e => e.NodeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .Property(e => e.NodeHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .Property(e => e.HierarchyIndex)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .Property(e => e.NotesNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLNodeMatrix>()
                .HasMany(e => e.BanquetInformation)
                .WithOptional(e => e.GLNodeMatrix)
                .HasForeignKey(e => e.AccountsPostingHeadId);

            modelBuilder.Entity<GLNodeMatrix>()
                .HasMany(e => e.BanquetRequisites)
                .WithOptional(e => e.GLNodeMatrix)
                .HasForeignKey(e => e.AccountsPostingHeadId);

            modelBuilder.Entity<GLNodeMatrix>()
                .HasMany(e => e.BanquetReservationBillPayment)
                .WithOptional(e => e.GLNodeMatrix)
                .HasForeignKey(e => e.AccountsPostingHeadId);

            modelBuilder.Entity<GLNodeMatrix>()
                .HasMany(e => e.HotelRoomType)
                .WithOptional(e => e.GLNodeMatrix)
                .HasForeignKey(e => e.AccountsPostingHeadId);

            modelBuilder.Entity<GLNotesConfiguration>()
                .Property(e => e.ConfigurationType)
                .IsUnicode(false);

            modelBuilder.Entity<GLNotesConfiguration>()
                .Property(e => e.NotesNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLProfitLossGroupHead>()
                .Property(e => e.GroupHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLProfitLossHead>()
                .Property(e => e.PLHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLProfitLossHead>()
                .Property(e => e.NotesNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLProfitLossHead>()
                .Property(e => e.CalculationMode)
                .IsUnicode(false);

            modelBuilder.Entity<GLProfitLossHead>()
                .Property(e => e.DisplayMode)
                .IsUnicode(false);

            modelBuilder.Entity<GLProject>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<GLProject>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<GLProject>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<GLProject>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.NodeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.NodeHead)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.ReportType)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.AccountType)
                .IsUnicode(false);

            modelBuilder.Entity<GLReportConfiguration>()
                .Property(e => e.CalculationType)
                .IsUnicode(false);

            modelBuilder.Entity<GLVoucherApprovedInfo>()
                .Property(e => e.ApprovedType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelAirlineInformation>()
                .Property(e => e.AirlineName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelAirlineInformation>()
                .Property(e => e.FlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyBillGenerationDetails>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyBillGenerationDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyBillGenerationDetails>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPayment>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPayment>()
                .Property(e => e.AdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPayment>()
                .Property(e => e.PaymentAdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.LedgerNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.PaidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.PaidAmountCurrent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.AdvanceAmountRemaining)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.DayConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.GainOrLossAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.RoundedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyPaymentLedger>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelCompanyPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingDRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingCRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelCompanyPaymentLedgerClosingMaster>()
                .Property(e => e.ProfitLossClosing)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HotelComplementaryItem>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelComplementaryItem>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<HotelDayProcessing>()
                .Property(e => e.ProcessType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelEmpTaskAssignment>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelFloor>()
                .Property(e => e.FloorName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelFloor>()
                .Property(e => e.FloorDescription)
                .IsUnicode(false);

            modelBuilder.Entity<HotelFloorBlock>()
                .Property(e => e.BlockName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelFloorBlock>()
                .Property(e => e.BlockDescription)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillApproved>()
                .Property(e => e.RoomType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillApproved>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillApproved>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillApproved>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.PaymentDescription)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestBillPayment>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.CompanyAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.WebAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.ContactDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.TelephoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.DiscountPercent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HotelGuestCompany>()
                .Property(e => e.SignupStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestDayLetCheckOut>()
                .Property(e => e.DayLetDiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestDayLetCheckOut>()
                .Property(e => e.DayLetStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestDocuments>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestDocuments>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestExtraServiceBillApproved>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestExtraServiceBillApproved>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestExtraServiceBillApproved>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestExtraServiceBillApproved>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestExtraServiceBillApproved>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.PayMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestHouseCheckOut>()
                .Property(e => e.RebateRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestSex)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestPhone)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestCity)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestNationality)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestDrivinlgLicense)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestAuthentication)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.PassportNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.PIssuePlace)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.VisaNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformation>()
                .Property(e => e.GuestPreferences)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestSex)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestPhone)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestCity)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestNationality)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestDrivinlgLicense)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestAuthentication)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.PassportNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.PIssuePlace)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.VisaNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestInformationOnline>()
                .Property(e => e.GuestPreferences)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.Organization)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.TelephoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.CellNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestReference>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestRoomShiftInfo>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.GuestName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.ServiceRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBill>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceBillApproved>()
                .Property(e => e.PaidServiceAchievementStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceInfo>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceInfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<HotelGuestServiceInfo>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelLinkedRoomMaster>()
                .HasMany(e => e.HotelLinkedRoomDetails)
                .WithRequired(e => e.HotelLinkedRoomMaster)
                .HasForeignKey(e => e.MasterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HotelMonthToDateInfo>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ReservationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ReservedCompany)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ContactAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ReservedMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ReservationType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.ReservationMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelOnlineRoomReservationDetail>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.ServiceDate)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.PaymentDescription)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.POSTerminalBank)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.OperatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HotelPaymentSummary>()
                .Property(e => e.ReportType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRegistrationAireportPickupDrop>()
                .Property(e => e.ArrivalFlightName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRegistrationAireportPickupDrop>()
                .Property(e => e.ArrivalFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRegistrationAireportPickupDrop>()
                .Property(e => e.DepartureFlightName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRegistrationAireportPickupDrop>()
                .Property(e => e.DepartureFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationAireportPickupDrop>()
                .Property(e => e.PickupDropType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationAireportPickupDrop>()
                .Property(e => e.ArrivalFlightName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationAireportPickupDrop>()
                .Property(e => e.ArrivalFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationAireportPickupDrop>()
                .Property(e => e.DepartureFlightName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationAireportPickupDrop>()
                .Property(e => e.DepartureFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelReservationBillPayment>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomInventory>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomLogFile>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomNumber>()
                .Property(e => e.RoomNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomNumber>()
                .Property(e => e.RoomName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomNumber>()
                .Property(e => e.CleanupStatus)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomNumber>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomOwner>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.RegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.CommingFrom)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.NextDestination)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.VisitPurpose)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.ReservedCompany)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.AirportPickUp)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.AirportDrop)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistration>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestDOB)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestSex)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestPhone)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestCity)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestNationality)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestDrivinlgLicense)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.GuestAuthentication)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.PassportNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.PIssueDate)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.PIssuePlace)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.PExpireDate)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.VisaNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.VIssueDate)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomRegistrationDetail>()
                .Property(e => e.VExpireDate)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ReservationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ReservedCompany)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ContactAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ReservedMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ReservationType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.ReservationMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.AirportPickUp)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.AirportDrop)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.RoomInfo)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservation>()
                .Property(e => e.GuestRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationDetail>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationDetailOnline>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ReservationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ReservedCompany)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ContactAddress)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ReservedMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ReservationType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.ReservationMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.AirportPickUp)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomReservationOnline>()
                .Property(e => e.AirportDrop)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomStatus>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomStatus>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomType>()
                .Property(e => e.RoomType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelRoomType>()
                .Property(e => e.TypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<HotelSegmentHead>()
                .Property(e => e.SegmentName)
                .IsUnicode(false);

            modelBuilder.Entity<HotelSegmentHead>()
                .Property(e => e.SegmentType)
                .IsUnicode(false);

            modelBuilder.Entity<HotelStock>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.HierarchyIndex)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<InvCategory>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<InvCogsClosing>()
                .Property(e => e.CogsAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.DineTimeFrom)
                .HasPrecision(3);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.DineTimeTo)
                .HasPrecision(3);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalSalesQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.Netsales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.ServiceCharge)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.VatAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.GrandTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalRevenue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalVoidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.ErrorCorrectsAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.ChecksAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.ChecksPaidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.OutstandingAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalCashPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalCardPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvCostCenterNDineTimeWiseItemTransaction>()
                .Property(e => e.TotalRefund)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalSalesQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.Netsales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.ServiceCharge)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.VatAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.GrandTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalRevenue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalVoidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.ErrorCorrectsAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.ChecksAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.ChecksPaidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.OutstandingAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalCashPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalCardPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransaction>()
                .Property(e => e.TotalRefund)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.TotalSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.ServiceCharge)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.VatAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.GrnadTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.NetSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWiseItemTransactionDetails>()
                .Property(e => e.TTLNetSales)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvDineTimeWisePaymentDetails>()
                .Property(e => e.DineTimeFrom)
                .HasPrecision(3);

            modelBuilder.Entity<InvDineTimeWisePaymentDetails>()
                .Property(e => e.DineTimeTo)
                .HasPrecision(3);

            modelBuilder.Entity<InvDineTimeWisePaymentDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.ItemType)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.StockType)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.ProductType)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.AverageCost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItem>()
                .Property(e => e.AdjustmentFrequency)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemClassification>()
                .HasMany(e => e.InvItemClassificationCostCenterMapping)
                .WithOptional(e => e.InvItemClassification)
                .HasForeignKey(e => e.ClassificationId);

            modelBuilder.Entity<InvItemCostCenterMapping>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemSpecialRemarks>()
                .Property(e => e.SpecialRemarks)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemSpecialRemarks>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemStockAdjustment>()
                .Property(e => e.AdjustmentFrequency)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemStockAdjustmentDetails>()
                .Property(e => e.AverageCost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemStockInformation>()
                .Property(e => e.StockQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemStockSerialInformation>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalSalesAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalDiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalNetSalesAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalServiceChargeAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalVatAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.GrossSalesAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalVoidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalErrorAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalVariance)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalVarianceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalStockCountDeifference)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalStockCountDeifferenceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalReceivedQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalReceivedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalUsageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalUsageCost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalCashPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalCardPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalRefundAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalRevenue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalPerGuestUsageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransaction>()
                .Property(e => e.TotalPerGuestUsageAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.BeginingStockQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PurchaseQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.UsageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.WastageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.WastageCost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.AdjustmentStockQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.DayEndStockQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.StockCountDifference)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.StockCountDifferenceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PriceToday)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PriceYestarday)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PriceFluctuation)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.Vat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.ServiceCharge)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PerGuestUsageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionDetails>()
                .Property(e => e.PerGuestUsageAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvItemTransactionLog>()
                .Property(e => e.TransactionalOpeningQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLog>()
                .Property(e => e.TransactionQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLog>()
                .Property(e => e.ClosingQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.TransactionalOpeningQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.ReceiveQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.OutItemQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.WastageQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.AdjustmentQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.SalesQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionLogSummary>()
                .Property(e => e.ClosingQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvItemTransactionPaymentDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvLocation>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<InvLocation>()
                .Property(e => e.HierarchyIndex)
                .IsUnicode(false);

            modelBuilder.Entity<InvLocation>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<InvLocation>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<InvLocation>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<InvManufacturer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<InvManufacturer>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<InvManufacturer>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.ItemUnit)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.ConvertionUnit)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.TotalUnitWillDeduct)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.UnitDeduction)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.RecipeDeduction)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.UnitDiffernce)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.QuantityMain)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.ParentQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.StockQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.AverageCost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.DeductionQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvRecipeDeductionDetails>()
                .Property(e => e.TotalCost)
                .HasPrecision(18, 5);

            modelBuilder.Entity<InvServiceBandwidth>()
                .Property(e => e.BandWidthName)
                .IsUnicode(false);

            modelBuilder.Entity<InvServicePackage>()
                .Property(e => e.PackageName)
                .IsUnicode(false);

            modelBuilder.Entity<InvServicePriceMatrix>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<InvTransactionMode>()
                .Property(e => e.HeadName)
                .IsUnicode(false);

            modelBuilder.Entity<InvTransactionMode>()
                .Property(e => e.CalculationType)
                .IsUnicode(false);

            modelBuilder.Entity<LCInformation>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<LCInformation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<LCInformationDetail>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LCInformationDetail>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LCOverHeadExpense>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LCOverHeadName>()
                .Property(e => e.OverHeadName)
                .IsUnicode(false);

            modelBuilder.Entity<LCOverHeadName>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<LCPaymentLedger>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MembershipNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.NameTitle)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.FatherName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MotherName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MemberAddress)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.ResidencePhone)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.OfficePhone)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.PersonalEmail)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.OfficeEmail)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.HomeFax)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.OfficeFax)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.MailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.PassportNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.Organization)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.Occupation)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberBasics>()
                .Property(e => e.Balance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<MemMemberFamilyMember>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberFamilyMember>()
                .Property(e => e.Occupation)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberFamilyMember>()
                .Property(e => e.Relationship)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberFamilyMember>()
                .Property(e => e.UsageMode)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberReference>()
                .Property(e => e.Arbitrator)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberReference>()
                .Property(e => e.ArbitratorMode)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberReference>()
                .Property(e => e.Relationship)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MemMemberType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAllowanceDeductionHead>()
                .Property(e => e.AllowDeductName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAllowanceDeductionHead>()
                .Property(e => e.AllowDeductType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAllowanceDeductionHead>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalEvalutionBy>()
                .Property(e => e.AppraisalType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalEvalutionRatingFactorDetails>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalMarksIndicator>()
                .Property(e => e.AppraisalIndicatorName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalMarksIndicator>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalRatingFactor>()
                .Property(e => e.RatingFactorName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalRatingFactor>()
                .Property(e => e.RatingWeight)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollAppraisalRatingFactor>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalRatingScale>()
                .Property(e => e.RatingScaleName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAppraisalRatingScale>()
                .Property(e => e.RatingValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollAppraisalRatingScale>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAttendanceDevice>()
                .Property(e => e.ReaderType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAttendanceDevice>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollAttendanceDevice>()
                .Property(e => e.MacAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollBonusSetting>()
                .Property(e => e.BonusType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollBonusSetting>()
                .Property(e => e.AmountType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDepartment>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDepartment>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDesignation>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDesignation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDonor>()
                .Property(e => e.DonorCode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollDonor>()
                .Property(e => e.DonorName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEducationLevel>()
                .Property(e => e.LevelName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAdvanceTaken>()
                .Property(e => e.PayMonth)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAdvanceTaken>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAdvanceTaken>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAllowanceDeduction>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAppraisalEvalution>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpAttendance>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpCareerInfo>()
                .Property(e => e.PresentSalary)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpCareerInfo>()
                .Property(e => e.ExpectedSalary)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpCareerTraining>()
                .Property(e => e.DurationType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpDependent>()
                .Property(e => e.DependentName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpDependent>()
                .Property(e => e.Relationship)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpDependent>()
                .Property(e => e.Age)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpEducation>()
                .Property(e => e.ExamName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpEducation>()
                .Property(e => e.InstituteName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpEducation>()
                .Property(e => e.PassYear)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpEducation>()
                .Property(e => e.SubjectName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpEducation>()
                .Property(e => e.PassClass)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpExperience>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpExperience>()
                .Property(e => e.CompanyUrl)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpExperience>()
                .Property(e => e.JoinDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpExperience>()
                .Property(e => e.LeaveDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpExperience>()
                .Property(e => e.Achievements)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpGrade>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpGrade>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpGrade>()
                .Property(e => e.BasicAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmpIncrement>()
                .Property(e => e.IncrementMode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpIncrement>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLastMonthBenifitsPayment>()
                .Property(e => e.LeaveBalanceDays)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpLeaveInformation>()
                .Property(e => e.LeaveMode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLeaveInformation>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLeaveInformation>()
                .Property(e => e.LeaveStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLeaveInformation>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLoan>()
                .Property(e => e.LoanNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLoan>()
                .Property(e => e.LoanType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLoan>()
                .Property(e => e.LoanTakenForMonthOrYear)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLoan>()
                .Property(e => e.LoanStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpLoan>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmpPassword)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.OfficialEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.ReferenceBy)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.FathersName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.MothersName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.BloodGroup)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Religion)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Height)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.MaritalStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.NationalId)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PassportNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PIssuePlace)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PresentAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PresentCity)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PresentZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PresentCountry)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PresentPhone)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PermanentAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PermanentCity)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PermanentZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PermanentCountry)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PermanentPhone)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.PersonalEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmergencyContactName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmergencyContactRelationship)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmergencyContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.EmergencyContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.ActivityCode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Nationality)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.AlternativeEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployee>()
                .Property(e => e.Balance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeeBillGenerationDetails>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeeBillGenerationDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeeBillGenerationDetails>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePayment>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePayment>()
                .Property(e => e.AdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePayment>()
                .Property(e => e.PaymentAdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.ModuleName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.LedgerNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.PaidAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.PaidAmountCurrent)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.AdvanceAmountRemaining)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.DayConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.GainOrLossAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.RoundedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployeePaymentLedger>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmployeePaymentLedgerClosingDetails>()
                .Property(e => e.ClosingDRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedgerClosingDetails>()
                .Property(e => e.ClosingCRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedgerClosingDetails>()
                .Property(e => e.ClosingBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeePaymentLedgerClosingMaster>()
                .Property(e => e.ProfitLossClosing)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmployeeStatus>()
                .Property(e => e.EmployeeStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpPF>()
                .Property(e => e.PFType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpPromotion>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.SalaryHeadNote)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.SalaryType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.SalaryEffectiveness)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetail>()
                .Property(e => e.AmountType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.SalaryHeadNote)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.SalaryType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.SalaryEffectiveness)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpSalaryProcessDetailTemp>()
                .Property(e => e.AmountType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTax>()
                .Property(e => e.EmpTaxContribution)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpTax>()
                .Property(e => e.CompanyTaxContribution)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpTax>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTaxDeductionSetting>()
                .Property(e => e.RangeFrom)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpTaxDeductionSetting>()
                .Property(e => e.RangeTo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpTaxDeductionSetting>()
                .Property(e => e.DeductionPercentage)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollEmpTaxDeductionSetting>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTimeSlab>()
                .Property(e => e.WeekEndMode)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTimeSlab>()
                .Property(e => e.WeekEndFirst)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTimeSlab>()
                .Property(e => e.WeekEndSecond)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTimeSlabRoster>()
                .Property(e => e.DayName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTrainingDetail>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTrainingOrganizer>()
                .Property(e => e.TrainingType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTrainingOrganizer>()
                .Property(e => e.ContactNo)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTrainingType>()
                .Property(e => e.TrainingName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpTrainingType>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpType>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpType>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpWorkStation>()
                .Property(e => e.WorkStationName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollGratutitySettings>()
                .Property(e => e.GratutiyPercentage)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollHoliday>()
                .Property(e => e.HolidayName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollHoliday>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollInterviewType>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollJobCircular>()
                .Property(e => e.JobDescription)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollJobCircular>()
                .Property(e => e.EducationalQualification)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollJobCircular>()
                .Property(e => e.AdditionalJobRequirement)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollLeaveType>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollLoanCollection>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollLoanHoldup>()
                .Property(e => e.OverDueAmount)
                .HasPrecision(18, 18);

            modelBuilder.Entity<PayrollLoanHoldup>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollLoanSetting>()
                .Property(e => e.CompanyLoanInterestRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollLoanSetting>()
                .Property(e => e.PFLoanInterestRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollLoanSetting>()
                .Property(e => e.MinPFMustAvailableToAllowLoan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollLoanSetting>()
                .Property(e => e.MinJobLengthToAllowCompanyLoan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollPFSetting>()
                .Property(e => e.InterestDistributionRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollRosterHead>()
                .Property(e => e.RosterName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollSalaryFormula>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollSalaryFormula>()
                .Property(e => e.AmountType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollSalaryHead>()
                .Property(e => e.SalaryHead)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollSalaryHead>()
                .Property(e => e.SalaryType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollSalaryHead>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollServiceChargeDistribution>()
                .Property(e => e.DistributionPercentage)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PayrollServiceChargeDistributionDetails>()
                .Property(e => e.DistributionPercentage)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.TaxBandForMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.TaxBandForFemale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.CompanyContributionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.CompanyContributionAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.EmpContributionType)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollTaxSetting>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollWorkingDay>()
                .Property(e => e.WorkingPlan)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollWorkingDay>()
                .Property(e => e.DayOffOne)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollWorkingDay>()
                .Property(e => e.DayOffTwo)
                .IsUnicode(false);

            modelBuilder.Entity<PMFinishedProduct>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMMemberPaymentLedger>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMMemberPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingDRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingCRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMMemberPaymentLedgerClosingMaster>()
                .Property(e => e.ProfitLossClosing)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMProductOut>()
                .Property(e => e.ProductOutFor)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductOut>()
                .Property(e => e.IssueType)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductOut>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductOutDetails>()
                .Property(e => e.AdjustmentQuantity)
                .HasPrecision(18, 5);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.ReceiveNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.PurchaseBy)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.ReceiveType)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceived>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReceivedBillPayment>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReturn>()
                .Property(e => e.ReturnType)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReturn>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductReturn>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductSerialInfo>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.PONumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.POType)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.IsLocalOrForeignPO)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.ReceivedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMPurchaseOrder>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMPurchaseOrderDetails>()
                .Property(e => e.MessureUnit)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.PRNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.RequisitionBy)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.DelivaredStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.DelivarOutStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisition>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMRequisitionDetails>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMSales>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSales>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<PMSales>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillingInfo>()
                .Property(e => e.BillingContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillingInfo>()
                .Property(e => e.BillingPersonDepartment)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillingInfo>()
                .Property(e => e.BillingPersonDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillingInfo>()
                .Property(e => e.BillingPersonPhone)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillingInfo>()
                .Property(e => e.BillingPersonEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesDetail>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesInvoice>()
                .Property(e => e.InvoiceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesInvoice>()
                .Property(e => e.InvoiceDetailId)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SiteId)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SiteName)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SiteAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SiteContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SitePhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesSiteInfo>()
                .Property(e => e.SiteEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesTechnicalInfo>()
                .Property(e => e.TechnicalContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesTechnicalInfo>()
                .Property(e => e.TechnicalPersonDepartment)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesTechnicalInfo>()
                .Property(e => e.TechnicalPersonDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesTechnicalInfo>()
                .Property(e => e.TechnicalPersonPhone)
                .IsUnicode(false);

            modelBuilder.Entity<PMSalesTechnicalInfo>()
                .Property(e => e.TechnicalPersonEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.WebAddress)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplier>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPayment>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPayment>()
                .Property(e => e.AdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPayment>()
                .Property(e => e.PaymentAdjustmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentDetails>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.LedgerNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.ConvertionRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.CurrencyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.DueAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.AdvanceAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.AdvanceAmountRemaining)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.ChequeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPaymentLedger>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PMSupplierPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingDRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingCRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedgerClosingDetails>()
                .Property(e => e.ClosingBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierPaymentLedgerClosingMaster>()
                .Property(e => e.ProfitLossClosing)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PMSupplierProductReturn>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PPumpMachineTest>()
                .Property(e => e.BeforeMachineReadNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PPumpMachineTest>()
                .Property(e => e.AfterMachineReadNumber)
                .IsUnicode(false);

            modelBuilder.Entity<PPumpMachineTest>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<PPumpMachineTest>()
                .Property(e => e.CreatedDate)
                .IsUnicode(false);

            modelBuilder.Entity<PPumpMachineTest>()
                .Property(e => e.LastModifiedDate)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBearer>()
                .Property(e => e.BearerPassword)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.SourceName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.PayMode)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.AdditionalChargeType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.BillStatus)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBill>()
                .Property(e => e.UserType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillClassificationDiscount>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.PaymentType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.ChecqueNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.CardType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.CardNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.CardHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBillPayment>()
                .Property(e => e.CardReference)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBuffet>()
                .Property(e => e.BuffetName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBuffet>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantBuffet>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantCombo>()
                .Property(e => e.ComboName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantCombo>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantCombo>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantEmpKotBillDetail>()
                .Property(e => e.BillNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantEmpKotBillDetail>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantEmpKotBillDetail>()
                .Property(e => e.JobStatus)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKitchen>()
                .Property(e => e.KitchenName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.ItemType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.DiscountAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.DiscountedAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.ServiceRate)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.ServiceCharge)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.VatAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.CitySDCharge)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.AdditionalCharge)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantKotBillDetail>()
                .Property(e => e.DeliveryStatus)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillMaster>()
                .Property(e => e.SourceName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillMaster>()
                .Property(e => e.KotStatus)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantKotBillMaster>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantRecipeDetail>()
                .Property(e => e.RecipeItemName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantRecipeDetail>()
                .Property(e => e.ItemUnit)
                .HasPrecision(18, 5);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ReservationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ReservedCompany)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ContactAddress)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ContactNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.MobileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.FaxNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ReservationMode)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ReservationType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.ReservationStatus)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.PaymentMode)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.Reason)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservation>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservationItemDetail>()
                .Property(e => e.ItemType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservationItemDetail>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantReservationTableDetail>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTable>()
                .Property(e => e.TableNumber)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTable>()
                .Property(e => e.TableCapacity)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTable>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTableReservationDetail>()
                .Property(e => e.DiscountType)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTableStatus>()
                .Property(e => e.StatusName)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurantTableStatus>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SalesBandwidthInfo>()
                .Property(e => e.BandwidthType)
                .IsUnicode(false);

            modelBuilder.Entity<SalesBandwidthInfo>()
                .Property(e => e.BandwidthName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesContractDetails>()
                .Property(e => e.DocumentName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesContractDetails>()
                .Property(e => e.DocumentPath)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.CustomerType)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.WebAddress)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactPerson)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactFax)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactPerson2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactDesignation2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.Department2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactEmail2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactPhone2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesCustomer>()
                .Property(e => e.ContactFax2)
                .IsUnicode(false);

            modelBuilder.Entity<SalesServiceBundle>()
                .Property(e => e.BundleName)
                .IsUnicode(false);

            modelBuilder.Entity<SalesServiceBundle>()
                .Property(e => e.BundleCode)
                .IsUnicode(false);

            modelBuilder.Entity<SalesServiceBundle>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<SalesServiceBundleDetails>()
                .Property(e => e.IsProductOrService)
                .IsUnicode(false);

            modelBuilder.Entity<SalesServiceBundleDetails>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SecurityActivityLogs>()
                .Property(e => e.ActivityType)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityActivityLogs>()
                .Property(e => e.EntityType)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityActivityLogs>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityActivityLogs>()
                .Property(e => e.Module)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityLogError>()
                .Property(e => e.ErrorDetails)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityLogError>()
                .Property(e => e.ErrorStatus)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuGroup>()
                .Property(e => e.GroupIconClass)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PageId)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PageName)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PageDisplayCaption)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PageExtension)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PagePath)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityMenuLinks>()
                .Property(e => e.PageType)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityObjectTab>()
                .Property(e => e.ObjectGroupHead)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityObjectTab>()
                .Property(e => e.ObjectHead)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityObjectTab>()
                .Property(e => e.MenuHead)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityObjectTab>()
                .Property(e => e.ObjectType)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityObjectTab>()
                .Property(e => e.FormName)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserGroup>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserPassword)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserEmail)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserPhone)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityUserInformation>()
                .Property(e => e.UserDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<SMBillingPeriod>()
                .Property(e => e.BillingPeriodName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySalesCall>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySalesCall>()
                .Property(e => e.FollowupType)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySalesCall>()
                .Property(e => e.Purpose)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.SiteName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BusinessContactName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BusinessContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BusinessContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.TechnicalContactName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.TechnicalContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.TechnicalContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BillingContactName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BillingContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.BillingContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<SMCompanySite>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SMContractPeriod>()
                .Property(e => e.ContractPeriodName)
                .IsUnicode(false);

            modelBuilder.Entity<SMCurrentVendor>()
                .Property(e => e.VendorName)
                .IsUnicode(false);

            modelBuilder.Entity<SMIndustry>()
                .Property(e => e.IndustryName)
                .IsUnicode(false);

            modelBuilder.Entity<SMItemOrServiceDelivery>()
                .Property(e => e.DeliveryTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<SMQuotationDetails>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SMQuotationDetails>()
                .Property(e => e.TotalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SMSalesOrder>()
                .Property(e => e.SONumber)
                .IsUnicode(false);

            modelBuilder.Entity<SMSalesOrder>()
                .Property(e => e.ApprovedStatus)
                .IsUnicode(false);

            modelBuilder.Entity<SMSalesOrder>()
                .Property(e => e.DeliveryStatus)
                .IsUnicode(false);

            modelBuilder.Entity<SMSalesOrder>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SMServiceType>()
                .Property(e => e.ServiceName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpBankInfo>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpBankInfo>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpNomineeInfo>()
                .Property(e => e.NomineeName)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpNomineeInfo>()
                .Property(e => e.Relationship)
                .IsUnicode(false);

            modelBuilder.Entity<PayrollEmpNomineeInfo>()
                .Property(e => e.Age)
                .IsUnicode(false);

            modelBuilder.Entity<PMProductOutSerialInfo>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<SalesService>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<SalesService>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<SalesService>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<SalesService>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityLogFile>()
                .Property(e => e.InpuTMode)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityLogFile>()
                .Property(e => e.LogMemo)
                .IsUnicode(false);

            modelBuilder.Entity<SecurityLogFile>()
                .Property(e => e.LogMode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<viewChkFromLdg>()
                .Property(e => e.VcheqNo)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.EmpType)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.CategoryCode)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<viewComEmployeeInfo>()
                .Property(e => e.Department)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.CompanyCode)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.ProjectCode)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.Narration)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.NodeNarration)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.Hierarchy)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.NodeHead)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.NodeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.ChequeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.DRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.CRAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.VcheqNo)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.GLStatus)
                .IsUnicode(false);

            modelBuilder.Entity<viewCommonVoucher>()
                .Property(e => e.PayerOrPayee)
                .IsUnicode(false);

            modelBuilder.Entity<ViewLedgerDetailAmountSum>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ViewLedgerDetailAmountSum>()
                .Property(e => e.InWordAmount)
                .IsUnicode(false);
        }
    }
}
