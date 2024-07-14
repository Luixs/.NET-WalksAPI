using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Walks.API.CustomActionFilters;
using Walks.API.Data;
using Walks.API.Models.Domain;
using Walks.API.Models.DTO;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly WalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(WalksDbContext walksDbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._dbContext = walksDbContext;
            this._regionRepository = regionRepository;
        }
        /// <summary>
        /// GET ALL REGIONS FROM THE DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            // -- Get data from DB
            var regions = await _regionRepository.GetAllAsync();

            #region WITHOUT MAPPER
            // --- Mapping Domain using our DTO
            //List<RegionDto> regionsDto = new List<RegionDto>();
            //foreach (var r in regions)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = r.Id,
            //        Name = r.Name,
            //        Code = r.Code,
            //        RegionImageUrl = r.RegionImageUrl
            //    });
            //}
            #endregion
            var regionsDto = _mapper.Map<List<RegionDto>>(regions);

            // -- Return 
            return Ok(regionsDto);
        }


        /// <summary>
        /// GET REGION BY ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
        {
            // --- Get data from DB
            var regionDB = await _regionRepository.GetByIdAsync(id);

            if(regionDB == null) return NotFound();

            #region WITHOUT MAPPER
            // -- Map/Convert the domain to DTO Model
            //RegionDto region = new RegionDto()
            //{
            //    RegionImageUrl = regionDB.RegionImageUrl,
            //    Code = regionDB.Code,
            //    Name = regionDB.Name,
            //    Id = regionDB.Id
            //};
            #endregion
            var region = _mapper.Map<RegionDto>(regionDB);

            return Ok(region);

        }

        /// <summary>
        /// CREATE A NEW REGION
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidadeModel]
        public async Task<IActionResult> CreateNewRegion([FromBody] AddRegionDto model)
        {
            if (model.Name.IsNullOrEmpty() || model.Code.IsNullOrEmpty()) return BadRequest("The code and name are required!");

            var regionDB = _mapper.Map<Region>(model);

           // --- Cal db
           regionDB = await _regionRepository.CreateAsync(regionDB);

            var regionResponse = _mapper.Map<RegionDto>(regionDB);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionResponse.Id }, regionResponse);
        }


        /// <summary>
        /// UPDATE SOME REGION
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] AddRegionDto model)
        {
            try
            {
                if (model.Name.IsNullOrEmpty() && model.Code.IsNullOrEmpty() && model.RegionImageUrl.IsNullOrEmpty()) return BadRequest("Passing at least one parameter to change.");

                // --- Validate Update
                if (!ModelState.IsValid) return BadRequest(ModelState); 


                // --- Convert DTO to DomainModel
                var regionDomain = _mapper.Map<Region>(model);

                var regionDB = await _regionRepository.UpdateAsync(id, regionDomain);
                if (regionDB == null) return NotFound();

                // --- Model to return

                var respModel = _mapper.Map<RegionDto>(regionDB);

                return Ok(respModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message[..100]);  
            }
        }

        /// <summary>
        /// DELETE SOME REGION
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            try
            {
                // --- Find the Region
                var regionDB = await _regionRepository.DeleteAsync(id);
                if (regionDB == null) return NotFound();

                // --- Return to user
                var respModel = _mapper.Map<RegionDto>(regionDB);

                return Ok(respModel);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message[..100]);
            }
        }

    }
}
