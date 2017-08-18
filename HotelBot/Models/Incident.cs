using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceChatBot.Models
{

	public enum Priority
	{
		High,
		Medium,
		Low
	}


	[Serializable]
	public class Data
	{
		public Priority? Priority;
        public User OpenedBy;
        public User AssginedTo;
        public string Title;
        public string Description;
        public DateTime? DueDate;
        public DateTime CreatedDate;

		public static IForm<Data> BuildForm()
		{
			return new FormBuilder<Data>()
				.Message("Welcome to Service Assistance bot!")
				.Build();
		}

	}
}