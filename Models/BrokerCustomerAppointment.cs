namespace agendaEFD.Models
{
    public partial class BrokerCustomerAppointment
    {
        public Broker BrokerVm { get; set; }
        public Customer CustomerVm { get; set; }
        public Appointment AppointmentVm { get; set; }
    }
}
