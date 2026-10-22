using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyBook.Services.Core.Contracts
{
    public interface IAppointmentService
    {
        Task<bool> IsAvailableAsync(DateTime date, TimeSpan startTime, int serviceId);
    }
}
