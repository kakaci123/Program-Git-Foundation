import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import weka.core.Instances;
import weka.core.converters.CSVLoader;



public abstract class Classifiers {
	
	static String   trainPath;				// Training	資料集路徑
	static String	testPath;				// Test路徑
	static String   outputPath;				// 輸出路徑
	
	static String 	ClassifierType;			// 分類器類型
	static String[] options;				//參數
	static String[] copyOpt;				//輸出
	static int 	function;
	static int cross;
	
	
	
	Instances 		Training;				// 訓練資料集
	Instances		Test;
	
	 //FileReader reader;
	 //FileReader reader = new FileReader(testPath);
	 //Instances instances = new Instances(reader);
	 
	//Standardize filter = new Standardize();
	//filter.setInputFormat();
	//Instances newTest = filter.useFilter(Test, filter);
	//Instances data = new Instances(new BufferedReader(new FileReader("./Dataset/SICH-anyICH_OS1_test.csv")));
	
	 

	double	Accuracy;
	double 	Precision;
	double 	Recall;
	double	Fmeasure;
	double  TruePositive;
	double	TrueNegative;
	double	areaUnderROC;


	public Classifiers(String[] args){
		
		function=Integer.parseInt(args[0]);
		if(function==1 || function==2){
			testPath = args[1];			
		}
		if(function==3){
			cross=Integer.parseInt(args[1]);
		}
		
		outputPath	= "./Dataset/result";
		trainPath = args[2]; //資料集路徑
		 
		
		System.out.println(" 測試資料來源");
		System.out.println("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
		System.out.print("訓練資料來源:");
		System.out.println(trainPath);
		System.out.print("測試資料來源:");
		System.out.println(testPath);
		System.out.println("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n");
		
		 
		
		System.out.println("function"+function+"!!!!!!!!!!!");
		//扣掉前兩個參數	args[0]:資料集	args[1]:分類器
		options = new String[args.length-4];
		copyOpt = new String[args.length];
		
		System.arraycopy(args, 4, options, 0, options.length);
		System.arraycopy(args, 0, copyOpt, 0, args.length);
		
		System.out.println("\n參數：");
		System.out.println("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
		System.out.println(options[0]+" "+options[1]+" "+options[2]+options[3]);
		System.out.println("function:"+copyOpt[0]+" 參數(來源)："+copyOpt[1]+" 檔案路徑："+copyOpt[2]+" 參數："+copyOpt[3]+" "+copyOpt[4]+" "+copyOpt[5]+" ");
		System.out.println("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
		
	}
	
	public Instances LoadDatasetCSV(String path){
			try {
//				System.out.println("=== Load CSV Dataset ===");
				Instances dataset;
				CSVLoader loader = new CSVLoader();
				loader.setSource(new File(path));
				dataset = loader.getDataSet();
				dataset.setClassIndex(dataset.numAttributes()-1);	// 設定分類類別屬性的位置(最後一個)
//		    	System.out.println("\n=== Dataset ===\n");
//		    	System.out.println(new Instances(Dataset, 0));
				//System.out.println(dataset);
								System.out.println("Training Loaded!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				System.out.println("*********************************************************************************************\n");
				return dataset;
			} catch (IOException e) {
				System.out.println("Load Training IOException!");
				e.printStackTrace();
			}
			return null;
		}
		
		
	
	public Instances LoadTest(String path2){
		try {
//			System.out.println("=== Load CSV Dataset ===");
			Instances dataset2;
			CSVLoader loader2 = new CSVLoader();
			loader2.setSource(new File(path2));
			dataset2 = loader2.getDataSet();
			dataset2.setClassIndex(dataset2.numAttributes()-1);	// 設定分類類別屬性的位置(最後一個)
//		    System.out.println("\n=== Dataset ===\n");
//		    System.out.println(new Instances(Dataset, 0));
			//System.out.println(dataset2);
			System.out.println("Test Loaded!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			System.out.println("*********************************************************************************************\n");
		    return dataset2;
		} catch (IOException e) {
			System.out.println("Load Testing IOException!");
			e.printStackTrace();
		}
		return null;
	}
	
	
	public void output() throws IOException{
		File	newFile = new File(outputPath);
		FileWriter	fw  = new FileWriter(newFile, true);
		
		String accuracy   = String.format("%6.4f", Accuracy);
//		String precision  = String.format("%6.4f", Precision);
//		String recall	  = String.format("%6.4f", Recall);
		String F1		  = String.format("%6.4f", Fmeasure);
		String TPR		  = String.format("%6.4f", TruePositive);
		String TNR		  = String.format("%6.4f", TrueNegative);
		String AUC		  = String.format("%6.4f", areaUnderROC);
		
		fw.write(accuracy +"\t" +TPR +"\t" +TNR +"\t" +AUC +"\t\t\t");
			
		
		fw.write("function:"+copyOpt[0]+"\t參數(來源):"+copyOpt[1]+"\t");
		
		for(int i=2; i<copyOpt.length; i++)
			fw.write(copyOpt[i] +"\t");
		fw.write("\n");
		fw.close();
	}
}
