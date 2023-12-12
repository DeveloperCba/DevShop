namespace DevShop.Identity.Spa.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
        public string IconClass { get; set; }
        public List<MenuItem> Subitens { get; set; }
    }
}
