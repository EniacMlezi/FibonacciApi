using FibonacciApi.Models;
using FibonacciApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace FibonacciApi.Controllers
{
    /// <summary>
    /// Controller describing api/Fibonacci. Request new Fibonacci numbers and view old Fibonacci number requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly IFibonacciRequestRepository fibonacciHistoryRepository;

        public FibonacciController(IFibonacciRequestRepository fibonacciHistoryRepository)
        {
            this.fibonacciHistoryRepository = fibonacciHistoryRepository;
        }

        /// <summary>
        /// Gets the <paramref name="n"/>th Fibonacci number
        /// </summary>
        /// <param name="n">The index in the Fibonacci sequence to retrieve.</param>
        /// <returns>The <paramref name="n"/>th Fibonacci number.</returns>
        /// <exception cref="ArgumentException">Requests are limited to avoid large results.</exception>
        [HttpGet]
        public string Get(int n)
        {
            if (n < 0 || n > 10000)
            {
                throw new ArgumentException("Fibonacci requests must be >= 0 and are limited to 10000", nameof(n));
            }

            BigInteger result = Fibonacci(n);
            string fibonacciString = result.ToString();

            fibonacciHistoryRepository.AddRequest(new FibonacciRequest
            {
                Time = DateTimeOffset.UtcNow,
                Number = n,
                Result = fibonacciString
            });
            fibonacciHistoryRepository.Save();

            return fibonacciString;
        }

        // Fast doubling algorithm, https://www.nayuki.io/page/fast-fibonacci-algorithms
        private static BigInteger Fibonacci(int n)
        {
            BigInteger a = BigInteger.Zero;
            BigInteger b = BigInteger.One;
            for (int i = 31; i >= 0; i--)
            {
                BigInteger d = a * (b * 2 - a);
                BigInteger e = a * a + b * b;
                a = d;
                b = e;
                if ((((uint)n >> i) & 1) != 0)
                {
                    BigInteger c = a + b;
                    a = b;
                    b = c;
                }
            }
            return a;
        }

        /// <summary>
        /// Gets all previous <see cref="FibonacciRequest"/>s.
        /// </summary>
        /// <returns>A list of all previous requests.</returns>
        [HttpGet]
        [Route("History")]
        public IEnumerable<FibonacciRequest> GetHistory()
        {
            return fibonacciHistoryRepository.GetRequests();
        }

        /// <summary>
        /// Gets a page of previous <see cref="FibonacciRequest"/>s.
        /// </summary>
        /// <param name="pageSize">Number of requests to retrieve.</param>
        /// <param name="startFromId">The <see cref="FibonacciRequest"/> Id to start retrieving from. Leave null to start from first entry.</param>
        /// <returns>A list of <paramref name="pageSize"/> previous requests.</returns>
        /// <summary>
        [HttpGet]
        [Route("History/{pageSize}/{startFromId?}")]
        public IEnumerable<FibonacciRequest> GetHistory(int pageSize, int? startFromId)
        {
            return fibonacciHistoryRepository.GetRequestsPaginated(pageSize, startFromId);
        }
    }
}