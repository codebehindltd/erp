using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website
{
    public partial class InnboardMembership : System.Web.UI.MasterPage
    {
        public string innBoardDateFormat = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("InnboardCalender", "InnboardCalenderFormat");

            string[] formats = Regex.Split(commonSetupBO.SetupValue, "~");
            //userInformationBO.ServerDateFormat = formats[0];
            //userInformationBO.ClientDateFormat = formats[1];

            innBoardDateFormat = formats[1];
        }
    }
}