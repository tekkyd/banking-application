using BankingApplication.CustomAttribute;
using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controllers;

[CustomerAuthentication]
public class DepositController : Controller
{
    private readonly BankingApplicationContext _context;
    private readonly ISessionWrapper _session;

    private int CustomerID => _session.GetInt32(nameof(Customer.CustomerID));

    public DepositController(BankingApplicationContext context, ISessionWrapper session)
    {
        _context = context;
        _session = session;
    }

    public IActionResult Index(int id) => View(_context.Accounts.Find(id));

    // Action to handle new deposit transaction
    [HttpPost]
    public IActionResult Index(int id, decimal amount, string comment)
    {
        // validation
        var account = _context.Accounts.Find(id);
        // Check if amount is greater than 0
        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        // if valid, round the amount to 2 decimals
        else if (decimal.Round(amount, 2) != amount)
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        // Check if comment satisfies business rules
        if (comment != null)
        {
            if (comment.Length > 30)
                ModelState.AddModelError(nameof(comment), "Comment length Exceeded 30 characters ");
        }

        // if failed, return to deposit view
        if (!ModelState.IsValid)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        // Create a transaction object for the deposit
        var transaction = new Transaction
        {
            TransactionType = "D",
            AccountNumber = account.AccountNumber,
            Amount = amount,
            Comment = comment,
        };

        // Redirect to confirmation page
        return RedirectToAction("Index", "Confirmation", transaction);

    }
}