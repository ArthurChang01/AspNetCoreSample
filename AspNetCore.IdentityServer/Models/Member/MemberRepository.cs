using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.IdentityServer.Models.Member
{
    public class MemberRepository : IMemberRepository
    {
        private MemberDbContext _ctx;
        private DbSet<Account> _account;

        public MemberRepository(MemberDbContext ctx)
        {
            this._ctx = ctx;
            this._account = this._ctx.Set<Account>();
        }

        public async Task<Account> LogIn(string account, string password)
        {
            Account act = null;

            act = await _account.Include(o => o.User).FirstOrDefaultAsync<Account>(o => o.Email.Equals(account) && o.Password.Equals(password));

            return act;
        }
    }
}