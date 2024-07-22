using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebPruebaHeinsohn.Models;

namespace WebPruebaHeinsohn.Controllers
{
    public class TareaController : Controller
    {
        private readonly HttpClient _httpClient;

        public TareaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44343/api");
        }

        /// <summary>
        /// Método principal en el cual se consultan y se cargan las tareas registradas en el sistema
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Tarea/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tareas = JsonConvert.DeserializeObject<IEnumerable<TareaViewModel>>(content);

                return View("Index", tareas);
            }

            return View(new List<TareaViewModel>());
        }

        /// <summary>
        /// Método que carga el historico de estados de una tarea en particular
        /// </summary>
        /// <param name="id">Id tarea</param>
        /// <returns></returns>
        public async Task<IActionResult> History(int id)
        {

            var response = await _httpClient.GetAsync($"/api/TareaEstado/GetByIdTarea/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tareaEstados = JsonConvert.DeserializeObject<IEnumerable<TareaEstadoViewModel>>(content);

                ViewBag.NombreTareaConsultada = tareaEstados.First(a=> !String.IsNullOrEmpty(a.NombreTarea)).NombreTarea;

                // Devuelve el historico de los estados que corresponden a la tarea.
                return View("History", tareaEstados);
            }
            else
            {
                // Manejar el caso de error al obtener el historico de la tarea.
                return RedirectToAction("Index"); // Redirige a la página de lista de tareas.
            }
        }

        /// <summary>
        /// Llamado a la vista de creación de Tareas
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            var responseEstados = await _httpClient.GetAsync($"api/Estado/GetAll?activo={true}");

            if (responseEstados.IsSuccessStatusCode)
            {
                var contentEstados = await responseEstados.Content.ReadAsStringAsync();
                var estados = JsonConvert.DeserializeObject<IEnumerable<EstadoViewModel>>(contentEstados);

                ViewBag.Estados = estados;
            }

            return View();
        }

        /// <summary>
        /// Operación para realizar proceso de guardado de tareas 
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(TareaViewModel tarea)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(tarea);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Tarea/AddOrEdit", content);

                if (response.IsSuccessStatusCode)
                {
                    // Se retorna al listado de tareas
                    return RedirectToAction("Index");
                }
                else
                {
                    // En caso de error se muestra el mensaje correspondiente de error.
                    ModelState.AddModelError(string.Empty, "Error al crear la tarea.");
                }
            }
            else
            {
                var responseEstados = await _httpClient.GetAsync($"api/Estado/GetAll?activo={true}");

                if (responseEstados.IsSuccessStatusCode)
                {
                    var contentEstados = await responseEstados.Content.ReadAsStringAsync();
                    var estados = JsonConvert.DeserializeObject<IEnumerable<EstadoViewModel>>(contentEstados);

                    ViewBag.Estados = estados;
                }
            }
            return View(tarea);
        }

        /// <summary>
        /// Llamado a la vista para cargar formulario para editar una tarea consultada
        /// </summary>
        /// <param name="id">Id tarea a modificar</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var responseEstados = await _httpClient.GetAsync($"api/Estado/GetAll?activo={true}");

            if (responseEstados.IsSuccessStatusCode)
            {
                var contentEstados = await responseEstados.Content.ReadAsStringAsync();
                var estados = JsonConvert.DeserializeObject<IEnumerable<EstadoViewModel>>(contentEstados);

                ViewBag.Estados = estados;
            }

            var response = await _httpClient.GetAsync($"/api/Tarea/GetById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tarea = JsonConvert.DeserializeObject<TareaViewModel>(content);

                // Devuelve la vista de edición con los detalles de la tarea.
                return View(tarea);
            }
            else
            {
                // Redireccionamos a la vista de index en caso de error
                return RedirectToAction("Index"); // Redirige a la página de index de la tarea.
            }
        }

        /// <summary>
        /// Operación que realiza el proceso de modificación de una tarea correspondiente
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(TareaViewModel tarea)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(tarea);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"/api/Tarea/AddOrEdit", content);

                if (response.IsSuccessStatusCode)
                {
                    // En caso de actualización exitosa, se redirige a la pagina de index.
                    return RedirectToAction("Index");
                }
                else
                {
                    // En caso de error mostramos un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al actualizar la tarea.");
                }
            }
            else
            {
                var responseEstados = await _httpClient.GetAsync($"api/Estado/GetAll?activo={true}");

                if (responseEstados.IsSuccessStatusCode)
                {
                    var contentEstados = await responseEstados.Content.ReadAsStringAsync();
                    var estados = JsonConvert.DeserializeObject<IEnumerable<EstadoViewModel>>(contentEstados);

                    ViewBag.Estados = estados;
                }
            }

            // Si hay errores de validación, vuelve a mostrar el formulario de edición con los errores.
            return View(tarea);
        }

        /// <summary>
        /// Método que realiza proceso de eliminación de una tarea y su histórico de estados
        /// </summary>
        /// <param name="id">ID de tarea a eliminar</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Tarea/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Maneja el caso de eliminación exitosa, por ejemplo, redirigiendo a la página de lista de tareas.
                return RedirectToAction("Index");
            }
            else
            {
                // Maneja el caso de error en la solicitud DELETE, por ejemplo, mostrando un mensaje de error.
                TempData["Error"] = "Error al eliminar la tarea.";
                return RedirectToAction("Index");
            }
        }
    }
}
