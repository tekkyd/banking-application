using BankingApplication.CustomAttribute;
using BankingApplication.Data;
using BankingApplication.Models;
using BankingApplication.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BankingApplication.Controllers;

[CustomerAuthentication]
public class StatementsController : Controller
{
    private readonly BankingApplicationContext _context;
    private readonly ISessionWrapper _session;

    private int CustomerID => _session.GetInt32(nameof(Customer.CustomerID));

    public StatementsController(BankingApplicationContext context, ISessionWrapper session)
    {
        _context = context;
        _session = session;
    }

    public IActionResult Index(int id, int page = 1)
    {
        // the number of transactions per page
        const int pageSize = 6;
        // Page list to iterate over
        var pagedList = _context.Transactions.Where(x => x.AccountNumber == id).
        Include(x => x.Account).Where(x => x.AccountNumber == id).
        OrderByDescending(x => x.TransactionTimeUtc).ToPagedList(page, pageSize);


        return View(pagedList);
    }   

}