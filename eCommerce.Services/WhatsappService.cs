using eCommerce.Services;
using eCommerce.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eCommerce.Services
{
    public partial class WhatsAppService
    {
        private HttpClient _httpClient;
        private string _version;
        private string _phoneNumberId;
        private string _recipientPhoneNumber;
        private string _apiUrl;
        private string _accessToken;
        private string _wabaId;
        private string _userAccessToken;
        private string _businessId;

        #region Define as Singleton
        private static WhatsAppService _Instance;

        public static WhatsAppService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new WhatsAppService();
                }

                return (_Instance);
            }
        }
        public WhatsAppService()
        {
            _version = "v16.0"; 

            _phoneNumberId = ConfigurationsHelper.WA_PhoneNumberId; 
            //_businessId = ConfigurationsHelper.WA_BusinessId;
            _userAccessToken = ConfigurationsHelper.WA_UserAccessToken;
            //_wabaId = ConfigurationsHelper.WA_WabaId;

            _apiUrl = $"https://graph.facebook.com/{_version}/{_phoneNumberId}/messages";

            _httpClient = new HttpClient();
        }
        #endregion Define as Singleton

        public async Task<int> SendAsync(string recipientPhoneNumber,string message)
        {

            try
            {
                _recipientPhoneNumber = recipientPhoneNumber;
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Add("Version", _version);
                //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //_httpClient.DefaultRequestHeaders.Add("WABA-ID", _wabaId);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_userAccessToken}");
                //_httpClient.DefaultRequestHeaders.Add("Recipient-Phone-Number", _recipientPhoneNumber);
                //_httpClient.DefaultRequestHeaders.Add("Phone-Number-ID", _phoneNumberId);
                //_httpClient.DefaultRequestHeaders.Add("Business-ID", _businessId);

                var messageData = new
                {
                    messaging_product = "whatsapp",
                    recipient_type = "individual",
                    to = recipientPhoneNumber,
                    type = "text",
                    text = new
                    {
                        preview_url = true,
                        body = message
                    }
                };

                var requestContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(messageData),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(_apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return (int) response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}

