using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary;
using Newtonsoft.Json;

namespace ConsumRest
{
    public class Worker
    {
        private const string URI = "http://localhost:55289/api/hotels";

        public async Task Start()
        {
            //List<Hotel> hoteller = GetAll();
            //foreach (Hotel hotel in hoteller)
            //{
            //    Console.WriteLine("Hotel: " + hotel);
            //}

            Console.WriteLine("Hent hotel nr 1");
            Hotel oneHotel = GetOne(1);
            Console.WriteLine("Hotel nr 1 : " + oneHotel);

            Console.WriteLine("Delete hotel nr 57");
            Console.WriteLine("Delete hotel nr 57 : " + Delete(57));
            //Console.WriteLine("Delete hotel nr 56");
            //Console.WriteLine("Delete hotel nr 56 : " + Delete(56));
            Hotel opretHotel = new Hotel(57, "Ar lux hotel", "Street 123 London");
            Console.WriteLine("Opretter hotel 57 ::" + post(opretHotel));

            Hotel opdateretHotel = new Hotel(57, "Ar Delux hotel", "Street 321 København");
            Console.WriteLine("Opdaterer hotel 57 ::" + put(opdateretHotel.Id, opdateretHotel));


            List<Hotel> hoteller = GetAllAsync();

            foreach (Hotel hotel in hoteller)
            {

                Console.WriteLine("Hotel: " + hotel);
            }
        }

        

        public List<Hotel> GetAllAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (HttpClient client = new HttpClient())
            {
                Task<string> stringAsync = client.GetStringAsync(URI);
                string jsonString = stringAsync.Result;
                hoteller = JsonConvert.DeserializeObject<List<Hotel>>(jsonString);
            }
            return hoteller;
        }

        public Hotel GetOne(int id)
        {
            Hotel hotel = new Hotel();
            using (HttpClient client = new HttpClient())
            {
                Task<string> stringAsync = client.GetStringAsync(URI + "/" + id);
                string jsonString = stringAsync.Result;
                hotel = JsonConvert.DeserializeObject<Hotel>(jsonString);
            }
            return hotel;
        }

        public bool Delete(int id)
        {
            bool ok = false;
            using (HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> deleteAsync = client.DeleteAsync(URI + "/" + id);
                HttpResponseMessage resp = deleteAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    string jsonStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonStr);
                }
                else
                {
                    ok = false;
                }
            }
            return ok;
        }


        public bool post(Hotel hotel)
        {
            bool ok = false;
            using (HttpClient client = new HttpClient())
            {
                string jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> postAsync = client.PostAsync(URI, content);
                HttpResponseMessage resp = postAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    string jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }
            return ok;
        }

        public bool put(int id, Hotel hotel)
        {
            bool ok = false;
            using (HttpClient client = new HttpClient())
            {
                string jsonStr = JsonConvert.SerializeObject(hotel);
                StringContent content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> putAsync = client.PutAsync(URI + "/" + id, content);
                HttpResponseMessage resp = putAsync.Result;
                if (resp.IsSuccessStatusCode)
                {
                    string jsonResStr = resp.Content.ReadAsStringAsync().Result;
                    ok = JsonConvert.DeserializeObject<bool>(jsonResStr);
                }
                else
                {
                    ok = false;
                }
            }
            return ok;
        }


    }
}
