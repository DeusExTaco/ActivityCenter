using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ActivityCenter.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public int UserId { get; set; }
        
        [Required]
        [MinLength(2), MaxLength(45)]
        [Column("first_name", TypeName="VARCHAR(45)")]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(2), MaxLength(45)]
        [Column("last_name", TypeName="VARCHAR(45)")]
        public string LastName { get; set; }
        
        [EmailAddress]
        [Required]       
        [Column("email", TypeName="VARCHAR(255)")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        [PasswordCheck]
        [Column("password", TypeName="VARCHAR(255)")]
        public string Password {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        
        [Required]
        [FutureDate]
        [Over18]
        [Column("birthday", TypeName="DATETIME")]     
        public DateTime Birthday {get; set;}
        
        [Column("created_at", TypeName="DATETIME")]
        public DateTime created_at {get;set;} = DateTime.Now;
        
        [Column("updated_at", TypeName="DATETIME")]
        public DateTime updated_at {get;set;} = DateTime.Now;

        public List<Event> CreatedEvents {get;set;}

        public ICollection<Participant> Participants {get;set;}
    }

    class UserImpl : User
    {
    }
}