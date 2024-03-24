using System.ComponentModel;

namespace APIBanco.Enum
{
    public enum TipoTransferencia
    {
        [Description("Saque")]
        Saque = 100,

        [Description("Deposito")]
        Deposito = 101,
        
        [Description("Transferencia Interna")]
        transferenciaInterna=102,
        
        [Description("Transferencia Externa")]
        transferenciaExterna=103,
        
        [Description("Transferenciad de Salario")]
        transferenciaSalario=104,
        
        [Description("Pix")]
        pix=105,
        
        [Description("Pagamento de Boleto")]
        pagamentoBoleto=106,
        
        [Description("Pagamento Tributo")]
        pagamentoTributo=107,
        
        [Description("Pagamento Consumo")]
        pagamentoConsumo=108,
        
        [Description("Debito Automatico")]
        debitoAutomatico=109,
    }
    public static class TipoTransferenciaExtensions
    {
        public static string GetDescription(this TipoTransferencia value)
        {
            var campo = value.GetType().GetField(value.ToString());

            var atributo = (DescriptionAttribute)Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute));

            return atributo == null ? value.ToString() : atributo.Description;
        }
    }
}
