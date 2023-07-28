using Loyalty.Plugins._Business;
using Loyalty.Plugins.Common;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins.BonusTransaction
{
    public class ExecuteBonusTransaction_CalculateCustomerPoint_PostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginInitializer pluginInitializer = new PluginInitializer(serviceProvider);
            pluginInitializer.PluginContext.GetContextInputEntity<Entity>(pluginInitializer.TargetKey, out Entity Entity);


            ContactBusiness contactBusiness = new ContactBusiness(pluginInitializer.Service, pluginInitializer.TracingService);
            contactBusiness.updateCustomerCard(new _Models.Contact.UpdateCustomerCardRequest
            {
                contactId = Entity.GetAttributeValue<EntityReference>("plt_contactid").Id
            });
        }
    }
}
