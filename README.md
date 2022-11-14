# FibonacciApi

This project creates the backend web API for requesting a fibonacci number, and for seeing previous requests. The WebAPI is made using ASP.NET Core. Use [https://fibonacciapi20221114115416.azurewebsites.net/api/](https://fibonacciapi20221114115416.azurewebsites.net/api/) for a quick demo of the api.

Available endpoints are:
* /api/Fibonacci?n={n}      
    Request the nth Fibonacci number
* /api/Fibonacci/History
    Request history of all previous Fibonacci requests
* /api/Fibonacci/History/{pageSize}/{startFromId?}
    Request history of a number (pageSize) of previous Fibonacci requests, starting at startFromId or from first if startFromId is left empty.

## overview

A default ASP WebAPI controller is used for creating the endpoints. The Fibonacci number is calculated using the Fast Doubling algorithm. BigInteger is used to easily convert the resulting Fibonacci number to a string (and because Fibonacci numbers get big fast). The history can be acquired all at once, or on a per page basis to avoid getting too much traffic at once.

The controller uses a FibonacciRequestRepository. The repository pattern is used to leverage advantages of DI/IoC. Using the Repository pattern we can also easily implement Unit of Work.

From the FibonacciRequestRepository, a code-first EntityFramework database is edited. Currently an in-memory EF database is used, for production a proper database would need to be added (piece of cake using EF).

wwwroot is currenty filled with the web build outputs of the [fibonacci_frontend](https://github.com/EniacMlezi/fibonacci_frontend) application. The front-end and back-end should be hosted seperately. But for simplicity's sake in this case an exception is made.
