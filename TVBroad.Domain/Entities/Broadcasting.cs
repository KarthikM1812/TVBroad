using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVBroad.Domain.Entities
{
        public class Broadcasting
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Title is required")]
            [MaxLength(100)]
            public string Title { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [MaxLength(500)]
            public string Description { get; set; }

            [Required(ErrorMessage = "Start time is required")]
            public DateTime StartTime { get; set; }

            [Required(ErrorMessage = "End time is required")]
            public DateTime EndTime { get; set; }

            // Enum or string status: Pending, Approved, Rejected
            [Required]
            [MaxLength(20)]
            public string Status { get; set; } = "Pending";

            // Timestamps
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public DateTime? UpdatedAt { get; set; }
        }
    }
