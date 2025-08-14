using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Million.API.Controllers
{
    /// <summary>
    /// Controlador para transacciones de propiedades
    /// </summary>
    [Route("Propertys")]
    [Produces("application/json")]
    [ApiController]
    [ControllerName("Million Properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _service;

        public PropertiesController(IPropertyService service) => _service = service;

        // <summary>
        /// Registrar las propiedades.
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpPost]
        [Authorize]
        [SwaggerOperation(
            Summary = "Registrar Propiedades ",
            Description = "Registra Todas las propiedades que se requieran"
        )]
        [SwaggerResponse(201, "las propiedades fueron guardadas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(typeof(PagedResult<PropertyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId,
            [FromBody] CreatePropertyRequest req, CancellationToken ct)
        {
            var dto = await _service.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.IdProperty }, dto);
        }

        // <summary>
        /// Actualizar las propiedades.
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpPut("{id:int}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Actualizar Propiedades ",
            Description = "Actualizar las propiedades que se requieran"
        )]
        [SwaggerResponse(201, "las propiedades fueron guardadas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId, int id, 
            [FromBody] UpdatePropertyRequest req, CancellationToken ct)
        {
            var dto = await _service.UpdateAsync(id, req, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        // <summary>
        /// Cambiar las precio de las propiedades.
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpPatch("{id:int}/price")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Cambiar Precio ",
            Description = "Cambiar precio de las propiedades"
        )]
        [SwaggerResponse(201, "las propiedades fueron guardadas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePrice(
            [FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId, 
            int id, [FromBody] ChangePriceRequest req, CancellationToken ct)
        {
            var ok = await _service.ChangePriceAsync(id, req, ct);
            return ok ? NoContent() : NotFound();
        }
        // <summary>
        /// Añadir imagen
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpPost("{id:int}/images")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Añadir Imagen ",
            Description = "Añadir Imagen de las propiedades"
        )]
        [SwaggerResponse(201, "las propiedades fueron guardadas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImage(
            [FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId,
            int id, 
            [FromBody] AddImageRequest req,
            CancellationToken ct)
        {
            var ok = await _service.AddImageAsync(id, req, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// Obtiene las propiedades registradas por su ID.
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Visualizar Propiedades Por Id ",
            Description = "Traer Propiedades Por Id"
        )]
        [SwaggerResponse(200, "las propiedades fueron encontradas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId,
            int id, CancellationToken ct)
        {
            var dto = await _service.GetAsync(id, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Obtiene todos las propiedades registradas.
        /// </summary>
        /// <param name="uuId">Identificador único de la solicitud.</param>
        /// <param name="timestamp">Marca de tiempo de la solicitud.</param>
        /// <param name="systemId">Identificador del sistema que realiza la solicitud.</param>
        /// <param name="request">Dato necesario para obtener la informacion de los catalogos.</param>
        /// <returns>Una respuesta HTTP indicando el resultado de la consulta.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Visualizar Propiedades ",
            Description = "Traer Todas las propiedades"
        )]
        [SwaggerResponse(200, "las propiedades fueron encontradas exitosamente.")]
        [SwaggerResponse(400, "La solicitud es inválida o contiene errores.")]
        [SwaggerResponse(500, "Error interno del servidor.")]
        [ProducesResponseType(typeof(PagedResult<PropertyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(
            [FromHeader] Guid uuId,
            [FromHeader] string timestamp,
            [FromHeader] string systemId,
            [FromQuery] PropertyListRequest req, CancellationToken ct)
        {
            var result = await _service.ListAsync(req, ct);
            Response.Headers["X-Total-Count"] = result.Total.ToString();
            return Ok(result);
        }
    }
}