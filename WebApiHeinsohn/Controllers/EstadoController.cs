using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiHeinsohn.Data;
using WebApiHeinsohn.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiHeinsohn.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class EstadoController : ControllerBase
    {
        private readonly EstadoData _estadoData;

        public EstadoController(EstadoData estadoData)
        {
            _estadoData = estadoData;
        }

        /// <summary>
        /// Método que obtiene todos los estados activos o inactivos que se encuentran en el sistema
        /// </summary>
        /// <returns>Listado de estados en el sistema</returns>
        // GET: api/<EstadoController>/GetAll/false
        [HttpGet]
        public async Task<IActionResult> GetAll(bool? activo)
        {
            List<Estado> lstEstado = await _estadoData.GetAll(activo);

            return StatusCode(StatusCodes.Status200OK, lstEstado);
        }

        /// <summary>
        /// Método que retorna un estado que corresponde al ID consultado
        /// </summary>
        /// <param name="id">Parámetro que corresponde al ID a filtrar</param>
        /// <returns>Un estado en particular que corresponde al ID filtrado</returns>
        // GET api/<EstadoController>/GetById/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Estado objEstado = await _estadoData.GetById(id);
            return StatusCode(StatusCodes.Status200OK, objEstado);
        }

        /// <summary>
        /// Método que retorna un listado de estado que corresponde a un filtro ingresado
        /// </summary>
        /// <param name="id">Parámetro que corresponde al criterio a filtrar</param>
        /// <returns>Listado de estados que cumplen el filtro</returns>
        // GET api/<EstadoController>/GetByFilter/5
        [HttpGet("{filter}")]
        public async Task<IActionResult> GetByFilter(string filter)
        {
            List<Estado> lstEstado = await _estadoData.GetByFilter(filter);
            return StatusCode(StatusCodes.Status200OK, lstEstado);
        }

        /// <summary>
        /// Método que se encarga de crear o modificar un estado en el sistema
        /// </summary>
        /// <param name="objEstado"></param>
        /// <returns>Confirmación (true o false) de proceso realizado</returns>
        // POST api/<EstadoController>/AddOrEdit
        [HttpPost]
        public async Task<IActionResult> AddOrEdit([FromBody] Estado objEstado)
        {
            bool response = await _estadoData.AddOrEdit(objEstado);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = response });
        }

        /// <summary>
        /// Método que se usa para eliminar un estado en el sistema
        /// </summary>
        /// <param name="id">Id correspondiente al estado a eliminar</param>
        /// <returns>Confirmación (true o false) de proceso realizado</returns>
        // DELETE api/<EstadoController>/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool response = await _estadoData.Delete(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = response });
        }
    }
}
