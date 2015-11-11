using System;
using System.Collections.Generic;

namespace Model
{
	public class Language
	{
		public int Id { set; get; }
		public string Name { set; get; }

		public Language(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public static List<Language> Get()
		{
			return new List<Language>()
			{ 
				new Language(0, "DANISH" ),
				new Language(1, "ENGLISH"),
				new Language(2, "GERMAN" ),
				new Language(3, "FRENCH" ),
				new Language(4, "SPANISH"),
				new Language(5, "CHINESE"),
				new Language(6, "RUSSIAN")
			};
		}
	}
}

