using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RealEstateCRM
{
    public class GlobalData
    {
        public void SendGreetingsSMS(string mobile, string message)
        {
            string sender = WebConfigurationManager.AppSettings["SmsSender"];
            string SMSAPI = WebConfigurationManager.AppSettings["SMSAPI"];
            string sURL = "http://www.bulksmsapps.com/api/apismsv2.aspx?apikey=" + SMSAPI + "&sender=" + sender + "&number=" + mobile + "&message=" + message + "";
            string sResponse = GetResponse(sURL);
        }
        public void SendPlotBookingSMS(string mobile, string plotno)
        {
            string message = "Congratulations! Your plot " + plotno + " as been confirmed successfully - Vaishnavi Developers";
            string sender = WebConfigurationManager.AppSettings["SmsSender"];
            string SMSAPI = WebConfigurationManager.AppSettings["SMSAPI"];
            string sURL = "http://www.bulksmsapps.com/api/apismsv2.aspx?apikey=" + SMSAPI + "&sender=" + sender + "&number=" + mobile + "&message=" + message + "";
            //string sResponse = GetResponse(sURL);
            SendTwiliosms(message, mobile);
        }
        public void SendPaymentSMSToCustomer(string mobile, string payment, string plotno)
        {
            string message = "Dear Customer, Payment of " + payment + " as been received successfully for your plot " + plotno + "- Vaishnavi Developers";
            string sender = WebConfigurationManager.AppSettings["SmsSender"];
            string SMSAPI = WebConfigurationManager.AppSettings["SMSAPI"];
            string sURL = "http://www.bulksmsapps.com/api/apismsv2.aspx?apikey=" + SMSAPI + "&sender=" + sender + "&number=" + mobile + "&message=" + message + "";
            //string sResponse = GetResponse(sURL);
            SendTwiliosms(message, mobile);
        }
        public void SendPaymentSMSToMD(string mobile, string payment, string plotno)
        {
            string message = "Payment of " + payment + " as been received successfully for the plot " + plotno + "- Vaishnavi Developers";
            string sender = WebConfigurationManager.AppSettings["SmsSender"];
            string SMSAPI = WebConfigurationManager.AppSettings["SMSAPI"];
            string sURL = "http://www.bulksmsapps.com/api/apismsv2.aspx?apikey=" + SMSAPI + "&sender=" + sender + "&number=" + mobile + "&message=" + message + "";
            //string sResponse = GetResponse(sURL);
            SendTwiliosms(message, mobile);
        }
        public static string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL); request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8); string sResponse = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return sResponse;
        }
        public void TestSms()
        {
            SendTwiliosms("Payment of as been received successfully for the plot 101 - Vaishnavi Developers", "9985340876");
            //SendTwilioWhatsApp("Hello", "9985340876");
        }

        private void SendTwilioWhatsApp(string messagetext, string mobile)
        {
            string accountSid = "AC4a7da49579404f61acc192ddebc02f0a";
            string authToken = "a3f3ed28c5b48c5238dc2681cb2400a5";
            string senderno = "whatsapp:+12296584573";
            string mobileno = "whatsapp:+91" + mobile;
            TwilioClient.Init(accountSid, authToken);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
            var message = MessageResource.Create(
                body: messagetext,
                from: new Twilio.Types.PhoneNumber(senderno),
                to: new Twilio.Types.PhoneNumber(mobileno)
            );
        }

        private void SendTwiliosms(string messagetext, string mobile)
        {
            string accountSid = "AC4a7da49579404f61acc192ddebc02f0a";
            string authToken = "a3f3ed28c5b48c5238dc2681cb2400a5";
            string senderno = "+12296584573";
            string mobileno = "+91" + mobile;
            TwilioClient.Init(accountSid, authToken);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;
            var message = MessageResource.Create(
                body: messagetext,
                from: new Twilio.Types.PhoneNumber(senderno),
                to: new Twilio.Types.PhoneNumber(mobileno)
            );
        }
    }
}