using BankingApplication.CustomAttribute;
using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers;

[CustomerAuthentication]
public class AccountController : Controller
{
    private readonly BankingApplicationContext _context;
    private readonly ISessionWrapper _session;
    private int CustomerID => _session.GetInt32(nameof(Customer.CustomerID));

    public AccountController(BankingApplicationContext context, ISessionWrapper session)
    {
        _context = context;
        _session = session;
    }

    // Action to display list of options for user to perform
    public IActionResult Index (int id) => View(_context.Accounts.Find(id));
}