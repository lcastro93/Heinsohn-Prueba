using System.Data.SqlClient;
using System.Data;
using WebApiHeinsohn.Models;

namespace WebApiHeinsohn.Data
{
    public class TareaData
    {
        private readonly string cn;

        public TareaData(IConfiguration configuration)
        {
            cn = configuration.GetConnectionString("Default")!;
        }

        /// <summary>
        /// Operación que retorna todas las tareas que se encuentran registradas
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tarea>> GetAll()
        {
            List<Tarea> lstTareas = new List<Tarea>();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("tarea_get", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lstTareas.Add(new Tarea
                        {
                            Id = Convert.ToInt32(reader["tar_id"]),
                            Nombre = reader["tar_nombre"].ToString(),
                            IdEstado = Convert.ToInt32(reader["tes_est_id"]),
                            NombreEstado = reader["est_nombre"].ToString()
                        });
                    }
                }
            }
            return lstTareas;
        }

        /// <summary>
        /// Operación que retorna una tarea en especifica consultada por el ID
        /// </summary>
        /// <param name="Id">Id de la tarea a consultar en el sistema</param>
        /// <returns></returns>
        public async Task<Tarea> GetById(int? Id)
        {
            Tarea objTarea = new Tarea();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("tarea_get", con);
                cmd.Parameters.AddWithValue("tar_id", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objTarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["tar_id"]),
                            Nombre = reader["tar_nombre"].ToString(),
                            IdEstado = Convert.ToInt32(reader["tes_est_id"]),
                            NombreEstado = reader["est_nombre"].ToString()
                        };
                    }
                }
            }
            return objTarea;
        }

        /// <summary>
        /// Operación que retorna todas las tareas que cumplan con un filtro o criterio de búsqueda
        /// </summary>
        /// <param name="filter">Criterio con el cual se consultarán las tareas del sistema</param>
        /// <returns></returns>
        public async Task<List<Tarea>> GetByFilter(string? filter)
        {
            List<Tarea> lstTareas = new List<Tarea>();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("tarea_get", con);
                cmd.Parameters.AddWithValue("tar_criterio", filter);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lstTareas.Add(new Tarea
                        {
                            Id = Convert.ToInt32(reader["tar_id"]),
                            Nombre = reader["tar_nombre"].ToString(),
                            IdEstado = Convert.ToInt32(reader["tes_est_id"]),
                            NombreEstado = reader["est_nombre"].ToString()
                        });
                    }
                }
            }
            return lstTareas;
        }

        /// <summary>
        /// Método que realiza creación o modificación de una tarea en el sistema
        /// </summary>
        /// <param name="objEstado">Parametro con la información para la creación o modificación de la tarea</param>
        /// <returns>Confirmación (true o false) de la realización del proceso</returns>
        public async Task<bool> AddOrEdit(Tarea objTarea)
        {
            bool response = true;

            using (var con = new SqlConnection(cn))
            {

                SqlCommand cmd = new SqlCommand("tarea_add_edit", con);
                cmd.Parameters.AddWithValue("@tar_id", objTarea.Id);
                cmd.Parameters.AddWithValue("@tar_nombre", objTarea.Nombre);
                cmd.Parameters.AddWithValue("@tar_est_id", objTarea.IdEstado);
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
        /// Método que realiza proceso de eliminación de una tarea en el sistema
        /// </summary>
        /// <param name="id">Id de la tarea a eliminar</param>
        /// <returns>Confirmación (true o false) de la realización del proceso</returns>
        public async Task<bool> Delete(int Id)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(cn))
            {

                SqlCommand cmd = new SqlCommand("tarea_delete", con);
                cmd.Parameters.AddWithValue("@tar_id", Id);
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
