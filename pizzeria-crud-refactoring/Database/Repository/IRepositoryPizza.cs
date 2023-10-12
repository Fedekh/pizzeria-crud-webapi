using pizzeria_crud_refactoring.Models;

namespace pizzeria_crud_refactoring.Database.Repository
{
    public interface IRepositoryPizza
    {
        public List<Pizza> GetPizzas();
        public List<Pizza> GetPizzasByName(string search);
        public Pizza GetPizzaById(long id);
        public bool Create(Pizza pizzaAdd);
        public bool Update(long id, Pizza pizzaEdit);
        public bool Delete(long id);
    }
}
