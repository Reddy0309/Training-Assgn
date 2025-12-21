using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace CodingWiki_Model.Models
{
    
    public class BookAuthorMap
    {
        
        public int Book_Id { get; set; }
        
        public int Author_Id { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
