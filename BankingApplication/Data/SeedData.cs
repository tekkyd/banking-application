using BankingApplication.Models;
using Newtonsoft.Json;
namespace BankingApplication.Data;

public static class SeedData
{

    public static void InitDb(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<BankingApplicationContext>();

        // check if DB is already seeded
        if (context.Customers.Any())
            return;

        // Deserialise JSON file
        var customers = JSONDeserialise();


        // insert customers in DB 
        foreach (var customer in customers)
        {
            context.Customers.Add(
                new Customer
                {
                    CustomerID = customer.CustomerID,
                    Name = customer.Name,
                    Address = customer.Address,
                    City = customer.City,
                    Postcode = customer.Postcode,
                    Islocked = false
                }
            ); ;
            // insert accounts in DB
            foreach (var account in customer.Accounts)
            {
                context.Accounts.Add(
                    new Account
                    {
                        AccountNumber = account.AccountNumber,
                        AccountType = account.AccountType,
                        CustomerID = account.CustomerID,
                        Balance = getInitialBalance(account)
                    }
                ); ;
                // insert transactions into DB
                foreach (var transaction in account.Transactions)
                {

                    context.Transactions.Add(
                        new Models.Transaction
                        {   
                            TransactionType = "D",
                            AccountNumber = account.AccountNumber,
                            Amount = transaction.Amount,
                            Comment = transaction.Comment,
                            TransactionTimeUtc = transaction.TransactionTimeUtc
                        }
                    );
                }
            }
            // insert login into DB
            context.Logins.Add(
                new Login
                {
                    LoginID = customer.Login.LoginID,
                    CustomerID = customer.CustomerID,
                    PasswordHash = customer.Login.PasswordHash
                }
            );

            // commit to DB 
            context.SaveChanges();
        }
    }

    // get the initial balance for each account 
    public static decimal getInitialBalance(Account account)
    {
        decimal balance = 0;

        foreach(var transaction in account.Transactions)
        {
            balance += transaction.Amount;
        }

        return balance;
    }




    

public static List<Customer> JSONDeserialise()
{
    // JSON String
    string json = @"
    [
        {
            ""CustomerID"": 1001,
            ""Name"": ""Derek Tek"",
            ""Address"": ""123 Melbourne Street"",
            ""City"": ""Melbourne"",
            ""PostCode"": ""3000"",
            ""Accounts"": [
                {
                    ""AccountNumber"": 1100,
                    ""AccountType"": ""S"",
                    ""CustomerID"": 1001,
                    ""Transactions"": [
                        {
                            ""Amount"": 1000,
                            ""Comment"": ""Opening balance"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:00:00 PM""
                        }
                    ]
                },
                {
                    ""AccountNumber"": 1200,
                    ""AccountType"": ""C"",
                    ""CustomerID"": 1001,
                    ""Transactions"": [
                        {
                            ""Amount"": 1500,
                            ""Comment"": ""First deposit"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:30:00 PM""
                        },
                        {
                            ""Amount"": 5000,
                            ""Comment"": ""Second deposit"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:45:00 PM""
                        }
                    ]
                }
            ],
            ""Login"": {
                ""LoginID"": ""1234"",
                ""PasswordHash"": ""Rfc2898DeriveBytes$50000$MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE=""
            }
        },
        {
            ""CustomerID"": 1002,
            ""Name"": ""Guest User"",
            ""Address"": ""123 Guest Street"",
            ""City"": ""Sydney"",
            ""PostCode"": ""2200"",
            ""Accounts"": [
                {
                    ""AccountNumber"": 2100,
                    ""AccountType"": ""S"",
                    ""CustomerID"": 1002,
                    ""Transactions"": [
                        {
                            ""Amount"": 1000,
                            ""Comment"": ""Opening balance"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:00:00 PM""
                        }
                    ]
                },
                {
                    ""AccountNumber"": 2200,
                    ""AccountType"": ""C"",
                    ""CustomerID"": 1002,
                    ""Transactions"": [
                        {
                            ""Amount"": 1500,
                            ""Comment"": ""First deposit"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:30:00 PM""
                        },
                        {
                            ""Amount"": 5000,
                            ""Comment"": ""Second deposit"",
                            ""TransactionTimeUtc"": ""02/01/2025 08:45:00 PM""
                        }
                    ]
                }
            ],
            ""Login"": {
                ""LoginID"": ""0000"",
                ""PasswordHash"": ""Rfc2898DeriveBytes$50000$MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE=""
            }
        }
    ]";

    // Deserialize JSON String
    List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
    {
        DateFormatString = "dd/MM/yyyy hh:mm:ss tt"
    });

    return customers;
}


}