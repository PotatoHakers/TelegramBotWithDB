using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using BusinessLogic.Interfaces;
using Common.Mapper;
using BusinessLogic.Implementations;
using Model;

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables();
}

var builder = new ConfigurationBuilder();
BuildConfig(builder);

builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();
string connection = config.GetConnectionString("DefaultConnection");

var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile()));
IMapper mapper = mappingConfig.CreateMapper();

var host = Host.CreateDefaultBuilder()
.ConfigureServices((context, services) =>
{
    services.AddTransient<IOrderService, OrderService>();
    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
    services.AddSingleton(mapper);
})
.Build();

var orderService = ActivatorUtilities.CreateInstance<OrderService>(host.Services);


#region RolottoSizeCostVar
int BachoSizeClassic = 230; int BachoSizeGrande = 420;
double BachoCostClassic = 5.40; double BachoCostGrande = 9.40;
int BonoSizeClassic = 245; int BonoSizeGrande = 450;
double BonoCostClassic = 5.40; double BonoCostGrande = 8.90;
int BoscoSizeClassic = 200; int BoscoSizeGrande = 370;
double BoscoCostClassic = 4.40; double BoscoCostGrande = 7.40;
int VerdeSizeClassic = 185; int VerdeSizeGrande = 350;
double VerdeCostClassic = 3.90; double VerdeCostGrande = 6.40;
int DolcheSizeClassic = 150; int DolcheSizeGrande = 270;
double DolcheCostClassic = 3.90; double DolcheCostGrande = 5.90;
int CarneSizeClassic = 150; int CarneSizeGrande = 310;
double CarneCostClassic = 4.40; double CarneCostGrande = 7.40;
int CaroteSizeClassic = 200; int CaroteSizeGrande = 400;
double CaroteCostClassic = 3.90; double CaroteCostGrande = 6.40;
int OroSizeClassic = 230; int OroSizeGrande = 440;
double OroCostClassic = 5.40; double OroCostGrande = 9.40;
int PicanteSizeClassic = 200; int PicanteSizeGrande = 370;
double PicanteCostClassic = 4.40; double PicanteCostGrande = 7.40;
int RichestoSizeClassic = 215; int RichestoSizeGrande = 400;
double RichestoCostClassic = 4.40; double RichestoCostGrande = 7.40;
int TezoroSizeClassic = 200; int TezoroSizeGrande = 400;
double TezoroCostClassic = 3.90; double TezoroCostGrande = 6.40;
int ExoticSizeClassic = 205; int ExoticSizeGrande = 390;
double ExoticCostClassic = 5.90; double ExoticCostGrande = 9.40;
#endregion



var botClient = new TelegramBotClient(token: "5443651302:AAEyzORweKDSaUvaqWIawIH46kEzJg2zais");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};

botClient.StartReceiving(
HandleUpdatesAsync,
HandleErrorAsync,
receiverOptions,
cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine(me.Username);
Console.ReadLine();

cts.Cancel();

async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }
    if (update.Type == UpdateType.CallbackQuery)
    {
        await HandleCallbackQuery(botClient, update.CallbackQuery);
        return;
    }

    async Task HandleMessage(ITelegramBotClient botClient, Message message)
    {
        ReplyKeyboardMarkup keyboardOrder = new(new[]
          {
            new KeyboardButton[] { "Купить ролотто"}
            })
        {
            ResizeKeyboard = true
        };

        if (message.Text == "/start")
        {
            ReplyKeyboardMarkup keyboard = new(new[]
            {
              new KeyboardButton[] { "Купить ролотто"}
            });
            await botClient.SendTextMessageAsync(message.Chat.Id, "Нажмите на кнопку купить ролотто", replyMarkup: keyboard);
        }


        if (message.Text == "Оформить заказ")
        {
            InlineKeyboardMarkup keyboard = new(new[]
            {
              new  []
              {
              InlineKeyboardButton.WithCallbackData ("Подтвердить имя", "Name")
              }
            });
            await botClient.SendTextMessageAsync(message.Chat.Id, "Укажите ваше имя", replyMarkup: keyboard);
            return;
        }

        if (message.Text == "Заказать")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Спасибо  за ваш заказ. Напишите на /start или напишите фразу - Купить ролотто, для повторного заказа");
        }

        if (message.Text == "Купить ролотто")
        {
            ReplyKeyboardMarkup keyboard = new(new[]
            {
              new KeyboardButton[] { "Купить ролотто"}
            });
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/RTDiyJJ.png",
                    caption: $"Ролотто Бачо. \nКлассический ({BachoSizeClassic} грамм) - {BachoCostClassic}. \nГрандэ ({BachoSizeGrande} грамм) - {BachoCostGrande}. \nИнгридиенты: фокачча, сливочный соус, маринованный лук, помидоры, сыр моцарелла, мясо на выбор.");
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/6szArOv.png",
                    caption: $"Ролотто Боно. \nКлассический ({BonoSizeClassic} грамм) - {BonoCostClassic}. \nГрандэ ({BonoSizeGrande} грамм) - {BonoCostGrande}. \nИнгридиенты: фокачча, фирменный соус, сыр моцарелла, листья салата, помидоры, свежие огурцы, мясо на выбор.");
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/HWLgAwT.png",
                    caption: ($"Ролотто Боско. \nКлассический ({BoscoSizeClassic} грамм) - {BoscoCostClassic}. \nГрандэ ({BoscoSizeGrande} грамм) - {BoscoCostGrande}. \nИнгридиенты: фокачча, фирменный соус, грибы, листья салата, маринованный лук, мясо на выбор."));
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/CkD7Gq1.png",
                    caption: ($"Ролотто Вэрдэ. \nКлассический ({VerdeSizeClassic} грамм) - {VerdeCostClassic}. \nГрандэ ({VerdeSizeGrande} грамм) - {VerdeCostGrande}. \nИнгридиенты: фокачча, фирменный соус, листья салата, помидоры, свежие огурцы."));
            await botClient.SendPhotoAsync(
                   chatId: message.Chat.Id,
                   photo: "https://i.imgur.com/6ciYYIQ.png",
                   caption: ($"Ролотто Дольче. \nКлассический ({DolcheSizeClassic} грамм) - {DolcheCostClassic}. \nГрандэ ({DolcheSizeGrande} грамм) - {DolcheCostGrande}. \nИнгридиеныт: фокачча, шоколад, сыр моцарелла."));
            await botClient.SendPhotoAsync(
                   chatId: message.Chat.Id,
                   photo: "https://i.imgur.com/jBvfKPV.png",
                   caption: ($"Ролотто Карнэ. \nКлассический ({CarneSizeClassic} грамм) - {CarneCostClassic}. \nГрандэ ({CarneCostGrande} грамм) - {CarneSizeGrande}. \nИнгридиенты: фокачча, сыр моцарелла, мясо на выбор."));
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/63tcMPq.png",
                    caption: ($"Ролотто Каротэ. \nКлассический ({CaroteSizeClassic} грамм) - {CaroteCostClassic}. \nГрандэ ({CaroteSizeGrande} грамм) - {CaroteCostGrande}. \nИнгридиенты: фокачча, фирменный соус, салат из пикантной моркови, салями."));
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/dpUjnFq.png",
                    caption: ($"Ролотто Оро. \nКлассический ({OroSizeClassic} грамм) - {OroCostClassic}. \nГрандэ ({OroSizeGrande} грамм) - {OroCostGrande}. \nИнгридиенты: фокачча, сырный соус, салат из капусты, огурца, морковь, специи, сыр моцарелла, мясо на выбор."));
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/gWpPEcM.png",
                    caption: ($"Ролотто Пиканте. \nКлассический ({PicanteSizeClassic} грамм) - {PicanteCostClassic}. \nГрандэ ({PicanteSizeGrande} грамм) - {PicanteCostGrande}. \nИнгридиенты: фокачча, фирменный соус, маринованные огурчики, маринованный лук, листья салата, мясо на выбор."));
            await botClient.SendPhotoAsync(
                   chatId: message.Chat.Id,
                   photo: "https://i.imgur.com/XgiYaXa.png",
                   caption: ($"Ролотто Ричесто. \nКлассический ({RichestoSizeClassic} грамм) - {RichestoCostClassic}. \nГрандэ ({RichestoSizeGrande} грамм) - {RichestoCostGrande}. \nИнгридиенты: фокачча, фирменный соус, листья салата, помидоры, свежий огурец, мясо на выбор."));
            await botClient.SendPhotoAsync(
                   chatId: message.Chat.Id,
                   photo: "https://i.imgur.com/635Rq83.png",
                   caption: ($"Ролотто Тэзоро. \nКлассический ({TezoroSizeClassic} грамм) - {TezoroCostClassic}. \nГрандэ ({TezoroSizeGrande} грамм) - {TezoroCostGrande}. \nИнгридиенты: фокачча, фирменный соус, салат (капуста, морковь, свежий огурец, специи), мясо на выбор."));
            await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: "https://i.imgur.com/8V0NK8j.png",
                    caption: ($"Ролотто Экзотик. \nКлассический ({TezoroSizeClassic} грамм) - {TezoroCostClassic}. \nГрандэ ({TezoroSizeGrande} грамм) - {TezoroCostGrande}. \nИнгридиенты: фокачча, фирменный соус, ананасы, сыр моцарелла, мясо на выбор."));
        }



        var text = update.Message.Text;
        switch (text)
        {
            case "Купить ролотто":
                InlineKeyboardMarkup keyboard = new(new[]
        {
                    new[]
                {
                   InlineKeyboardButton.WithCallbackData("Ролотто Боно", "Ролотто Боно"),
                   InlineKeyboardButton.WithCallbackData("Ролотто Боско", "Ролотто Боско"),
                },
                    new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ролотто Экзотик", "Ролотто Экзотик"),
                    InlineKeyboardButton.WithCallbackData("Ролотто Бачо", "Ролотто Бачо"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ролотто Вэрдэ", "Ролотто Вэрдэ"),
                    InlineKeyboardButton.WithCallbackData("Ролотто Дольче", "Ролотто Дольче"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ролотто Карнэ", "Ролотто Карнэ"),
                    InlineKeyboardButton.WithCallbackData("Ролотто Каротэ", "Ролотто Каротэ"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ролотто Оро", "Ролотто Оро"),
                    InlineKeyboardButton.WithCallbackData("Ролотто Пиканте", "Ролотто Пиканте"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ролотто Ричесто", "Ролотто Ричесто"),
                    InlineKeyboardButton.WithCallbackData("Ролотто Тэзоро", "Ролотто Тэзоро"),
                }

            });
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Выберите блюдо:",
                    replyMarkup: keyboard);
                return;

                break;


        }
    }
}

async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    if (callbackQuery.Data.StartsWith("Ролотто"))
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
           new[]
           {
             InlineKeyboardButton.WithCallbackData("Курица", "Chicken"),
             InlineKeyboardButton.WithCallbackData("Ветчина", "Ham"),
           },
             new []
             {
             InlineKeyboardButton.WithCallbackData("Салями", "Salami"),
             InlineKeyboardButton.WithCallbackData("Бекон","Bacon"),
             }
        });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите ингридиенты: ", replyMarkup: keyboard);
        return;
    }

    if (callbackQuery.Data.StartsWith("Ролотто"))
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
           new[]
           {
             InlineKeyboardButton.WithCallbackData("Курица", "Chicken"),
             InlineKeyboardButton.WithCallbackData("Ветчина", "Ham"),
           },
             new []
             {

             InlineKeyboardButton.WithCallbackData("Салями", "Salami"),
             InlineKeyboardButton.WithCallbackData("Бекон","Bacon"),
             }
        });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите ингридиенты: ", replyMarkup: keyboard);
        return;
    }


    if (callbackQuery.Data.StartsWith("Chicken") || callbackQuery.Data.StartsWith("Ham") || callbackQuery.Data.StartsWith("Salami") || callbackQuery.Data.StartsWith("Bacon"))
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
                    new[]
                {
                   InlineKeyboardButton.WithCallbackData("Классический", "Clasic"),
                   InlineKeyboardButton.WithCallbackData("Грандэ", "Grande"),
                    }
    });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Выберите размер: ", replyMarkup: keyboard);
        return;

    }

    if (callbackQuery.Data.StartsWith("Clasic") || callbackQuery.Data.StartsWith("Grande"))
    {
        ReplyKeyboardMarkup keyboardOrder = new(new[]
           {
              new KeyboardButton[] {"Оформить заказ"}
            });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Нажмите на кнопку оформить заказ", replyMarkup: keyboardOrder);
        return;
    }

    if (callbackQuery.Data.StartsWith("Name"))
    {
        InlineKeyboardMarkup keyboardButton = new(new[]
        {
          new []
          {
             InlineKeyboardButton.WithCallbackData("Подтвердить телефон", "Phone")
          }
       });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Укажите ваш телефон", replyMarkup: keyboardButton);
        return;
    }

    if (callbackQuery.Data.StartsWith("Phone"))
    {
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new []
            {
              InlineKeyboardButton.WithCallbackData("Подтвердить адресс", "Address")
            }
          });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Укажите ваш адресс", replyMarkup: keyboardMarkup);
        return;
    }

    if (callbackQuery.Data.StartsWith("Address"))
    {
        InlineKeyboardMarkup keyboardMarkup = new(new[]
        {
            new []
            {
              InlineKeyboardButton.WithCallbackData("Подтвердить способ оплаты", "Pay")
            }
          });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Укажите способ оплаты", replyMarkup: keyboardMarkup);
        return;

    }

    if (callbackQuery.Data.StartsWith("Pay"))
    {
        ReplyKeyboardMarkup keyboard = new(new[]
             {
              new KeyboardButton[] {"Заказать"}
            });
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Подтвердите заказ", replyMarkup: keyboard);
        return;
    }


}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException => $"Ошибка телеграм АПИ:\n {apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
