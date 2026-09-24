using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public interface INotificationService
    {
        void NotifyCandidate(int applicationId);
    }
}
