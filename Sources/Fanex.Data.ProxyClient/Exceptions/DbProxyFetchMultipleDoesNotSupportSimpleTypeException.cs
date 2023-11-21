namespace Fanex.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class DbProxyFetchMultipleDoesNotSupportSimpleTypeException : DbProxyException
    {
        public DbProxyFetchMultipleDoesNotSupportSimpleTypeException(string message)
            : base(message)
        {
        }

        internal static DbProxyFetchMultipleDoesNotSupportSimpleTypeException Create(
            Type type,
            string endpoint,
            string requestJson)
        {
            var messageHeader = $"Function FetchMultiple does not support simple type: {type.Name}";
            var message = $"{messageHeader}" + Environment.NewLine
                          + $"endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine
                          + $"requestJson: {ObscurePassword(requestJson)}" + Environment.NewLine
                          + "responseJson: Empty";

            return new DbProxyFetchMultipleDoesNotSupportSimpleTypeException(message);
        }

        private DbProxyFetchMultipleDoesNotSupportSimpleTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}