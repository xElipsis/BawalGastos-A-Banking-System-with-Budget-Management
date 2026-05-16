using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Model
{
    internal class Login
    {
        private string? username;
        private DateTime lastlogin;
        private int failedattempts;
        private bool islocked;
        private DateTime lockeduntil;

        public Login(string username, DateTime lastlogin, int failedattempts, bool islocked, DateTime lockeduntil)
        {
            Username = username;
            LastLogin = lastlogin;
            FailedAttempts = failedattempts;
            IsLocked = islocked;
            LockedUntil = lockeduntil;
        }

        public Login(string username)
        {
            Username = username;
            FailedAttempts = 0;
            IsLocked = false;
        }

        public string Username { get => username; set => username = value; }
        public DateTime LastLogin { get => lastlogin; set => lastlogin = value; }
        public int FailedAttempts { get => failedattempts; set => failedattempts = value; }
        public bool IsLocked { get => islocked; set => islocked = value; }
        public DateTime LockedUntil { get => lockeduntil; set => lockeduntil = value; }
    }
}
