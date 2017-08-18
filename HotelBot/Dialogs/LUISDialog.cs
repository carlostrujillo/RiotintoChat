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
	[LuisModel("924a8347-45e9-4db8-8524-34d036926f68", "fabdbd96b09f4b7cb20d8eb7679d1ff2")]
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


		[LuisIntent("Geta")]
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

		[LuisIntent("CreateIncident")]
		public async Task CreateIncident(IDialogContext context, LuisResult result)
		{
			var enrollmentForm = new FormDialog<Data>(new Data(), this.Incident, FormOptions.PromptInStart);
			context.Call<Data>(enrollmentForm, CreateIncidentAzure);
		}

        private async Task CreateIncidentAzure(IDialogContext context, IAwaitable<object> result)
        {
            // save to azure
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