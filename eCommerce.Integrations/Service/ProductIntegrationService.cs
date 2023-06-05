using eCommerce.Integrations.Model.Request;
using eCommerce.Integrations.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eCommerce.Integrations.Service
{
    public class ProductIntegrationService
    {

        public async Task<ProductResponse> GetProducts()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://s.hesabate.com/store_api.php");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("api@mahroum.com"), "username");
                content.Add(new StringContent("Abcd@12345"), "password");
                content.Add(new StringContent("download"), "action");
                content.Add(new StringContent("products"), "type");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var resultData = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ProductResponse>(resultData);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task SendOrder(OrderRequest orderRequest)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://s.hesabate.com/store_api.php");
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("api@mahroum.com"), "username");
                content.Add(new StringContent("Abcd@12345"), "password");
                content.Add(new StringContent("upload"), "action");
                //content.Add(new StringContent("products"), "type");
                //request.Content = content;
                //var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                //var resultData = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.SerializeObject(orderRequest);





                var content1 = new StringContent("{\"" + orderRequest.id + "\":" + result + "}", null, "application/json");
                request.Content = content1;
                var response1 = await client.SendAsync(request);
                response1.EnsureSuccessStatusCode();
                Console.WriteLine(await response1.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {

            }

        }


        public static string SanitizeJsonString(string jsonString)
        {
            // Define a list of reserved characters not allowed in file paths
            string[] reservedCharacters = { "<", ">", ":", "\"", "/", "\\", "|", "?", "*" };

            // Replace reserved characters with underscores
            foreach (var character in reservedCharacters)
            {
                jsonString = jsonString.Replace(character, "");
            }

            // Remove control characters
            jsonString = Regex.Replace(jsonString, @"\t|\n|\r", "");

            // Remove non-printable characters
            jsonString = new string(jsonString.Where(c => !char.IsControl(c)).ToArray());

            return jsonString;
        }
    }
}
