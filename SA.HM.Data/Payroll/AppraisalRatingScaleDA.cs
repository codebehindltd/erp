using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class AppraisalRatingScaleDA: BaseService
    {
        public List<AppraisalRatingScaleBO> GetAllRatingFactorScale()
        {
            List<AppraisalRatingScaleBO> rtngFactScaleBOList = new List<AppraisalRatingScaleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllRtngFactorScale_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                AppraisalRatingScaleBO rtngFactScaleBO = new AppraisalRatingScaleBO();

                                rtngFactScaleBO.RatingScaleId = Convert.ToInt32(reader["RatingScaleId"]);
                                rtngFactScaleBO.RatingScaleName = reader["RatingScaleName"].ToString();
                                rtngFactScaleBO.IsRemarksMandatory = Convert.ToBoolean(reader["IsRemarksMandatory"]);
                                rtngFactScaleBO.RatingValue = Convert.ToDecimal(reader["RatingValue"]);
                                rtngFactScaleBO.RatingScaleNameWithRatingScale = reader["RatingScaleNameWithRatingScale"].ToString();
                                rtngFactScaleBO.Remarks = reader["Remarks"].ToString();

                                rtngFactScaleBOList.Add(rtngFactScaleBO);
                            }
                        }
                    }
                }
            }
            return rtngFactScaleBOList;
        }
    }
}
