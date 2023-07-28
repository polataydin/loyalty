using Loyalty.Plugins.Common;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins
{
    public class ExecuteCalculateRollupFields_PostCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginInitializer pluginInitializer = new PluginInitializer(serviceProvider);
            XrmHelper xrmHelper = new XrmHelper(pluginInitializer.Service);

            pluginInitializer.PluginContext.GetContextInputEntity<Entity>(pluginInitializer.TargetKey, out Entity Entity);

            xrmHelper.CalculateRollupField(Entity.GetAttributeValue<EntityReference>("plt_contactid").LogicalName,
                                           Entity.GetAttributeValue<EntityReference>("plt_contactid").Id,
                                           "plt_earnedamount");


            xrmHelper.CalculateRollupField(Entity.GetAttributeValue<EntityReference>("plt_contactid").LogicalName,
                                           Entity.GetAttributeValue<EntityReference>("plt_contactid").Id,
                                           "plt_redeemamount");
           
        }
    }
}
