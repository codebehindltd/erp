using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common
{
    public class ComaprisionOperator
    {
        public string EQ { get; private set; }
        public string GT { get; private set; }
        public string LT { get; private set; }
        public string GE { get; private set; }
        public string LE { get; private set; }

        public ComaprisionOperator()
        {
            EQ = " = ";
            GT = " > ";
            LT = " < ";
            GE = " >= ";
            LE = " <= ";
        }
    }
}