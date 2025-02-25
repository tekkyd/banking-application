
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankingApplication.CustomAttribute;
using System;
using SimpleHashing.Net;
using BankingApplication.Wrapper;

namespace BankingApplication.Controllers;

[CustomerAuthentication]
public class CustomerController : Controller
{
    private readonly BankingApplicationContext _context;
    private readonly ISessionWrapper _session;

    // get customerID from session
    private int CustomerID => _session.GetInt32(nameof(Customer.CustomerID));


    public CustomerController(BankingApplicationContext context, ISessionWrapper session)
    {
        _context = context;
        _session = session;

    }

    // Action to display the customer accounts
    public IActionResult Index()
    {
        var customer = _context.Customers.Include(x => x.Accounts).
            FirstOrDefault(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    // Action to display customer profile
    public IActionResult Profile() => View(_context.Customers.Find(CustomerID));

}