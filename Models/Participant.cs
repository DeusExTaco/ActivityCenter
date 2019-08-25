using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ActivityCenter.Models
{
    public class Participant 
    {
        [Key]
        [Column("id")]
        public int ParticipantId {get;set;}
        
        [Column("user_id")]
        public int UserId {get;set;}
        
        [Column("event_id")]
        public int EventId {get;set;}
        
        public User User {get;set;}
        public Event Event {get; set;}
    }

    class ParticipantImpl : Participant
    {
    }
}