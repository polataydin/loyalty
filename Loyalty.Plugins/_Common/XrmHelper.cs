using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;

namespace Loyalty.Plugins.Common
{
    public class XrmHelper
    {
        public IOrganizationService Service { get; set; }
        public XrmHelper(IOrganizationService Service)
        {
            this.Service = Service;
        }
        public CalculateRollupFieldResponse CalculateRollupField(string entityLogicalName, Guid entityId, string fieldName)
        {
            CalculateRollupFieldRequest request = new CalculateRollupFieldRequest
            {
                Target = new EntityReference(entityLogicalName, entityId),
                FieldName = fieldName
            };

            return (CalculateRollupFieldResponse)Service.Execute(request);
        }
        public void setState(EntityReference entityReference, int state, int status)
        {
            SetStateRequest setStateRequest = new SetStateRequest
            {
                EntityMoniker = entityReference,
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };
            Service.Execute(setStateRequest);
        }
    }
    public static class XrmHelperStatics
    {
        public static ulong NextULong(this Random self, ulong min, ulong max)
        {
            // Get a random 64 bit number.

            var buf = new byte[sizeof(ulong)];
            self.NextBytes(buf);
            ulong n = BitConverter.ToUInt64(buf, 0);

            // Scale to between 0 inclusive and 1 exclusive; i.e. [0,1).

            double normalised = n / (ulong.MaxValue + 1.0);

            // Determine result by scaling range and adding minimum.

            double range = (double)max - min;

            return (ulong)(normalised * range) + min;
        }
        public static string Mask(this string source, int start, int maskLength, char maskCharacter)
        {
            if (start > source.Length - 1)
            {
                throw new ArgumentException("Start position is greater than string length");
            }

            if (maskLength > source.Length)
            {
                throw new ArgumentException("Mask length is greater than string length");
            }

            if (start + maskLength > source.Length)
            {
                throw new ArgumentException("Start position and mask length imply more characters than are present");
            }

            string mask = new string(maskCharacter, maskLength);
            string unMaskStart = source.Substring(0, start);
            string unMaskEnd = source.Substring(start + maskLength, source.Length - maskLength);

            return unMaskStart + mask + unMaskEnd;
        }
    }
}
