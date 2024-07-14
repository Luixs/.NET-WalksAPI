using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walks.API.Models.Domain;
using Walks.API.Models.DTO;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {

        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        /// <summary>
        /// CREATE A NEW WALK
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewWalk(RequestWalkDto model)
        {
            // --- Convert the DTO to Model
            var modelDB = _mapper.Map<Walk>(model);

            // --- Creating a new Walk
            await _walkRepository.CreateAsync(modelDB);

            return Ok(_mapper.Map<WalkDto>(modelDB));
        }

        /// <summary>
        /// GET ALL WALKS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var dbList = await _walkRepository.GetAllAsync();
            var dtoList = _mapper.Map<List<WalkDto>>(dbList);

            return Ok(dtoList);
        }

        /// <summary>
        /// GET BY ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUniqueWalk([FromRoute] Guid id)
        {
            var walkFound = await _walkRepository.GetUniqueAsync(id);

            if (walkFound == null) return NotFound();

            var dtoWalk = _mapper.Map<WalkDto>(walkFound);

            return Ok(dtoWalk);
        }

        /// <summary>
        /// UPDATE WALK
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] RequestWalkDto model)
        {
            var modelDB = _mapper.Map<Walk>(model);

            modelDB = await _walkRepository.UpdateWalkAsync(id, modelDB);

            if(modelDB == null) return NotFound();

            var dtoWalk = _mapper.Map<WalkDto>(modelDB);

            return Ok(dtoWalk);
        }

        /// <summary>
        /// Delete a WALK
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var modelDeleted = await _walkRepository.DeleteWalkAsync(id);

            if (modelDeleted == null) return NotFound();

            // -- Mapping DTO
            var dtoModel = _mapper.Map<WalkDto>(modelDeleted);

            return Ok(dtoModel);
        }
    }
}
