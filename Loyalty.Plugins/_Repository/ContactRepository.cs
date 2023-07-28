using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Loyalty.Plugins._Repository
{
    public class ContactRepository : RepositoryHandler
    {
        public ContactRepository(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }
        public Entity getContactById(Guid contactId)
        {
            return Service.Retrieve("contact", contactId, new ColumnSet(true));
        }
        public Entity getContactById(Guid contactId, string[] columns)
        {
            return Service.Retrieve("contact", contactId, new ColumnSet(columns));
        }
    }
}
