using System.ComponentModel.DataAnnotations;

namespace WebbsäkerhetInlämning.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string MessageContent { get; set; }
    }
}
