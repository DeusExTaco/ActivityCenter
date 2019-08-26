using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class Event
    {
        [Key] [Column("id")] public int EventId { get; set; }

        [Required] [Column("user_id")] public int UserId { get; set; }

        [Required]
        [Column("event_title", TypeName = "VARCHAR(45)")]
        [MinLength(2)]
        [MaxLength(45)]
        public string EventTitle { get; set; }

        [Required]
        [PastDate]
        [Column("date", TypeName = "DATETIME")]
        public DateTime Date { get; set; }

        [Required]
        [Column("time", TypeName = "DATETIME")]
        public DateTime Time { get; set; }

        [Required] [Column("duration")] public int Duration { get; set; }

        [Required]
        [Column("duration_units", TypeName = "VARCHAR(10)")]
        public string DurationUnits { get; set; }

        [Required]
        [Column("description", TypeName = "TEXT")]
        public string Description { get; set; }

        [Column("created_at", TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at", TypeName = "DATETIME")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User Creator { get; set; }

        public ICollection<Participant> Participants { get; set; }
    }

    internal class EventImpl : Event
    {
    }
}