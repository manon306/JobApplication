using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.interfaces
{
    public interface IBackgroundJobScheduler
    {
        public void Enqueue<T>(Expression<Action<T>> methodCall);
        public void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    }
}
