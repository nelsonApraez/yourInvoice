namespace yourInvoice.Common.Integration.Truora
{
    public interface ITruora
    {
        Task<Dictionary<string, string>> CreateApiKeyAsync(Guid generalInformationId);
        Task<ProcessInfoResponse> GetProcessAsync(string processId);
    }
}
