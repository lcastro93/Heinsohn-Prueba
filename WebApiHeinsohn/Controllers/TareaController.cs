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
    public class TareaController : ControllerBase
    {
        private readonly TareaData _tareaData;

        public TareaController(TareaData tareaData)
        {
            _tareaData = tareaData;
        }


        /// <summary>
        /// Método que obtiene todas las tareas que se encuentran en el sistema
        /// </summary>
        /// <returns>Listado de tareas en el sistema</returns>
        // GET: api/<TareaController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Tarea> lstTareas = await _tareaData.GetAll();

            return StatusCode(StatusCodes.Status200OK, lstTareas);
        }

        /// <summary>
        /// Método que retorna una tarea que corresponde al ID consultado
        /// </summary>
        /// <param name="id">Parámetro que corresponde al ID a filtrar</param>
        /// <returns>Una tarea en particular que corresponde al ID filtrado</returns>
        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Tarea objTarea = await _tareaData.GetById(id);
            return StatusCode(StatusCodes.Status200OK, objTarea);
        }

        /// <summary>
        /// Método que retorna un listado de tareas que corresponde a un filtro ingresado
        /// </summary>
        /// <param name="filter">Parámetro que corresponde al criterio a filtrar</param>
        /// <returns>Listado de tareas que cumplen el filtro</returns>
        // GET api/<TareaController>/5
        [HttpGet("{filter}")]
        public async Task<IActionResult> GetByFilter(string filter)
        {
            List<Tarea> lstTareas = await _tareaData.GetByFilter(filter);
            return StatusCode(StatusCodes.Status200OK, lstTareas);
        }

        /// <summary>
        /// Método que se encarga de crear o modificar una tarea en el sistema
        /// </summary>
        /// <param name="objTarea"></param>
        /// <returns>Confirmación (true o false) de proceso realizado</returns>
        // POST api/<TareaController>
        [HttpPost]
        public async Task<IActionResult> AddOrEdit([FromBody] Tarea objTarea)
        {
            bool response = await _tareaData.AddOrEdit(objTarea);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = response });
        }

        /// <summary>
        /// Método que se usa para eliminar una tarea en el sistema
        /// </summary>
        /// <param name="id">Id correspondiente de la tarea a eliminar</param>
        /// <returns>Confirmación (true o false) de proceso realizado</returns>
        // DELETE api/<TareaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool response = await _tareaData.Delete(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = response });
        }
    }
}
