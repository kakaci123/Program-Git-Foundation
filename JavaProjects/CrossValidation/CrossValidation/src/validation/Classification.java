package validation;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Set;
import java.util.TreeMap;
import java.util.TreeSet;

public class Classification {
	
	public static void main(String[] args) {
		Classification c = new Classification();
		String data = "";
		try{
			  // Open the file that is the first 
			  // command line parameter
			  FileInputStream fstream = new FileInputStream("test.txt");
			  // Get the object of DataInputStream
			  DataInputStream in = new DataInputStream(fstream);
			  BufferedReader br = new BufferedReader(new InputStreamReader(in));
			  String strLine;
			  //Read File Line By Line
			  while ((strLine = br.readLine()) != null)   {
				  data += strLine + "\n";
			  }
		  		in.close();
		  		
		    }catch (Exception e){//Catch exception if any
		    	System.err.println("Error: " + e.getMessage());
		    }
		    c.readData(data);
		    c.writeToFile("result.txt");
	}
	
	private Map<String, List<String>> dataMap = new TreeMap<String, List<String>>();
	public Classification(){
	}
	private String OnlyData(String data){
		String[] temp = data.split("@data");
		if(temp.length < 2){
			return "";
		}
		return temp[1];
	}
	private String[] spliteLine(String data){
		return data.split("\n");
	}
	public void readData(String data){
		String[] lines = spliteLine(OnlyData(data));
		for(String line: lines){
			if(line.equals("")){
				continue;
			}

			String[] cloumns = line.split(",");
			
			String className = cloumns[cloumns.length-1];
			if(dataMap.containsKey(className)){
				dataMap.get(className).add(line);
			}else{
				List<String> t = new ArrayList<String>();
				t.add(line);
				dataMap.put(className, t);
			}
		}
	}
	
	public void writeToFile(String filename){
		try{
			  FileWriter fWrtieStream = new FileWriter(filename);
			  BufferedWriter out = new BufferedWriter(fWrtieStream);
			  Set<String> keys = new TreeSet<String>(dataMap.keySet());
			  for(String key: keys){
				  out.write(key);
				  out.newLine();
				  int count =1;
				  for(String data: dataMap.get(key)){
					  String[] columns = data.split(",");
					  String toOut = "";
					  toOut += count++ + "\t";
					  
					  for(int i=0; i < columns.length; i ++){
						  
						  if(i == columns.length -1){
							  continue;
						  }
						  if(i != 0){
							  toOut += "\t";
						  }
						  try{
							  toOut += (int) Double.parseDouble(columns[i]);
						  }catch(NumberFormatException e){
							  toOut +=" ";
						  }
					  }
					  if(toOut.contains("?") || toOut.contains(".")){
						  System.out.println(toOut);
					  }
					  out.write(toOut);
					  out.newLine();
				  }
				  
			  }
			
			  out.close();
		    }catch (Exception e){//Catch exception if any
		    	System.err.println("Error: " + e.getMessage());
		    }
	}
}
