
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ES2UserType_Level : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Level data = (Level)obj;
		// Add your writer.Write calls here.
		writer.Write(data.isLevelCompleted);
		writer.Write(data.time);
		writer.Write(data.star);

	}
	
	public override object Read(ES2Reader reader)
	{
		Level data = new Level();
		Read(reader, data);
		return data;
	}
	
	public override void Read(ES2Reader reader, object c)
	{
		Level data = (Level)c;
		// Add your reader.Read calls here to read the data into the object.
		data.isLevelCompleted = reader.Read<System.Boolean>();
		data.time = reader.Read<System.Int32>();
		data.star = reader.Read<System.Int32>();

	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_Level():base(typeof(Level)){}
}
