using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using Webhook_Console.Models;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private readonly static string Cin7BaseUrl = "https://inventory.dearsystems.com/ExternalApi/v2/";
        private readonly static string AuthAccountId = "a3a7db8e-d840-457d-8bfe-2eb30da41764";
        private readonly static string AuthApplicationKey = "ad1b3a7d-bd96-f1c3-11a0-c6020c721340";
        private readonly static Uri MondayEndpoint = new Uri("https://api.monday.com/v2/");
        private readonly static string MondayApiAccessToken = "eyJhbGciOiJIUzI1NiJ9.eyJ0aWQiOjI4MzA3MjQ1MywiYWFpIjoxMSwidWlkIjozNzk4MTIzOSwiaWFkIjoiMjAyMy0wOS0yMFQwODo1NTo0OC4wMDBaIiwicGVyIjoibWU6d3JpdGUiLCJhY3RpZCI6MTQwOTEyNjQsInJnbiI6InVzZTEifQ.zbbR_SgO2nGilF0gXblH_g1GKgz8Kdy2c2LVToQXvuk";
        private readonly static string BoardId = "5790363144";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("getting customers from cin7");
                var customerList = new List<CustomerList>();
                Cin7_GetCustomerList(ref customerList, 1, 1000);
                Monday_CreateOrUpdateItem(customerList);

            }
            catch (Exception ex)
            {
                Log.ExceptionLog(ex);
            }


        }
        public static void Monday_CreateOrUpdateItem(List<CustomerList> customerList)
        {
            var graphQlClient = new GraphQLHttpClient(new GraphQLHttpClientOptions
            {
                EndPoint = MondayEndpoint,
            }, new NewtonsoftJsonSerializer());
            graphQlClient.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", MondayApiAccessToken);
            graphQlClient.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            graphQlClient.HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("API-version", "2023-10");

            foreach (var customer in customerList)
            {
                Console.WriteLine("checking existance");
                var request = new GraphQLRequest
                {
                    Query = $@"query {{
                              boards (ids: {BoardId}) {{
                                items_page (query_params: {{rules: [{{column_id: ""text"", compare_value: [""{customer.ID}""], operator:contains_text}}]}}) {{
                                  items {{
                                    id
                                    name
                                  }}
                                }}
                              }}
                            }}"
                };
                var response1 = graphQlClient.SendQueryAsync<ItemsModel>(request).Result;
                var exist = response1.Data.Boards[0].Items_Page.Items.Any();

                if (!exist)
                {
                    Console.WriteLine("creating item");
                    var createRequest = new GraphQLRequest
                    {
                        Query = @"mutation { 
                            create_item (board_id: {{BoardID}}, group_id: ""topics"",  item_name: ""{{Customer Name}}"", column_values: ""{ \""status\"": {\""label\"":\""{{Status}}\""}, \""currency\"": {\""label\"":\""{{Currency}}\""}, \""paymentterm\"": {\""label\"":\""{{Payment Term}}\""}, \""taxrule\"": {\""label\"":\""{{Tax Rule}}\""}, \""pricetier\"": {\""label\"":\""{{Price Tier}}\""}, \""discount\"": \""{{Discount}}\"", \""creditlimit\"": \""{{Credit Limit}}\"", \""oncredithold\"": {\""label\"":\""{{Credit Hold}}\""}, \""carrier\"": {\""label\"":\""{{Carrier}}\""}, \""salesrepresentative\"": {\""label\"":\""{{Sales Representative}}\""}, \""location\"": {\""label\"":\""{{Location}}\""}, \""taxnumber\"": \""{{Tax Number}}\"", \""tags\"": \""{{Tags}}\"", \""text\"": \""{{Customer ID}}\"" }"", create_labels_if_missing: true) {
                                id 
                            }
                        }"
                        .Replace("{{BoardID}}", BoardId)
                        .Replace("{{Customer Name}}", customer.Name)
                        .Replace("{{Status}}", customer.Status)
                        .Replace("{{Currency}}", customer.Currency)
                        .Replace("{{Payment Term}}", customer.PaymentTerm)
                        .Replace("{{Tax Rule}}", customer.TaxRule)
                        .Replace("{{Price Tier}}", customer.PriceTier)
                        .Replace("{{Discount}}", customer.Discount?.ToString())
                        .Replace("{{Credit Limit}}", customer.CreditLimit?.ToString())
                        .Replace("{{Credit Hold}}", customer.IsOnCreditHold?.ToString())
                        .Replace("{{Carrier}}", customer.Carrier)
                        .Replace("{{Sales Representative}}", customer.SalesRepresentative?.ToString())
                        .Replace("{{Location}}", customer.Location)
                        .Replace("{{Tax Number}}", customer.TaxNumber)
                        .Replace("{{Tags}}", customer.Tags)
                        .Replace("{{Customer ID}}", customer.ID)
                    };

                    var response2 = graphQlClient.SendQueryAsync<object>(createRequest).Result;

                }
                else
                {
                    Console.WriteLine("updating item");
                    var updateRequest = new GraphQLRequest
                    {
                        Query = @"mutation {
                            change_multiple_column_values (board_id: {{BoardID}}, item_id: {{Pulse ID}}, column_values: ""{ \""status\"": {\""label\"":\""{{Status}}\""}, \""currency\"": {\""label\"":\""{{Currency}}\""}, \""paymentterm\"": {\""label\"":\""{{Payment Term}}\""}, \""taxrule\"": {\""label\"":\""{{Tax Rule}} \""}, \""pricetier\"": {\""label\"":\""{{Price Tier}}\""}, \""discount\"": \""{{Discount}}\"", \""creditlimit\"": \""{{Credit Limit}}\"", \""oncredithold\"": {\""label\"":\""{{Credit Hold}}\""}, \""carrier\"": {\""label\"":\""{{Carrier}}\""}, \""salesrepresentative\"": {\""label\"":\""{{Sales Representative}}\""}, \""location\"": {\""label\"":\""{{Location}}\""}, \""taxnumber\"": \""{{Tax Number}}\"", \""tags\"": \""{{Tags}}\"", \""text\"": \""{{Customer ID}}\"" }"", create_labels_if_missing: true) {
                            id
                          }
                        }"
                        .Replace("{{BoardID}}", BoardId)
                        .Replace("{{Pulse ID}}", response1.Data.Boards[0].Items_Page.Items[0].Id)
                        .Replace("{{Status}}", customer.Status)
                        .Replace("{{Currency}}", customer.Currency)
                        .Replace("{{Payment Term}}", customer.PaymentTerm)
                        .Replace("{{Tax Rule}}", customer.TaxRule)
                        .Replace("{{Price Tier}}", customer.PriceTier)
                        .Replace("{{Discount}}", customer.Discount?.ToString())
                        .Replace("{{Credit Limit}}", customer.CreditLimit?.ToString())
                        .Replace("{{Credit Hold}}", customer.IsOnCreditHold?.ToString())
                        .Replace("{{Carrier}}", customer.Carrier)
                        .Replace("{{Sales Representative}}", customer.SalesRepresentative?.ToString())
                        .Replace("{{Location}}", customer.Location)
                        .Replace("{{Tax Number}}", customer.TaxNumber)
                        .Replace("{{Tags}}", customer.Tags)
                        .Replace("{{Customer ID}}", customer.ID)
                    };

                    var response3 = graphQlClient.SendQueryAsync<object>(updateRequest).Result;
                }

            }
        }

        public static void Cin7_GetCustomerList(ref List<CustomerList> customerList, int page, int limit)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("api-auth-accountid", AuthAccountId);
                client.DefaultRequestHeaders.TryAddWithoutValidation("api-auth-applicationkey", AuthApplicationKey);

                var res = client.GetAsync($"{Cin7BaseUrl}customer?Page={page}&Limit={limit}").Result;
                //var json = res.Content.ReadAsStringAsync().Result;
                //var list = System.Text.Json.JsonSerializer.Deserialize<CustomerListModel>(json);
                var content = res.Content.ReadFromJsonAsync<CustomerListModel>().Result;
                customerList.AddRange(content!.CustomerList);
                if (content!.CustomerList.Any() && content.Total > limit)
                {
                    Cin7_GetCustomerList(ref customerList, ++page, 1000);
                }
            }
        }

    }
}