using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System.Linq;
using System.Threading;
using System.Web.Security;
using WebMatrix.WebData;

namespace Billing.API.Helpers.Identity
{
    public class BillingIdentity
    {
        private UnitOfWork _unitOfWork;

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

                if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);

                return new CurrentUserModel()
                {
                    Id = agent.Id,
                    Name = agent.Name,
                    Username = agent.Username,
                    Roles = Roles.GetRolesForUser(username).ToList()
                };
            }
        }

        public bool HasRole(string role)
        {
            return CurrentUser.Roles.Contains(role);
        }

        public bool HasNotAccess(int id)
        {
            return !(CurrentUser.Id == id || HasRole("admin"));
        }

    }
}