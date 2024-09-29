namespace Application.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string? Email { get; set; }
    }
}
