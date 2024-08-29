using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Service.CityRepo;

namespace SolarWatchORM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepo _cityRepo;

        public CitiesController(ICityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] City city)
        {
            if (city == null)
            {
                return BadRequest("City data is null.");
            }

            await _cityRepo.AddNewCity(city);
            return CreatedAtAction(nameof(GetCityById), new { id = city.Id }, city);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] City city)
        {
            if (id != city.Id)
            {
                return BadRequest("City ID mismatch.");
            }

            bool success = await _cityRepo.UpdateCity(id, city);

            if(!success)
            {
                return NotFound("Wrong input details");
            }

            return Ok($"Successfully updated city with id: {id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            bool success = await _cityRepo.DeleteCity(id);
            if (!success)
            {
                return NotFound("Wrong input details");
            }

            return Ok($"Successfully deleted city with id: {id}");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _cityRepo.GetCityById(id);
            if (city == null)
            {
                return NotFound($"No city found with id: {id}");
            }

            return Ok(city);
        }
    }
}
