using AutoMapper;
using DataAccess.Repository.LogServices;
using DataAccess.Repository.Models;
using DataAccess.Repository.RepositoryEF.IRepositoryEF;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using TraderBlotter.Api.Models.Dto;

namespace TraderBlotter.Api.Controllers
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MappingController : Controller
    {
        private readonly IMapper _mapper;
        private static ILog _log = LogService.GetLogger(typeof(TradeViewController));
        private readonly IDealerClientMappingRepository _dealerClientMappingRepository;
        private readonly IGroupDealerMappingRepository _groupDealerMappingRepository;

        public MappingController(IDealerClientMappingRepository dealerClientMappingRepository, IMapper mapper, IGroupDealerMappingRepository groupDealerMappingRepository)
        {
            _dealerClientMappingRepository = dealerClientMappingRepository;
            _mapper = mapper;
            _groupDealerMappingRepository = groupDealerMappingRepository;
        }

        [HttpPost]
        [Route("addDealerClientMapping")]
        public IActionResult AddDealerClientMapping([FromBody]List<DealerClientMappingDto> lstDealerClientMappingDtos)
        {
            try
            {
                _log.Info($"MappingController: AddDealerClientMapping Called - Input- {JsonConvert.SerializeObject(lstDealerClientMappingDtos)}");
                var lstmapping = new List<DealerClientMappingView>();
                foreach (var item in lstDealerClientMappingDtos)
                {
                    lstmapping.Add(_mapper.Map<DealerClientMappingView>(item));
                }
                _dealerClientMappingRepository.MergeDealerClientMapping(lstmapping);
                _log.Info($"MappingController: AddDealerClientMapping Called - Finished- {HttpStatusCode.OK}");

                return Ok();
            }
            catch (Exception ex)
            {
                _log.Error($"MappingController: AddDealerClientMapping Error - ",ex);
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 });
            }
            
        }

        [HttpPost]
        [Route("addGroupDealerMapping")]
        public IActionResult AddGroupDealerMapping([FromBody]List<GroupDealerMappingDto> lstGroupDealerMappingDtos)
        {
            try
            {
                _log.Info($"MappingController: AddGroupDealerMapping Called - Input- {JsonConvert.SerializeObject(lstGroupDealerMappingDtos)}");

                var lstmapping = new List<GroupDealerMappingView>();
                foreach (var item in lstGroupDealerMappingDtos)
                {
                    lstmapping.Add(_mapper.Map<GroupDealerMappingView>(item));
                }
                _groupDealerMappingRepository.MergeGroupDealerMapping(lstmapping);

                _log.Info($"MappingController: AddGroupDealerMapping Called - Finished - {HttpStatusCode.OK}");

                return Ok();
            }
            catch (Exception ex)
            {
                _log.Error($"MappingController: AddGroupDealerMapping Error - ", ex);
                return StatusCode(500, new ErrorModel { Message = ex.Message, HttpStatusCode = 500 });
            }

        }
    }
}