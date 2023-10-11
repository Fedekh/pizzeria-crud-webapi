using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using pizzeria_crud_refactoring.Models;
using pizzeria_mvc.Database;

namespace pizzeria_crud_refactoring.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private PizzaContext _db;

        public PizzaController(PizzaContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetPizzas()
        {
          //cosi uscirebbe un ciclo infinito, che c# riesce a gestire perche usa puntatori, json no, occorre evitare ricorrenze cicliche
          // per evitarlo occorre aggiungere una stringa in Program.cs
          List<Pizza> pizzas = _db.Pizza
                                        .Include(p => p.Ingredients)
                                        .Include(P => P.Category)
                                        .ToList();
           return Ok(pizzas);
        }

        [HttpGet]
        public IActionResult SearchPizzas(string? search)
        {
            if (search == null) return BadRequest(new { Message = "non hai inserito nessuna stringa di ricerca" });

            List<Pizza> foundedPizzas = _db.Pizza
                                                .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                                                .Include(p => p.Ingredients)
                                                .Include(P => P.Category)
                                                .ToList();

            return Ok(foundedPizzas);
        }

        [HttpGet("{id}")]
        public IActionResult SearchbyId(long id)
        {
            Pizza? pizza = _db.Pizza
                                    .Where(p=>p.Id == id)
                                    .Include(p=>p.Ingredients)
                                    .Include(P=>P.Category)
                                    .FirstOrDefault();
           
            if (pizza == null) return NotFound();

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult Create([FromBody]Pizza pizza)
        {
            try
            {
                _db.Pizza.Add(pizza);
                _db.SaveChanges();
                return Ok(pizza);
            }
            catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message} );
            }

        }

        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody] Pizza pizza)
        {
            Pizza? pizzaEdit = _db.Pizza
                                    .Where(p => p.Id == id)
                                    .FirstOrDefault();


            if (pizzaEdit == null) return NotFound();
            
                pizzaEdit.Name = pizza.Name;
                pizzaEdit.Price = pizza.Price;
                pizzaEdit.Description = pizza.Description;
                pizzaEdit.Photo = pizza.Photo;

                _db.SaveChanges();
                return Ok(pizzaEdit);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Pizza? pizza = _db.Pizza
                                   .Where(p => p.Id == id)
                                   .Include(p => p.Ingredients)
                                   .Include(P => P.Category)
                                   .FirstOrDefault();

            if (pizza == null) return NotFound();

            _db.Remove(pizza);
            _db.SaveChanges();
            return Ok();
        }
    }
}
