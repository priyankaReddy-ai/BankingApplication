using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingApplication.Models;

namespace BankingApplication.Controllers
{
    public class HomeController : Controller
    {
        IAccountStatemnt accountStat = new AccountStatemnt();
        ITransctionStatement transactionStat = new TransctionStatement();


        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Transactions()
        {
            return View();
        }
        public ActionResult InitiateTransaction()
        {
            return View();
        }
        #region Accounts Operations
        [HttpGet]
        public ActionResult GetAllAccounts()
        {
           
                var accounts = accountStat.GetAllAccounts();
                var JsonResult = Json(accounts, JsonRequestBehavior.AllowGet);
                JsonResult.MaxJsonLength = int.MaxValue;
                return JsonResult;
        }
        [HttpPost]
        public bool AddUpdateAccount(Account account)
        {
            var isSuccess = accountStat.SaveAccount(account);
            return isSuccess;
        }
        [HttpPost]
        public bool DeleteAccount(int accountID)
        {
            var isSuccess = accountStat.DeleteAccount(accountID);
            return isSuccess;
        }
        #endregion
        #region Transactions

        [HttpGet]
        public ActionResult GetAllTransactions()
        {

            var accounts = transactionStat.GetAllTransactions();
            var JsonResult = Json(accounts, JsonRequestBehavior.AllowGet);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }
        [HttpPost]
        public bool TransferAmount(Transaction transaction)
        {

            var isSuccess = transactionStat.SaveTransaction(transaction);
            return isSuccess;
        }
        #endregion
    }
}
