﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISystemAccountService
    {
        Task<SystemAccount> Login(string email, string password);
    }
}
