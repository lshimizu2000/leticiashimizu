using System.ComponentModel.DataAnnotations;

namespace HelpDeskMvc.Models
{
    public class Chamado
    {
        public int Id { get; set; }

        // Required informa ao MVC que o título é obrigatório.
        [Required(ErrorMessage = "Informe o título do chamado.")]
        [StringLength(
            150,
            MinimumLength = 5,
            ErrorMessage = "O título deve ter entre 5 e 150 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        // A descrição possui uma validação de tamanho.
        [Required(ErrorMessage = "Informe a descrição do problema.")]
        [StringLength(
            1000,
            MinimumLength = 10,
            ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o nome do solicitante.")]
        [StringLength(120)]
        public string Solicitante { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecione uma prioridade.")]
        public string Prioridade { get; set; } = "Média";

        [Required(ErrorMessage = "Selecione um status.")]
        public string Status { get; set; } = "Aberto";

        [Display(Name = "Data de abertura")]
        [DataType(DataType.DateTime)]
        public DateTime DataAbertura { get; set; }
    }
}
