using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace Loyalty.Plugins._Repository
{
    public class CardTypeRepository : RepositoryHandler
    {
        public CardTypeRepository(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }
        public Entity getCardTypeByPoint(decimal customerPoint)
        {
            trace(Convert.ToInt32(customerPoint).ToString());
            
            QueryExpression queryExpression = new QueryExpression("plt_cardtypes");
            queryExpression.ColumnSet = new ColumnSet(true);
            queryExpression.Criteria.AddCondition("plt_minimumpoints", ConditionOperator.LessEqual, Convert.ToInt32(customerPoint));
            queryExpression.Criteria.AddCondition("plt_maximumpoints", ConditionOperator.GreaterEqual, Convert.ToInt32(customerPoint));
            queryExpression.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            return Service.RetrieveMultiple(queryExpression).Entities.FirstOrDefault();
        }
    }
}
