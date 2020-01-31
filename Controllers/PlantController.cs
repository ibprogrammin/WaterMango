using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterMangoApp.WaterMangoAppDataSetTableAdapters;

/// <summary>
/// WaterMango PlantController API Controller for RestAPI requests
/// </summary>
namespace WaterMangoApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly ILogger<PlantController> _logger;
        
        // List to store all the plant objects from the database to be passed through the API
        public static List<WaterMangoApp.Plant> Plants;

        public PlantController(ILogger<PlantController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This will function will expose an array of Plant Objects from the database through a RestFul API
        /// in json format
        /// </summary>
        /// <returns>An array of WaterMango.Plant Objects</returns>
        // GET: /Plant
        [HttpGet]
        public IEnumerable<Plant> Get()
        {
            Plants = new List<Plant>();

            // fill the PlantTableAdapter with the data in the plant database table
            PlantTableAdapter plantAdapter = new PlantTableAdapter();
            WaterMangoAppDataSet.PlantDataTable plantTable = plantAdapter.GetData();

            // iterate through the table rows and add the WaterMango.Plant objects to the Plants list
            foreach (WaterMangoAppDataSet.PlantRow row in plantTable.Rows)
            {
                Plants.Add(new Plant(row.Id, row.Name, row.LastWatered, row.SecondsWatered));
            }

            // return the Plants list as an array
            return Plants.ToArray();
        }

        /// <summary>
        /// This function will return a single Plant object from the database in json format through a Rest API.
        /// </summary>
        /// <param name="id">The id of the plant to return</param>
        /// <returns>a single WaterMango.Plant object</returns>
        // GET: /Plant/5
        [HttpGet("{id}", Name = "Get")]
        public Plant Get(int id)
        {
            Plants = new List<Plant>();

            // fill the PlantTableAdapter with the data in the plant database table
            PlantTableAdapter plantAdapter = new PlantTableAdapter();
            WaterMangoAppDataSet.PlantDataTable plantTable = plantAdapter.GetData();

            // iterate through the table rows and add the WaterMango.Plant objects to the Plants list
            foreach (WaterMangoAppDataSet.PlantRow row in plantTable.Rows)
            {
                Plants.Add(new Plant(row.Id, row.Name, row.LastWatered, row.SecondsWatered));
            }

            // return the Plant with the currently requested Id
            return Plants.Where(x => x.Id == id).First();
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Plant
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// Save the plant with the requested Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="plant"></param>
        // PUT: /Plant/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Plant plant)
        {
            Plant currentPlant = Plants.Where(x => x.Id == id).First();
            if(currentPlant != null)
            {
                currentPlant.Name = plant.Name;
                currentPlant.LastWatered = plant.LastWatered;
                currentPlant.SecondsWatered = plant.SecondsWatered;
                currentPlant.Watered = plant.Watered;
            }
        }

        /// <summary>
        /// Water the requested plant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The number of seconds the plant has been watered</returns>
        // PUT: /Plant/5/water
        [HttpPut("{id}/water")]
        public int Water(int id)
        {
            Plants = new List<Plant>();

            // fill the PlantTableAdapter with the data in the plant database table
            PlantTableAdapter plantAdapter = new PlantTableAdapter();
            WaterMangoAppDataSet.PlantDataTable plantTable = plantAdapter.GetData();
            
            // Select the requested Plant object
            WaterMangoAppDataSet.PlantRow plantRow = plantTable.Where(x => x.Id == id).First();

            // Make sure it is not null
            if (plantRow != null)
            {
                // Check that the plant can be watered
                if (Plant.CanPlantBeWatered(plantRow.LastWatered))
                {
                    // iterate the number of seconds the plant has been watered by 1
                    plantRow.SecondsWatered += 1;

                    // if the plant has been watered for 10 seconds or more
                    if(plantRow.SecondsWatered > 9)
                    {
                        // reset the time the plant has been watered and then set the last watered time to Now
                        plantRow.SecondsWatered = 0;
                        plantRow.LastWatered = DateTime.Now;
                    }
                    
                    // update the plant table in the database
                    plantAdapter.Update(plantTable);

                    // return the number of seconds this plant has been watered
                    return plantRow.SecondsWatered;
                }
                else return -1; // Plant cannot be watered within 30 seconds
                
            }
            else return 0;
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
