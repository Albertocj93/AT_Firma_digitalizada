using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Configuration;
using webApiATSA.Models;
using System.Transactions;

namespace webApiATSA.DataB
{
    public class usuario
    {
        public IConfiguration config { get; set; }

        public String conecction {
            get { return config.GetConnectionString("DefaultConnectionString"); }
        }

        //WEB
        public DataSet getAll()
        {
            //String connectionstring = config.GetConnectionString("DefaultConnectionString");
            SqlConnection cn = new SqlConnection(conecction);

            cn.Open();

            SqlCommand com = new SqlCommand("SP_usuario_getAll", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            cn.Close();

            return ds;
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

        public String setPersonalIns(PersonalModel modelo, UsuarioModel modelU, List<int> lstIdPerfil)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            String idUsuario = "";
            String idPerfil = "";
            using (SqlConnection cn = new SqlConnection(conecction)) { 
                try
                    {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_personal_INS", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@NombresCompletos", modelo.NombresCompletos);
                    com.Parameters.AddWithValue("@apellidos", modelo.apellidos);
                    com.Parameters.AddWithValue("@Email", modelo.Email);
                    com.Parameters.AddWithValue("@CodigoSAP", modelo.CodigoSAP);
                    com.Parameters.AddWithValue("@activo", modelo.activo);
                    com.Parameters.AddWithValue("@MecanicoLinea", modelo.MecanicoLinea);
                    com.Parameters.AddWithValue("@id_gerencia", modelo.id_gerencia);
                    com.Parameters.AddWithValue("@dni", modelo.dni);
                    com.Parameters.AddWithValue("@puestoTrabajo", modelo.puestoTrabajo);
                    com.Parameters.AddWithValue("@licencia", modelo.licencia);
                    //com.ExecuteNonQuery();
                    String n = (String)com.ExecuteScalar();

                    modelU.id_personal = Convert.ToInt32(n);
                    if(modelU.Usr != "") { //Solo si se ingresa a usuario deberá registrar usuario, perfiles y enviar correo.
                        idUsuario = setUsuarioIns(modelU, cn, transaction);

                        UsuarioPerfilModel modelUP = new UsuarioPerfilModel();
                        modelUP.id_usuario = Convert.ToInt32(idUsuario);
                        foreach (var item in lstIdPerfil)
                        {
                            modelUP.id_perfil = item;
                            idPerfil = setUsuarioPerfilIns(modelUP, cn, transaction);
                        }
                    }

                    transaction.Commit();
                    valorReturn = n;
                    return valorReturn;
                }
                catch (Exception ex)
                {
                    valorReturn = "Error| "+ex.Message;
                    transaction.Rollback();
                }
                finally
                {
                    cn.Close();
                }
            }
            return valorReturn;
        }
        public String setUsuarioIns(UsuarioModel modelo, SqlConnection cn, SqlTransaction transaction)
        {
            //SqlConnection cn = new SqlConnection(conecction);
            try {
                SqlCommand com = new SqlCommand("SP_usuario_INS", cn, transaction);
                com.CommandTimeout = 0;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Usr", modelo.Usr);
                com.Parameters.AddWithValue("@Pwd", modelo.Pwd);
                com.Parameters.AddWithValue("@id_personal", modelo.id_personal);
                //com.ExecuteNonQuery();
                //cn.Open();
                String n = (String)com.ExecuteScalar();
                //cn.Close();
                //transaction.Commit();
                return n;
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

        public String setUsuarioUp(UsuarioModel modelo, SqlConnection cn, SqlTransaction transaction)
        {
            //SqlConnection cn = new SqlConnection(conecction);
            try
            {
                SqlCommand com = new SqlCommand("SP_usuario_UPD", cn, transaction);
                com.CommandTimeout = 0;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Usr", modelo.Usr);
                com.Parameters.AddWithValue("@Pwd", modelo.Pwd);
                com.Parameters.AddWithValue("@id_personal", modelo.id_personal);
                //com.ExecuteNonQuery();
                //cn.Open();
                String n = (String)com.ExecuteScalar();
                //cn.Close();
                //transaction.Commit();
                return n;
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
        public String setUsuarioPerfilIns(UsuarioPerfilModel modelo, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                SqlCommand com = new SqlCommand("SP_usuarioPerfil_INS", cn, transaction);
                com.CommandTimeout = 0;
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id_usuario", modelo.id_usuario);
                com.Parameters.AddWithValue("@id_perfil", modelo.id_perfil);
                String n = (String)com.ExecuteScalar();
                return n;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public String setPersonalUpd(PersonalModel modelo, UsuarioModel modelU, List<int> lstIdPerfil, String chk)
        {
            SqlTransaction transaction = null;
            String valorReturn = "";
            String idUsuario = "";
            String idPerfil = "";
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    SqlCommand com = new SqlCommand("SP_personal_UPD", cn, transaction);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", modelo.id);
                    com.Parameters.AddWithValue("@NombresCompletos", modelo.NombresCompletos);
                    com.Parameters.AddWithValue("@apellidos", modelo.apellidos);
                    com.Parameters.AddWithValue("@Email", modelo.Email);
                    com.Parameters.AddWithValue("@CodigoSAP", modelo.CodigoSAP);
                    com.Parameters.AddWithValue("@activo", modelo.activo);
                    com.Parameters.AddWithValue("@MecanicoLinea", modelo.MecanicoLinea);
                    com.Parameters.AddWithValue("@id_gerencia", modelo.id_gerencia);
                    com.Parameters.AddWithValue("@dni", modelo.dni);
                    com.Parameters.AddWithValue("@puestoTrabajo", modelo.puestoTrabajo);
                    com.Parameters.AddWithValue("@licencia", modelo.licencia);
                    com.Parameters.AddWithValue("@chk", chk);
                    com.ExecuteNonQuery();
                    //String n = (String)com.ExecuteScalar();

                    modelU.id_personal = Convert.ToInt32(modelo.id);
                    if (modelU.Usr != "")
                    { //Solo si se ingresa a usuario deberá registrar usuario, perfiles y enviar correo.
                        if(chk == "on") 
                            idUsuario = setUsuarioIns(modelU, cn, transaction);
                        else
                            idUsuario = setUsuarioUp(modelU, cn, transaction);

                        UsuarioPerfilModel modelUP = new UsuarioPerfilModel();
                        modelUP.id_usuario = Convert.ToInt32(idUsuario);
                        foreach (var item in lstIdPerfil)
                        {
                            modelUP.id_perfil = item;
                            idPerfil = setUsuarioPerfilIns(modelUP, cn, transaction);
                        }
                    }

                    transaction.Commit();
                    valorReturn = Convert.ToString(modelo.id);
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

        public void setEstadoPersonalUP(PersonalModel modelo)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_estadoPersonal_UPD", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id", modelo.id);
            com.Parameters.AddWithValue("@activo", modelo.activo);
            com.ExecuteNonQuery();
            cn.Close();
        }

        public DataSet getGerenciaAll()
        {
            SqlConnection cn = new SqlConnection(conecction);

            cn.Open();

            SqlCommand com = new SqlCommand("SP_gerencia_getAll", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dsg = new DataSet();
            da.Fill(dsg);

            cn.Close();

            return dsg;
        }
        public DataSet getPerfilAll()
        {
            SqlConnection cn = new SqlConnection(conecction);

            cn.Open();

            SqlCommand com = new SqlCommand("SP_perfil_getAll", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dsg = new DataSet();
            da.Fill(dsg);

            cn.Close();

            return dsg;
        }
        public DataSet getUsuarioAllFil(PersonalModel modelo)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_usuario_getFilt", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NombresCompletos", modelo.NombresCompletos);
            com.Parameters.AddWithValue("@id_gerencia", modelo.id_gerencia);
            com.Parameters.AddWithValue("@activo", modelo.activo);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cn.Close();

            return ds;
        }
        public DataSet getUsuarioPerfilGetIdUsu(int id_usuario)
        {
            SqlConnection cn = new SqlConnection(conecction);
            cn.Open();
            SqlCommand com = new SqlCommand("SP_usuarioPerfil_getIdUs", cn);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@id_usuario", id_usuario);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dsg = new DataSet();
            da.Fill(dsg);

            cn.Close();

            return dsg;
        }

        //API
        public async Task<List<UsuarioModel>> GetAll()
        {
            using (SqlConnection cn = new SqlConnection(conecction))
            {
                using (SqlCommand com = new SqlCommand("spUsuarioSEL", cn))
                {
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<UsuarioModel>();
                    await cn.OpenAsync();

                    using (var reader = await com.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        private UsuarioModel MapToValue(SqlDataReader reader)
        {
            return new UsuarioModel()
            {
                //codigoUsuario = (String)reader["codigoUsuario"],
                //nombres = (String)reader["nombres"],
                //apellidos = (String)reader["apellidos"],
                //telefono = (int)reader["telefono"]
            };
        }
    }
}
