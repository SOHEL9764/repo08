﻿using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SampleWebApp.Model
{
    public class DAL
    {
        private readonly string _connectionString;

        public DAL(IConfiguration configuration)
        {
            // Assuming the secret name in Azure Key Vault is "SqlConnectionString"
            _connectionString = configuration["SqlConnectionString"];
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TblUsers", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        User user = new User
                        {
                            Id = Convert.ToString(dt.Rows[i]["Id"]),
                            FirstName = Convert.ToString(dt.Rows[i]["FirstName"]),
                            LastName = Convert.ToString(dt.Rows[i]["LastName"])
                        };
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        public int AddUser(User user)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TblUsers VALUES(@FirstName, @LastName)", con);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public User GetUser(string id)
        {
            User user = new User();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TblUsers WHERE ID = @Id", con);
                da.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    user.Id = Convert.ToString(dt.Rows[0]["Id"]);
                    user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                }
            }
            return user;
        }

        public int UpdateUser(User user)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE TblUsers SET FirstName = @FirstName, LastName = @LastName WHERE ID = @Id", con);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Id", user.Id);
                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public int DeleteUser(string id)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM TblUsers WHERE ID = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }
    }
}
