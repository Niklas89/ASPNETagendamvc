using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using agendaEFD.Data;

namespace agendaEFD.Models
{
    public partial class Broker
    {
        public Broker()
        {
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        public int IdBroker { get; set; }

        [Required(ErrorMessage = "Le champs Nom doit etre rempli.")]
        [Display(Name = "Votre nom de famille")]
        public string Lastname { get; set; } = null!;

        [Required(ErrorMessage = "Le champs Prénom doit etre rempli.")]
        [Display(Name = "Votre prénom")]
        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "Le champs Email doit etre rempli.")]
        [Display(Name = "Votre adresse email")]
        [EmailAddress(ErrorMessage = "Vous n'avez pas entré une adresse email.")]
        public string Mail { get; set; } = null!;

        [Required(ErrorMessage = "Le champs Numéro de téléphone doit etre rempli.")]
        [Display(Name = "Votre numéro de téléphone")]
        [MinLength(10, ErrorMessage = "Le numéro doit comporter au moins 10 chiffres.")]
        [MaxLength(10, ErrorMessage = "Le numéro doit comporter maximum 10 chiffres.")]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
