using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebPruebaHeinsohn.Models;

namespace WebPruebaHeinsohn.Controllers
{
    public class EstadoController : Controller
    {
        private readonly HttpClient _httpClient;

        public EstadoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44343/api");
        }

        /// <summary>
        /// Método principal en el cual se consultan y se cargan los estados activos registrados en el sistema
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Estado/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var estados = JsonConvert.DeserializeObject<IEnumerable<EstadoViewModel>>(content);

                return View("Index", estados);
            }

            return View(new List<EstadoViewModel>());
        }

        /// <summary>
        /// Llamado a la vista de creación de Estado
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Método para creación de estados 
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(EstadoViewModel estado)
        { 
            // Se valida si los datos están diligenciados de forma correcta
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(estado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Estado/AddOrEdit", content);

                if (response.IsSuccessStatusCode)
                {
                    // Se retorna al listado de estados
                    return RedirectToAction("Index");
                }
                else
                {
                    // En caso de error se muestra el mensaje correspondiente de error.
                    ModelState.AddModelError(string.Empty, "Error al crear el estado.");
                }
            }
            return View(estado);
        }

        /// <summary>
        /// Llamado a la vista para cargar formulario para editar los estados
        /// </summary>
        /// <param name="id">Id del Estado a editar</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Estado/GetById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var estado = JsonConvert.DeserializeObject<EstadoViewModel>(content);

                // Devuelve la vista de edición con los detalles del estado.
                return View(estado);
            }
            else
            {
                // Redireccionamos a la vista de listado de estados en caso de error
                return RedirectToAction("Index"); // Redirige a la página con el listado de estados.
            }
        }

        /// <summary>
        /// Método que realiza el proceso de modificación de un estado correspondiente
        /// </summary>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EstadoViewModel estado)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(estado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"/api/Estado/AddOrEdit", content);

                if (response.IsSuccessStatusCode)
                {
                    // En caso de actualización exitosa, se redirige a la pagina de index.
                    return RedirectToAction("Index");
                }
                else
                {
                    // En caso de error mostramos un mensaje de error.
                    ModelState.AddModelError(string.Empty, "Error al actualizar el estado.");
                }
            }

            // Si hay errores de validación, vuelve a mostrar el formulario de edición con los errores.
            return View(estado);
        }

        /// <summary>
        /// Método que realiza proceso de eliminación (logica) de un estado correspondiente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Estado/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Maneja el caso de eliminación exitosa, por ejemplo, redirigiendo a la página de lista de estados.
                return RedirectToAction("Index");
            }
            else
            {
                // Maneja el caso de error en la solicitud DELETE, por ejemplo, mostrando un mensaje de error.
                TempData["Error"] = "Error al eliminar el estado.";
                return RedirectToAction("Index");
            }
        }
    }
}
