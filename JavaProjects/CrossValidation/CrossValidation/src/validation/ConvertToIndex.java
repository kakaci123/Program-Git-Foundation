package validation;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

public class ConvertToIndex {
	
	public static void main(String[] args) {
		String filename = "data\\balance-scale.csv";
		String outFilename = filename.replaceFirst("\\.csv", ".conv.csv");
		outFilename = outFilename.replaceFirst("data", "result");
		ConvertToIndex convert = new ConvertToIndex(filename, outFilename);
		convert.readFile();
	}
	
	private String filename;
	private String outFilename;
	public ConvertToIndex(String filename, String outFilename){
		this.setFilename(filename);
		this.setOutFilename(outFilename);
	}
	public void setFilename(String filename) {
		this.filename = filename;
	}
	public String getFilename() {
		return filename;
	}
	private List<UniqueIndex> uniqueIndex = new ArrayList<UniqueIndex>();
	public void readFile(){

		  try{
			  // Open the file that is the first 
			  // command line parameter
			  FileInputStream fstream = new FileInputStream(this.filename);
			  FileWriter fWrtieStream = new FileWriter(this.outFilename);
			  BufferedWriter out = new BufferedWriter(fWrtieStream);
			  // Get the object of DataInputStream
			  DataInputStream in = new DataInputStream(fstream);
			  BufferedReader br = new BufferedReader(new InputStreamReader(in));
			  String strLine;
			  boolean isFirst = true;
			  //Read File Line By Line
			  while ((strLine = br.readLine()) != null)   {
				  // Print the content on the console
				  String[] contains = strLine.split(",");
				  if(isFirst){
					  isFirst = false;
					  for(String s: contains){
						  uniqueIndex.add(new UniqueIndex(s));
					  }
					  out.write(strLine);
					  continue;
				  }
				  out.write("\n");
				  out.write(processLine(contains));
				  //System.out.println (strLine +"\t"+ contains.length);
			  }
			  //Close the input stream
		  		in.close();
		  		out.close();
		    }catch (Exception e){//Catch exception if any
		    	System.err.println("Error: " + e.getMessage());
		    }
		  
	}
	public String processLine(String[] contains){
		String result = "";
		if(contains.length > uniqueIndex.size()){
			System.out.println(contains.length +" > " + uniqueIndex.size());
			return result;
		}
		for(int i= 0; i< contains.length; i++){
			if(i != 0){
				result += ",";
			}
			
			if(contains[i].equals("") || contains[i].equals(" ")){
				result += " ";
				continue;
			}
			if(!uniqueIndex.get(i).getName().toLowerCase().equals("class")){
				result +=  String.valueOf(i+1);
			}
			result += String.valueOf(uniqueIndex.get(i).getIndex(contains[i]));
		}
		if(result.contains("?")){
			System.out.println("error:" + result);
			for(String s: contains){
				System.out.print(s+" ");
			}
			System.out.println();
		}
		return result;
	}
	public void setOutFilename(String outFilename) {
		this.outFilename = outFilename;
	}
	public String getOutFilename() {
		return outFilename;
	}
}
