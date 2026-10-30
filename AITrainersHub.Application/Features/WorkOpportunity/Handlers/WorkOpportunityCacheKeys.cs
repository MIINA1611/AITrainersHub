using AITrainersHub.Application.Features.WorkOpportunity.Queries;

namespace AITrainersHub.Application.Features.WorkOpportunity.Handlers
{
    /// <summary>
    /// Centralizes the cache keys invalidated by work opportunity commands.
    /// </summary>
    internal static class WorkOpportunityCacheKeys
    {
        public static void AddListCacheKeys(HashSet<string> cacheKeys, Guid leaderId)
        {
            cacheKeys.Add(nameof(ReadWorkOpportunitiesQuery));
            cacheKeys.Add(nameof(ReadPendingWorkOpportunitiesQuery));

            AddFilterVariants(cacheKeys, "all");
            AddFilterVariants(cacheKeys, leaderId.ToString());
        }

        private static void AddFilterVariants(HashSet<string> cacheKeys, string leader)
        {
            foreach (var isApproved in new[] { "all", "True", "False" })
            {
                foreach (var isActive in new[] { "all", "True", "False" })
                {
                    cacheKeys.Add($"{nameof(ReadWorkOpportunitiesQuery)}_{leader}_{isApproved}_{isActive}");
                }
            }
        }
    }
}
