using CommunityToolkit.Mvvm.Messaging.Messages;

namespace crud_maui.Ultilidade
{
    public class EmpregadoMensageria : ValueChangedMessage<EmpregadoMensagem>
    {
        public EmpregadoMensageria(EmpregadoMensagem value) : base(value)
        {
        }
    }

}
