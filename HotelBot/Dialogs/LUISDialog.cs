using ServiceChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServiceChatBot.Dialogs
{
    [LuisModel("55143a21-98a4-4090-9e96-41944ba0d475", "56748e227cf7404d95e6174c84497cb3")]
    [Serializable]
	public class LUISDialog : LuisDialog<Data>
	{
		private readonly BuildFormDelegate<Data> Incident;
		[field: NonSerialized()]
		protected Activity _message;

		protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
		{
			_message = (Activity)await item;
			await base.MessageReceived(context, item);
		}

		public LUISDialog(BuildFormDelegate<Data> incident)
		{
			this.Incident = incident;
		}


		[LuisIntent("None")]
		public async Task None(IDialogContext context, LuisResult result)
		{
			await context.PostAsync("I'm sorry I don't know what you mean.");
			context.Wait(MessageReceived);
		}

		[LuisIntent("Greeting")]
		public async Task Greeting(IDialogContext context, LuisResult result)
		{
			context.Call(new GreetingDialog(), Callback);
		}

		private async Task Callback(IDialogContext context, IAwaitable<object> result)
		{
			context.Wait(MessageReceived);
		}

		[LuisIntent("get_Datatype")]
		public async Task GetDataType(IDialogContext context, LuisResult result)
		{
			var enrollmentForm = new FormDialog<Data>(new Data(), this.Incident, FormOptions.PromptInStart);
			context.Call<Data>(enrollmentForm, Callback);
		}

        [LuisIntent("RespondQuery")]
        public async Task RespondQuery(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count > 0)
            {
                foreach( var entity in result.Entities)
                {
                    if (entity.Entity.ToLower() == "financial")
                    {
                        await context.PostAsync("No, It's confindential. Do not share");
                        context.Wait(MessageReceived);
                        return;
                    }
                }
            }
            //  EntityRecommendation rec = new EntityRecommendation();
            //  rec.Type = ;
            await context.PostAsync("What kind of document");
            context.Wait(MessageReceived);
            return;

        }


        [LuisIntent("QueryMyIncidents")]
		public async Task QueryMyIncidents(IDialogContext context, LuisResult result)
		{
			foreach (var entity in result.Entities.Where(Entity => Entity.Type == "Amenity"))
			{
				var value = entity.Entity.ToLower();
				if (value == "pool")
				{
					//await context.PostAsync("Yes we have that!");

					Activity replyMessage = _message.CreateReply();


					
					//replyMessage.ChannelData = ;

					await context.PostAsync(replyMessage);

					context.Wait(MessageReceived);
					return;
				}
				else
				{
					await context.PostAsync("I'm sorry we don't have that.");
					context.Wait(MessageReceived);
					return;
				}
			}
			await context.PostAsync("I'm sorry we don't have that.");
			context.Wait(MessageReceived);
			return;
		}

	}
}