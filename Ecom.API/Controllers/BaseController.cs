﻿using AutoMapper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _work;
        protected readonly IMapper _mapper;
        public BaseController(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }
    }
}
