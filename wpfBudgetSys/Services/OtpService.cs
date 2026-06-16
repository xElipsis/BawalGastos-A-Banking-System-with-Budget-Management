using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    public class OtpService
    {
        private readonly OtpRepository otpRepository = new();

        public void SendOtp(string email)
        {
            string otpCode = OtpGenerator.Generate();

            // Save to DB
            otpRepository.Insert(email, otpCode);

            // Send to email
            EmailHelper.SendOtp(email, otpCode);
        }

        public bool VerifyOtp(string email, string otpCode)
        {
            bool isValid = otpRepository.Verify(email, otpCode);

            if (isValid)
                otpRepository.MarkAsUsed(email, otpCode);

            return isValid;
        }
    }
}
