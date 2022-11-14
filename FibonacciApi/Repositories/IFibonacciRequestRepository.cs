using FibonacciApi.Models;

namespace FibonacciApi.Repositories
{
    public interface IFibonacciRequestRepository
    {
        /// <summary>
        /// Add a request to the repository.
        /// </summary>
        /// <param name="request">The request to add.</param>
        void AddRequest(FibonacciRequest request);

        /// <summary>
        /// Gets all previous requests in the repository.
        /// </summary>
        /// <returns>A list of all previous requests.</returns>
        IEnumerable<FibonacciRequest> GetRequests();
        
        /// <summary>
        /// Gets a page of previous requests in the repository.
        /// </summary>
        /// <param name="pageSize">Number of requests to retrieve.</param>
        /// <param name="startFromId">The <see cref="FibonacciRequest"/> Id to start retrieving from. Leave null to start from first entry.</param>
        /// <returns>A list of <paramref name="pageSize"/> previous requests.</returns>
        IEnumerable<FibonacciRequest> GetRequestsPaginated(int pageSize, int? startFromId);
        
        /// <summary>
        /// Commit all repository changes to the database.
        /// </summary>
        void Save();
    }
}
