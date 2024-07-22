using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHeinsohn.Data;
using WebApiHeinsohn.Models;

namespace WebApiHeinsohn.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class TareaEstadoController : ControllerBase
    {
        private readonly TareaEstadoData _tareaEstadoData;

        public TareaEstadoController(TareaEstadoData tareaEstadoData)
        {
            _tareaEstadoData = tareaEstadoData;
        }

        /// <summary>
        /// Método que retorna el listado de estados relacionados a una tarea
        /// </summary>
        /// <param name="id">Parámetro que corresponde al ID de la tarea a consultar</param>
        /// <returns>Listado de estados que corresponden a la tarea consultada</returns>
        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTarea(int id)
        {
            List<TareaEstado> lstTareaEstados = await _tareaEstadoData.GetByIdTarea(id);
            return StatusCode(StatusCodes.Status200OK, lstTareaEstados);
        }
    }
}
