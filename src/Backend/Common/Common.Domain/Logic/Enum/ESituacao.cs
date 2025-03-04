using System.ComponentModel;

namespace Common.Domain.Logic.Enum
{
    public enum ESituacao
    {
        [Description("Contratado")]
        Contratado = 1,
        [Description("Demitido")]
        Demitido = 2,
        [Description("Aguardando Documentacao")]
        AguardandoDocumentacao = 3,
        [Description("Documentacao Enviada")]
        DocumentacaoEnviada = 4,
        [Description("Rescisao Pendente")]
        RescisaoPendente = 5,
        [Description("Contrato Rescindido")]
        ContratoRescindido = 6
    }
}
