using Newtonsoft.Json;
using ScribrAPI.Model;
using System;
using System.Net;

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
     
        public static dynamic GetMovieFromID(String movieID)
        {
            String APIKEY = "6910e659fc35ebb1a5f364f7477b8e3d";
            String movieAPILink = "https://api.themoviedb.org/3/movie/" + movieID + "?api_key=" + 
                APIKEY + "&language=en-US";

            //Uses HTTP client to get the JSON from the web
            String movieInfoJSON = new WebClient().DownloadString(movieAPILink);

            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(movieInfoJSON);
            String title = jsonObj["title"];
            String poster = "https://image.tmdb.org/t/p/w780/" + jsonObj["poster_path"];
            String date = jsonObj["release_date"];
            int runtime = jsonObj["runtime"];
            String link = "https://www.imdb.com/title/" + jsonObj["imdb_id"];
            String description = jsonObj["overview"];
            String genres = jsonObj["genres"][0].name;

            DateTime objDate = DateTime.Parse(date);

            // getting the genres for the movie.
            for (int i = 1; i < jsonObj["genres"].Count ; i++)
            {
                genres += ", " + jsonObj["genres"][i].name;
            }
                

                Movie movie = new Movie()
            {
                MovieTitle = title,
                PosterUrl = poster,
                ReleaseDate = objDate,
                MovieLength = runtime,
                Imdblink = link,
                Discription = description,
                Genres = genres
             };
            return movie;
        }
    }
    
}
