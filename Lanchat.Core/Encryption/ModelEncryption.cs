using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lanchat.Core.Extensions;

namespace Lanchat.Core.Encryption
{
    internal class ModelEncryption : IModelEncryption
    {
        private readonly INodeAes nodeAes;

        public ModelEncryption(INodeAes nodeAes)
        {
            this.nodeAes = nodeAes;
        }

        public void EncryptObject(object data)
        {
            var props = GetPropertiesWithEncryptAttribute(data);
            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, nodeAes.EncryptString(value), null);
            });
        }

        public void DecryptObject(object data)
        {
            var props = GetPropertiesWithEncryptAttribute(data);
            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, nodeAes.DecryptString(value), null);
            });
        }

        private static IEnumerable<PropertyInfo> GetPropertiesWithEncryptAttribute(object data)
        {
            var props = data
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(EncryptAttribute)));
            return props;
        }
    }
}