using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using pizzeria_crud_refactoring.Database.Repository;
using pizzeria_crud_refactoring.Models;
using pizzeria_mvc.Database;

namespace pizzeria_crud_refactoring.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private IRepositoryPizza _repoPizza;

        public PizzaController(IRepositoryPizza repo)
        {
            _repoPizza = repo;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
          //cosi uscirebbe un ciclo infinito, che c# riesce a gestire perche usa puntatori, json no, occorre evitare ricorrenze cicliche
          // per evitarlo occorre aggiungere una stringa in Program.cs
          List<Pizza> pizzas = _repoPizza.GetPizzas();
           return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult GetPizzasByName(string? search)
        {
            if (search == null) return BadRequest(new { Message = "non hai inserito nessuna stringa di ricerca" });

            List<Pizza> foundedPizzas = _repoPizza.GetPizzasByName(search);
            return Ok(foundedPizzas);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizzaById(long id)
        {
            Pizza? pizza = _repoPizza.GetPizzaById(id);
           
            if (pizza == null) return NotFound();

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Pizza pizza)
        {
            try
            {
                bool result = _repoPizza.Create(pizza);
                if (result) { return Ok(); }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message} );
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody] Pizza pizza)
        {
            bool result = _repoPizza.Update(id, pizza);

            if (result) { return Ok(); }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            bool result = _repoPizza.Delete(id);
            if (result) { return Ok(); }
            return BadRequest();
        }
    }
}
