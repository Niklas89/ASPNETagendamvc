using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agendaEFD.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Appointments = new HashSet<Appointment>();
        }

        [Key]
        public int IdCustomer { get; set; }

        [Required(ErrorMessage = "Le champs Nom doit etre rempli.")]
        [Display(Name = "Le nom de famille")]
        public string? Lastname { get; set; } = null;

        [Required(ErrorMessage = "Le champs Prénom doit etre rempli.")]
        [Display(Name = "Le prénom")]
        public string? Firstname { get; set; } = null;

        [Required(ErrorMessage = "Le champs Email doit etre rempli.")]
        [Display(Name = "L'adresse email")]
        [EmailAddress(ErrorMessage = "Vous n'avez pas entré une adresse email.")]
        public string? Mail { get; set; } = null;

        [Required(ErrorMessage = "Le champs Numéro de téléphone doit etre rempli.")]
        [Display(Name = "Le numéro de téléphone")]
        [MinLength(10, ErrorMessage = "Le numéro doit comporter au moins 10 chiffres.")]
        [MaxLength(10, ErrorMessage = "Le numéro doit comporter maximum 10 chiffres.")]
        public string? PhoneNumber { get; set; } = null;

        [Required(ErrorMessage = "Le champs Budget doit etre rempli.")]
        [Display(Name = "Le budget en chiffres (sans le €)")]
        public int Budget { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
