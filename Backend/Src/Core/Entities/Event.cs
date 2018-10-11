using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Backend.Core.Entities {
    public class Event : EntityObject {

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime RegistrationStart { get; set; }

        [Required]
        public DateTime RegistrationEnd { get; set; }

        public bool PresentationsLocked { get; set; }

        [Required]
        public List<Area> Areas { get; set; }

        [Required]
        public bool IsExpiredLockMode { get; set; }

        [Required]
        public RegistrationState RegistrationState { get; set; }
    }
}