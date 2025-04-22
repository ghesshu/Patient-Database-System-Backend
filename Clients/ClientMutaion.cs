using Axon_Job_App.Common;
using Axon_Job_App.Features.Clients;
using Cai;

namespace Axon_Job_App.Features.Clients;

public class ClientMutation
{
    [Mutation<CallResult<ClientResponse>>]
    public record CreateClient(CreateClientRequest Input);

    [Mutation<CallResult<ClientResponse>>]
    public record UpdateClient(long Id, UpdateClientRequest Input);

    [Mutation<CallResult>]
    public record DeleteClient(long Id);

    [Mutation<CallResult>]
    public record ClientVStatus(long Id, VerificationStatus Status);
}