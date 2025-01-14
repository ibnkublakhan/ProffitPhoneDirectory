using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Args;
using Microsoft.Extensions.Options;

public class AlertServer
{
    private readonly ITelegramBotClient bot;
    private readonly ChatId chat;
    private readonly int? topicId;

    public AlertServer( IConfiguration _configuration )
    {
        chat = _configuration["Alert:ChatId"];
        topicId = Convert.ToInt32( _configuration["Alert:TopicId"] );
        bot = new TelegramBotClient( _configuration["Alert:BotToken"] ?? "" );
    }

    public async Task SendAlertAsync( string message ) => await bot.SendTextMessageAsync( chat, message, topicId, Telegram.Bot.Types.Enums.ParseMode.Html );

    public async Task HandleUpdateAsync( ITelegramBotClient botClient, Update update, CancellationToken cancellationToken )
    {
        // Некоторые действия
        Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject( update ) );
        if ( update.Type == Telegram.Bot.Types.Enums.UpdateType.Message )
        {
            var message = update.Message;

            var text = message?.Text;

            if ( string.IsNullOrEmpty( text ) == false )
            {
                await botClient.SendTextMessageAsync( chat, text, topicId );
            }
        }
    }

    public async Task HandleErrorAsync( ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken )
    {
        // Некоторые действия
        Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject( exception, Newtonsoft.Json.Formatting.Indented ) );
    }
}
