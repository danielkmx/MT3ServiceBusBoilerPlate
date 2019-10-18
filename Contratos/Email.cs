namespace Contratos
{
    public class Email
    {
        public string Destinatario { get; set; }

        public string Assunto { get; set; }

        public string Mensagem { get; set; }

        public string Nome { get; set; }

        public Attachment Anexo { get; set; }
    }
}