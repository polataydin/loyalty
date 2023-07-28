using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace Loyalty.Plugins._Repository
{
    public class EnvironmentVariableRepository : RepositoryHandler
    {
        public EnvironmentVariableRepository(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }
        public string getEnvironmentVariable(string key)
        {
            var query = new QueryExpression("environmentvariabledefinition")
            {
                ColumnSet = new ColumnSet("statecode", "defaultvalue", "valueschema", "schemaname", "environmentvariabledefinitionid", "type"),

                LinkEntities =
                        {
                            new LinkEntity
                            {
                                JoinOperator = JoinOperator.LeftOuter,
                                LinkFromEntityName = "environmentvariabledefinition",
                                LinkFromAttributeName = "environmentvariabledefinitionid",
                                LinkToEntityName = "environmentvariablevalue",
                                LinkToAttributeName = "environmentvariabledefinitionid",
                                Columns = new ColumnSet("statecode", "value", "environmentvariablevalueid"),
                                EntityAlias = "v"
                            }
                        }
            };
            query.Criteria.Conditions.Add(new ConditionExpression("schemaname", ConditionOperator.Equal, key));
            var result = Service.RetrieveMultiple(query).Entities.FirstOrDefault();


            var schemaName = result.GetAttributeValue<string>("schemaname");
            var value = result.GetAttributeValue<AliasedValue>("v.value")?.Value?.ToString();
            var defaultValue = result.GetAttributeValue<string>("defaultvalue");

            this.trace($"- schemaName:{schemaName}, value:{value}, defaultValue:{defaultValue}");

            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}
