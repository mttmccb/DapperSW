using DapperSW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DapperSW.Controllers
{
    public class FilmCrapController : Controller
    {
        private MySettings _settings;
        public FilmCrapController(IOptions<MySettings> settings)
        {
            _settings = settings.Value;
        }
        public IActionResult Index()
        {
            var sql = @"Select film_id, title, description
            FROM film
            ORDER BY random() LIMIT @limit";
            List<Film> results = new List<Film>();

            var con = new NpgsqlConnection(_settings.ConnectionString);
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql);
            cmd.Connection = con;

            cmd.Parameters.Add("@limit", NpgsqlTypes.NpgsqlDbType.Integer).Value = 10;

            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                results.Add(new Film
                {
                    Id = Convert.ToInt32(rdr["film_id"]),
                    Title = rdr["title"].ToString(),
                    Description = rdr["description"].ToString()
                });
            }
            return View(results.AsEnumerable());
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
