using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot_Manager.Domains
{
    [Table("Guild")]
    public class Guild
    {
        [Key]
        private ulong guild_Id { get; set; }

        [Required]
        private string guild_Name { get; set; }
        [Required]
        private string guild_Owner { get; set; }
        [Required]
        public string? LogChannel { get; set; }
    }
}
