using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BankingApplication.Models
{

    public interface IAccountStatemnt {

        List<Account> GetAllAccounts();
        bool SaveAccount(Account account);
        bool DeleteAccount(int accountID);
    }
    public class AccountStatemnt: IAccountStatemnt
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["BankingConnection"].ToString();

        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            try
            {
                SqlDataAdapter adapter;
                DataSet ds = new DataSet();
                string query = "Select * from tbl_Accounts where IsDeleted=0 order by Id desc";
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        con.Open();
                        adapter = new SqlDataAdapter(query, con);
                        adapter.Fill(ds);
                    }
                    accounts = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Account
                    {
                        AccountID = dataRow.Field<int>("Id"),
                        AccountName = dataRow.Field<string>("AccountName"),
                        Balance = dataRow.Field<decimal>("Balance"),
                    }).ToList();
                }
            }
            catch (Exception ex)
            { }
            return accounts;
        }

        public bool SaveAccount(Account account)
        {
            bool isSuccess = false;
            try
            {
                string query = string.Empty;
                if (account.AccountID == 0)
                {
                    query = "INSERT INTO tbl_Accounts(AccountName, Balance,IsDeleted) VALUES(@Name, @Balance,@IsDeleted)";
                }
                else
                {
                    query = "Update tbl_Accounts set AccountName=@Name, Balance=@Balance where Id=@AccountId ";

                }
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.Parameters.AddWithValue("@Name", account.AccountName);
                        cmd.Parameters.AddWithValue("@Balance", account.Balance);
                        if (account.AccountID == 0)
                            cmd.Parameters.AddWithValue("@IsDeleted", 0);
                        else
                            cmd.Parameters.AddWithValue("@AccountId", account.AccountID);
                        int result = cmd.ExecuteNonQuery();
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

        public bool DeleteAccount(int accountID)
        {
            bool isSuccess = false;
            try
            {
                string query = "Update tbl_Accounts set IsDeleted=1 where Id=@AccountId ";

                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        con.Open();
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("@AccountId", accountID);
                        int result = cmd.ExecuteNonQuery();

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
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }


    }
}