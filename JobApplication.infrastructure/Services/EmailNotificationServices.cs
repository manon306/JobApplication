using JobApplication.Application.interfaces;
using JobApplication.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.infrastructure.Services
{
    public class EmailNotificationServices : INotificationService
    {
        private readonly IApplicationRepo applicationRepo;
        private readonly ILogger<EmailNotificationServices> logger;

        public EmailNotificationServices(IApplicationRepo applicationRepo, ILogger<EmailNotificationServices> logger)
        {
            this.applicationRepo = applicationRepo;
            this.logger = logger;
        }

        public void NotifyCandidate(int candidateId)
        {
            //var application = applicationRepo.Get().FirstOrDefault(a => a.Id == applicationId);

            //if (application is null)
            //{
            //    logger.LogWarning("application {applicationId}is not found ", applicationId);
            //    return;
            //}
            logger.LogInformation("Send Email :  cadidate {CandidateId} has applied to {JobId} and applicationId is {applicationId}"
                );
            
        }
    }
}
