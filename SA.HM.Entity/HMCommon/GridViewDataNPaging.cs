using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManagement.Entity.HMCommon
{
    /// <summary>
    /// Prepare Grid paging and and Calculate Page Index
    /// </summary>
    /// <typeparam name="T">T is the Grid Data Type (i.e. OvertimeBO, AttendanceBO etc)</typeparam>
    /// <typeparam name="U">U is Paging information type (i.e. GridPaging type)</typeparam>
    public class GridViewDataNPaging<T, U>
        where T : class
        where U : class
    {
        private int pageNumber = 1, recordsPerPage = 3, pageLinkShowed = 5, isCurrentOrPreviousPage = 1;

        public List<T> GridData { get; set; }
        public U GridPageLinks { get; set; }
        public string GridBody { get; set; }
        public int UserGroupId { get; set; }

        /// <summary>
        /// Initialize GridViewDataNPaging class property
        /// </summary>
        /// <param name="RecordsPerPage">number of data want to show in each page</param>
        /// <param name="PageLinkShowed">how many page link want to show</param>
        /// <param name="IsCurrentOrPreviousPage">whether want to show current or previous page. (It needs when all record delete from current page.). Flag is set 0 or 1.</param>
        public GridViewDataNPaging(int RecordsPerPage, int PageLinkShowed, int IsCurrentOrPreviousPage)
        {
            recordsPerPage = RecordsPerPage;
            pageLinkShowed = PageLinkShowed;
            isCurrentOrPreviousPage = IsCurrentOrPreviousPage;
        }

        /// <summary>
        ///  Prepare Gird paging and Page link
        /// </summary>
        /// <param name="data">The grid data that need to show in current page</param>
        /// <param name="TotalRecords">Total records/rows Of the data without apply paging criteria.</param>
        public void GridPagingProcessing(List<T> data, int TotalRecords)
        {
            int totalPages = 0, startPage = 1, endPage = 1;

            GridPaging p = new GridPaging();

            if (TotalRecords > 0 && TotalRecords <= recordsPerPage)
            {
                totalPages = 1;
            }
            else if (TotalRecords == 0)
            {
                totalPages = 0;
            }
            else
            {
                if ((TotalRecords % recordsPerPage) != 0)
                {
                    totalPages = (TotalRecords / recordsPerPage) + 1;
                }
                else
                {
                    totalPages = (TotalRecords / recordsPerPage);
                }
            }

            if (totalPages < pageLinkShowed)
            {
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                if ((pageNumber + pageLinkShowed - 1) <= totalPages)
                {
                    startPage = pageNumber;
                    endPage = (startPage + pageLinkShowed) - 1;
                }
                else if ((pageNumber + pageLinkShowed) > totalPages)
                {
                    startPage = (totalPages - pageLinkShowed) + 1;
                    endPage = startPage + pageLinkShowed - 1;

                    startPage = startPage <= 0 ? 1 : startPage;
                }
            }

            StringBuilder sbPagingPrevious = new StringBuilder();
            StringBuilder sbPagingNext = new StringBuilder();
            StringBuilder sbPaging = new StringBuilder();
            string PreviousPage = string.Empty, NextPage = string.Empty, Paging = string.Empty;


            for (int i = startPage; i <= endPage; i++)
            {
                string url = "GridPaging(" + i + ", " + isCurrentOrPreviousPage + ")";

                if (i == pageNumber)
                {
                    sbPaging.AppendFormat("<li  class=\"active\"> <a href=\"javascript:void();\">{0}</a> </li>", i);
                }
                else
                {
                    sbPaging.AppendFormat("<li> <a href=\"javascript:void();\" onclick=\"return {1};\">{0}</a> </li>", i, url);
                }
            }

            if (startPage > 1)
            {
                string urlPrevious = "GridPaging(" + (startPage - 1) + ", " + isCurrentOrPreviousPage + ")";

                sbPagingPrevious.AppendFormat("<li> <a title=\"{0}\" href=\"{1}\" onclick=\"return {2};\">{3}</a> </li>", "Previous page", "javascript:void();", urlPrevious, "&laquo;");
                PreviousPage = string.Format("{0}", sbPagingPrevious);
            }

            if ((startPage + pageLinkShowed) <= totalPages)
            {
                string urlNext = "GridPaging(" + (startPage + 1) + ", " + isCurrentOrPreviousPage + ")";

                sbPagingNext.AppendFormat("<li> <a title=\"{0}\" href=\"{1}\" onclick=\"return {2};\">{3}</a> </li>", "Next page", "javascript:void();", urlNext, "&raquo;");
                NextPage = string.Format("{0}", sbPagingNext);
            }

            if (PreviousPage == "" || pageNumber == 1)
            {
                sbPagingPrevious.AppendFormat("<li class=\"disabled\"> <a title=\"{0}\" href=\"{1}\" >{2}</a> </li>", "Previous page", "javascript:void();", "&laquo;");
                PreviousPage = string.Format("{0}", sbPagingPrevious);
                p.IsPreviousButtonVisible = 0;
            }

            if (NextPage == "" || endPage == 1)
            {
                sbPagingNext.AppendFormat("<li class=\"disabled\"> <a title=\"{0}\" href=\"{1}\">{2}</a> </li>", "Next page", "javascript:void();", "&raquo;");
                NextPage = string.Format("{0}", sbPagingNext);
                p.IsNextButtonVisible = 0;
            }

            Paging = string.Format("{0}", sbPaging);

            p.NextButton = NextPage;
            p.PreviousButton = PreviousPage;
            p.Pagination = Paging;
            p.CurrentPageNumber = pageNumber;

            this.GridPageLinks = (U)Convert.ChangeType(p, typeof(U));
            this.GridData = data;
        }

        /// <summary>
        ///  Prepare Gird paging and Page link
        /// </summary>
        /// <param name="data">The grid data that need to show in current page</param>
        /// <param name="TotalRecords">Total records/rows Of the data without apply paging criteria.</param>
        public void GridPagingProcessing(List<T> data, int TotalRecords, string GridPagingFunctioNameJS)
        {
            int totalPages = 0, startPage = 1, endPage = 1;

            GridPaging p = new GridPaging();

            if (TotalRecords > 0 && TotalRecords <= recordsPerPage)
            {
                totalPages = 1;
            }
            else if (TotalRecords == 0)
            {
                totalPages = 0;
            }
            else
            {
                if ((TotalRecords % recordsPerPage) != 0)
                {
                    totalPages = (TotalRecords / recordsPerPage) + 1;
                }
                else
                {
                    totalPages = (TotalRecords / recordsPerPage);
                }
            }

            if (totalPages < pageLinkShowed)
            {
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                if ((pageNumber + pageLinkShowed) > totalPages)
                {
                    startPage = totalPages - pageLinkShowed + 1;
                    endPage = startPage + pageLinkShowed - 1;
                }
                else if ((pageNumber + pageLinkShowed - 1) <= totalPages)
                {
                    startPage = pageNumber;
                    endPage = startPage + pageLinkShowed - 1;
                }
            }

            StringBuilder sbPagingPrevious = new StringBuilder();
            StringBuilder sbPagingNext = new StringBuilder();
            StringBuilder sbPaging = new StringBuilder();
            string PreviousPage = string.Empty, NextPage = string.Empty, Paging = string.Empty;


            for (int i = startPage; i <= endPage; i++)
            {
                string url = GridPagingFunctioNameJS + "(" + i + ", " + isCurrentOrPreviousPage + ")";

                if (i == pageNumber)
                {
                    sbPaging.AppendFormat("<li  class=\"active\"> <a href=\"javascript:void();\">{0}</a> </li>", i);
                }
                else
                {
                    sbPaging.AppendFormat("<li> <a href=\"javascript:void();\" onclick=\"javascript:return {1};\">{0}</a> </li>", i, url);
                }
            }

            if (startPage > 1)
            {
                string urlPrevious = GridPagingFunctioNameJS + "(" + (startPage - 1) + ", " + isCurrentOrPreviousPage + ")";

                sbPagingPrevious.AppendFormat("<li> <a title=\"{0}\" href=\"{1}\" onclick=\"javascript:return {2};\">{3}</a> </li>", "Previous page", "javascript:void();", urlPrevious, "&laquo;");
                PreviousPage = string.Format("{0}", sbPagingPrevious);
            }

            if ((startPage + pageLinkShowed) <= totalPages)
            {
                string urlNext = GridPagingFunctioNameJS + "(" + (startPage + 1) + ", " + isCurrentOrPreviousPage + ")";

                sbPagingNext.AppendFormat("<li> <a title=\"{0}\" href=\"{1}\" onclick=\"javascript:return {2};\">{3}</a> </li>", "Next page", "javascript:void();", urlNext, "&raquo;");
                NextPage = string.Format("{0}", sbPagingNext);
            }

            if (PreviousPage == "" || pageNumber == 1)
            {
                sbPagingPrevious.AppendFormat("<li class=\"disabled\"> <a title=\"{0}\" href=\"{1}\" >{2}</a> </li>", "Previous page", "javascript:void();", "&laquo;");
                PreviousPage = string.Format("{0}", sbPagingPrevious);
                p.IsPreviousButtonVisible = 0;
            }

            if (NextPage == "" || endPage == 1)
            {
                sbPagingNext.AppendFormat("<li class=\"disabled\"> <a title=\"{0}\" href=\"{1}\">{2}</a> </li>", "Next page", "javascript:void();", "&raquo;");
                NextPage = string.Format("{0}", sbPagingNext);
                p.IsNextButtonVisible = 0;
            }

            Paging = string.Format("{0}", sbPaging);

            p.NextButton = NextPage;
            p.PreviousButton = PreviousPage;
            p.Pagination = Paging;
            p.CurrentPageNumber = pageNumber;

            this.GridPageLinks = (U)Convert.ChangeType(p, typeof(U));
            this.GridData = data;
        }

        /// <summary>
        /// The method calculate whether it shows current page or previous page
        /// </summary>
        /// <param name="gridRecordsCount">How many data shows in the current page</param>
        /// <param name="pageNumber">The current Page Index</param>
        /// <returns>PageNumber</returns>
        public int PageNumberCalculation(int gridRecordsCount, int pageNumber)
        {
            if (gridRecordsCount == 1 && isCurrentOrPreviousPage != 1)
            {
                this.pageNumber = pageNumber > 1 ? pageNumber - 1 : 1;
            }
            else if (gridRecordsCount > 1 && isCurrentOrPreviousPage != 1)
            {
                this.pageNumber = pageNumber;
                this.isCurrentOrPreviousPage = 1;
            }
            else
            {
                this.pageNumber = pageNumber;
            }

            return this.pageNumber;
        }
    }
}
