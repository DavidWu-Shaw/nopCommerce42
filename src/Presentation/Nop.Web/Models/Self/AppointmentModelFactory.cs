using Nop.Core.Domain.Self;

namespace Nop.Web.Models.Self
{
    public static class AppointmentModelFactory
    {
        public static AppointmentModel ConvertToModel(Appointment appointment)
        {
            var model = new AppointmentModel
            {
                id = appointment.Id.ToString(),
                text = appointment.Label,
                start = appointment.StartTimeUtc.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss"),
                end = appointment.EndTimeUtc.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss"),
                resource = appointment.ResourceId.ToString()
            };
            model.tags = new TagModel
            {
                status = ((AppointmentStatusType)appointment.StatusId).ToString(),
                doctor = appointment.Product.Name
            };

            return model;
        }
    }
}
