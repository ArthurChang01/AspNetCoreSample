﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.Models.Member
{
    public class Account
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public User User { get; set; }
    }
}