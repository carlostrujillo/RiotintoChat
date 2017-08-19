using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceChatBot.Models
{

	public enum Classification
	{
		Internal,
		Confidential,
		HighlyConfidential
	}

    public enum DataType
    {


    }

	[Serializable]
	public class Data
	{
        public string Name;

        public Classification? Classificaion;
        public List<DataType> DataType;

		public static IForm<Data> BuildForm()
		{
			return new FormBuilder<Data>()
				.Message("Welcome to Service Assistance bot!")
				.Build();
		}

	}
}