using Smarty.TelegramGate.Domain.Entities;
using Smarty.TelegramGate.Domain.Exceptions;
using Smarty.TelegramGate.Domain.Interfaces;

namespace Smarty.TelegramGate.Domain.Pipeline.Nodes;

public sealed class InvokeMessageHandlersPipelineNode : IPipelineNode
{
    readonly IEnumerable<IMessageHandler> _handlers;
    public InvokeMessageHandlersPipelineNode(IEnumerable<IMessageHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task<MessageBase?> PushAsync(MessageBase message)
    {
        bool handled = false;
        foreach (var handler in _handlers)
        {
            handled |=  await handler.HandleMessageAsync(message);
        }

        if (!handled)
        {
            //throw new Exception();
        }

        return message;
    }
}
