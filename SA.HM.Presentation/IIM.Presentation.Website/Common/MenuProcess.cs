using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HotelManagement.Entity.Security;
using HotelManagement.Data.Security;

namespace HotelManagement.Presentation.Website.Common
{
    public class MenuProcess
    {
        public string UserMenu(int userGroupId)
        {
            string menu = string.Empty;
            string linkName = string.Empty;

            MenuDA menuDa = new MenuDA();

            List<MenuGroupBO> menuGroup = new List<MenuGroupBO>();
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            List<MenuWiseLinkViewBO> userWiseMenu = new List<MenuWiseLinkViewBO>();

            menuGroup = menuDa.GetMenuGroupByUserGroupId(userGroupId, "");
            menuWiseLinks = menuDa.GetMenuWiseLinksByUserGroupId(userGroupId, "");

            menu = "<div id='MenuExpandable' style='float: left'>";
            menu += "<a href='/HMCommon/frmHMHome.aspx' class='nav-header'><i class='icon-th-list'></i>Dashboard</a>";

            foreach (MenuGroupBO mg in menuGroup)
            {
                linkName = mg.MenuGroupName.Replace(" ", string.Empty);

                menu += string.Format("<a id='grp{0}' href='#{1}' class='nav-header collapsed' " +
                                      "data-toggle='collapse'><i class='icon-home'></i>{2}<i class='icon-chevron-up'> " +
                                      "</i></a>", linkName, linkName, mg.MenuGroupName);

                userWiseMenu = (from m in menuWiseLinks where m.MenuGroupId == mg.MenuGroupId select m).OrderBy(o => o.LinksDisplaySequence).ToList();

                menu += string.Format("<ul id='{0}' class='nav nav-list collapse'>", linkName);

                foreach (MenuWiseLinkViewBO mwl in userWiseMenu)
                {
                    menu += string.Format("<li><a href='{0}'>" +
                                          "<i class='icon-file-alt'></i>{1}</a></li>", (mwl.PagePath + mwl.PageId + mwl.PageExtension), mwl.PageName);
                }
                menu += "</ul>";
            }
            menu += "</div>";

            return menu;
        }
        public string UserMainMenu(int userGroupId)
        {
            string menu = string.Empty;
            string linkName = string.Empty;

            MenuDA menuDa = new MenuDA();

            List<MenuGroupBO> menuGroup = new List<MenuGroupBO>();
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            List<MenuWiseLinkViewBO> userWiseMenu = new List<MenuWiseLinkViewBO>();

            menuGroup = menuDa.GetMenuGroupByUserGroupId(userGroupId, "Page");
            menuWiseLinks = menuDa.GetMenuWiseLinksByUserGroupId(userGroupId, "Page");

            menu = "<div class='MenuCollapsible'>";
            menu += "<ul>";
            menu += "<li><a href='/HMCommon/frmInnboardDashboard.aspx'><span><i class='icon-home'></i>&nbsp;&nbsp;Dashboard</span></a></li>";

            foreach (MenuGroupBO mg in menuGroup)
            {
                linkName = mg.MenuGroupName.Replace(" ", string.Empty);

                menu += string.Format("<li><a href='{0}'><span><i class='{2}'></i>&nbsp;&nbsp;{1}</span></a>", linkName, mg.MenuGroupName, mg.GroupIconClass);

                userWiseMenu = (from m in menuWiseLinks where m.MenuGroupId == mg.MenuGroupId select m).OrderBy(o => o.LinksDisplaySequence).ToList();

                menu += "<ul>";
                foreach (MenuWiseLinkViewBO mwl in userWiseMenu)
                {
                    menu += string.Format("<li><a href='{0}'>{1}</a></li>", (mwl.PagePath + "/" + mwl.PageId + "." + mwl.PageExtension), mwl.PageName);
                    menu += "</li>";
                }
                menu += "</ul>";
            }
            menu += "</ul> </div>";

            //menu = "<div id='MenuExpandable' style='float: left'>";
            //menu += "<a href='/HMCommon/frmHMHome.aspx' class='nav-header'><i class='icon-th-list'></i>Dashboard</a>";

            //foreach (MenuGroupBO mg in menuGroup)
            //{
            //    linkName = mg.MenuGroupName.Replace(" ", string.Empty);

            //    menu += string.Format("<a id='grp{0}' href='#{1}' class='nav-header collapsed' " +
            //                          "data-toggle='collapse'><i class='icon-home'></i>{2}<i class='icon-chevron-up'> " +
            //                          "</i></a>", linkName, linkName, mg.MenuGroupName);

            //    userWiseMenu = (from m in menuWiseLinks where m.MenuGroupId == mg.MenuGroupId select m).OrderBy(o => o.LinksDisplaySequence).ToList();

            //    menu += string.Format("<ul id='{0}' class='nav nav-list collapse'>", linkName);

            //    foreach (MenuWiseLinkViewBO mwl in userWiseMenu)
            //    {
            //        menu += string.Format("<li><a href='{0}'>" +
            //                              "<i class='icon-file-alt'></i>{1}</a></li>", (mwl.PagePath + mwl.PageId + mwl.PageExtension), mwl.PageName);
            //    }
            //    menu += "</ul>";
            //}
            //menu += "</div>";

            return menu;
        }
        public string UserReportMenu(int userGroupId)
        {
            string menu = string.Empty;
            string linkName = string.Empty;

            MenuDA menuDa = new MenuDA();

            List<MenuGroupBO> menuGroup = new List<MenuGroupBO>();
            List<MenuWiseLinkViewBO> menuWiseLinks = new List<MenuWiseLinkViewBO>();
            List<MenuWiseLinkViewBO> userWiseMenu = new List<MenuWiseLinkViewBO>();

            menuGroup = menuDa.GetMenuGroupByUserGroupId(userGroupId, "Report");
            menuWiseLinks = menuDa.GetMenuWiseLinksByUserGroupId(userGroupId, "Report");

            menu = "<div class='MenuCollapsible'>";
            menu += "<ul>";
            menu += "<li><a href='/HMCommon/frmInnboardDashboard.aspx'><span><i class='icon-home'></i>&nbsp;&nbsp;Dashboard</span></a></li>";

            foreach (MenuGroupBO mg in menuGroup)
            {
                linkName = mg.MenuGroupName.Replace(" ", string.Empty);

                menu += string.Format("<li><a href='{0}'><span><i class='{2}'></i>&nbsp;&nbsp;{1}</span></a>", linkName, mg.MenuGroupName, mg.GroupIconClass);

                userWiseMenu = (from m in menuWiseLinks where m.MenuGroupId == mg.MenuGroupId select m).OrderBy(o => o.LinksDisplaySequence).ToList();

                menu += "<ul>";
                foreach (MenuWiseLinkViewBO mwl in userWiseMenu)
                {
                    menu += string.Format("<li><a href='{0}'>{1}</a></li>", (mwl.PagePath + "/" + mwl.PageId + "." + mwl.PageExtension), mwl.PageName);
                    menu += "</li>";
                }
                menu += "</ul>";
            }
            menu += "</ul> </div>";

            //menu = "<div id='MenuExpandable' style='float: left'>";
            //menu += "<a href='/HMCommon/frmHMHome.aspx' class='nav-header'><i class='icon-th-list'></i>Dashboard</a>";

            //foreach (MenuGroupBO mg in menuGroup)
            //{
            //    linkName = mg.MenuGroupName.Replace(" ", string.Empty);

            //    menu += string.Format("<a id='grp{0}' href='#{1}' class='nav-header collapsed' " +
            //                          "data-toggle='collapse'><i class='icon-home'></i>{2}<i class='icon-chevron-up'> " +
            //                          "</i></a>", linkName, linkName, mg.MenuGroupName);

            //    userWiseMenu = (from m in menuWiseLinks where m.MenuGroupId == mg.MenuGroupId select m).OrderBy(o => o.LinksDisplaySequence).ToList();

            //    menu += string.Format("<ul id='{0}' class='nav nav-list collapse'>", linkName);

            //    foreach (MenuWiseLinkViewBO mwl in userWiseMenu)
            //    {
            //        menu += string.Format("<li><a href='{0}'>" +
            //                              "<i class='icon-file-alt'></i>{1}</a></li>", (mwl.PagePath + mwl.PageId + mwl.PageExtension), mwl.PageName);
            //    }
            //    menu += "</ul>";
            //}
            //menu += "</div>";

            return menu;
        }
    }
}