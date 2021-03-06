using Cosmos.SDK.Protos.Staking;
using Google.Protobuf;
using System.Text.Json;

namespace Terra.NET.API.Serialization.Json.Messages.Staking
{
    [MessageDescriptor(TerraType = TERRA_DESCRIPTOR, CosmosType = COSMOS_DESCRIPTOR)]
    internal record MessageDelegate(string DelegatorAddress, string ValidatorAddress, DenomAmount Amount)
        : Message(TERRA_DESCRIPTOR, COSMOS_DESCRIPTOR)
    {
        public const string TERRA_DESCRIPTOR = "staking/MsgDelegate";
        public const string COSMOS_DESCRIPTOR = "/cosmos.staking.v1beta1.MsgDelegate";

        internal override NET.Message ToModel()
        {
            return new NET.Messages.Staking.MessageDelegate(this.DelegatorAddress, this.ValidatorAddress, this.Amount.ToModel());
        }

        internal override IMessage ToProto(JsonSerializerOptions? serializerOptions = null)
        {
            var undelegate = new MsgDelegate
            {
                DelegatorAddress = this.DelegatorAddress,
                ValidatorAddress = this.ValidatorAddress,
                Amount = this.Amount.ToProto()
            };

            return undelegate;
        }
    }
}
