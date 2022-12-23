using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Getting all Villas");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<VillaDTO>(villaList));
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id==0)
            {
                _logger.LogError("Get Villa Error with Id" + id);
                return BadRequest();
            }
            var villa =await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa==null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<List<VillaDTO>>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>>CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
          
            if (await _db.Villas.FirstOrDefaultAsync(u=>u.Name.ToLower()== createDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CutomError", "Villa already exit");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if(villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            Villa model = _mapper.Map<Villa>(createDTO);
            //Villa model = new()
            //{
            //    Name= createDTO.Name,
            //    Occupancy= createDTO.Occupancy,
            //    Details= createDTO.Details,
            //    Amenity= createDTO.Amenity,
            //    ImageUrl= createDTO.ImageUrl,
            //    Rate= createDTO.Rate,
            //    Sqft= createDTO.Sqft
            //};
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla",new { id=model.Id}, model);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<IActionResult> DeleteVilla(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id==id);
            if (villa==null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
           await _db.SaveChangesAsync();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            Villa model = _mapper.Map<Villa>(updateDTO);
            //Villa model = new()
            //{
            //    Id = updateDTO.Id,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Details = updateDTO.Details,
            //    Amenity = updateDTO.Amenity,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft
            //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa =await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
            //VillaUpdateDTO villaDTO = new()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Details = villa.Details,
            //    Amenity = villa.Amenity,
            //    ImageUrl = villa.ImageUrl,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft
            //};
            if (villa==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO,ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);
            //Villa model = new()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Details = villaDTO.Details,
            //    Amenity = villaDTO.Amenity,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft
            //};
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

    }
}
