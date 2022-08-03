using System;

void Main()
{
	var importer = new Importer();
	importer.Mappings=new List<KeyValuePair<string,string>>{
		new KeyValuePair<string,string>("Batch ID","BatchID"),
		new KeyValuePair<string,string>("Project Number","ProjectNumber"),
		new KeyValuePair<string,string>("Project Name","ProjectName"),
		new KeyValuePair<string,string>("Project Engineer","ProjectEngineer"),
		new KeyValuePair<string,string>("Developer Project Manager","DeveloperProjectManager"),
		new KeyValuePair<string,string>("Document Name","DocumentName"),
		new KeyValuePair<string,string>("Document Category","DocumentCategory"),
		new KeyValuePair<string,string>("Container ID (Bardcode)","ContainerID"),
		new KeyValuePair<string,string>("ROOT","ROOT"),
		new KeyValuePair<string,string>("Document Status","DocumentStatus"),

	};
	var list = importer.Import<Product>(@"C:\Users\imoreno\Desktop\InformationMetadata.csv");
	Console.WriteLine(list);
}

public class Product

        {

            public string BatchID { get; set; }
            public string ProjectNumber { get; set; }
            public string ProjectName { get; set; }
            public string ProjectEngineer { get; set; }
            public string DeveloperProjectManager { get; set; }
            public string DocumentName { get; set; }
            public string DocumentCategory { get; set; }
            public string ContainerID { get; set; }
            public string ROOT { get; set; }
            public string DocumentStatus { get; set; }
        }

 public class Importer
        {
            public List<KeyValuePair<string, string>> Mappings;
            public List<T> Import<T>(string file)
            {
                List<T> list = new List<T>();
                List<string> lines = System.IO.File.ReadAllLines(file).ToList();
                string headerLine = lines[0];
                var headerInfo = headerLine.Split(',').ToList().Select((v, i) => new
                {
                    ColName = v,
                    ColIndex = i
                });
                Type type = typeof(T);
                var properties = type.GetProperties();
                var dataLines = lines.Skip(1);
                dataLines.ToList().ForEach(line =>
                {
                    var values = line.Split(',');
                    T obj = (T)Activator.CreateInstance(type);
                    //Set values to object properties from csv columns
                    foreach (var prop in properties)
                    {
                        //find mapping for the prop
                        var mapping = Mappings.SingleOrDefault(m => m.Value == prop.Name);
                        var colName = mapping.Key;
                        var colIndex = headerInfo.SingleOrDefault(s => s.ColName == colName).ColIndex;
                        var value = values[colIndex];
                        var propType = prop.PropertyType;
                        prop.SetValue(obj, Convert.ChangeType(value, propType));
                    }
                    list.Add(obj);
                });
                return list;
            }
        }