using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace RocpileRhymes.Controllers
{
    public class HomeController : Controller
    {
        private List<SearchResult> videos = new List<SearchResult>();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Search(String query)
        {
            try
            {
                SearchVideo(query);
            } catch(Exception e) 
            {
                return Content(e.ToString());
            }
            string results = "";
            foreach (SearchResult title in videos)
            {
                results += "<a target=\"_blank\" href =  \"https://youtube.com/watch?v=" + title.Id.VideoId + "\">" + title.Snippet.Title + "</a> <br />";
            }
            return Content(results);
        }
        private void SearchVideo(string search)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDxpKtj8J3L-tAdDQe0SBcYbYpzAc3xy6A",
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = search; 
            searchListRequest.MaxResults = 10;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = searchListRequest.ExecuteAsync().Result;

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(searchResult);
                        break;
                }
            }
        }
    }
}
