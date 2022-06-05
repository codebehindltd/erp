using HotelManagement.Data.SDC;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.SDC;
using HotelManagement.Entity.UserInformation;
using InnBoardSDC.SDCTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website.Common.SDCTool
{
    public class SdcInvoiceHandler
    {
        public Boolean IsInvoiceReceived { get; set; }
        public Boolean IsDeviceConnected { get; set; }
        public string SdcConnectionFailedMessage { get; set; }
        public string _BillType = string.Empty;
        public SdcInvoiceHandler(string BillType)
        {
            this._BillType = BillType;
        }
        public void SdcInvoiceProcess(UserInformationBO userInformationBO, int BillId, List<SdcBillReportBO> billDetails)
        {
            InvoiceHandler invoiceHandler = new InvoiceHandler();

            invoiceHandler.BillId = BillId;
            invoiceHandler.UInfoBO = userInformationBO;
            //invoiceHandler.CashierId = "11";
            invoiceHandler.CashierId = userInformationBO.UserInfoId.ToString();
            invoiceHandler.BuyerInfo = "1123132";
            invoiceHandler.CurrencyCode = "BDT";
            invoiceHandler.PayType = "PAYTYPE_CASH";
            invoiceHandler.InvoiceType = "SDCA0000";
            invoiceHandler.CheckCode = "5AEAD1D3D154FE1D39836A37023663DF9BE0DFEF9398EBD4141AA0A0A9DA07F3";

            List<sdcTool.msgItems.invoiceReq.GoodsInfoItem> goodsInfo = new List<sdcTool.msgItems.invoiceReq.GoodsInfoItem>();

            foreach (SdcBillReportBO bo in billDetails)
            {
                if (bo.ItemId > 0)
                {
                    sdcTool.msgItems.invoiceReq.GoodsInfoItem goodItem = new sdcTool.msgItems.invoiceReq.GoodsInfoItem();
                    goodItem.code = bo.ItemCode;
                    goodItem.hsCode = bo.HsCode;
                    goodItem.item = bo.ItemName;
                    goodItem.price = bo.UnitRate.ToString();
                    goodItem.qty = bo.PaxQuantity.ToString();
                    goodItem.sd_category = bo.SdCategory;
                    goodItem.vat_category = bo.VatCategory;

                    goodsInfo.Add(goodItem);
                }
            }

            invoiceHandler.OnInvoiceDataReceive += InvoiceHandler_OnInvoiceDataReceive;
            invoiceHandler.OnDeviceConnectionFailed += InvoiceHandler_OnDeviceConnectionFailed;
            invoiceHandler.DoInvoice(goodsInfo);
        }

        private void InvoiceHandler_OnDeviceConnectionFailed(string message)
        {
            this.IsDeviceConnected = false;
            this.IsInvoiceReceived = true;
            this.SdcConnectionFailedMessage = message;
        }

        private void InvoiceHandler_OnInvoiceDataReceive(UserInformationBO UInfoBO, int BillId, sdcTool.msgItems.invoiceRes invoiceResponse)
        {
            sdcTool.msgItems.invoiceRes.dataItems data = (sdcTool.msgItems.invoiceRes.dataItems)JsonHelper.JsonConvertObject<sdcTool.msgItems.invoiceRes.dataItems>(invoiceResponse.data);

            SDCInvoiceInformationDA da = new SDCInvoiceInformationDA();
            SDCInvoiceInformationBO bo = new SDCInvoiceInformationBO();
            bo.BillId = BillId;
            bo.SDCInvoiceNumber = data.invoiceNo;
            bo.QRCode = data.qrCode;
            bo.BillType = this._BillType;
            int sdcLastId = 0;
            da.SaveSDCInvoiceInformation(bo, out sdcLastId);
            this.IsDeviceConnected = true;
            this.IsInvoiceReceived = true;
        }
    }
}