﻿namespace OverviewServer.Services.Background.Workers
{
	public class WorkerService : BackgroundService
	{
		private readonly ILogger<WorkerService> _logger;

		public WorkerService(ILogger<WorkerService> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
				//Console.WriteLine("Worker service running");
			}
		}
	}
}
