using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoAndAudioDownloader.BusinessLogic.Configurations;

namespace VideoAndAudioDownloader.BusinessLogic.Services
{
    public class HostedService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<HostedService> _logger;
        private readonly HostedServiceSettings _hostedServiceSettings
            ;

        public HostedService(IServiceProvider provider,ILogger<HostedService> logger,IOptions<HostedServiceSettings> options)
        {
            _provider = provider;
            _logger = logger;
            _hostedServiceSettings = options.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //TODO print every download
                await Task.Delay(_hostedServiceSettings.TimeDelay,stoppingToken);
            }
        }
    }
}
