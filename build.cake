var projectName = "Orc.Memento";
var projectsToPackage = new [] { "Orc.Memento" };
var company = "WildGums";
var startYear = 2010;
var defaultRepositoryUrl = string.Format("https://github.com/{0}/{1}", company, projectName);

#l "./deployment/cake/variables.cake"
#l "./deployment/cake/tasks.cake"
