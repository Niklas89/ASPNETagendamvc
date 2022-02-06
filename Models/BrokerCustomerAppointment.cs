namespace agendaEFD.Models
{
    public class BrokerCustomerAppointment
    {
        public Broker BrokerVm { get; set; }
        public Customer CustomerVm { get; set; }
        public Appointment AppointmentVm { get; set; }
    }
}
