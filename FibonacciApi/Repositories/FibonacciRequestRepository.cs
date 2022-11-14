using FibonacciApi.Contexts;
using FibonacciApi.Models;

namespace FibonacciApi.Repositories
{
    /// <summary>
    /// Enables access to Fibonacci Request history database.
    /// </summary>
    public class FibonacciRequestRepository : IFibonacciRequestRepository, IDisposable
    {
        private readonly FibonacciRequestContext context;

        public FibonacciRequestRepository(FibonacciRequestContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public void AddRequest(FibonacciRequest request)
        {
            context.FibonacciRequests.Add(request);
        }

        /// <inheritdoc/>
        public IEnumerable<FibonacciRequest> GetRequests()
        {
            return context.FibonacciRequests.OrderByDescending(fr => fr.Time).ToArray();
        }

        /// <inheritdoc/>
        public IEnumerable<FibonacciRequest> GetRequestsPaginated(int pageSize, int? startFromId)
        {
            int takenCounter = 0;
            bool startFromIdEncountered = startFromId is null;  // If is null, start retrieving from first request.
            foreach (var request in GetRequests())
            {
                if (startFromIdEncountered) 
                {
                    yield return request;
                    if (++takenCounter >= pageSize)
                    {
                        yield break;    // Retrieved enough.
                    }
                }

                if (request.Id == startFromId) 
                {
                    startFromIdEncountered = true; // Found the requested Id, start retrieving.
                }
            }
            yield break; // Ran out of requests to retrieve.
        }

        /// <inheritdoc/>
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
