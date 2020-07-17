using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using webApiATSA.Models;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace webApiATSA.DataB
{
    public class firmaDigital
    {
        public IConfiguration config { get; set; }

        public String conecction
        {
            get { return config.GetConnectionString("DefaultConnectionString"); }
        }

        String rutaImagen = "http://localhost:50148/Images/Firmas/";
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
        public String setFirmaDigitalIns(FirmaDigitalModel modelo, String extension, String emailU)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            String idFirma = "";
            String dni = "";
            String imagen = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigital_INS", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id_usuario", modelo.id_usuario);
                    com.Parameters.AddWithValue("@extension", extension);
                    //String n = (String)com.ExecuteScalar();
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        idFirma = dr["id_firma"].ToString();
                        dni = dr["dni"].ToString();
                        imagen = dr["imagen"].ToString();
                    }

                    List<string> toE = new List<string>() {
                                   emailU
                    };
                    List<string> ccE = new List<string>() { "" };
                    List<string> ccoE = new List<string>() { "" };
                    string asunto = "Se creó su firma digitalizada - ATSA";
                    string cuerpo = "";
                    cuerpo += "Estimado usuario, </br></br>";
                    cuerpo += "Se informa que su firma ha sido creada satisfactoriamente con el código "+ idFirma + "</br>";
                    cuerpo += "<img src='"+ rutaImagen+imagen + "'/></br>";
                    cuerpo += "En caso Ud. no haya autorizado esta acción o se ha creado su firma incorrectamente por favor de notificar a su gerencia y TI para que se realicen las correciones e investigaciones correspondientes.</br></br>Saludos cordiales.";
                    Boolean email = MailSenderOffice365(toE, ccE, ccoE, asunto, cuerpo);

                    transaction.Commit();
                    valorReturn = dni;
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
        public String setFirmaDigitalUpd(FirmaDigitalModel modelo, String extension, String emailU)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            String idFirma = "";
            String dni = "";
            String imagen = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigital_UPD", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id_firma", modelo.Id_firma);
                    com.Parameters.AddWithValue("@extension", extension);
                    //com.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        idFirma = dr["id_firma"].ToString();
                        dni = dr["dni"].ToString();
                        imagen = dr["imagen"].ToString();
                    }

                    List<string> toE = new List<string>() {
                                   emailU
                    };
                    List<string> ccE = new List<string>() { "" };
                    List<string> ccoE = new List<string>() { "" };
                    string asunto = "Se modificó su firma digitalizada - ATSA";
                    string cuerpo = "";
                    cuerpo += "Estimado usuario, </br></br>";
                    cuerpo += "Se informa que su firma ha sido modificada satisfactoriamente con el código " + idFirma + "</br>";
                    cuerpo += "<img src='" + rutaImagen + imagen + "'/></br>";
                    cuerpo += "En caso Ud. no haya autorizado esta acción o se ha editado su firma incorrectamente por favor de notificar a su gerencia y TI para que se realicen las correciones e investigaciones correspondientes.</br></br>Saludos cordiales.";
                    Boolean email = MailSenderOffice365(toE, ccE, ccoE, asunto, cuerpo);

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

        private static string Host = "smtp.office365.com";
        private static int Port = Convert.ToInt32("587");
        private static string mailInterno = "";
        private static string mailUser = "reportessig@atsaperu.com";
        private static string mailPassword = "Seguridad2019";
        public bool MailSenderOffice365(List<string> to, List<string> CC, List<string> Cco, string Asunto, string Cuerpo)
        {

            bool resp = false;
            try
            {
                //envio correo
                SmtpClient client = new SmtpClient(Host, Port);
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(mailUser, mailPassword);
                MailAddress from = new MailAddress(mailUser, String.Empty, System.Text.Encoding.UTF8);

                //MailAddress to = new MailAddress(e_to);
                MailMessage message = new MailMessage();//e_from,e_to

                message.From = from;

                if (!string.IsNullOrEmpty(mailInterno))
                {
                    message.To.Add(new MailAddress(mailInterno));
                }
                else
                {
                    foreach (var x in to)
                    {
                        if (!String.IsNullOrEmpty(x))
                        {
                            message.To.Add(x);
                        }
                    }

                    foreach (string oCC in CC)
                    {
                        if (!String.IsNullOrEmpty(oCC))
                        {
                            message.CC.Add(oCC);
                        }
                    }

                    // Cco
                    foreach (string oCco in Cco)
                    {
                        if (!String.IsNullOrEmpty(oCco))
                        {
                            message.Bcc.Add(new MailAddress(oCco));
                        }
                    }
                }

                message.IsBodyHtml = true;
                message.Body = Cuerpo;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.Subject = Asunto;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                client.Send(message);

                resp = true;
                return resp;
            }
            catch (SmtpException ex)
            {
                throw new Exception("MailSender : " + ex); // InsertLog.Instanse.Insert("MailSender : " + ex);
                //return false;
            }

        }
    }
}
