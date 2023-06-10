namespace NotificadorEmail.API.Classes
{
    public class Paciente
    {
        public int ID { get; set; }
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
        public string? Regiao { get; set; }
    }
}
