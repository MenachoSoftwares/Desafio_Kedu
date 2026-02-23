using Kedu.Domain.Enums;

namespace Kedu.Application.DTOs;
public class CobrancaFiltroDto
{
    public StatusCobranca? Status { get; set; }

    public bool? VencidaApenas { get; set; }

    public DateTime? DataVencimentoDe { get; set; }

    public DateTime? DataVencimentoAte { get; set; }
}
