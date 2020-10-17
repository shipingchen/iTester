using System;
using System.Collections;

namespace com.csiro.xml.datatype
{
	/// <summary>
	/// A utility class to privide a set of common methods/services for 
	/// the common data, such as print, getPropertyValueByName, etc.
	/// </summary>
	public class DataUtility
	{
		public DataUtility()
		{

		}

		static public void print(Config it)
		{
			Console.Write("<Config");
			Console.Write(" name=" + it.name);
			Console.Write(" note=" + it.note);
			Console.WriteLine(">");

			IEnumerator list = it.GetEnumerator();
			while(list.MoveNext())
			{
				print((Catelog) list.Current);
			}

			Console.WriteLine("<\\Config>");
		}

		static public void print(Catelog it)
		{
			Console.Write("\t<Catelog");
			Console.Write(" name="  + it.name);
			Console.Write(" type="  + it.type);
			Console.Write(" note="  + it.note);
			Console.WriteLine(">");

			IEnumerator list = it.GetEnumerator();
			while(list.MoveNext())
			{
				print((Item) list.Current);
			}

			Console.WriteLine("\t<\\Catelog>");
		}

		static public void print(Item it)
		{
			Console.Write("\t\t<Item");
			Console.Write(" name="  + it.name);
			Console.Write(" type="  + it.type);
			Console.Write(" note="  + it.note);
			Console.WriteLine(">");

			IEnumerator list = it.GetEnumerator();
			while(list.MoveNext())
			{
				print((Property) list.Current);
			}

			Console.WriteLine("\t\t<\\Item>");
		}

		static public void print(Property it)
		{
			Console.Write("\t\t\t<Property");
			Console.Write(" name="  + it.name);
			Console.Write(" value=" + it.value);
			Console.Write(" note="  + it.note);
			Console.WriteLine(">");
		}

		static public string getPropertyValueByName(Item it, string name)
		{
			IEnumerator list = it.GetEnumerator();

			while(list.MoveNext())
			{
				Property prop = (Property) list.Current;
				if(prop.name.Equals(name)) return prop.value;
			}

			return null;
		}

		static public string getPropertyValueByName(Item it, string name, string defaultValue)
		{
			IEnumerator list = it.GetEnumerator();

			while(list.MoveNext())
			{
				Property prop = (Property) list.Current;
				if(prop.name.Equals(name)) return prop.value;
			}

			return defaultValue;
		}

		static public Catelog getTheFirstCatelog(Config config)
		{
			IEnumerator list = config.GetEnumerator();

			if(list.MoveNext()) return (Catelog) list.Current;
			return              null;
		}

		static public Item getTheFirstItem(Catelog catelog)
		{
			IEnumerator list = catelog.GetEnumerator();

			if(list.MoveNext()) return (Item) list.Current;
			return              null;
		}

		static public Item getTheFirstItem(Config config)
		{
			Catelog catelog = getTheFirstCatelog(config);

			if(catelog!=null) 
				return getTheFirstItem(catelog);
			else
				return null;
		}

		static public Catelog getCatelogByName(Config config, string name)
		{
			IEnumerator list = config.GetEnumerator();
			while(list.MoveNext())
			{
				Catelog it = (Catelog) list.Current;
				if(it.name.Equals(name))
				{
					return it;
				}
			}

			return null;
		}

		static public Item getItemByName(Catelog catelog, string name)
		{
			IEnumerator list = catelog.GetEnumerator();

			while(list.MoveNext())
			{
				Item it = (Item) list.Current;
				if(it.name.Equals(name))
				{
					return it;
				}
			}

			return null;
		}
	}
}
