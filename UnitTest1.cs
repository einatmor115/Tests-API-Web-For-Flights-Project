using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Flights_project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTestProject_OutOfProject
{
    [TestClass]
    public class UnitTest1
    {
        Center_UnitTestProject_OutOfProject center = new Center_UnitTestProject_OutOfProject();
        HttpClient client = new HttpClient();
        
        [TestMethod]
        public void AnonymousGetAllAirlineCompanies()
        {

            string AnonymousGetAllFlightsURL = "http://localhost:56181/api/AnonymousFacadeApi/GetAllAirlineCompanies";
            List<AirlineCompany> airlines = center.AnonymousUserFacade.GetAllAirlineCompanies().ToList();
            
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(AnonymousGetAllFlightsURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(AnonymousGetAllFlightsURL).Result;

            List<AirlineCompany> airlinesAPI = response.Content.ReadAsAsync<List<AirlineCompany>>().Result;

            Assert.AreEqual(airlines.Count, airlinesAPI.Count);
            for (int i = 0; i < airlines.Count; i++)
            {
                Assert.AreEqual(airlines[i].ID, airlinesAPI[i].ID);
                Assert.AreEqual(airlines[i].AirlineName, airlinesAPI[i].AirlineName);
                Assert.AreEqual(airlines[i].CountryCode, airlinesAPI[i].CountryCode);
                Assert.AreEqual(airlines[i].Password, airlinesAPI[i].Password);
                Assert.AreEqual(airlines[i].UserName, airlinesAPI[i].UserName);
            }
        }

        [TestMethod]
        public void AdminstratorRemoveAirLine()
        {

            string AdminstratorRemoveAirLineURL = "http://localhost:56181/api/AdministratorFacdeAPIController/RemoveAirline/";

            List<AirlineCompany> airLinesC = center.AnonymousUserFacade.GetAllAirlineCompanies().ToList();
            AirlineCompany airline = airLinesC[0];
            AdminstratorRemoveAirLineURL += $"{airLinesC[0].ID}";

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{center.AdministratorToken.User.UserName}:{center.AdministratorToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(AdminstratorRemoveAirLineURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.DeleteAsync(AdminstratorRemoveAirLineURL).Result;

            List<AirlineCompany> airLinesNew = center.AnonymousUserFacade.GetAllAirlineCompanies().ToList();
            Assert.IsFalse(airLinesNew[0] == airline);
        }

        [TestMethod]
        public void CustomerGetAllMyFlights()
        {
            string customerGetAllFlightsURL = "http://localhost:56181/api/CustomerFacadeApiController/GetAllMyFlights";

            List<Flight> flights = center.CustomerFacade.GetAllMyFlights(center.CustomerToken).ToList();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{center.CustomerToken.User.UserName}:{center.CustomerToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(customerGetAllFlightsURL);
            HttpResponseMessage response = client.GetAsync(customerGetAllFlightsURL).Result;

            List<Flight> custFlightsFromDB = response.Content.ReadAsAsync<List<Flight>>().Result;
            Assert.AreEqual(flights.Count, custFlightsFromDB.Count);
        }

        [TestMethod]
        public void AirLineGetAllFlights()
        {
            string AirLineGetAllFlightsURL = "http://localhost:56181/api/CompanyFacdeAPIController/GetAllFlights";

            List<Flight> flights = center.AirlineFacade.GetAllFlights(center.AirLineToken).ToList();
 
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{center.AirLineToken.User.UserName}:{center.AirLineToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(AirLineGetAllFlightsURL);
            HttpResponseMessage response = client.GetAsync(AirLineGetAllFlightsURL).Result;

            List<Flight> airLineFlightsFromDB = response.Content.ReadAsAsync<List<Flight>>().Result;
            Assert.AreEqual(flights.Count, airLineFlightsFromDB.Count);
        }

        //[TestMethod]
        //cant send object from body
        public void AirLineCreateFlight()
        {
            string AirLineGetAllFlightsURL = "http://localhost:56181/api/companyfacdeapicontroller/createflight";

            List<Flight> flights = center.AdministratorFacade.GetAllFlights().ToList();
            Flight f = new Flight();
            for (int i = 0; i < flights.Count; i++)
            {
                if (flights[i].ID == center.AirLineToken.User.ID)
                {

                    f = flights[i];
                }
            }
            f = flights[0];

            client.DefaultRequestHeaders.Accept.Clear(); 
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{center.AirLineToken.User.UserName}:{center.AirLineToken.User.Password}");
            string ss = Convert.ToBase64String(plainTextBytes);
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", ss);
            client.DefaultRequestHeaders.Authorization = ahv;

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(AirLineGetAllFlightsURL);
            HttpResponseMessage response = client.GetAsync(AirLineGetAllFlightsURL).Result;

            var myContent = JsonConvert.SerializeObject(f);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.PostAsync(AirLineGetAllFlightsURL, byteContent);

            List<Flight> airLineAPI = center.AirlineFacade.GetAllFlights(center.AirLineToken).ToList();

           Assert.IsTrue(airLineAPI[airLineAPI.Count] == f);
        }
    }
}
