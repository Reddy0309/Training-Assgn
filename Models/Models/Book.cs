using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodingWiki_Model.Models;

namespace DataModels.Models
{
    public class Book
    {
        
        public int BookId { get; set; }
        public string Title { get; set; }
        
        public string ISBN { get; set; }
        public double Price { get; set; }
        
        public string PriceRange { get; set; }
        //Navigation Property
        public BookDetail BookDetail { get; set; }

        //Foreign Key
        //[ForeignKey("Publisher")]
        public int Publisher_Id { get; set; }
        public Publisher Publisher { get; set; }

        public List<BookAuthorMap> BookAuthorMap { get; set; }



    }
}
//Fluent APIss
//