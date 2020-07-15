using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Security;
using webApiATSA.Models;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Globalization;
using System.ComponentModel.Design;

namespace webApiATSA.DataB
{
    public class Login
    {

        public IConfiguration config { get; set; }
        public String conecction
        {
            get { return config.GetConnectionString("DefaultConnectionString"); }
        }
        public String setUsuarioLogin(UsuarioModel modelU)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_usuario_getLogin", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Usr", modelU.Usr);
                    com.Parameters.AddWithValue("@Pwd", modelU.Pwd);
                    //String n = (String)com.ExecuteScalar();
                    string n = com.ExecuteScalar().ToString();

                    transaction.Commit();
                    valorReturn = n;
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    valorReturn = "Error| " + ex.Message;
                    transaction.Rollback();
                }
                finally
                {
                    cn.Close();
                }
            }
            return valorReturn;
        }

        public DataSet getId(PersonalModel modelo)
        {
            SqlTransaction transaction = null;
            DataSet valorReturn = null;
            String Error = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_personal_getId", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", modelo.id);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    transaction.Commit();
                    valorReturn = ds;
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    Error = "Error| " + ex.Message;
                    transaction.Rollback();
                }
                finally
                {
                    cn.Close();
                }
            }
            return valorReturn;
        }

        public String setcontrasenaUPD(UsuarioModel modelU)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_contrasena_UPD", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id", modelU.id);
                    com.Parameters.AddWithValue("@Pwd", modelU.Pwd);
                    com.ExecuteNonQuery();

                    transaction.Commit();
                    valorReturn = Convert.ToString(modelU.id);
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    valorReturn = "Error| " + ex.Message;
                    transaction.Rollback();
                }
                finally
                {
                    cn.Close();
                }
            }
            return valorReturn;
        }       

        public DataSet getIdEmail(PersonalModel modelo)
        {
            SqlTransaction transaction = null;
            DataSet valorReturn = null;
            String Error = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_personal_getIdEmail", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Email", modelo.Email);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    transaction.Commit();
                    valorReturn = ds;
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    Error = "Error| " + ex.Message;
                    transaction.Rollback();
                }
                finally
                {
                    cn.Close();
                }
            }
            return valorReturn;
        }

        public async Task<Boolean> Get(String user, String psw)
        {
            try
            {
                string username = user;
                string password = psw;
                string clientId = "805ed463-7ee1-40ab-8541-eb57b4369139";
                //string tenant = "5726fb42-8426-4577-9964-f13a62ad5eb9";
                //string objeto = "070ee50a-dea3-48e8-8983-638397953ef9";

                // Open connection
                string authority = "https://login.microsoftonline.com/atsaperu.com";
                string[] scopes = new string[] { "user.read" };
                IPublicClientApplication app;
                app = PublicClientApplicationBuilder.Create(clientId)
                      .WithAuthority(authority)
                      .Build();
                var accounts = await app.GetAccountsAsync();
                Microsoft.Identity.Client.AuthenticationResult result = null;
                if (accounts.Any())
                {
                    result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                }
                else { 
                    var securePassword = new SecureString();
                    foreach (char c in password.ToCharArray())  // you should fetch the password
                        securePassword.AppendChar(c);  // keystroke by keystroke
                    result = await app.AcquireTokenByUsernamePassword(scopes, username, securePassword).ExecuteAsync();
                }
                //return result.IdToken;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
