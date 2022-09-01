using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bot_Manager.Domains
{
    [Table("User")]
    internal class User
    {
        [Key]
        public string Id { get { return Id; } set { Id = value.ToString(); } }


        [Required]
        public DateTime Created { get; set; }

    }
}