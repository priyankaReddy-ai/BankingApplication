using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BankingApplication.Models
{
    public interface ITransctionStatement
    {

        List<Transaction> GetAllTransactions();
        bool SaveTransaction(Transaction transaction);
    }
    public class TransctionStatement: ITransctionStatement
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["BankingConnection"].ToString();
        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            try
            {

                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                string query = "Select t1.Id,a.AccountName 'FromAccount',b.AccountName 'ToAccount',t1.TransactionTime ,t1.AmountDebited,a.Balance 'FromAccBal',b.Balance 'ToAccBal' from tbl_Accounts  a join tbl_Transactions t1 on t1.FromAccount=a.Id ";
query+="join tbl_Accounts b on t1.ToAccount = b.Id where a.IsDeleted = 0 and b.IsDeleted = 0 order by Id desc";
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        con.Open();
                        adapter = new SqlDataAdapter(query, con);
                        adapter.Fill(ds);
                    }
                    transactions = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Transaction
                    {
                        TransactionID = dataRow.Field<int>("Id"),
                        FromAccount = dataRow.Field<string>("FromAccount"),
                        ToAccount = dataRow.Field<string>("ToAccount"),
                        TransactionTime = dataRow.Field<DateTime>("TransactionTime"),
                        AmountDebited = dataRow.Field<decimal>("AmountDebited"),
                        FromAccBal = dataRow.Field<decimal>("FromAccBal"),
                        ToAccBal = dataRow.Field<decimal>("ToAccBal")
                    }).ToList();
                }
            }
            catch (Exception ex)
            { }
            return transactions;
        }

        public bool SaveTransaction(Transaction transaction)
        {
            bool isSuccess = false;
            try
            {
                DateTime currentDate = DateTime.Now;
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_InitiateTransaction", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FromAccount", transaction.FromAccount));
                        cmd.Parameters.Add(new SqlParameter("@ToAccount", transaction.ToAccount));
                        cmd.Parameters.Add(new SqlParameter("@TransactionTime", currentDate));
                        cmd.Parameters.Add(new SqlParameter("@AmountDebited", transaction.AmountDebited));
                        cmd.ExecuteScalar();
                        con.Close();
                        isSuccess = true;
                    }
                   
                }
            }
            catch (Exception ex)
            {
            }
            return isSuccess;
        }
    }


    public class Transaction
    {
        public int TransactionID { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime TransactionTime { get; set; }
        public decimal AmountDebited { get; set; }
        public decimal FromAccBal { get; set; }
        public decimal ToAccBal { get; set; }



        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }


    }
}