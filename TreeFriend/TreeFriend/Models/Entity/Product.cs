using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace TreeFriend.Models.Entity {
    public class Product {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Subtitle { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime StarDate { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int Count { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Picture { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
