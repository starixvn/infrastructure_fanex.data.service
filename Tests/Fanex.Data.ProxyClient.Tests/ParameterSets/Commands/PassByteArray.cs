using Fanex.Data.Repository;

namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Commands
{
    public class PassByteArray : NonQueryCommand
    {
        public int ImageId { get; set; }

        [SpParam(IsSpecialType = true)]
        public byte[] Image { get; set; }

        public byte[] Response { get; set; }

        public override string GetSettingKey() => "TestByteArray";

        public override bool IsValid() => true;
    }
}
