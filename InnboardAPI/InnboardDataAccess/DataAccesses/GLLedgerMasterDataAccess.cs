using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
