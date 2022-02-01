using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using lab10.Models;
using lab10.Attributes;
using lab10.Data;


namespace lab10.Controllers
{
    [EnableCors]
    [Route("api/article/")]
    [ApiController]

    public class ApiArticleController : ControllerBase
    {
        private readonly IRepository<Article> _repository;
        public ApiArticleController(IRepository<Article> repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IEnumerable<Article> Get([FromQuery(Name = "start_id")] int start_id,  [FromQuery(Name = "number")] int number) => 
            _repository.Get(start_id, number);

        [HttpGet("{id}")]
        public Article Get(int id) => 
            _repository[id];

        [HttpPost]
        [ApiKey]
        [EnableCors]
        public Article Post([FromBody] Article res) =>
            _repository.Add(res);

        [ApiKey]
        [HttpPut]
        public Article Put([FromBody] Article res) =>
            _repository.Update(res);

        [ApiKey]
        [HttpDelete("{id}")]
        public void Delete(int id){
            _repository.Delete(id);
        }
    }
}