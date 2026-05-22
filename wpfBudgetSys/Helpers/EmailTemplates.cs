using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Helpers
{
    public static class EmailTemplates
    {
        public static string DepositConfirmation(string accountNumber, decimal amount, string referenceNumber)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: auto; padding: 20px;
                            border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #2e7d32;'>Deposit Successful</h2>
                    <p>Dear Customer,</p>
                    <p>
                        We are pleased to inform you that your deposit
                        transaction has been successfully processed.
                    </p>
                    <table style='width: 100%;
                                  border-collapse: collapse;
                                  margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Account Number:</td>
                            <td style='padding: 8px;'>{accountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Deposit Amount:</td>
                            <td style='padding: 8px;'>₱{amount:N2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transaction Date:</td>
                            <td style='padding: 8px;'>{DateTime.Now:MMMM dd, yyyy hh:mm tt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Reference Number:</td>
                            <td style='padding: 8px;'>{referenceNumber}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>Thank you for using our banking services.</p>
                    <p>
                        Regards,<br>
                        <strong>Your Banking System</strong>
                    </p>

                </div>
            </body>
            </html>";
        }

        public static string WithdrawalConfirmation(string accountNumber, decimal amount, string referenceNumber)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: auto; padding: 20px;
                            border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #c62828;'>Withdrawal Successful</h2>
                    <p>Dear Customer,</p>
                    <p>
                        We are writing to confirm that your withdrawal
                        transaction has been successfully processed.
                    </p>
                    <table style='width: 100%;
                                  border-collapse: collapse;
                                  margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Account Number:</td>
                            <td style='padding: 8px;'>{accountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Withdrawal Amount:</td>
                            <td style='padding: 8px;'>₱{amount:N2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transaction Date:</td>
                            <td style='padding: 8px;'>{DateTime.Now:MMMM dd, yyyy hh:mm tt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Reference Number:</td>
                            <td style='padding: 8px;'>{referenceNumber}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>
                        Thank you for using our banking services.
                    </p>
                    <p>
                        Regards,<br>
                        <strong>Your Banking System</strong>
                    </p>
                </div>
            </body>
            </html>";
        }

        public static string TransferConfirmation(string fromAccountNumber, string toAccountNumber, decimal amount, string referenceNumber)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: auto; padding: 20px;
                            border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #1565c0;'>Transfer Successful</h2>
                    <p>Dear Customer,</p>
                    <p>
                        We are pleased to inform you that your transfer
                        transaction has been successfully processed.
                    </p>
                    <table style='width: 100%;
                                  border-collapse: collapse;
                                  margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>From Account Number:</td>
                            <td style='padding: 8px;'>{fromAccountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>To Account Number:</td>
                            <td style='padding: 8px;'>{toAccountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transfer Amount:</td>
                            <td style='padding: 8px;'>₱{amount:N2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transaction Date:</td>
                            <td style='padding: 8px;'>{DateTime.Now:MMMM dd, yyyy hh:mm tt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Reference Number:</td>
                            <td style='padding: 8px;'>{referenceNumber}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>
                        Thank you for using our banking services.
                    </p>
                    <p>
                        Regards,<br>
                        <strong>Your Banking System</strong>
                    </p>
                </div>
            </body>
            </html>";
        }

        public static string ReceivedTransferNotification(string fromAccountNumber, string toAccountNumber, decimal amount, string referenceNumber)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: auto; padding: 20px;
                            border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #1565c0;'>Transfer Received</h2>
                    <p>Dear Customer,</p>
                    <p>
                        We are pleased to inform you that you have received a transfer.
                    </p>
                    <table style='width: 100%;
                                  border-collapse: collapse;
                                  margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>From Account Number:</td>
                            <td style='padding: 8px;'>{fromAccountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>To Account Number:</td>
                            <td style='padding: 8px;'>{toAccountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transfer Amount:</td>
                            <td style='padding: 8px;'>₱{amount:N2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transaction Date:</td>
                            <td style='padding: 8px;'>{DateTime.Now:MMMM dd, yyyy hh:mm tt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Reference Number:</td>
                            <td style='padding: 8px;'>{referenceNumber}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>
                        Thank you for using our banking services.
                    </p>
                    <p>
                        Regards,<br>
                        <strong>Your Banking System</strong>
                    </p>
                </div>
            </body>
            </html>";
        }

        public static string PaymentConfirmation(string accountNumber, string category, string payee, decimal amount, string referenceNumber)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <div style='max-width: 600px; margin: auto; padding: 20px;
                            border: 1px solid #ddd; border-radius: 10px;'>
                    <h2 style='color: #2e7d32;'>Payment Successful</h2>
                    <p>Dear Customer,</p>
                    <p>
                        We are pleased to inform you that your payment
                        transaction has been successfully processed.
                    </p>
                    <table style='width: 100%;
                                  border-collapse: collapse;
                                  margin-top: 20px;'>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Account Number:</td>
                            <td style='padding: 8px;'>{accountNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Category:</td>
                            <td style='padding: 8px;'>{category}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Payee:</td>
                            <td style='padding: 8px;'>{payee}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Payment Amount:</td>
                            <td style='padding: 8px;'>₱{amount:N2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Transaction Date:</td>
                            <td style='padding: 8px;'>{DateTime.Now:MMMM dd, yyyy hh:mm tt}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px; font-weight: bold;'>Reference Number:</td>
                            <td style='padding: 8px;'>{referenceNumber}</td>
                        </tr>
                    </table>
                    <p style='margin-top: 20px;'>Thank you for using our banking services.</p>
                    <p>
                        Regards,<br>
                        <strong>Your Banking System</strong>
                    </p>
                </div>
            </body>
            </html>";
        }
    }
}
