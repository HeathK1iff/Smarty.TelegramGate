
using Microsoft.Extensions.Caching.Memory;
using Smarty.TelegramGate.Domain.Entities;
using Smarty.TelegramGate.Domain.Utils;

namespace Smarty.TelegramGate.Domain.Pipeline.Nodes;

public sealed class StoreLastMessagePipelineNode : IPipelineNode
{
    readonly IMemoryCache _memoryCache;
    public StoreLastMessagePipelineNode(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public Task<MessageBase?> PushAsync(MessageBase message)
    {
        if (message is CommandMessage)
        {
            return Task.FromResult<MessageBase?>(message);
        }

        if (!MessageUtils.TryGetSessionId(message, out var sessionId))
        {
            return Task.FromResult<MessageBase?>(message); 
        }
        
        _memoryCache.Set(sessionId, message);
        
        return Task.FromResult(message)!;
    }
}
