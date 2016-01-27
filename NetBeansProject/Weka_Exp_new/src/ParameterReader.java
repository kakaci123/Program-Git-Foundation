import java.io.BufferedReader;
import java.io.FileReader;
import java.text.SimpleDateFormat;
import java.util.Date;

public class ParameterReader {

	static String paraPath = "./Dataset/parameter";
	static int lineNum;

	public static void main(String[] args) {

		int tmpLineNum = 0;
		checkParameterLineNum();
		
		while(tmpLineNum<lineNum){
			System.out.printf("#ParameterLine -> %2d. ", tmpLineNum+1);
			readParameter(tmpLineNum);
			tmpLineNum++;
			checkParameterLineNum();
		}
		
		Date date = new Date();
		SimpleDateFormat dateformat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
		System.out.println("\n");
		System.out.println(" ******************************************");
		System.out.printf (" *%5sEND TIME:%20s %5s*\n", "", dateformat.format(date), "");
		System.out.println(" ******************************************\n\n");
		
	}

	public static void readParameter(int lineNum) {
		try {
			FileReader 	   fR = new FileReader(paraPath);
			BufferedReader bR = new BufferedReader(fR);

			String regexPattern = "[\\s]";
			String line = "";
			for(int i=0; i<=lineNum; i++)
				line = bR.readLine();
				
			if (line != null)
			{
				String[] option = line.split(regexPattern);
				
				
				//System.out.println(option[0]);
				//System.out.println(option[1]);
				//System.out.println(option[2]);
				//System.out.println(option[3]);
				//System.out.println(option[4]);
				//System.out.println(option[5]);
				//System.out.println(option[6]);
				//System.out.println(option[7]);
				
				printArr(option);
				WekaDemo WK = new WekaDemo();
				WK.main(option);
			} 
			fR.close();
			bR.close();
		} catch (Exception e) {
			System.out.println("Read Parameter Error!!");
			e.printStackTrace();
		}
	}

	public static void checkParameterLineNum(){
		try {
			FileReader 	   fR = new FileReader(paraPath);
			BufferedReader bR = new BufferedReader(fR);
			
			String line = "";
			lineNum = 0;
			while (line != null) {
				line = bR.readLine();
				if (line != null) {
					lineNum++;
				} 
			}
			fR.close();
			bR.close();
		} catch (Exception e) {
			System.out.println("Check Parameter Error!!");
			e.printStackTrace();
		}
	}
	
	public static void printArr(String[] arr) {
		System.out.print("|  ");
		for (int i=0; i<arr.length; i++){
				System.out.printf("%s  ", arr[i]);
		}
		System.out.println("|");
	}
}
