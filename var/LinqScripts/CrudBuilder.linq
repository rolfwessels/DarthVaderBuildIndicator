<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Humanizer</NuGetReference>
  <Namespace>Humanizer</Namespace>
</Query>

string  _location = @"..\..\src";
string  _template = @"SetSettingsContext";
string  _toName = @"JenkinsStatusContext";
string  _filter = "Man";
string[]  _fileTypes = new [] { @".cs",".js",".html",".txt"};
string[]  _exclude = new [] { @"bower_components" ,".OAuth2.","RequestClientDetailsHelper","Mappers\\MapClient.cs" , "Enums\\","node_modules","Configuration","Attribute"};

void Main()
{
	_location = Path.GetFullPath(Path.Combine(Path.GetDirectoryName (Util.CurrentQueryPath),_location)).Dump();	
	var files =  Directory.GetFiles(_location,"*"+_template+"*",SearchOption.AllDirectories).Where(file => _fileTypes.Contains(Path.GetExtension(file)) && !_exclude.Any(x=> file.Contains(x)));
	var replaces = files.Select(x=> new { file = x, newFile = ReplaceAll(x)}).ToArray().Dump();
	foreach (var replaceFiles in replaces)
	{
			
			var file = replaceFiles.file;
			var newFile = replaceFiles.newFile;
			if (!File.Exists(newFile)) {
				var replace = Util.ReadLine("Would you like to create "+file.Replace(_location,"")+" [Y/n]").ToUpper() != "N";
				if (replace) {
					var fileContent = File.ReadAllText(file);
					File.WriteAllText(newFile,ReplaceAll(fileContent));
					newFile.Dump("Created");
					AddFileToProject(newFile,file);
				}
				else {
					newFile.Dump("Skip");
				}
			}
	}
}

public string AddFileToProject(string fileName, string oldFile) {
	string projectName = null;
	var path = Path.GetDirectoryName(fileName);
	do
	{
		projectName = Directory.GetFiles(path,"*.csproj").FirstOrDefault();
		path = Path.GetDirectoryName(path);
	} while (string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(path) );
	if (!string.IsNullOrEmpty(projectName)) {
		var projectFile = File.ReadAllLines(projectName).ToList();
		for (int i = 0; i < projectFile.Count; i++)	
		{
			
			if (projectFile[i].Contains("\\"+Path.GetFileName(oldFile))) {
				
				projectFile.Insert(i+1,ReplaceAll(projectFile[i]).Dump());
			}
		}
		File.WriteAllLines(projectName,projectFile.ToArray());
	}
	return projectName;
}


public string ReplaceAll(string text) {
	return text
		.Replace(_template.Pluralize(),_toName.Pluralize()) // StockCategories Samples
		.Replace(InitialLower(_template.Pluralize()),InitialLower(_toName.Pluralize()))  // stockCategories samples
		.Replace(_template,_toName) // StockCategory Sample
		.Replace(InitialLower(_template),InitialLower(_toName)) // stockCategory sample
		 
		;
}


public string InitialLower(string text) {
	return text.Substring(0,1).ToLower()+text.Substring(1);
}

// Define other methods and classes here