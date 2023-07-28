using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyalty.Plugins.Common
{
    public class PluginInitializer
    {
        public ITracingService TracingService { get; set; }
        public IOrganizationService Service { get; set; }
        public IPluginExecutionContext PluginContext { get; set; }

        public Guid PrimaryId { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public Guid InitiatingUserId { get; set; }
        public readonly String TargetKey = "Target";
        public readonly String PreImgKey = "PreImg";
        public readonly String PostImgKey = "PostImg";
        public readonly String RelationShipKey = "Relationship";
        public readonly String AssociateKey = "Associate";
        public readonly String DisAssociateKey = "Associate";
        public readonly String EntityId = "EntityId";
        public readonly String ListId = "ListId";
        public readonly String EntityName = "EntityName";
        public readonly String CampaignId = "CampaignId";


        public PluginInitializer(IServiceProvider serviceProvider, bool isTraceEnabled = true)
        {
            TracingService = serviceProvider.GetRelateObject<ITracingService>();
            PluginContext = serviceProvider.GetRelateObject<IPluginExecutionContext>();
            IOrganizationServiceFactory factory = serviceProvider.GetRelateObject<IOrganizationServiceFactory>();
            Service = factory.GetService<IOrganizationService>(this.PluginContext.UserId);
            InitiatingUserId = this.PluginContext.InitiatingUserId;
            if (isTraceEnabled) this.TraceMe("Current User : " + this.PluginContext.InitiatingUserId);
        }
        public PluginInitializer(IServiceProvider serviceProvider, string userId)
        {
            TracingService = serviceProvider.GetRelateObject<ITracingService>();
            PluginContext = serviceProvider.GetRelateObject<IPluginExecutionContext>();
            IOrganizationServiceFactory factory = serviceProvider.GetRelateObject<IOrganizationServiceFactory>();
            Service = factory.GetService<IOrganizationService>(new Guid(userId));
            InitiatingUserId = this.PluginContext.InitiatingUserId;
        }
        
        public void TraceMe(string traceText)
        {
            if (this.TracingService != null)
            {
                this.TracingService.Trace(DateTime.Now + " - " + traceText);
            }
        }
    }
    public static class PluginHelperStatics
    {
        public static readonly String errorKey = "CustomErrorMessagefinder: ";
        ///<summary>
        /// Get Entity Object In Plugin Context
        ///</summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextInputEntity(this IPluginExecutionContext context, string key, out Entity entityObject)
        {
            Boolean retVal = context.InputParameters != null && context.InputParameters.Contains(key);
            if (retVal)
                entityObject = (context.InputParameters[key] as Entity);
            else entityObject = null;

            return retVal;
        }
        ///<summary>
        /// Get Entity Object In Plugin Context
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextInputEntity<T>(this IPluginExecutionContext context, string key, out T entityObject) where T : Microsoft.Xrm.Sdk.Entity
        {
            Boolean retVal = context.InputParameters != null && context.InputParameters.Contains(key);
            if (retVal)
                entityObject = (context.InputParameters[key] as Entity).ToEntity<T>();
            else entityObject = null;

            return retVal;

        }

        public static Boolean GetContextParameter<T>(this IPluginExecutionContext context, string key, out T entityObject)
        {
            Boolean retVal = context.InputParameters != null && context.InputParameters.Contains(key);
            if (retVal)
                entityObject = (T)context.InputParameters[key];
            else entityObject = default(T);

            return retVal;

        }
        ///<summary>  
        /// Get Entity Object In Plugin Context
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextOutputEntity(this IPluginExecutionContext context, string key, out Entity entityObject)
        {

            Boolean retVal = context.OutputParameters != null && context.OutputParameters.Contains(key);
            if (retVal)
                entityObject = (context.OutputParameters[key] as Entity);
            else entityObject = null;

            return retVal;

        }
        ///<summary>
        /// Get Entity Object In Plugin Context 
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextOutputEntity<T>(this IPluginExecutionContext context, string key, out T entityObject) where T : Microsoft.Xrm.Sdk.Entity
        {
            Boolean retVal = context.OutputParameters != null && context.OutputParameters.Contains(key);
            if (retVal)
                entityObject = (context.OutputParameters[key] as Entity).ToEntity<T>();
            else entityObject = null;

            return retVal;

        }
        ///<summary>
        /// Get Object In ParameterCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextInputParameter<T>(this IPluginExecutionContext context, string key, out T parameterObject)
        {
            Boolean retVal = context.InputParameters != null && context.InputParameters.Contains(key);
            parameterObject = default(T);

            if (!retVal)
                return retVal;

            if (typeof(T).IsClass)
                parameterObject = (T)context.InputParameters[key];
            else
                parameterObject = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(context.InputParameters[key].ToString());

            return retVal;

        }
        ///<summary>
        /// Get Object In ParameterCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextOutputParameter<T>(this IPluginExecutionContext context, string key, out T parameterObject)
        {
            Boolean retVal = context.OutputParameters != null && context.OutputParameters.Contains(key);
            parameterObject = default(T);

            if (!retVal)
                return retVal;

            if (typeof(T).IsClass)
                parameterObject = (T)context.OutputParameters[key];
            else
                parameterObject = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(context.OutputParameters[key].ToString());

            return retVal;

        }
        ///<summary>
        /// Get Entity Object In ParameterCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextPreImages(this IPluginExecutionContext context, string key, out Entity entityObject)
        {
            Boolean retVal = context.PreEntityImages != null && context.PreEntityImages.Contains(key);
            if (retVal)
                entityObject = (context.PreEntityImages[key] as Entity);
            else entityObject = null;

            return retVal;

        }
        ///<summary>
        /// Get Entity Object In ImageCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:ProxyType entity generated by svcutil.exe.Inherits Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextPreImages<T>(this IPluginExecutionContext context, string key, out T entityObject) where T : Microsoft.Xrm.Sdk.Entity
        {
            Boolean retVal = context.PreEntityImages != null && context.PreEntityImages.Contains(key);
            if (retVal)
                entityObject = (context.PreEntityImages[key] as Entity).ToEntity<T>();
            else entityObject = null;

            return retVal;

        }
        ///<summary>
        /// Get Entity Object In ParameterCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextPostImages(this IPluginExecutionContext context, string key, out Entity entityObject)
        {
            Boolean retVal = context.PostEntityImages != null && context.PostEntityImages.Contains(key);
            if (retVal)
                entityObject = (context.PostEntityImages[key] as Entity);
            else entityObject = null;

            return retVal;

        }
        ///<summary>
        /// Get Entity Object In ImageCollection
        /// </summary>
        /// <param name="context">Execution context of the current plugin</param>
        /// <param name="key">parameter name</param>
        /// <param name="entityObject">Output parameter:ProxyType entity generated by svcutil.exe.Inherits Microsoft.Xrm.Sdk.Entity</param>
        /// <returns> if the entityobject with the "key" name exists,returns true and set the object,else returns false and set the object null.</returns> 
        public static Boolean GetContextPostImages<T>(this IPluginExecutionContext context, string key, out T entityObject) where T : Microsoft.Xrm.Sdk.Entity
        {
            Boolean retVal = context.PostEntityImages != null && context.PostEntityImages.Contains(key);
            if (retVal)
                entityObject = (context.PostEntityImages[key] as Entity).ToEntity<T>();
            else entityObject = null;

            return retVal;

        }

        public static void ThrowException<T>(this IPluginExecutionContext context, string Message) where T : Exception
        {
            HandleExceptions<T>(Message);
        }

        public static T GetRelateObject<T>(this IServiceProvider Provider)
        {
            Boolean retVal = Provider != null;
            if (retVal)
                return (T)Provider.GetService(typeof(T));
            else return default(T);
        }

   

        public static T GetService<T>(this IOrganizationServiceFactory Service, Guid UserId)
        {
            Boolean retVal = Service != null;
            if (retVal)
                return (T)Service.CreateOrganizationService(UserId);
            else return default(T);
        }

        public static void HandleExceptions<E>(string message) where E : Exception
        {
            message = message.Replace(errorKey, "");
            throw Activator.CreateInstance(typeof(E), errorKey + message) as E;
        }
    }
}
