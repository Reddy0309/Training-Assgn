using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Models
{
    public class BookDetail
    {
        
        public int BookDetail_Id { get; set; }
        public int NoOfChapters { get; set; }
        public int NumberOfPages { get; set; }
        public double Weight  { get; set; }

        ////Foreign Key
        //[ForeignKey("Book")]
        public int BookId { get; set; }
        ////Navigation Property
        public Book Book { get; set; }

        
    }
}
