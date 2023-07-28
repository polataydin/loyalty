using Loyalty.Plugins._Models.CustomerCard;
using Loyalty.Plugins._Repository;
using Loyalty.Plugins.Common;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins._Business
{
    public class CustomerCardBusiness : BusinessHandler
    {
        public CustomerCardBusiness(IOrganizationService Service, ITracingService TracingService) : base(Service, TracingService)
        {
        }
        public Guid createCustomerDefaultCard(Guid contactId)
        {
            Entity e = new Entity("plt_customercard");
            e["plt_contactid"] = new EntityReference("contact", contactId);          

            EnvironmentVariableRepository environmentVariableRepository = new EnvironmentVariableRepository(this.Service, this.TracingService);
            var variable = environmentVariableRepository.getEnvironmentVariable("plt_defaultcardtype");

            this.trace(string.Format("variable {0}", variable));
            e["plt_cardtypeid"] = new EntityReference("plt_cardtypes", new Guid(variable));

            var rng = new Random();
            ulong randomNumber = rng.NextULong(1000000000000, 9999999999999999);

            e["plt_name"] = randomNumber.ToString();
            e["plt_cardnumber"] = randomNumber.ToString();

            return Service.Create(e);
        }
        public Guid createCustomerCard(CreateCustomerCardRequest createCustomerCardRequest)
        {
            Entity e = new Entity("plt_customercard");
            e["plt_contactid"] = new EntityReference("contact", createCustomerCardRequest.contactId);           
            e["plt_cardtypeid"] = new EntityReference("plt_cardtypes", createCustomerCardRequest.cardTypeId);
            e["plt_name"] = createCustomerCardRequest.cardNumber;
            e["plt_cardnumber"] = createCustomerCardRequest.cardNumber;

            return Service.Create(e);
        }
        public void deactivateExistingCard(Guid cardId)
        {
            new XrmHelper(this.Service).setState(new EntityReference { LogicalName = "plt_customercard", Id = cardId }, 1, 2);
        }
    }
}
