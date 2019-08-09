using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using InterConnect.MailApi;

/// <summary>
/// Summary description for SendMail
/// </summary>
public class SendMail
{

	public SendMail()
	{
	}


    internal static void sendUserCredentialsToTheirEmail(string UserName, string Password, string FirstName, string LastName, string phone, string email, string vendorCode, string userRole)
    {

        if (!string.IsNullOrEmpty(email))
        {

            string Subject = "SMS PORTAL USER CREDENTIALS";
            string Message = "Dear " + FirstName + " " + LastName + ", <br/>" +
                             "Your " + vendorCode + " USER Account has been created. Below are the details to access the " + vendorCode + " Online SMS Web Portal.<br/>" +
                             "URL: " + Credentials.APP_URL + "<br/>" +
                             "Username: " + UserName + "<br/>" +
                             "Password: " + Password + "<br/>" +
                             "Thank you.<br/><br/>";

            sendEmailNoAttachment(vendorCode, FirstName, email, Subject, Message);

        }


    }

    public static void sendEmailNoAttachment(String bankCode, String name, string email, string Subject, string Message)
    {

        Email mail = new InterConnect.MailApi.Email();
        EmailAddress addr = new EmailAddress();
        addr.Address = email;
        addr.Name = name;
        addr.AddressType = EmailAddressType.To;

        EmailAddress[] addresses = { addr };
        mail.MailAddresses = addresses;
        mail.From = bankCode;
        mail.Message = Message;
        mail.Subject = Subject;


        Messenger mapi = new Messenger();

        System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
        Result result = mapi.PostEmail(mail);

    }

    public static bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }




}