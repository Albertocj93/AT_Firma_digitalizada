using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using webApiATSA.Models;

namespace webApiATSA.DataB
{
    public class FirmaDigitalApi
    {
        public IConfiguration config { get; set; }

        public String conecction
        {
            get { return config.GetConnectionString("DefaultConnectionString"); }
        }

        String rutaImagen = "http://localhost:50148/Images/Firmas/";
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
                    SqlCommand com = new SqlCommand("SP_usuario_getLoginApi", cn, transaction);
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

        public String setFirmaDigitalHist(FirmaDigitalHistModel modelFd, String id_personal)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigitalHist_INS", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id_Personal", id_personal);
                    com.Parameters.AddWithValue("@nombreDocumento", modelFd.nombreDocumento);
                    com.Parameters.AddWithValue("@sistema", modelFd.sistema);
                    com.Parameters.AddWithValue("@ip", modelFd.ip);
                    com.Parameters.AddWithValue("@localizacion", modelFd.localizacion);
                    string n = com.ExecuteScalar().ToString();

                    PersonalModel us = new PersonalModel();
                    us.id = Convert.ToInt32(id_personal);
                    DataSet ds = getUlt(us, cn, transaction);

                    String email = "";
                    String fecha = "";
                    String id_firma = "";
                    String serie = "";
                    String Psw = "";
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        email = row["Email"].ToString();
                        fecha = row["fecha"].ToString();
                        id_firma = row["Id_firma"].ToString();
                        serie = row["codigoSerie"].ToString();
                        Psw = row["Pwd"].ToString();
                    }
                    //Envío correo
                    List<string> toE = new List<string>() {
                           email
                           //"acarranzaj@atsaperu.com"
                    };
                    List<string> ccE = new List<string>() { "" };
                    List<string> ccoE = new List<string>() { "" };
                    string asunto = "Se registró su firma digitalizada en el docuemtno" + modelFd.nombreDocumento;
                    string cuerpo = "";
                    cuerpo += "Estimado usuario: </br></br>";
                    cuerpo += "Se informa que su firma ha sido registrada satisfactoriamente en el documento: "+ modelFd.nombreDocumento+"</br>";
                    cuerpo += "Fecha/Hora: " + fecha + "</br>";
                    cuerpo += "Código de Firma: " + id_firma + "</br>";
                    cuerpo += "Serie: " + serie + "</br></br>";
                    cuerpo += "En caso Ud. no haya realizado esta acción y se haya asignado su firma incorrectamente a un documento por favor notificar a su gerencia y TI para que se realicen las correcciones e investigaciones correspondientes.</br></br>Saludos cordiales.";
                    Boolean emailSend =  MailSenderOffice365(toE, ccE, ccoE, asunto, cuerpo);

                    transaction.Commit();
                    valorReturn = rutaImagen + n + "| " + id_firma + "| " + serie;
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

        public String setFirmaDigitalHistId(FirmaDigitalHistModel modelFd)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                String fecha = "";
                String nombreDocumento = "";
                String sistema = "";
                String ip = "";
                String localizacion = "";
                String nombres = "";
                String apellidos = "";
                String puestoTrabajo = "";
                String gerencia = "";
                String dni = "";

                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_firmaDigitalHist_getIdApi", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Id_firma", modelFd.Id_firma);
                    com.Parameters.AddWithValue("@codigoSerie", modelFd.codigoSerie);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        fecha = row["fecha"].ToString();
                        nombreDocumento = row["nombreDocumento"].ToString();
                        sistema = row["sistema"].ToString();
                        ip = row["ip"].ToString();
                        localizacion = row["localizacion"].ToString();
                        nombres = row["nombres"].ToString();
                        apellidos = row["apellidos"].ToString();
                        puestoTrabajo = row["puestoTrabajo"].ToString();
                        gerencia = row["gerencia"].ToString();
                        dni = row["dni"].ToString();
                    }

                    transaction.Commit();
                    valorReturn = fecha + "| " + nombreDocumento + "| " + sistema + "| " + ip + "| " + localizacion + "| " +
                        nombres + "| " + apellidos + "| " + puestoTrabajo + "| " + gerencia + "| " + dni;
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

        public DataSet getUlt(PersonalModel modelo, SqlConnection cn, SqlTransaction transaction)
        {
            DataSet valorReturn = null;
                try
                {
                    SqlCommand com = new SqlCommand("SP_firmaDigitalHist_getUlt", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", modelo.id);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    valorReturn = ds;
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    //cn.Close();
                }
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
