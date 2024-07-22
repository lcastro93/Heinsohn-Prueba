using System.Data;
using System.Data.SqlClient;
using System.Security;
using WebApiHeinsohn.Models;

namespace WebApiHeinsohn.Data
{
    public class EstadoData
    {
        private readonly string cn;

        public EstadoData(IConfiguration configuration)
        {
            cn = configuration.GetConnectionString("Default")!;
        }

        /// <summary>
        /// Método que obtiene todos los estados que se encuentran en el sistema
        /// </summary>
        /// <param name="State">Parametros opcional si desea todos, o solo los activos o inactivos</param>
        /// <returns>Listado de estados</returns>
        public async Task<List<Estado>> GetAll(bool? State)
        {
            List<Estado> lstEstados = new List<Estado>();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("estado_get", con);
                cmd.Parameters.AddWithValue("est_estado", State);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lstEstados.Add(new Estado
                        {
                            Id = Convert.ToInt32(reader["est_id"]),
                            Nombre = reader["est_nombre"].ToString(),
                            Activo = Convert.ToBoolean(reader["est_estado"].ToString())
                        });
                    }
                }
            }
            return lstEstados;
        }

        /// <summary>
        /// Método que retorna el objeto que corresponda al Id enviado
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Un estado que coincida con el ID</returns>
        public async Task<Estado> GetById(int? Id)
        {
            Estado objEstado = new Estado();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("estado_get", con);
                cmd.Parameters.AddWithValue("est_id", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objEstado = new Estado
                        {
                            Id = Convert.ToInt32(reader["est_id"]),
                            Nombre = reader["est_nombre"].ToString(),
                            Activo = Convert.ToBoolean(reader["est_estado"].ToString())
                        };
                    }
                }
            }
            return objEstado;
        }

        /// <summary>
        /// Método que obtiene todos los estados que cumplen el criterio de búsqueda ingresado
        /// </summary>
        /// <param name="filter">Filtro o criterio correspondiente de los estados</param>
        /// <returns>Listado de Estados</returns>
        public async Task<List<Estado>> GetByFilter(string? filter)
        {
            List<Estado> lstEstados = new List<Estado>();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("estado_get", con);
                cmd.Parameters.AddWithValue("est_criterio", filter);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lstEstados.Add(new Estado
                        {
                            Id = Convert.ToInt32(reader["est_id"]),
                            Nombre = reader["est_nombre"].ToString(),
                            Activo = Convert.ToBoolean(reader["est_estado"].ToString())
                        });
                    }
                }
            }
            return lstEstados;
        }

        /// <summary>
        /// Método que realiza creación o modificación de los estados en el sistema
        /// </summary>
        /// <param name="objEstado">Parametro con la información para la creación o modificación del estado</param>
        /// <returns>Confirmación (true o false) de la realización del proceso</returns>
        public async Task<bool> AddOrEdit(Estado objEstado)
        {
            bool response = true;

            using (var con = new SqlConnection(cn))
            {

                SqlCommand cmd = new SqlCommand("estado_add_edit", con);
                cmd.Parameters.AddWithValue("@est_id", objEstado.Id);
                cmd.Parameters.AddWithValue("@est_nombre", objEstado.Nombre);
                cmd.Parameters.AddWithValue("@est_estado", objEstado.Activo);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    response = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    response = false;
                }
            }
            return response;
        }

        /// <summary>
        /// Método que realiza proceso de eliminación de un estado en el sistema
        /// </summary>
        /// <param name="id">Id del estado a eliminar</param>
        /// <returns>Confirmación (true o false) de la realización del proceso</returns>
        public async Task<bool> Delete(int Id)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(cn))
            {

                SqlCommand cmd = new SqlCommand("estado_delete", con);
                cmd.Parameters.AddWithValue("@est_id", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
