using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace WebClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient(); 
            var listMenu = new List<string>();
            listMenu.Add("1.Найти клиента по идентификатору Id");
            listMenu.Add("2.Сгенерировать и сохранить нового клиента");
            listMenu.Add("3.Выход");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Меню:");

                foreach(var item in listMenu)
                {
                    Console.WriteLine($"{item}");
                }

                Console.WriteLine();
                Console.WriteLine("Введите номер из меню (1,2,3): ");

                var itemMenu = Console.ReadLine()!.Trim();

                if(!listMenu.Select(m => m.Substring(0, 1)).Contains(itemMenu.ToLower()))
                {
                    Console.WriteLine("Введите правильный номер меню!");
                    continue;
                }

                if (itemMenu=="1")
                {
                    //while (true)
                    //{
                        Console.WriteLine("Введите Id покупателя:");
                        string clientId = Console.ReadLine()!.Trim(); 

                        if (Int64.TryParse(clientId, out var id))
                        {
                            var response = await client.GetAsync($"https://localhost:5001/Customers/{clientId}");

                            if(response is {StatusCode:System.Net.HttpStatusCode.OK })
                            {
                                var customer = await response.Content.ReadFromJsonAsync<Customer>();
                                Console.WriteLine("Результат запроса:");
                                Console.WriteLine($"Id={customer.Id}");
                                Console.WriteLine($"Firstname={customer.Firstname}");
                                Console.WriteLine($"Lastname={customer.Lastname}");
                                Console.WriteLine();

                            } else if (response is {StatusCode:System.Net.HttpStatusCode.NotFound })
                            {
                                Console.WriteLine("Результат запроса:");
                                Console.WriteLine($"Покупатель с Id = {clientId} в базе данных не найден.");
                                Console.WriteLine();

                            }                         
                        }
                    //}
                } else if (itemMenu=="2") 
                {
                    Customer customer_client = new();
                    customer_client = CustomerGenerator.Generator();

                    Console.WriteLine("Сгенерированный новый клиент:");
                    Console.WriteLine($"Id={customer_client.Id}");
                    Console.WriteLine($"Firstname={customer_client.Firstname}");
                    Console.WriteLine($"Lastname={customer_client.Lastname}");
                    Console.WriteLine();

                    var response = await client.PostAsync("https://localhost:5001/Customers/", customer_client, new JsonMediaTypeFormatter());

                    if (response is { StatusCode: System.Net.HttpStatusCode.OK })
                    {
                        var customer = await response.Content.ReadFromJsonAsync<Customer>();
                        Console.WriteLine("Запись в базу данных вновь созданного покупателя прошла успешно:");
                        Console.WriteLine($"Id={customer.Id}");
                        Console.WriteLine($"Firstname={customer.Firstname}");
                        Console.WriteLine($"Lastname={customer.Lastname}");
                        Console.WriteLine();
                    }else if (response is { StatusCode: System.Net.HttpStatusCode.Conflict }) //409 ошибка
                    {
                        Console.WriteLine("Ошибка записи в базу данных:");
                        Console.WriteLine($"Покупатель с Id = {customer_client.Id} в базе данных уже существует.");
                        Console.WriteLine();
                    }

                }
                else
                {
                    break;
                }

            }
                      
        }

        private static CustomerCreateRequest RandomCustomer()
        {            
            throw new NotImplementedException();
        }
    }
}