﻿using InnboardAPI.DataAccesses;
using InnboardDomain.CriteriaDtoModel;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace InnboardDataAccess.DataAccesses
{
    public class GLLedgerMasterDataAccess : GenericDataAccess<GLLedgerMaster>, IGLLedgerMaster
    {
        public new GLLedgerMaster Save(GLLedgerMaster ledgerMaster)
        {
            string query = string.Format(@"SELECT dbo.FnVoucherNumber('{0}','{1}','','',{2}) as VoucherNo", ledgerMaster.VoucherDate, ledgerMaster.VoucherType.ToString(), ledgerMaster.ProjectId);

            string voucherNo = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            ledgerMaster.ReferenceVoucherNumber = ledgerMaster.VoucherNo;
            ledgerMaster.VoucherNo = voucherNo;
            return base.Save(ledgerMaster);
        }

        public async Task<List<GLLedgerMasterVwBO>> GetVoucherBySearchCriteria(GeneralLedgerCriteriaDto criteriaDto)
        {
            List<GLLedgerMasterVwBO> voucherSearch = new List<GLLedgerMasterVwBO>();

            var voucherNo = string.IsNullOrEmpty(criteriaDto.voucherNo) ? "null" : "'" + criteriaDto.voucherNo +"'";
            var referenceNo = string.IsNullOrEmpty(criteriaDto.referenceNo) ? "null" : "'" + criteriaDto.referenceNo + "'";
            var referenceVoucherNo = string.IsNullOrEmpty(criteriaDto.referenceVoucherNo) ? "null" : "'" + criteriaDto.referenceVoucherNo + "'";
            var narration = string.IsNullOrEmpty(criteriaDto.narration) ? "null" : "'" + criteriaDto.narration + "'";
            var voucherType = string.IsNullOrEmpty(criteriaDto.voucherType) ? "null" : "'" + criteriaDto.voucherType + "'";
            var voucherStatus = string.IsNullOrEmpty(criteriaDto.voucherStatus) ? "null" : "'" + criteriaDto.voucherStatus + "'";
            var fromDate = criteriaDto.fromDate==null ? "null" : "'" + criteriaDto.fromDate + "'";
            var toDate = criteriaDto.toDate==null ? "null" : "'" + criteriaDto.toDate + "'";

            string query = $@"EXEC [dbo].[GetVoucherBySearchCriteria_SP] {criteriaDto.companyId}, {criteriaDto.projectId}, {criteriaDto.userGroupId}, {criteriaDto.userInfoId}, " + fromDate+ $", " + toDate + $", " + voucherNo + $", " + referenceNo + $", " + referenceVoucherNo + $", " + narration + $", " + voucherType + $", " + voucherStatus + $", {criteriaDto.pageParams.PageSize}, {criteriaDto.pageParams.PageNumber}, NULL";
            voucherSearch = await InnboardDBContext.Database.SqlQuery<GLLedgerMasterVwBO>(query).ToListAsync();

            return voucherSearch;
        }

        public async Task<List<GLLedgerMasterVwBO>> GetVoucherInformation(int userId, int ledgerMasterId)
        {
            SqlParameter param1 = new SqlParameter("@UserInfoId", userId);
            SqlParameter param2 = new SqlParameter("@LedgerMasterId", ledgerMasterId);
            //string query = $@"EXEC [dbo].[GetVoucherInformationForApps_SP] {userId}, {ledgerMasterId}";
            var result= await InnboardDBContext.Database.SqlQuery<GLLedgerMasterVwBO>("EXEC [dbo].[GetVoucherInformationForApps_SP] @UserInfoId, @LedgerMasterId", param1, param2).ToListAsync();
            return result;
        }
    }
}
