using Newtonsoft.Json;

namespace NotificadorEmail.API.Classes
{
    public class Agendamento
    {
        public int ID { get; set; }
        public string? TipoAgendamento { get; set; }
        public string? NomePaciente { get; set; }
        public string? NomeDoutor { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }

        [JsonIgnore]
        public bool ConfirmacaoAtraso { get; private set; }

        [JsonProperty("ConfirmacaoAtraso")]
        private bool ConfirmacaoAtrasoJson
        {
            get { return ConfirmacaoAtraso; }
            set { ConfirmacaoAtraso = value; }
        }

        public void SetConfirmacaoAtraso(bool confirmacao)
        {
            ConfirmacaoAtraso = confirmacao;
        }
    }
}
