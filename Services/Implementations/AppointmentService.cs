using BeautyBook.Services.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyBook.Services.Implementations
{
    public class AppointmentService
    {
        public class AppointmentService : IAppointmentService
        {
            public Task<bool> IsAvailableAsync(
                DateTime date,
                TimeSpan startTime,
                int serviceId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
