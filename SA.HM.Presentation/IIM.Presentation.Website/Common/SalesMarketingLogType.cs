using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using static HotelManagement.Presentation.Website.Common.ConstantHelper;

namespace HotelManagement.Presentation.Website.Common
{
    public class SalesMarketingLogType<T> where T : class
    {
        /// <summary>
        /// Generate Log message based on given object1 & object2 Property and Log Type.
        /// For each event type log if any one save then type will be Evnet+Create (ex- ContactCreated, CompanyCreated)
        /// For Update each event log type will be Event+Activity(ex- ContactActivity, CompanyActivity)
        /// </summary>
        /// <typeparam name="T">The Class in which the log generate</typeparam>
        /// <param name="logType">SalesandMarketingLogType Enum Type</param>
        /// <param name="object1">The data object which need to save. It is mandatory for Save & update</param>
        /// <param name="object2">The data object which retrive from database before update. It is mandatory for update</param>
        /// <returns>bool type value whether the log message saved or not</returns>
        public bool Log(SalesandMarketingLogType logType, T object1, T object2)
        {
            bool status = false;
            StringBuilder logMessage = new StringBuilder();
            List<ClassPropertyVeriance> variances = new List<ClassPropertyVeriance>();
            long companyId = 0, contactId = 0, dealId = 0, salesCallEntryId = 0, Id;
            List<SMDealWiseContactMap> contacts = new List<SMDealWiseContactMap>();
            List<SMDealWiseContactMap> preContacts = new List<SMDealWiseContactMap>();
            List<SalesCallParticipantBO> participantFromClient = new List<SalesCallParticipantBO>();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformation = new UserInformationBO();
            userInformation = hmUtility.GetCurrentApplicationUserInfo();
            long LogKeepingId = 0;
            try
            {
                if (SalesandMarketingLogType.CompanyCreated == logType)
                {
                    logMessage.Append(((string)(object1.GetType().GetProperty("CompanyName").GetValue(object1, null))) + " created by " + userInformation.UserName);
                    companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));
                }
                else if (SalesandMarketingLogType.CompanyActivity == logType)
                {
                    variances = object1.CompareTwoCalssPropertyFindDifference(object2);
                    companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));

                    logMessage.Append(userInformation.UserName);

                    foreach (ClassPropertyVeriance v in variances)
                    {
                        if (v.PropertyName == "CompanyOwnerId")
                        {
                            logMessage.Append(" updated contact owner from " +
                                         (
                                            (v.Value2 != null) ?
                                                ((string)(object2.GetType().GetProperty("CompanyOwnerName").GetValue(object2, null)))
                                            : "Un-Assigned"
                                         )
                                         + " to " +
                                         (
                                           (v.Value1 != null) ?
                                           ((string)(object1.GetType().GetProperty("CompanyOwnerName").GetValue(object1, null)))
                                            : "Un-Assigned"
                                         ));
                        }
                        else if (v.PropertyName == "LifeCycleStageId")
                        {
                            logMessage.Append(" updated company life cycle stage from " +
                                       (
                                          (v.Value2 != null) ?
                                              ((string)(object2.GetType().GetProperty("LifeCycleStage").GetValue(object2, null)))
                                          : "Un-Assigned"
                                       )
                                       + " to " +
                                       (
                                         (v.Value1 != null) ?
                                         ((string)(object1.GetType().GetProperty("LifeCycleStage").GetValue(object1, null)))
                                          : "Un-Assigned"
                                       ));
                        }
                    }
                }
                else if (SalesandMarketingLogType.ContactCreated == logType)
                {
                    logMessage.Append(((string)(object1.GetType().GetProperty("Name").GetValue(object1, null))) + " was created by " + userInformation.UserName);
                    contactId = Convert.ToInt64(object1.GetType().GetProperty("Id").GetValue(object1, null));

                    if (object1.GetType().GetProperty("CompanyId").GetValue(object1, null) != null)
                        companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));
                }
                else if (SalesandMarketingLogType.ContactActivity == logType)
                {
                    variances = object1.CompareTwoCalssPropertyFindDifference(object2);

                    contactId = Convert.ToInt64(object1.GetType().GetProperty("Id").GetValue(object1, null));

                    if (object1.GetType().GetProperty("CompanyId").GetValue(object1, null) != null)
                        companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));

                    logMessage.Append(userInformation.UserName);

                    foreach (ClassPropertyVeriance v in variances)
                    {
                        if (v.PropertyName == "ContactOwnerId")
                        {
                            logMessage.Append(" updated contact owner from " +
                                        (
                                           (v.Value2 != null) ?
                                               ((string)(object2.GetType().GetProperty("ContactOwner").GetValue(object2, null)))
                                           : "Un-Assigned"
                                        )
                                        + " to " +
                                        (
                                          (v.Value1 != null) ?
                                          ((string)(object1.GetType().GetProperty("ContactOwner").GetValue(object1, null)))
                                           : "Un-Assigned"
                                        ));
                        }
                        else if (v.PropertyName == "CompanyId")
                        {
                            logMessage.Append(" updated company reference from " +
                                       (
                                          (v.Value2 != null) ?
                                              ((string)(object2.GetType().GetProperty("CompanyName").GetValue(object2, null)))
                                          : "Un-Assigned"
                                       )
                                       + " to " +
                                       (
                                         (v.Value1 != null) ?
                                         ((string)(object1.GetType().GetProperty("CompanyName").GetValue(object1, null)))
                                          : "Un-Assigned"
                                       ));
                        }
                        else if (v.PropertyName == "LifeCycleId")
                        {
                            logMessage.Append(" updated contact life cycle stage from " +
                                       (
                                          (v.Value2 != null) ?
                                              ((string)(object2.GetType().GetProperty("LifeCycleStage").GetValue(object2, null)))
                                          : "Un-Assigned"
                                       )
                                       + " to " +
                                       (
                                         (v.Value1 != null) ?
                                         ((string)(object1.GetType().GetProperty("LifeCycleStage").GetValue(object1, null)))
                                          : "Un-Assigned"
                                       ));
                        }

                    }
                }
                else if (SalesandMarketingLogType.LoggedCall == logType || SalesandMarketingLogType.LoggedMessage == logType || SalesandMarketingLogType.LoggedEmail == logType || SalesandMarketingLogType.LoggedMeeting == logType)
                {
                    logMessage.Append(((string)(object1.GetType().GetProperty("LogDescription").GetValue(object1, null))));
                    
                    companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));
                    contactId = Convert.ToInt64(object1.GetType().GetProperty("ContactId").GetValue(object1, null));
                    dealId = Convert.ToInt64(object1.GetType().GetProperty("DealId").GetValue(object1, null));
                    salesCallEntryId = Convert.ToInt64(object1.GetType().GetProperty("Id").GetValue(object1, null));
                    participantFromClient = (List<SalesCallParticipantBO>)(object1.GetType().GetProperty("ParticipantFromClient").GetValue(object1, null));
                }
                else if (SalesandMarketingLogType.DealCreated == logType)
                {
                    logMessage.Append(((string)(object1.GetType().GetProperty("Name").GetValue(object1, null))) + " created by " + userInformation.UserName);
                    dealId = Convert.ToInt64(object1.GetType().GetProperty("Id").GetValue(object1, null));

                    if (object1.GetType().GetProperty("CompanyId").GetValue(object1, null) != null)
                        companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));

                    if (object1.GetType().GetProperty("Contacts").GetValue(object1, null) != null)
                        contacts.AddRange((List<SMDealWiseContactMap>)object1.GetType().GetProperty("Contacts").GetValue(object1, null));
                }
                else if (SalesandMarketingLogType.DealActivity == logType)
                {
                    variances = object1.CompareTwoCalssPropertyFindDifference(object2);
                    dealId = Convert.ToInt64(object1.GetType().GetProperty("Id").GetValue(object1, null));

                    logMessage.Append(userInformation.UserName);

                    if (object1.GetType().GetProperty("Contacts").GetValue(object1, null) != null)
                        contacts.AddRange((List<SMDealWiseContactMap>)object1.GetType().GetProperty("Contacts").GetValue(object1, null));
                    if (object2.GetType().GetProperty("Contacts").GetValue(object2, null) != null)
                        preContacts.AddRange((List<SMDealWiseContactMap>)object2.GetType().GetProperty("Contacts").GetValue(object2, null));

                    foreach (ClassPropertyVeriance v in variances)
                    {
                        if (v.PropertyName == "DealOwnerID")
                        {
                            logMessage.Append(" updated deal owner from " +
                                         (
                                            (v.Value2 != null) ?
                                                ((string)(object2.GetType().GetProperty("DealOwner").GetValue(object2, null)))
                                            : "Un-Assigned"
                                         )
                                         + " to " +
                                         (
                                           (v.Value1 != null) ?
                                           ((string)(object1.GetType().GetProperty("DealOwner").GetValue(object1, null)))
                                            : "Un-Assigned"
                                         ));
                        }
                        else if (v.PropertyName == "StageId")
                        {
                            logMessage.Append(" moved deal from " +
                                         (
                                            (v.Value2 != null) ?
                                                ((string)(object2.GetType().GetProperty("Stage").GetValue(object2, null)))
                                            : "Un-Assigned"
                                         )
                                         + " to " +
                                         (
                                           (v.Value1 != null) ?
                                           ((string)(object1.GetType().GetProperty("Stage").GetValue(object1, null)))
                                            : "Un-Assigned"
                                         ));
                        }
                        //else if (v.PropertyName == "Contacts")
                        //{
                        //    foreach (var contact in contacts.Where(i => !preContacts.Contains(i)))
                        //    {
                        //        logMessage.Append(" added contact " +
                        //                 (
                        //                    (contact.Name != null) ? contact.Name : ""
                        //                 ));
                        //    }
                        //    foreach (var contact in preContacts.Where(i => !contacts.Contains(i)))
                        //    {
                        //        logMessage.Append(" removed contact " +
                        //                 (
                        //                    (contact.Name != null) ? contact.Name : ""
                        //                 ));
                        //    }                            
                        //}
                    }

                    if (object1.GetType().GetProperty("CompanyId").GetValue(object1, null) != null)
                        companyId = Convert.ToInt64(object1.GetType().GetProperty("CompanyId").GetValue(object1, null));


                }

                if (!string.IsNullOrEmpty(logMessage.ToString()))
                {
                    SalesMarketingDA da = new SalesMarketingDA();
                    SMLogKeepingBO log = new SMLogKeepingBO();

                    if (logMessage.ToString().Trim().Length > 0 && (logMessage.ToString().Trim().Length != userInformation.UserName.Trim().Length))
                    {
                        if (dealId > 0)
                        {
                            
                            log.Type = logType.ToString();
                            log.Title = CommonHelper.SentenceCaseConvertionToUpperCase(logType.ToString());
                            log.Description = logMessage.ToString();
                            log.LogDateTime = DateTime.Now;
                            log.CompanyId = (int)companyId;

                            log.DealId = dealId;
                            log.SalesCallEntryId = salesCallEntryId;
                            log.CreatedBy = userInformation.UserInfoId;

                            status = da.SaveLogKeeping(log, out LogKeepingId);
                            foreach (var item in participantFromClient)
                            {
                                status = da.SaveLogKeepingContacts(LogKeepingId, item.ContactId);
                            }

                        }
                        else
                        {
                            log.Type = logType.ToString();
                            log.Title = CommonHelper.SentenceCaseConvertionToUpperCase(logType.ToString());
                            log.Description = logMessage.ToString();
                            log.LogDateTime = DateTime.Now;
                            log.CompanyId = (int)companyId;
                            log.ContactId = contactId;
                            log.DealId = dealId;
                            log.SalesCallEntryId = salesCallEntryId;
                            log.CreatedBy = userInformation.UserInfoId;

                            status = da.SaveLogKeeping(log, out LogKeepingId);
                            foreach (var item in participantFromClient)
                            {
                                status = da.SaveLogKeepingContacts(LogKeepingId, item.ContactId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        //public bool FrontOfficeLog(FrontOfficeLogActivityType logType, T object1, T object2, long activityId)
        //{
        //    bool status = false;
        //    List<ClassPropertyVeriance> variances = new List<ClassPropertyVeriance>();
        //    long reservationId;
        //    long tempId;
        //    HMUtility hmUtility = new HMUtility();
        //    ActivityLogsDA activityLogsDA = new ActivityLogsDA();
        //    UserInformationBO userInformation = new UserInformationBO();
        //    userInformation = hmUtility.GetCurrentApplicationUserInfo();
        //    if (FrontOfficeLogActivityType.ReservationCreated == logType)
        //    {
        //        reservationId = Convert.ToInt64(object1.GetType().GetProperty("ReservationId").GetValue(object1, null));
        //        //ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();

        //        //activityLog.ActivityId = activityId;
        //        //activityLog.FieldName = v.PropertyName;
        //        //activityLog.PreviousData = (string)(object1.GetType().GetProperty(v.PropertyName).GetValue(object1, null));
        //        //activityLog.CurrentData = (string)(object2.GetType().GetProperty(v.PropertyName).GetValue(object2, null));
        //        //activityLog.DetailDescription = "";

        //        //activityLogsDA.SaveActivityLogDetails(activityLog, out tempId);
        //    }
        //    else if (FrontOfficeLogActivityType.ReservationActivity == logType)
        //    {
        //        variances = object1.CompareTwoCalssPropertyFindDifference(object2);
        //        List<ActivityLogDetailsBO> fields = new List<ActivityLogDetailsBO>();
        //        //string[] cols = { "DateIn", "DateOut"};
        //        //Enum form = FOActivityLogFormName.Reservation;
        //        fields = activityLogsDA.GetSelectedFieldsByFormName(FOActivityLogFormName.Reservation.ToString());
        //        foreach (ClassPropertyVeriance v in variances)
        //        {
        //            foreach (var item in fields)
        //            {
        //                if (v.PropertyName == item.FieldName)
        //                {
        //                    ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();

        //                    activityLog.ActivityId = activityId;
        //                    activityLog.FieldName = v.PropertyName;
        //                    activityLog.PreviousData = "" + (object1.GetType().GetProperty(v.PropertyName).GetValue(object1, null));
        //                    activityLog.CurrentData = "" + (object2.GetType().GetProperty(v.PropertyName).GetValue(object2, null));
        //                    activityLog.DetailDescription = "";

        //                    status = activityLogsDA.SaveActivityLogDetails(activityLog, out tempId);
        //                }
        //            }
        //        }
        //    }
        //    else if (FrontOfficeLogActivityType.RegistrationActivity == logType)
        //    {
        //        variances = object1.CompareTwoCalssPropertyFindDifference(object2);
        //        List<ActivityLogDetailsBO> fields = new List<ActivityLogDetailsBO>();
        //        fields = activityLogsDA.GetSelectedFieldsByFormName(FOActivityLogFormName.Registration.ToString());
        //        foreach (ClassPropertyVeriance v in variances)
        //        {
        //            foreach (var item in fields)
        //            {
        //                if (v.PropertyName == item.FieldName)
        //                {
        //                    ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();

        //                    activityLog.ActivityId = activityId;
        //                    activityLog.FieldName = v.PropertyName;
        //                    activityLog.PreviousData = "" + (object1.GetType().GetProperty(v.PropertyName).GetValue(object1, null));
        //                    activityLog.CurrentData = "" + (object2.GetType().GetProperty(v.PropertyName).GetValue(object2, null));
        //                    activityLog.DetailDescription = "";

        //                    status = activityLogsDA.SaveActivityLogDetails(activityLog, out tempId);
        //                }
        //            }
        //        }
        //    }
        //    else if (FrontOfficeLogActivityType.ServiceBillActivity == logType)
        //    {
        //        variances = object1.CompareTwoCalssPropertyFindDifference(object2);
        //        List<ActivityLogDetailsBO> fields = new List<ActivityLogDetailsBO>();
        //        fields = activityLogsDA.GetSelectedFieldsByFormName(FOActivityLogFormName.ServiceBill.ToString());
        //        foreach (ClassPropertyVeriance v in variances)
        //        {
        //            foreach (var item in fields)
        //            {
        //                if (v.PropertyName == item.FieldName)
        //                {
        //                    ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();

        //                    activityLog.ActivityId = activityId;
        //                    activityLog.FieldName = v.PropertyName;
        //                    activityLog.PreviousData = "" + (object1.GetType().GetProperty(v.PropertyName).GetValue(object1, null));
        //                    activityLog.CurrentData = "" + (object2.GetType().GetProperty(v.PropertyName).GetValue(object2, null));
        //                    activityLog.DetailDescription = "";

        //                    status = activityLogsDA.SaveActivityLogDetails(activityLog, out tempId);
        //                }
        //            }
        //        }
        //    }
        //    return status;
        //}
        public bool LogDetails(FOActivityLogFormName formName, T previous, T current, long activityId)
        {
            bool status = false;
            List<ClassPropertyVeriance> variances = new List<ClassPropertyVeriance>();
            long tempId;
            HMUtility hmUtility = new HMUtility();
            ActivityLogsDA activityLogsDA = new ActivityLogsDA();
            UserInformationBO userInformation = new UserInformationBO();
            userInformation = hmUtility.GetCurrentApplicationUserInfo();

            variances = previous.CompareTwoCalssPropertyFindDifference(current);
            List<ActivityLogDetailsBO> fields = new List<ActivityLogDetailsBO>();
            //string[] cols = { "DateIn", "DateOut"};
            //Enum form = FOActivityLogFormName.Reservation;
            fields = activityLogsDA.GetSelectedFieldsByFormName(formName.ToString());
            foreach (ClassPropertyVeriance v in variances)
            {
                foreach (var item in fields)
                {
                    if (v.PropertyName == item.FieldName)
                    {
                        ActivityLogDetailsBO activityLog = new ActivityLogDetailsBO();

                        activityLog.ActivityId = activityId;
                        activityLog.FieldName = v.PropertyName;
                        activityLog.PreviousData = "" + (previous.GetType().GetProperty(v.PropertyName).GetValue(previous, null));
                        activityLog.CurrentData = "" + (current.GetType().GetProperty(v.PropertyName).GetValue(current, null));
                        activityLog.DetailDescription = "";

                        status = activityLogsDA.SaveActivityLogDetails(activityLog, out tempId);
                    }
                }
            }

            return status;
        }
    }
}