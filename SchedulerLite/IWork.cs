using System;
using System.Threading.Tasks;

namespace SchedulerLite
{
    public interface IWork
    {
        Task ExecuteAsync();
        Task OnExceptionAsync(Exception ex);
    }
}
