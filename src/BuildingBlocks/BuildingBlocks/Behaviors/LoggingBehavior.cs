using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors
{
    public  class LoggingBehavior<IRequest, IResponse>
        (ILogger<LoggingBehavior<IRequest, IResponse>> logger)
        : IPipelineBehavior<IRequest, IResponse>
        where IRequest : notnull, IRequest<IResponse>
        where IResponse : notnull
    {
        public async Task<IResponse> Handle(IRequest request, RequestHandlerDelegate<IResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start] Handling request={Request} - Response: {Response} - RequestData={RequestData}", typeof(IRequest).Name, typeof(IRequest).Name, request);
            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.ElapsedMilliseconds;
            if (timeTaken > 500)
            {
                logger.LogWarning("Long Running Request: {Request} ({ElapsedTime}ms) {@Request}", typeof(IRequest).Name, timeTaken, request);
            }
            logger.LogInformation("[End] Handled request={Request} - Response: {Response} - RequestData={RequestData} - ElapsedTime={ElapsedTime}ms", typeof(IRequest).Name, typeof(IResponse).Name, request, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
