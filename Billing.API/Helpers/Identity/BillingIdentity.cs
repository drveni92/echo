using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Billing.API.Helpers.Identity
{
    public class BillingIdentity
    {
        private UnitOfWork _unitOfWork;

        private string[] roles = { "user", "admin" };

        public BillingIdentity(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BillingIdentity()
        {
            _unitOfWork = new UnitOfWork();
        }

        public CurrentUserModel CurrentUser
        {
            get
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                if (string.IsNullOrEmpty(username)) return null;
                Agent agent = _unitOfWork.Agents.Get().FirstOrDefault(x => x.Username == username);
                if (agent == null) return null;
                return new CurrentUserModel()
                {
                    Id = agent.Id,
                    Name = agent.Name,
                    Roles = GetRoles()
                };
            }
        }

        public List<string> GetRoles()
        {
            List<string> user_roles = new List<string>();
            foreach (string role in roles)
            {
                if (HasRole(role)) user_roles.Add(role);
            }
            return user_roles;
        }

        public bool HasRole(string role)
        {
            return Thread.CurrentPrincipal.IsInRole(role);
        }

        public bool HasNotAccess(int id)
        {
            return !(CurrentUser.Id == id || HasRole("admin"));
        }

    }
}