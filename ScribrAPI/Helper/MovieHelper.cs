using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ScribrAPI.Model;

namespace ScribrAPI.Helper
{
    public class MovieHelper
    {
        public static void TestProgram()
        {
            Console.WriteLine(GetMovieFromID("tt4154796"));
            Console.ReadLine();
        }
        public static String GetMovieFromLink(String URL)
        {   // https://www.imdb.com/title/tt4154796/ --> tt4154796
            int index = URL.IndexOf("/tt") + 1;
            String movieID = URL.Substring(index, 9);
            return movieID;
        }
        public static Model.Movie GetMovieFromID(String movieID)
        {
            String APIKEY = "6910e659fc35ebb1a5f364f7477b8e3d";
            String movieAPILink = "https://api.themoviedb.org/3/movie/" + movieID + "?api_key=" + 
                APIKEY + "&language=en-US";

            //Uses HTTP client to get the JSON from the web
            String movieInfoJSON = new WebClient().DownloadString(movieAPILink);

            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(movieInfoJSON);
            String title = jsonObj["title"];
            String poster = jsonObj["poster_path"];
            String ReleaseDate = jsonObj["release_date"];
            String MovieLength = jsonObj["runtime"];
            String IMDBLink = "https://www.imdb.com/title/" + jsonObj[""];
            String Discription = jsonObj["overview"];
            String Genres = jsonObj["genres"][0].name;

            Movie movie = new Movie()
            {
                MovieTitle = title,
                PosterUrl = poster
            };
            return movie;
        }
    }
    
}
