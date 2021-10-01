using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<PlatformReadDto> GetPlaforms()
        {
            var platformItems = this._repo.GetAllPlatforms();
            return this._mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);
        }

        [HttpGet("{id}", Name = "GetPlaformById")]
        public ActionResult<PlatformReadDto> GetPlaformById(int id)
        {
            var platformItems = this._repo.GetPlatform(id);

            if (null == platformItems)
                return NotFound();

            return this._mapper.Map<PlatformReadDto>(platformItems);
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlaform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = this._mapper.Map<Platform>(platformCreateDto);
            this._repo.CreatePlatform(platformModel);
            this._repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlaformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
