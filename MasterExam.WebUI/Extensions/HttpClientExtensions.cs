using MasterExam.WebUI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Extensions
{
    public static class HttpClientExtensions
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static HttpClient AddHeader(this HttpClient httpClient)
        {
            //Retrieve appsetting.json
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            //int timeoutSec = 90;
            //cl.Timeout = new TimeSpan(0, 0, timeoutSec);
            string baseUrl = Configuration["BaseUrl"];
            string contentType = "application/json";
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            var userAgent = "d-fens HttpClient";
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            return httpClient;
        }

        public static HttpClient AddTokenToHeader(this HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token));

            return httpClient;
        }

        
    }
}
