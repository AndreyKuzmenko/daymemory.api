using DayMemory.Core.Notifications;
using DayMemory.Core.Commands;
using MediatR;

namespace DayMemory.Core.NotificationHandlers.User
{
    public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
    {
        private readonly IMediator _mediator;

        public UserCreatedNotificationHandler(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            await CreateQuestionList(notification.UserId!, "Аналіз статусу задачі",
                "Що я зробив?",
                "Що я не зробив?",
                "Що мені залишилось зробити?");

            await CreateQuestionList(notification.UserId!, "Пошук мотивації",
                "Що ти хочеш?",
                "Для чого тобі потрібна мотивація?",
                "Що тобі насправді важливо?",
                "Що тобі потрібно для досягнення цілі?",
                "Що ти можеш зробити для досягнення цілі?");

            await CreateQuestionList(notification.UserId!, "Бачити велику картину",
                "Що відбувається?",
                "В чому твоя складність чи проблема?",
                "В чому переваги даної ситуації / проблеми?",
                "Уяви себе через п'ять років від цього моменту. Що було важливого в цій ситуації?",
                "Як ти бачиш свою роль в цьому?");

            await CreateQuestionList(notification.UserId!, "Стенографіст",
                "Про що я зараз думаю?",
                "На що спрямована моя увага?",
                "Які нові ідеї з'явилися?",
                "Що мене порадувало протягом дня?",
                "Що привернуло увагу, викликало мій інтерес?");

            await CreateQuestionList(notification.UserId!, "Емоції та відчуття",
                "Що мене зараз хвилює?",
                "Про що я мрію??",
                "Як я почуваюся?",
                "Що я зараз бачу?",
                "Що я зараз чую?");

            await CreateQuestionList(notification.UserId!, "Підсумки дня",
                "Яке моє найбільше досягнення за сьогодні?",
                "Що пройшло добре? Чому?",
                "Що могло бути краще? Чому?",
                "Який у мене пріоритет на завтра?");

            await CreateQuestionList(notification.UserId!, "Ціль",
                "Що я хочу?",
                "Що мені це дасть?",
                "Навіщо мені це потрібно?",
                "Що хорошого станеться зі мною?",
                "Яка моя потреба буде задоволена?",
                "Чи можна це задовольнити іншим способом?",
                "Напишіть нове бажання, якщо побачили у цьому необхідність");
        }

        private async Task CreateQuestionList(string userId, string title, params string[] questions)
        {
            var item = new CreateQuestionListCommand()
            {
                UserId = userId,
                Text = title,
                Questions = questions
            };
            await _mediator.Send(item);
        }
    }
}
