using Dapper.FluentMap.Mapping;
using DapperSW.Models;

namespace DapperSW.Mappings
{
    public class FilmMap : EntityMap<Film>
    {
        public FilmMap()
        {
            // Map property 'Id' to column 'film_id'.
            Map(p => p.Id)
                .ToColumn("film_id");
            // Map property 'Title' to column 'strTitle'.
            Map(p => p.Title)
                .ToColumn("strTitle");

            // Ignore the 'Spoilers' property when mapping.
            Map(p => p.Spoilers)
                .Ignore();
        }
    }
}