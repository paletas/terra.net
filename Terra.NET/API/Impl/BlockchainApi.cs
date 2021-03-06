using Microsoft.Extensions.Logging;
using Terra.NET.API.Serialization.Json.Responses;

namespace Terra.NET.API.Impl
{
    internal class BlockchainApi : BaseApiSection, IBlockchainApi
    {
        public BlockchainApi(TerraApiOptions options, HttpClient httpClient, ILogger<BlockchainApi> logger) : base(options, httpClient, logger)
        {
        }

        public async Task<IReadOnlyDictionary<string, decimal>> GetGasPrices(CancellationToken cancellationToken = default)
        {
            return await base.Get<Dictionary<string, decimal>>("/v1/txs/gas_prices", cancellationToken).ConfigureAwait(false) ?? new Dictionary<string, decimal>();
        }

        public async Task<DenomMetadata[]> GetNativeDenoms(CancellationToken cancellationToken = default)
        {
            var metadataDenoms = await Get<DenomMetadataResponse>("/cosmos/bank/v1beta1/denoms_metadata", cancellationToken).ConfigureAwait(false);
            if (metadataDenoms == null) throw new InvalidOperationException();
            return metadataDenoms.Metadatas.Select(den =>
                new DenomMetadata(
                    den.Description,
                    den.Units.Select(denu => new DenomUnit(denu.Denom, denu.Decimals, denu.Aliases)).ToArray(),
                    den.BaseDenom,
                    den.DisplayDenom,
                    den.Name,
                    den.Symbol
                )
            ).ToArray();
        }

        public async Task<DenomSwapRate> GetSwapRate(string denomFrom, string denomTo, CancellationToken cancellationToken = default)
        {
            var swapRate = await Get<DenomSwapRate[]>($"/v1/market/swaprate/{denomFrom}", cancellationToken).ConfigureAwait(false);
            if (swapRate == null) throw new InvalidOperationException();
            return swapRate.Single(rate => rate.Denom == denomTo);
        }

        public async Task<DenomSwapRate[]> GetSwapRates(string denomFrom, CancellationToken cancellationToken = default)
        {
            var swapRates = await Get<DenomSwapRate[]>($"/terra/oracle/v1beta1/denoms/{denomFrom}/exchange_rate", cancellationToken).ConfigureAwait(false);
            if (swapRates == null) throw new InvalidOperationException();
            return swapRates;
        }

        public async Task<Coin> SimulateSwap(string denomFrom, ulong amountFrom, string denomTo, CancellationToken cancellationToken = default)
        {
            var swapSimulation = await Get<SimulateMarketSwapResponse>($"/terra/market/v1beta1/swap?offer_coin={amountFrom}{denomFrom}&ask_denom={denomTo}", cancellationToken).ConfigureAwait(false);
            if (swapSimulation == null) throw new InvalidOperationException();
            return new NativeCoin(swapSimulation.Result.Denom, swapSimulation.Result.Amount);
        }
    }
}
