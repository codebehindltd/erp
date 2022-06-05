using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public static class UtilityDA
    {
        public static string AgeCalculation(DateTime dateOfBirth)
        {
            DateTime adtCurrentDate = DateTime.Now;
            int ageNoOfYears, ageNoOfMonths, ageNoOfDays;
            string age = string.Empty;

            ageNoOfDays = adtCurrentDate.Day - dateOfBirth.Day;
            ageNoOfMonths = adtCurrentDate.Month - dateOfBirth.Month;
            ageNoOfYears = adtCurrentDate.Year - dateOfBirth.Year;

            if (ageNoOfDays < 0)
            {
                ageNoOfDays += DateTime.DaysInMonth(adtCurrentDate.Year, adtCurrentDate.Month);
                ageNoOfDays--;
            }

            if (ageNoOfMonths < 0)
            {
                ageNoOfMonths += 12;
                ageNoOfMonths--;
            }

            age = ageNoOfYears.ToString() + " years, " + ageNoOfMonths.ToString() + " months, " + ageNoOfDays.ToString() + " days";

            return age;

            // Antoher way
            //DateTime dob = Convert.ToDateTime("18 Feb 1987");
            //DateTime PresentYear = DateTime.Now;
            //TimeSpan ts = PresentYear - dob;
            //DateTime Age = DateTime.MinValue.AddDays(ts.Days);
            //MessageBox.Show(string.Format(" {0} Years {1} Month {2} Days", Age.Year - 1, Age.Month - 1, Age.Day - 1));
        }

        public static T ConvertDataRowToObjet<T>(DataRow dataRow) where T : class
        {
            T covertedObject = Activator.CreateInstance<T>();
            var properties = covertedObject.GetType().GetProperties();

            //foreach (var item in dataRow.Table.Columns)
            //{
            //    if (!dataRow.IsNull(item.ToString()) && dataRow[item.ToString()] != DBNull.Value)
            //        covertedObject.GetType().GetProperty(item.ToString()).SetValue(covertedObject, dataRow[item.ToString()], null);
            //}
            foreach (var item in covertedObject.GetType().GetProperties())
            {
                if (dataRow.Table.Columns.Contains(item.Name) && !dataRow.IsNull(item.Name) && dataRow[item.Name] != DBNull.Value)
                    item.SetValue(covertedObject, dataRow[item.Name], null);
            }
            return covertedObject;
        }
    }
}
