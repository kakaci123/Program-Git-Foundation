package validation;
import java.io.File;
import java.io.IOException;

import weka.core.Debug.Random;
import weka.core.converters.CSVLoader;
import weka.core.Instances;


public class CrossValidation {

	private String filename ="";
	private Instances randData = null;
	public CrossValidation(String filename, int seed) throws IOException {
		setFilename(filename);
		CSVLoader loader = new CSVLoader();											// read csv file
	    loader.setSource(new File(filename));											
	    Instances loaddata = loader.getDataSet();									// set csv as data
	    Random rand = new Random(seed);   // create seeded number generator
	    randData = new Instances(loaddata);
	    randData.randomize(rand);
	    
	}
	
	public String getTrain(int folderIndex, int folderNumber){
		if(folderIndex < 1){
			return "";
		}
		return randData.trainCV(folderNumber,folderIndex).toString();
		
	}
	public String getTest(int folderIndex, int folderNumber){
		if(folderIndex < 1){
			return "";
		}
		return randData.testCV(folderNumber,folderIndex).toString();
		
	}
	
	/**
	 * @param args
	 * @throws IOException 
	 */
	public static void main(String[] args) throws IOException {
		
		// TODO Auto-generated method stub
		String data="data\\balance-scale.csv";						// the file location
		CSVLoader loader = new CSVLoader();											// read csv file
	    loader.setSource(new File(data));											
	    Instances loaddata = loader.getDataSet();									// set csv as data
	    
	    int seed= 1;
	    Random rand = new Random(seed);   // create seeded number generator
	    Instances randData = new Instances(loaddata);
	    randData.randomize(rand);
	    //randData.stratify(folds);
	    int folds = 11;
	    for (int n = 1; n < folds; n++) {
	    System.out.println("-------------------------");
	       System.out.println("fold-"+n);
	       System.out.println("-------------------------");
	       System.out.println("-------------------------");
	       Instances train = randData.trainCV(folds, n);
	       Instances test = randData.testCV(folds, n);
	       System.out.println("train"+train);
	       System.out.println("-------------------------");
	       System.out.println("test"+test);
	       System.out.println("-------------------------");
	       System.out.println("-------------------------");
	       // further processing, classification, etc.
	    }
	}
	public void setFilename(String filename) {
		this.filename = filename;
	}
	public String getFilename() {
		return filename;
	}

}
