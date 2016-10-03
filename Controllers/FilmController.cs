using DapperSW.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using DapperSW.ViewModels;
using System.Linq;

namespace DapperSW.Controllers
{
    public class FilmController : Controller
    {
        private MySettings _settings;
        public FilmController(IOptions<MySettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult Index()
        {
            var sql = @"
            SELECT
                film_id as id, 
                title,
                description
            FROM film
            ORDER BY random() LIMIT {=limit}";
            IEnumerable<Film> results;
            using (var connection = new NpgsqlConnection(_settings.ConnectionString))
            {
                results = connection.Query<Film>(sql, new { limit = 10 });
            }
            return View(results);
        }

        public IActionResult Details(int id)
        {
            var sql = @"
            SELECT
                title,
                description,
                cast(release_year as int) as releaseyear,
                cast(rating as text) as rating
            FROM film
            WHERE film_id = @id;
            
            SELECT
                a.first_name,
                a.last_name
            FROM actor a
                INNER JOIN film_actor fa
                    ON a.actor_id = fa.actor_id
            WHERE fa.film_id = @id;

            SELECT
                c.name
            FROM category c
                INNER JOIN film_category fc
                    ON c.category_id = fc.category_id
            WHERE fc.film_id = @id";
            FilmDetails results = new FilmDetails();
            using (var connection = new NpgsqlConnection(_settings.ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                using (var multi = connection.QueryMultiple(sql, new { @id = id} )) {
                    results = multi.Read<FilmDetails>().Single();
                    results.Actors = multi.Read<Actor>().ToList();
                    results.Categories = multi.Read<Category>().ToList();
                }
            }
            return View(results);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
