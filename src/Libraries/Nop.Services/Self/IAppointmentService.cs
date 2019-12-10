﻿using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Self;

namespace Nop.Services.Self
{
    public partial interface IAppointmentService
    {
        Appointment GetAppointmentById(int appointmentId);
        void InsertAppointment(Appointment appointment);
        List<Appointment> GetAppointmentsByResource(DateTime startTimeUtc, DateTime endTimeUtc, int resourceId);
    }
}