using System.ComponentModel;

namespace APIBanco.Enum
{
    public enum StatusServico
    {
        [Description("Encerrado")]
        Encerrado = 0,

        [Description("Ativo")]
        Ativo = 1,

        [Description("Inativo")]
        Inativo = 2,

        [Description("Bloqueado")]
        Bloqueado = 3,
    }

    public static class StatusServicoExtensions
    {
        public static string GetDescription(this StatusServico value)
        {
            var campo = value.GetType().GetField(value.ToString());

            var atributo = (DescriptionAttribute)Attribute.GetCustomAttribute(campo, typeof(DescriptionAttribute));

            return atributo == null ? value.ToString() : atributo.Description;
        }
    }
}
