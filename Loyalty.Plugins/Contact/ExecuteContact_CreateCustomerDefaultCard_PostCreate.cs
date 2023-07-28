using Loyalty.Plugins._Business;
using Loyalty.Plugins.Common;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins.Contact
{
    public class ExecuteContact_CreateCustomerDefaultCard_PostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginInitializer pluginInitializer = new PluginInitializer(serviceProvider);
            pluginInitializer.PluginContext.GetContextInputEntity<Entity>(pluginInitializer.TargetKey, out Entity Entity);

            CustomerCardBusiness customerCardBusiness = new CustomerCardBusiness(pluginInitializer.Service, pluginInitializer.TracingService);
            ContactBusiness contactBusiness = new ContactBusiness(pluginInitializer.Service, pluginInitializer.TracingService);

            var res = customerCardBusiness.createCustomerDefaultCard(Entity.Id);
            contactBusiness.updateContactDefaultCardFields(Entity.Id, res);

        }
    }
}
