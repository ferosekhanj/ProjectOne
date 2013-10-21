using Mvc.Mailer;

namespace ProjectOne.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage ForgotPassword(string theEmail, string theToken)
		{
			ViewBag.Token = theToken;
			return Populate(x =>
			{
				x.Subject = "Reset your password";
				x.ViewName = "ForgotPassword";
                x.To.Add(theEmail);
			});
		}

        public virtual MvcMailMessage Welcome(string theName, string theEmail)
        {
            ViewBag.Name = theName;
            return Populate(x =>
            {
                x.Subject = "Welcome to mysalah";
                x.ViewName = "Welcome";
                x.To.Add(theEmail);
            });
        }
 	}
}