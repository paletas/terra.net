using Google.Protobuf;
using System.Text.Json;

namespace Terra.NET.API.Serialization.Json.Messages.Ibc
{
    [MessageDescriptor(TerraType = TERRA_DESCRIPTOR, CosmosType = COSMOS_DESCRIPTOR)]
    internal record MessageAcknowledgement()
        : Message(TERRA_DESCRIPTOR, COSMOS_DESCRIPTOR)
    {
        public const string TERRA_DESCRIPTOR = "ibc/MsgAcknowledgement";
        public const string COSMOS_DESCRIPTOR = "/ibc.core.channel.v1.MsgAcknowledgement";

        internal override IMessage ToProto(JsonSerializerOptions? serializerOptions = null)
        {
            throw new NotImplementedException();
        }

        internal override NET.Message ToModel()
        {
            return new NET.Messages.Ibc.MessageAcknowledgement();
        }
    }
}
