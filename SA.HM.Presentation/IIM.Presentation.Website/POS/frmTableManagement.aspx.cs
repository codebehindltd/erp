using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmTableManagement : BasePage
    {
        HiddenField innboardMessage;
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadCostCenter();
                this.GenerateTableAllocation();
                if (this.ddlCostCenterId.SelectedIndex != -1)
                {
                    this.LoadGridView(Convert.ToInt32(this.ddlCostCenterId.SelectedValue));
                }

                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomProductId.Value = seatingId.ToString();

                FileUpload();
            }
        }
        protected void gvTableManagement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvTableManagement.PageIndex = e.NewPageIndex;
            if (this.ddlCostCenterId.SelectedIndex != -1)
            {
                this.LoadGridView(Convert.ToInt32(this.ddlCostCenterId.SelectedValue));
            }
        }

        protected void gvTableManagement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _TableManagementId = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "CmdDelete")
            {
                TableManagementDA tableManagementDA = new TableManagementDA();
                bool status = (tableManagementDA.DeleteDocumentsInfoByTableManagementId(_TableManagementId));

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    this.LoadGridView(Convert.ToInt32(this.ddlCostCenterId.SelectedValue));
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }

               
            }
        }
        protected void gvTableManagement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                TableManagementBO documentTable = (TableManagementBO)(e.Row.DataItem);
                Label lblIsSaveValue = (Label)e.Row.FindControl("lblchkIsActiveStatus");
                Button button = (Button)e.Row.FindControl("btnAttachment");
                imgDelete.Visible = false;
                if (lblIsSaveValue.Text == "Inactive")
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = false;
                    button.Enabled = false;
                    imgDelete.Visible = false;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkIsActiveStatus")).Checked = true;
                    button.Enabled = true;
                }

                var VacantImageControl = e.Row.FindControl("VacantImage") as Image;
                if (VacantImageControl == null) return;
                var OccupiedImageControl = e.Row.FindControl("OccupiedImage") as Image;
                if (OccupiedImageControl == null) return;
                
                List<DocumentsBO> document = new List<DocumentsBO>();
                document = LoadTableDocument(documentTable.TableId);
                documentTable.VacantImagePath = "";
                documentTable.OccupiedImagePath = "";
                VacantImageControl.ImageUrl = DataBinder.Eval(e.Row.DataItem, "VacantImagePath").ToString();
                OccupiedImageControl.ImageUrl = DataBinder.Eval(e.Row.DataItem, "OccupiedImagePath ").ToString();

                for (int row = 0; row < document.Count; row++)
                {
                    //var Path = "<img src='" + document[row].Path + document[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                    var Path =  document[row].Path + document[row].Name ;

                    if (document[row].DocumentCategory == "VacantTableDocument")
                    {
                        documentTable.VacantImagePath = Path;
                        VacantImageControl.ImageUrl = DataBinder.Eval(e.Row.DataItem, "VacantImagePath").ToString();
                        imgDelete.Visible = true;

                    }
                    else
                    {
                        //documentTable.OccupiedImagePath = "<img src='" + document[row].Path + document[row].Name + "' style=\"width:40px; height: 40px; cursor: pointer; cursor: hand;\"  alt='Document Image' border='0' /> ";
                        documentTable.OccupiedImagePath = Path;
                        OccupiedImageControl.ImageUrl = DataBinder.Eval(e.Row.DataItem, "OccupiedImagePath ").ToString();
                        imgDelete.Visible = true;
                    }
                }

                    

            }
        }
        protected void ddlCostCenterIdId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCostCenterId.SelectedIndex != -1)
            {
                isNewAddButtonEnable = -1;
                this.LoadGridView(Convert.ToInt32(this.ddlCostCenterId.SelectedValue));
            }
        }
        protected void ddlSrcCostCenterId_SelectedIndexChanged(object sender, EventArgs e)
        {
            isNewAddButtonEnable = 1;
            this.GenerateTableAllocation();
        }
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (ddlCostCenterId.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Select Cost Center", AlertType.Warning);
                return;
            }

            int counter = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (GridViewRow row in gvTableManagement.Rows)
            {
                counter = counter + 1;
                bool isSave = ((CheckBox)row.FindControl("chkIsActiveStatus")).Checked;

                TableManagementBO tableManagementBO = new TableManagementBO();
                TableManagementDA tableManagementDA = new TableManagementDA();
                tableManagementBO.ActiveStat = isSave;
                Label lblTableManagementIdValue = (Label)row.FindControl("lblTableManagementId");
                tableManagementBO.CostCenterId = Convert.ToInt32(this.ddlCostCenterId.SelectedValue);
                tableManagementBO.TableManagementId = Convert.ToInt32(lblTableManagementIdValue.Text);
                Label lblgvTableIdValue = (Label)row.FindControl("lblgvTableId");
                tableManagementBO.TableId = Convert.ToInt32(lblgvTableIdValue.Text);

                tableManagementBO.XCoordinate = 0;
                tableManagementBO.YCoordinate = 0;
                tableManagementBO.TableWidth = 73;
                tableManagementBO.TableHeight = 55;

                int tmptblManagementId;

                if (lblTableManagementIdValue.Text == "0")
                {
                    if (isSave)
                    {
                        tableManagementBO.CreatedBy = userInformationBO.UserInfoId;
                        Boolean statusSave = tableManagementDA.SaveHMFloorManagementInfo(tableManagementBO, out tmptblManagementId);
                    }
                }
                else
                {
                    if (!isSave)
                    {
                        this.DeleteData(Convert.ToInt32(lblTableManagementIdValue.Text));
                    }
                }
            }

            if (gvTableManagement.Rows.Count == counter)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
            }

            if (this.ddlCostCenterId.SelectedIndex != -1)
            {
                isNewAddButtonEnable = -1;
                this.LoadGridView(Convert.ToInt32(this.ddlCostCenterId.SelectedValue));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            string tableAllocationValue = this.txtTableWiseRoomAllocationInfo.Value;
            string[] words;
            words = tableAllocationValue.Split('|');
            if (words.Length > 0)
            {
                for (int i = 0; i < words.Length - 1; i++)
                {
                    string[] wordsSplit;
                    wordsSplit = words[i].Split(',');

                    int mTableManagementId = Convert.ToInt32(wordsSplit[0]);
                    string mXCoordinate = (wordsSplit[1].Substring(0,wordsSplit[1].Length - 2));
                    string mYCoordinate = (wordsSplit[2].Substring(0,wordsSplit[2].Length - 2));
                    string mWidth = (wordsSplit[3].Substring(0,wordsSplit[3].Length - 2));
                    string mHeight = (wordsSplit[4].Substring(0,wordsSplit[4].Length - 2));
                    string divTransition = (wordsSplit[5])+','+ (wordsSplit[6])+ ',' + (wordsSplit[7])+ ',' + (wordsSplit[8])+ ',' + (wordsSplit[9])+ ',' + (wordsSplit[10]);

                    TableManagementBO tableManagementBO = new TableManagementBO();
                    TableManagementDA tableManagementDA = new TableManagementDA();
                    tableManagementBO.CostCenterId = Convert.ToInt32(this.ddlSrcCostCenterId.SelectedValue);
                    tableManagementBO.TableManagementId = mTableManagementId;

                    tableManagementBO.XCoordinate = Convert.ToDecimal(mXCoordinate);
                    tableManagementBO.YCoordinate = Convert.ToDecimal(mYCoordinate);
                    tableManagementBO.TableWidth = Convert.ToDecimal(mWidth);
                    tableManagementBO.TableHeight = Convert.ToDecimal(mHeight);
                    tableManagementBO.DivTransition = divTransition;

                    tableManagementBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean statusSave = tableManagementDA.UpdateTableManagementInfo(tableManagementBO);
                }
            }
            this.GenerateTableAllocation();
            CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
        }
        //************************ User Defined Function ********************//
        private void LoadGridView(int costCenterId)
        {
            this.CheckObjectPermission();
            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> files = entityDA.GetAllTableInfoByCostCenterId(costCenterId);

            this.gvTableManagement.DataSource = files;
            this.gvTableManagement.DataBind();
            this.GenerateTableAllocation();
        }
        public void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            var List = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo();
            this.ddlCostCenterId.DataSource = List;
            this.ddlCostCenterId.DataTextField = "CostCenter";
            this.ddlCostCenterId.DataValueField = "CostCenterId";
            this.ddlCostCenterId.DataBind();

            this.ddlSrcCostCenterId.DataSource = List;
            this.ddlSrcCostCenterId.DataTextField = "CostCenter";
            this.ddlSrcCostCenterId.DataValueField = "CostCenterId";
            this.ddlSrcCostCenterId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCostCenterId.Items.Insert(0, item);
            this.ddlSrcCostCenterId.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO != null)
            {
                
                btnSaveAll.Visible = isSavePermission;
                btnSave.Visible = isSavePermission;
                if (!isSavePermission)
                {
                    isNewAddButtonEnable = -1;
                }
            }
            else
            {
                Response.Redirect("/Login.aspx");
            }
        }
        private void DeleteData(int sEmpId)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            Boolean status = hmCommonDA.DeleteInfoById("RestaurantTableManagement", "TableManagementId", sEmpId);
        }
        private void GenerateTableAllocation()
        {
            TableManagementDA entityDA = new TableManagementDA();
            List<TableManagementBO> entityListBO = new List<TableManagementBO>();

            string fullContent = string.Empty;

            hfTotalRoomNumber.Value = null;

            if (this.ddlSrcCostCenterId.SelectedIndex != -1)
            {
                entityListBO = entityDA.GetTableManagementInfo(Convert.ToInt32(this.ddlSrcCostCenterId.SelectedValue));

                hfTotalRoomNumber.Value =  (entityListBO.Count.ToString());

                string topPart = @"<div class='row'>";

                string topTemplatePart = @"
                                            <div id='FloorRoomAllocation' class='col-md-12'>           
                                            ";

                string endTemplatePart = @"</div>
                                        </div>";

                string subContent = string.Empty;

                for (int iRoomNumber = 0; iRoomNumber < entityListBO.Count; iRoomNumber++)
                {
                    List<DocumentsBO> docList = new List<DocumentsBO>();

                    if (entityListBO[iRoomNumber].TableManagementId > 0)
                        docList = (new DocumentsDA().GetDocumentsByUserTypeAndUserId("VacantTableDocument", (int)entityListBO[iRoomNumber].TableManagementId));

                    docList = new HMCommonDA().GetDocumentListWithIcon(docList);

                    int len = docList.Count;
                    var Path = "";
                    if (len > 0)
                        Path = docList[len - 1].Path + docList[len - 1].Name;


                    //draggable" + iRoomNumber + " 
                    string Content0 = @"<div class='draggable draggable" + iRoomNumber + " draggableClassWithRotate RestaurantTableAvailableDiv' style='top:" + entityListBO[iRoomNumber].YCoordinate + "px; left:" + entityListBO[iRoomNumber].XCoordinate + "px; transform:" + entityListBO[iRoomNumber].DivTransition+ "; width:" + entityListBO[iRoomNumber].TableWidth + "px; height:" + entityListBO[iRoomNumber].TableHeight + "px; ' id='" + entityListBO[iRoomNumber].TableManagementId + "'>";

                    if (Path!= "")
                    {
                        Content0 += @"<img src=" + Path + " alt='Table Image' height=" + entityListBO[iRoomNumber].TableHeight + " width=" + entityListBO[iRoomNumber].TableWidth + "> ";
                    }
                    string Content1 = @"<a href='#" + entityListBO[iRoomNumber].TableId;
                    string Content2 = @"'><div class='FloorRoomAvailableDiv'>
                                                    </div></a>
                                                    <div class='TableNumberDiv'>" + entityListBO[iRoomNumber].TableNumber + "</br></div></div>";

                    subContent += Content0 + Content1 + Content2;
                }

                fullContent += topPart + topTemplatePart + subContent + endTemplatePart;

                this.ltlRoomTemplate.Text = fullContent;
            }
        }

        // Documents //
        public List<DocumentsBO> LoadTableDocument(long id)
        {
            int costCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
            TableManagementBO tb = new TableManagementBO();
            tb = new TableManagementDA().GetTableInfoByCostCenterNTableId(costCenterId, id);
            int OwnerId = tb.TableManagementId;
            
            List<DocumentsBO> docList = new List<DocumentsBO>();

            //docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("VacantTableDocument", randomId);

            if (OwnerId > 0)
                docList = (new DocumentsDA().GetDocumentsByUserTypeAndUserId("VacantTableDocument", (int)OwnerId));

            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            List<DocumentsBO> docList2 = new List<DocumentsBO>();

            //docList2 = new DocumentsDA().GetDocumentsByUserTypeAndUserId("OccupiedTableDocument", randomId);

            if (OwnerId > 0)
                docList2 = (new DocumentsDA().GetDocumentsByUserTypeAndUserId("OccupiedTableDocument", (int)OwnerId));

            docList2 = new HMCommonDA().GetDocumentListWithIcon(docList2);

            docList.AddRange(docList2);

            return docList;
        }

        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "LoadDetailGridInformation", jscript, true);

            flashUpload1.QueryParameters = "VacantImageId=" + Server.UrlEncode(RandomProductId.Value);
            flashUpload2.QueryParameters = "OccupiedImageId=" + Server.UrlEncode(RandomProductId.Value);

            //flashUpload.QueryParameters = "InventoryProductId=" + Server.UrlEncode(RandomProductId.Value) + "~" + txtCode.Text;
        }

        protected void btnUploadComplete_Click(object sender, EventArgs e)
        {
            DocumentsBO doc = new DocumentsBO();
            doc.CostCenterId = Convert.ToInt32(this.ddlCostCenterId.SelectedValue);
            doc.OwnerId = Convert.ToInt32( RandomProductId.Value);
            doc.Id = Convert.ToInt32(hfTableManagementId.Value);
            if(hfImageType.Value == "VacantImage")
            {
                doc.DocumentCategory = "VacantTableDocument";
            }
            else
            {
                doc.DocumentCategory = "OccupiedTableDocument";
            }
            


            bool status  = new TableManagementDA().UpdateDocumentsOwnerIdByTableManagementId(doc);
            if(status == true)
            {
                int costCenterId = Convert.ToInt32(ddlCostCenterId.SelectedValue);
                LoadGridView(costCenterId);

            }
            
        }

        
          
    }
}