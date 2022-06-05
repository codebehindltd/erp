using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.IO;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Common
{
    public class EmailHelper
    {
        /// <summary>
        /// Send mail to multiple email addresses with attachment
        /// </summary>
        /// <param name="Emails">ex. abc@yourdomain.com,xyz@yourdomain.com</param>
        /// <param name="email">All propeties of Email</param>
        /// <param name="tokens">Parameters for Email Templete</param>
        /// <returns></returns>
        public static bool SendEmail(string Emails , Email email, Dictionary<string, string> tokens)
        {
            bool issent = false;
            foreach (string em in Emails.Split(new char[] { ',', ';' }))
            {
                if (EmailIsValid(em))
                {
                    email.To = em;
                    issent = SendEmail(email,tokens);
                }
                    
            }
            return issent;
        }
        public static bool EmailIsValid(string text)
        {
            string email = "";
            string strRegex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex re = new Regex(strRegex, RegexOptions.Multiline);
            foreach (Match m in re.Matches(text))
                email = m.ToString();
            if (email != "")
                return true;
            return false;
        }
        /// <summary>
        /// Send mail to single email addresses with attachment
        /// </summary>
        /// <param name="email">EmailView Object including all properties to send email</param>
        /// <param name="tokens">Parameters for Email Templete</param>
        /// <returns></returns>
        public static bool SendEmail(Email email, Dictionary<string, string> tokens)
        {
            MailAddress from = new MailAddress(email.From, email.FromDisplayName);
            MailAddress to = new MailAddress(email.To, email.ToDisplayName);
            MailMessage message = new MailMessage(email.From, email.To);

            if (!string.IsNullOrEmpty(email.CC))
                message.CC.Add(email.CC);

            if (!string.IsNullOrEmpty(email.BCC))
                message.Bcc.Add(email.BCC);

            message.Subject = email.Subject;
            if (email.Body == null)
                message.Body = EmailHelper.GetMailBody(email.TempleteName, tokens);
            else
                message.Body = email.Body;
            message.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(email.AttachmentSavedPath))
            {
                if (File.Exists(email.AttachmentSavedPath))
                {
                    Attachment attachFile = new Attachment(email.AttachmentSavedPath);
                    message.Attachments.Add(attachFile);
                }
            }

            var credentials = new NetworkCredential(email.From, email.Password);
            var client = new SmtpClient(email.Host)
            {
                Credentials = credentials,
                EnableSsl = true,
                Port = Convert.ToInt32(email.Port)
            };

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }
        /// <summary>
        /// Get Mail Body
        /// </summary>
        /// <param name="templateName">mail template name</param>
        /// <param name="tokens">Parameters for Email Templete</param>
        /// <returns></returns>
        public static string GetMailBody(string templateName, Dictionary<string, string> tokens)
        {
            string text = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Request.MapPath(string.Format("~/EmailTemplates/{0}", templateName)));
            return tokens.Aggregate(text, (current, token) => current.Replace(string.Format("##{0}##", token.Key), token.Value));
        }
    }
}