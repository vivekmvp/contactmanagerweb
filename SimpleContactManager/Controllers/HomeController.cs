using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleContactManager.Models;

namespace SimpleContactManager.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }        

        public IActionResult Index()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/person/listall").Result;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    List<Person> data = JsonConvert.DeserializeObject<List<Person>>(stringData);
                    return View(data);
                }                
            }

            return View(new List<Person>());
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/person/details?Id=" + Id).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Person data = JsonConvert.DeserializeObject<Person>(stringData);
                    return View(data);
                }
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Person person)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("/person/edit", new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8)
                {
                    Headers =
                        {
                            ContentType = new MediaTypeWithQualityHeaderValue("application/json")
                        }
                }).ConfigureAwait(false);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Person data = JsonConvert.DeserializeObject<Person>(stringData);
                    //return View(data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/person/details?Id=" + Id).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Person data = JsonConvert.DeserializeObject<Person>(stringData);
                    return View(data);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Person person)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("/person/delete", new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8)
                {
                    Headers =
                        {
                            ContentType = new MediaTypeWithQualityHeaderValue("application/json")
                        }
                }).ConfigureAwait(false);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Person data = JsonConvert.DeserializeObject<Person>(stringData);
                    //return View(data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/person/details?Id=" + Id).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    Person data = JsonConvert.DeserializeObject<Person>(stringData);
                    return View(data);
                }
            }
            return RedirectToAction("Index");
        }











        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult InitializeData()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(configuration["ContactManagerApiBaseUrl"]);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("/person/addseeddata").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    bool data = JsonConvert.DeserializeObject<bool>(stringData);                    
                    //return View(data);
                }
            }

            return RedirectToAction("Index");   //Sample Data created.
        }
    }
}
