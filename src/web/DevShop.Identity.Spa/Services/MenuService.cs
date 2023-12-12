using DevShop.Identity.Spa.Models;

namespace DevShop.Identity.Spa.Services
{
    // MenuService.cs
    public class MenuService
    {
        public List<MenuItem> ObterItensMenu()
        {
            // Este é um exemplo com dados estáticos.
            var subItens = new List<MenuItem>
        {
            new MenuItem { Id = 5, Nome = "Subitem 1", Url = "#", IconClass = "fas fa-star" },
            new MenuItem { Id = 6, Nome = "Subitem 2", Url = "#", IconClass = "fas fa-cog" },
        };

            return new List<MenuItem>
        {
            new MenuItem { Id = 1, Nome = "Item 1", Url = "#", IconClass = "fas fa-home" },
            new MenuItem { Id = 2, Nome = "Item 2", Url = "#", IconClass = "fas fa-list" },
            new MenuItem { Id = 3, Nome = "Item 3", Url = "#", IconClass = "fas fa-user" },
            new MenuItem { Id = 4, Nome = "Item 4", Url = "#", IconClass = "fas fa-envelope", Subitens = subItens },
        };
        }
    }
}
