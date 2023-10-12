using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using pizzeria_crud_refactoring.Database.Repository;
using pizzeria_crud_refactoring.Models;
using pizzeria_mvc.Database;

namespace pizzeria_crud_refactoring.Database
{
    public class RepositoryPizza : IRepositoryPizza
    {
        private PizzaContext _db;

        public RepositoryPizza(PizzaContext db)
        {
            _db = db;
        }


        public List<Pizza> GetPizzas()
        {
            List<Pizza> pizzas = _db.Pizza
                                        .Include(p => p.Ingredients)
                                        .Include(P => P.Category)
                                        .ToList();
            return pizzas;
        }


        public Pizza GetPizzaById(long id)
        {
            Pizza? pizza = _db.Pizza
                                    .Where(p => p.Id == id)
                                    .Include(p => p.Ingredients)
                                    .Include(P => P.Category)
                                    .FirstOrDefault();

            if (pizza == null) throw new Exception("Pizza non trovata") ;

            return pizza;
        }
         

        public List<Pizza> GetPizzasByName(string search)
        {
            List<Pizza> foundedPizzas = _db.Pizza
                                                .Where(p => p.Name.ToLower().Contains(search.ToLower()))
                                                .Include(p => p.Ingredients)
                                                .Include(P => P.Category)
                                                .ToList();
            return foundedPizzas;
        }

        public bool Create(Pizza pizzaAdd)
        {
            try
            {

                _db.Pizza.Add(pizzaAdd);
                _db.SaveChanges();
                return true;
            }
            catch { return false; }
        }


        public bool Update(long id, Pizza pizzaEdit)
        {

            Pizza? pizzaToEdit = _db.Pizza.Where(p => p.Id == id).Include(p => p.Ingredients).FirstOrDefault();

            if (pizzaToEdit == null)
                return false;
            else
            {
                pizzaToEdit.Ingredients.Clear();  //sempre

                pizzaToEdit.Price = pizzaEdit.Price;
                pizzaToEdit.Description = pizzaEdit.Description;
                pizzaToEdit.Name = pizzaEdit.Name;
                pizzaToEdit.Photo = pizzaEdit.Photo;
                pizzaToEdit.CategoryId = pizzaEdit.CategoryId;

                pizzaEdit.Ingredients = new List<Ingredient>();

                foreach (Ingredient ingredient in pizzaEdit.Ingredients)
                {
                    pizzaToEdit.Ingredients.Add(ingredient);
                }

                _db.SaveChanges();
                return true;
            }
        }

        public bool Delete(long id)
        {
            Pizza? pizza = _db.Pizza
                                   .Where(p => p.Id == id)
                                   .Include(p => p.Ingredients)
                                   .Include(P => P.Category)
                                   .FirstOrDefault();

            if (pizza == null) return false;

            _db.Remove(pizza);
            _db.SaveChanges();
            return true;
        }
    }
}
