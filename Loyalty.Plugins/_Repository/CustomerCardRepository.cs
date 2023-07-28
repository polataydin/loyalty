using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Loyalty.Plugins._Repository
{
    public class CustomerCardRepository : RepositoryHandler
    {
        public CustomerCardRepository(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }

        public Entity getActiveCustomerCard(Guid contactId)
        {
            QueryExpression queryExpression = new QueryExpression("plt_customercard");
            queryExpression.ColumnSet = new ColumnSet(true);
            //todo 0 replace enum
            queryExpression.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            queryExpression.Criteria.AddCondition("plt_contactid", ConditionOperator.Equal, contactId);
            return Service.RetrieveMultiple(queryExpression).Entities.FirstOrDefault();
        }
    }
}
