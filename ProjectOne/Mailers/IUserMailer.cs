using Mvc.Mailer;

namespace ProjectOne.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage ForgotPassword(string theEmail, string theToken);
            MvcMailMessage Welcome(string theName, string theEmail);
	}
}