using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lab10.Models;
using lab10.Data;
using lab10.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace lab10.Controllers
{
    [EnableCors]
    [Route("api/Category/")]
    [ApiController]
    public class ApiCategoryController : ControllerBase
    {
        private readonly IRepository<Category> _repository;
        public ApiCategoryController(IRepository<Category> repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IEnumerable<Category> Get([FromQuery(Name = "start_id")] int start_id,  [FromQuery(Name = "number")] int number) => 
            _repository.Get(start_id, number);

        [HttpGet("{id}")]
        public Category Get(int id) => 
            _repository[id];

        [HttpPost]
        [EnableCors]
        [ApiKey]
        public Category Post([FromBody] Category res) =>
            _repository.Add(res);

        [ApiKey]
        [HttpPut]
        public Category Put([FromBody] Category res) =>
            _repository.Update(res);

        [ApiKey]
        [HttpDelete("{id}")]
        public void Delete(int id){
            _repository.Delete(id);
        }
    }
}