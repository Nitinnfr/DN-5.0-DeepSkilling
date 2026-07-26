using System;

namespace CustomerCommLib
{
    public class CustomerComm
    {
        private readonly IMailSender _mailSender;
 
        public CustomerComm(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }
 
        public bool SendMailToCustomer()
        {
            string customerEmail = "cust123@abc.com";
            string messageBody = "Some Message";
            
            bool isSuccess = _mailSender.SendMail(customerEmail, messageBody);
            return isSuccess;
        }
    }
}