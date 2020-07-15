using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace webApiATSA.DataB
{
    public class firmaDigital
    {
        public IConfiguration config { get; set; }

        public String conecction
        {
            get { return config.GetConnectionString("DefaultConnectionString"); }
        }

        //WEB
        public DataSet getAll()
        {
            //String connectionstring = config.GetConnectionString("DefaultConnectionString");
            SqlConnection cn = new SqlConnection(conecction);

            cn.Open();

            SqlCommand com = new SqlCommand("SP_firmaDigital_getAll", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            cn.Close();

            return ds;
        }
        public DataSet getId(FirmaDigitalModel modelo)
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
                    SqlCommand com = new SqlCommand("SP_firmaDigital_getId", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id_firma", modelo.Id_firma);
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
        public String setFirmaDigitalIns(FirmaDigitalModel modelo)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigital_INS", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id_usuario", modelo.id_usuario);
                    String n = (String)com.ExecuteScalar();

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
        public String setFirmaDigitalUpd(FirmaDigitalModel modelo)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigital_UPD", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id_firma", modelo.Id_firma);
                    com.ExecuteNonQuery();

                    transaction.Commit();
                    valorReturn = Convert.ToString(modelo.Id_firma);
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
        public DataSet getFirmaDigitalAllFil(String NombresCompletos, Int32 id_gerencia)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_firmaDigital_getFilt", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NombresCompletos", NombresCompletos);
            com.Parameters.AddWithValue("@id_gerencia", id_gerencia);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cn.Close();

            return ds;
        }
        public void setEstadoFirmaDigitalUP(FirmaDigitalModel modelo)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_estadoFirmaDigital_UPD", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id_firma", modelo.Id_firma);
            com.Parameters.AddWithValue("@estado", modelo.estado);
            com.ExecuteNonQuery();
            cn.Close();
        }
        public DataSet getFirmaDigitalHistAll()
        {
            SqlConnection cn = new SqlConnection(conecction);

            cn.Open();

            SqlCommand com = new SqlCommand("SP_firmaDigitalHist_getAll", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            cn.Close();

            return ds;
        }
        public DataSet getFirmaDigitalHistAllFil(String NombresCompletos, int id_gerencia, String id_firma, DateTime fechaDesde, DateTime fechaHasta)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_firmaDigitalHist_getFilt", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NombresCompletos", NombresCompletos);
            com.Parameters.AddWithValue("@id_gerencia", id_gerencia);
            com.Parameters.AddWithValue("@id_firma", id_firma);
            com.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            com.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cn.Close();

            return ds;
        }
    }
}
