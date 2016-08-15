using System.Collections.Generic;
using DapperSW.Models;

namespace DapperSW.ViewModels
{
    public class FilmDetails
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string Rating { get; set; }

        public List<Actor> Actors { get; set;}
        public List<Category> Categories { get; set;}
    }
}