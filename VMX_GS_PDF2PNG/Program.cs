using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VMX_GS_PDF2PNG
{
	class Program
	{
		static int Main(string[] args)
		{
			//log4net.GlobalContext.Properties["LogName"] = string.Format("pdf2png_{0}.log", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
			var convert = new Converter();
			return convert.Convert(args);
		}
	}

	public class Converter
	{

		public int Convert(string[] args)
		{
			int result = 0;
			string filePath = null;
			string outputPath = null;
			bool standardOrderStructure = false; //JJJJ/MM/TT
			try
			{
				if (args.Length != 2 && args.Length != 3)
				{
					Environment.Exit(-1);
				}
				filePath = args[0];
				outputPath = args[1];
				if (args.Length == 3)
				{
					standardOrderStructure = System.Convert.ToBoolean(args[2]);
				}
				if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
				{
					Environment.Exit(405);
				}

				if (!File.Exists(filePath))
				{
					Environment.Exit(404);
				}
				GhostScriptWrapper.CallAPI(GetArgs(filePath, outputPath, standardOrderStructure));
			}
			catch (Exception ex)
			{
				result = 500;
			}
			return result;
		}

		static string[] GetArgs(string inputPath, string outputPath, bool standardOrderStructure)
		{
			if(standardOrderStructure)
			{
				string filename = Path.GetFileNameWithoutExtension(inputPath);
				filename += "_";
				return new string[]
				{
				"",
				"-sDEVICE=png16m",
				"-dBATCH",
				"-dDownScaleFactor=3",
				"-r600",
				"-dNOPAUSE",
				"-dNOOUTERSAVE",
				string.Format("-sOutputFile={0}\\{1}%d.png", outputPath,filename),
				inputPath
				};
			}
			else
			{
				return new string[]
				{
				"",
				"-sDEVICE=png16m",
				"-dBATCH",
				"-dDownScaleFactor=3",
				"-r600",
				"-dNOPAUSE",
				"-dNOOUTERSAVE",
				string.Format("-sOutputFile={0}\\%03d.png", outputPath),
				inputPath
				};
			}
		}
	}
}
