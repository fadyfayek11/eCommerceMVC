using eCommerce.Entities;
using eCommerce.Shared.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerce.Shared.Helpers
{
    public static class ConfigurationsHelper
    {
        public static ConcurrentDictionary<string, string> ConfigurationsDictionary = new ConcurrentDictionary<string, string>();

        public static void UpdateConfigurations(List<Configuration> configurations)
        {
            if (configurations != null && configurations.Count > 0)
            {
                ConfigurationsDictionary = new ConcurrentDictionary<string, string>();

                foreach (var configuration in configurations)
                {
                    ConfigurationsDictionary.TryAdd(configuration.Key, configuration.Value);
                }
            }
        }

        public static void UpdateConfiguration(Configuration configuration)
        {
            if (configuration != null && ConfigurationsDictionary.ContainsKey(configuration.Key))
            {
                ConfigurationsDictionary[configuration.Key] = configuration.Value;
            }
        }

        public static T GetConfigValue<T>(string key)
        {
            if (!ConfigurationsDictionary.ContainsKey(key))
            {
                throw (new ApplicationException("No such Configuration found: " + key));
            }

            try
            {
                var value = ConfigurationsDictionary[key];

                if(!string.IsNullOrEmpty(value))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {
                throw (new ApplicationException(string.Format("Cannot convert Configuration value to {0}", typeof(T)), e));
            }
        }

        public static string ApplicationName
        {
            get
            {
                return (GetConfigValue<string>("ApplicationName"));

                //if (EnableMultilingual)
                //{
                //    return (GetConfigValue<string>("ApplicationName"));
                //}
                //else return (GetConfigValue<string>("ApplicationName"));
            }
        }

        public static string ApplicationIntro
        {
            get
            {
                return (GetConfigValue<string>("ApplicationIntro"));
            }
        }

        public static string AddressLine1
        {
            get
            {
                return (GetConfigValue<string>("AddressLine1"));
            }
        }

        public static string AddressLine2
        {
            get
            {
                return (GetConfigValue<string>("AddressLine2"));
            }
        }

        public static string PhoneNumber
        {
            get
            {
                return (GetConfigValue<string>("PhoneNumber"));
            }
        }

        public static string MobileNumber
        {
            get
            {
                return (GetConfigValue<string>("MobileNumber"));
            }
        }

        public static string Email
        {
            get
            {
                return (GetConfigValue<string>("Email"));
            }
        }

        public static string AdminEmailAddress
        {
            get
            {
                return (GetConfigValue<string>("AdminEmailAddress"));
            }
        }

        public static string FacebookURL
        {
            get
            {
                return (GetConfigValue<string>("FacebookURL"));
            }
        }

        public static string TwitterUsername
        {
            get
            {
                return (GetConfigValue<string>("TwitterUsername"));
            }
        }

        public static string TwitterURL
        {
            get
            {
                return (GetConfigValue<string>("TwitterURL"));
            }
        }

        public static string WhatsAppNumber
        {
            get
            {
                return (GetConfigValue<string>("WhatsAppNumber"));
            }
        }

        public static string InstagramURL
        {
            get
            {
                return (GetConfigValue<string>("InstagramURL"));
            }
        }

        public static string YouTubeURL
        {
            get
            {
                return (GetConfigValue<string>("YouTubeURL"));
            }
        }

        public static string LinkedInURL
        {
            get
            {
                return (GetConfigValue<string>("LinkedInURL"));
            }
        }

        public static string CurrencySymbol
        {
            get
            {
                return (GetConfigValue<string>("CurrencySymbol"));
            }
        }

        public static string PriceCurrencyPosition
        {
            get
            {
                return (GetConfigValue<string>("PriceCurrencyPosition"));
            }
        }

        public static bool EnableCreditCardPayment
        {
            get
            {
                return (GetConfigValue<bool>("EnableCreditCardPayment"));
            }
        }

        public static bool EnableCashOnDeliveryMethod
        {
            get
            {
                return (GetConfigValue<bool>("EnableCashOnDeliveryMethod"));
            }
        }

        public static bool EnablePayPalPaymentMethod
        {
            get
            {
                return (GetConfigValue<bool>("EnablePayPalPaymentMethod"));
            }
        }

        public static string PayPalClientID
        {
            get
            {
                return (GetConfigValue<string>("PayPalClientID"));
            }
        }
        
        public static decimal FlatDeliveryCharges
        {
            get
            {
                return (GetConfigValue<decimal>("FlatDeliveryCharges"));
            }
        }
        public static int DigitsAfterDecimalPoint
        {
            get
            {
                return (GetConfigValue<int>("DigitsAfterDecimalPoint"));
            }
        }

        public static int DefaultRating
        {
            get
            {
                return 5;
            }
        }

        public static bool EnableMultilingual
        {
            get
            {
                return (GetConfigValue<bool>("EnableMultilingual"));
            }
        }

        public static string DefaultLanguage
        {
            get
            {
                return (GetConfigValue<string>("DefaultLanguage"));
            }
        }

        public static string AuthorizeNet_ApiLoginID
        {
            get
            {
                return (GetConfigValue<string>("AuthorizeNet_ApiLoginID"));
            }
        }

        public static string AuthorizeNet_ApiTransactionKey
        {
            get
            {
                return (GetConfigValue<string>("AuthorizeNet_ApiTransactionKey"));
            }
        }

        public static bool AuthorizeNet_SandBox
        {
            get
            {
                return (GetConfigValue<bool>("AuthorizeNet_SandBox"));
            }
        }

        public static string SendGrid_APIKey
        {
            get
            {
                return (GetConfigValue<string>("SendGrid_APIKey"));
            }
        }

        public static string FromEmailAddress
        {
            get
            {
                return (GetConfigValue<string>("FromEmailAddress"));
            }
        }

        public static string FromEmailAddressName
        {
            get
            {
                return (GetConfigValue<string>("FromEmailAddressName"));
            }
        }

        public static string GoogleAnalyticsScript
        {
            get
            {
                return (GetConfigValue<string>("GoogleAnalyticsScript"));
            }
        }

        public static string FacebookMessengerScript
        {
            get
            {
                return (GetConfigValue<string>("FacebookMessengerScript"));
            }
        }
        public static string SMTP_Server
        {
            get
            {
                return (GetConfigValue<string>("SMTP_server"));
            }
        }
        public static string SMTP_Port
        {
            get
            {
                return (GetConfigValue<string>("SMTP_port"));
            }
        }
        public static string SMTP_Username
        {
            get
            {
                return (GetConfigValue<string>("SMTP_username"));
            }
        }
        public static string SMTP_Password
        {
            get
            {
                return (GetConfigValue<string>("SMTP_password"));
            }
        }
        public static string WA_PhoneNumberId
        {
            get
            {
                return (GetConfigValue<string>("WA_PhoneNumberId"));
            }
        }
        //public static string WA_BusinessId
        //{
        //    get
        //    {
        //        return (GetConfigValue<string>("WA_BusinessId"));
        //    }
        //}
        public static string WA_UserAccessToken
        {
            get
            {
                return (GetConfigValue<string>("WA_UserAccessToken"));
            }
        }
        //public static string WA_WabaId
        //{
        //    get
        //    {
        //        return (GetConfigValue<string>("WA_WabaId"));
        //    }
        //}
        public static Environments Environment
        {
            get
            {
                var env = System.Configuration.ConfigurationManager.AppSettings["Environment"];

                if(!string.IsNullOrEmpty(env))
                {
                    if(env.Equals("QA"))
                    {
                        return Environments.LIVE;
                    }
                    else if (env.Equals("STAGING"))
                    {
                        return Environments.STAGING;
                    }
                    else if (env.Equals("DEMO"))
                    {
                        return Environments.DEMO;
                    }
                }

                return Environments.LIVE;
            }
        }
    }
}