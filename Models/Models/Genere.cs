using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models
{
    public class Genere
    {
        [Key]
        public int GenereId { get; set; }
        [Column("Name")]
        public string GenereName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
