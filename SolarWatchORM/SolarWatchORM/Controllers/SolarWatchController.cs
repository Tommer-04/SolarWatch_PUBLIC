using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatchORM.Data;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;
using SolarWatchORM.Service.CityDataProvider;
using SolarWatchORM.Service.CityRepo;
using SolarWatchORM.Service.SunDataProvider;
using SolarWatchORM.Service.SunRepo;

namespace SolarWatchORM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class SolarWatchController : ControllerBase
    {

        private readonly ILogger<SolarWatchController> _logger;
        private readonly SolarWatchContext _context;
        private readonly ICityDataProvider _cityDataProvider;
        private readonly ICityRepo _cityRepo;
        private readonly ISunDataProvider _sunDataProvider;
        private readonly ISunRepo _sunRepo;
        public SolarWatchController(ILogger<SolarWatchController> logger, SolarWatchContext context, ICityDataProvider cityDataProvider, ICityRepo cityRepo, ISunDataProvider sunDataProvider, ISunRepo sunRepo)
        {
            _logger = logger;
            _context = context;
            _cityDataProvider = cityDataProvider;
            _cityRepo = cityRepo;
            _sunDataProvider = sunDataProvider;
            _sunRepo = sunRepo;
        }



        [HttpGet("getData")]
        public async Task<IActionResult> GetSunData(string CityName, DateOnly Date)
        {
            City? city = await _cityRepo.SearchByName(CityName);

            if (city == null)
            {
                city = await _cityDataProvider.ProvideData(CityName);

                if (city == null)
                {
                    return NotFound("No city can be found with the given name!");
                }

                if(await _cityRepo.SearchByName(city.Name) == null)
                {
                    await _cityRepo.AddNewCity(city);
                }
                
                city = await _cityRepo.SearchByName(city.Name);
            }

            Sun? sun = await _sunRepo.SearchByCityAndDate(city, Date);

            if (sun == null)
            {
                sun = await _sunDataProvider.ProvideData(city, Date);

                if (sun == null)
                {
                    return NotFound("Invalid date!");
                }

                await _sunRepo.AddNewSunRecord(sun);
            }

            return Ok(sun);
        }

    }
}
