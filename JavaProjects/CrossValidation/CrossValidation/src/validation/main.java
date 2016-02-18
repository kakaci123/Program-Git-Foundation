package validation;

import java.io.File;
import java.io.IOException;

public class main {
	public main(){}
	public static void main(String[] args) throws IOException {
		File file = new File("data");  
		File[] files = file.listFiles();  
		for(File f: files){
			String filename = f.getPath();
			File directory = new File(filename);
			if(!filename.endsWith(".csv") || !directory.canRead()){
				continue;
			}
			
			String name = filename.replaceFirst("\\.csv", "");
			directory = new File(name.replaceFirst("data", "result"));
			directory.mkdirs();
			String outFilename = directory.getAbsolutePath() + name.replaceFirst("data", "") + "conv.csv";
			
			directory = new File(outFilename);
			
			ConvertToIndex convert = new ConvertToIndex(filename, outFilename);
			convert.readFile();
			System.out.println(outFilename);
			CrossValidation validation = new CrossValidation(outFilename, 1);
			
			for(int i = 1; i < 11 ; i++){
				directory = new File(directory.getParent() + "\\fold_" + i);
				directory.mkdirs();
				Classification train = new Classification();
				train.readData(validation.getTrain(i, 11));
				train.writeToFile(directory.getAbsolutePath() + name.replaceFirst("data", "") + "_train" + i + ".txt");
				
				Classification test = new Classification();
				test.readData(validation.getTest(i, 11));
				test.writeToFile(directory.getAbsolutePath() + name.replaceFirst("data", "") + "_test" + i + ".txt");
				
			}
			
		}
	}
}
