using System.Data.SqlClient;
using System.Data;
using WebApiHeinsohn.Models;

namespace WebApiHeinsohn.Data
{
    public class TareaEstadoData
    {
        private readonly string cn;

        public TareaEstadoData(IConfiguration configuration)
        {
            cn = configuration.GetConnectionString("Default")!;
        }

        /// <summary>
        /// Operación que obtiene los estados de una tarea en particular
        /// </summary>
        /// <param name="IdTarea">Parametro obligatorio que corresponde al ID de la tarea</param>
        /// <returns></returns>
        public async Task<List<TareaEstado>> GetByIdTarea(int IdTarea)
        {
            List<TareaEstado> lstTareasEstado = new List<TareaEstado>();

            using (var con = new SqlConnection(cn))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("tarea_estado_get", con);
                cmd.Parameters.AddWithValue("tes_tar_id", IdTarea);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lstTareasEstado.Add(new TareaEstado
                        {
                            Id = Convert.ToInt32(reader["tes_id"]),
                            IdTarea = Convert.ToInt32(reader["tes_tar_id"]),
                            IdEstado = Convert.ToInt32(reader["tes_est_id"]),
                            NombreTarea = reader["tar_nombre"].ToString(),
                            NombreEstado = reader["est_nombre"].ToString(),
                            Fecha = Convert.ToDateTime(reader["tes_fecha"])
                        });
                    }
                }
            }
            return lstTareasEstado;
        }
    }
}
