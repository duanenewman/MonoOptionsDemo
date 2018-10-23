using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace MonoOptionsDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine($"Processing Options{Environment.NewLine}");

			try
			{
				HelpOptions(args);
				//CaseSensitiveOptions(args);
				//FlagOptions(args);
				//MulitpleFlagOptions(args);
				//RequiredValueOptions(args);
				//OptionalValueOptions(args);
				//MacroOptions(args);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.WriteLine($"{Environment.NewLine}Press a key to continue...");

			Console.ReadKey();
		}

		private static void ParseOptionsAndDisplayHelp(string[] args, OptionSet optionSet)
		{
			var extra = optionSet.Parse(args);
			if (extra.Count <= 0) return;

			Console.WriteLine($"{Environment.NewLine}Extra parameters detected:");
			foreach (var e in extra)
			{
				Console.WriteLine($"\t{e}");
			}

			Console.WriteLine($"{Environment.NewLine}Writing Help Text:");
			optionSet.WriteOptionDescriptions(Console.Out);
			Console.WriteLine();
		}

		// /h
		private static void HelpOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				"Extra stuff that will be printed with help",
				"Perhaps a USAGE line?",
				"USAGE: MonoOptionsDemo.exe /h",
				{ "a", "This is a short description", v => { }},
				{ "b", "This is a longer description that should get wrapped automatically by the WriteOptionDescriptions call.", v => { }},
				"",
				"These extra lines can go anywhere in the OptionSet",
				"",
				{ "c", "This is a longer description that also has some custom breakpoints to ensure that additional info is shown on a new line.\r\nLike This\r\nAnd This\r\n\tAnd it works with tabs!", v => { }},
				"",
				"There is a secret option here",
				{ "s", "This is a secret option, and will not show up!", v => { }, true},
				"You didn't see it, did you?"
			};

			ParseOptionsAndDisplayHelp(args, optionSet);

		}

		// /A=one /a=two
		private static void CaseSensitiveOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				"options are case sensitive",
				{ "a=", "is not equal with 'A'",  v => { Console.WriteLine($"a is '{v}'"); }},
				{ "A=", "is not equal with 'a'",  v => { Console.WriteLine($"A is '{v}'"); }},
				"unless you subclass OptionSet and do some special stuff",
			};

			ParseOptionsAndDisplayHelp(args, optionSet);

		}

		// /f /f- -f+ --flag
		private static void FlagOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				{ "f|flag", "a flag argument, a minus after the switch will disable it", v => { Console.WriteLine($"f is null: {string.IsNullOrEmpty(v)} ({v})"); }},
			};

			ParseOptionsAndDisplayHelp(args, optionSet);
		}

		// -abcd
		private static void MulitpleFlagOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				{ "a", "A", v => { Console.WriteLine($"a is set: {!string.IsNullOrEmpty(v)} ({v})"); }},
				{ "b", "B", v => { Console.WriteLine($"b is set: {!string.IsNullOrEmpty(v)} ({v})"); }},
				{ "c", "C", v => { Console.WriteLine($"c is set: {!string.IsNullOrEmpty(v)} ({v})"); }},
				{ "d", "D", v => { Console.WriteLine($"d is set: {!string.IsNullOrEmpty(v)} ({v})"); }},
			};

			ParseOptionsAndDisplayHelp(args, optionSet);
		}

		// /d=test /data="A string with spaces" -d:"Next one will fail" -d
		private static void RequiredValueOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				{ "d|data=", "this argument will require a value to be supplied.", v => { Console.WriteLine($"d is '{v}'"); }},
			};

			ParseOptionsAndDisplayHelp(args, optionSet);
		}

		// /d=test /data="A string with spaces" -d:"Next one will work too" -d
		private static void OptionalValueOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				{ "d|data:", "this argument will work with or without a value supplied.", v => { Console.WriteLine($"d is '{v}'"); }},
			};

			ParseOptionsAndDisplayHelp(args, optionSet);
		}

		// /p:Configuration=Debug /p=Platform:x64 /property="WarningLevel:2" /p="OutputPath=c:\dev\demo" /s:"My Secret"="noneya" /P /P:A /P=A:B /p x y
		private static void MacroOptions(string[] args)
		{
			var optionSet = new OptionSet()
			{
				{ "p|property=", "macro.. like msbuild property argument.", (p,v) => { Console.WriteLine($"property '{p}' is set to '{v}'"); }},
				{ "P:", "This version doesn't require a key or value", (p,v) => { Console.WriteLine($"optional property '{p}' is set to '{v}'"); }},
				"",
				"There is a secret option here",
				{ "s|secret=", "secret macro.. wont show in help.", (p,v) => { Console.WriteLine($"secret '{p}' is set to '{v}'"); }, true},
				"So it will not show in the help"
			};

			ParseOptionsAndDisplayHelp(args, optionSet);
		}

	}
}
