﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IAuthRepository
    {
        Task<AuthState> AuthorizeUser(string login, string password);
    }
}
