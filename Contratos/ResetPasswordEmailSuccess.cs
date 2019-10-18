namespace Contratos
{
    public class ResetPasswordEmailSuccess
    {
        public string Destinatario {get;set;}
        public string Assunto {get;set;}
        public string Mensagem {get;set;}
        public string Link { get; set; }
        public string Nome { get; set; }
    }
}