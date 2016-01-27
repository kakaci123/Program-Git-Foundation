import java.io.IOException;
import java.util.Random;
import weka.classifiers.Evaluation;
import weka.classifiers.functions.MultilayerPerceptron;

public class WekaANN extends Classifiers{
	
	public WekaANN(String[] args) {
		super(args);
		createClassifier();
		if (function==2){
			createClassifier2();
		}
	}

	public void crossValidation(){
		if (function==1) //Use training set
		{
			MultilayerPerceptron WekaANN = new MultilayerPerceptron();
			try{
				WekaANN.setOptions(options);
				WekaANN.buildClassifier(Training);
				Evaluation evaluate = new Evaluation(Training);
				evaluate.evaluateModel(WekaANN, Training);	
				System.out.println(evaluate.toSummaryString("\nResults\n======\n", false));
		    	
		    	Accuracy	 = evaluate.correct()/evaluate.numInstances();
//		    	Precision 	 = evaluate.weightedPrecision();
//		    	Recall	 	 = evaluate.weightedRecall();
		    	TruePositive = evaluate.truePositiveRate(0);
		    	TrueNegative = evaluate.trueNegativeRate(0);
		    	//Fmeasure	 = evaluate.weightedFMeasure();
		    	areaUnderROC = evaluate.areaUnderROC(0);
		    	
//		    	System.out.println(evaluate.toSummaryString());
//		    	System.out.println(evaluate.toMatrixString());
		    	
		    }catch (Exception e) {
		    	System.out.println("Cross validation error!!");
		}
	    
		}
		
		else if (function==2) //Supplied test set
		{
			//System.out.println("幹!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			MultilayerPerceptron WekaANN = new MultilayerPerceptron();
			try{
				WekaANN.setOptions(options);
				WekaANN.buildClassifier(Training);
				Evaluation evalf2 = new Evaluation(Training);			
				evalf2.evaluateModel(WekaANN, Test);	
				System.out.println(evalf2.toSummaryString("\nResults\n======\n", false));
				
				Accuracy	 = evalf2.correct()/evalf2.numInstances();
//		    	Precision 	 = evalf2.weightedPrecision();
//		    	Recall	 	 = evalf2.weightedRecall();
		    	TruePositive = evalf2.truePositiveRate(0);
		    	TrueNegative = evalf2.trueNegativeRate(0);
		    	Fmeasure	 = evalf2.weightedFMeasure();
		    	areaUnderROC = evalf2.areaUnderROC(0);
//		    	System.out.println(evaluate.toSummaryString());
//		    	System.out.println(evaluate.toMatrixString());
//		    	System.out.println(evaluate.predictions());
		    	
			}catch (Exception e) {
		    	System.out.println("Cross validation error!!");
			}	
		}
		
		
		else if (function==3) //十則驗證
		{
			MultilayerPerceptron WekaANN = new MultilayerPerceptron();
			try{
				WekaANN.setOptions(options);
				WekaANN.buildClassifier(Training);
		    	Evaluation evaluate = new Evaluation(Training);
		    	evaluate.crossValidateModel(WekaANN, Training, cross, new Random(1));
		    	System.out.println(evaluate.toSummaryString("\nResults\n======\n", false));
		    	
		    	
		    	Accuracy	 = evaluate.correct()/evaluate.numInstances();
//		    	Precision 	 = evaluate.weightedPrecision();
//		    	Recall	 	 = evaluate.weightedRecall();
		    	TruePositive = evaluate.truePositiveRate(0);
		    	TrueNegative = evaluate.trueNegativeRate(0);
		    	Fmeasure	 = evaluate.weightedFMeasure();
		    	areaUnderROC = evaluate.areaUnderROC(0);
//		    	System.out.println(evaluate.toSummaryString());
//		    	System.out.println(evaluate.toMatrixString());
//		    	System.out.println(evaluate.predictions());
		    	
		    }catch (Exception e) {
		    	System.out.println("Cross validation error!!");
			}	
		}
		
		else if (function==4) //Percentage split
		{
			
		}
		
		else //輸入錯誤
		{
			System.out.print("No function!!!!!!!");
		}
	}
	
	public void createClassifier(){
		System.out.println("\n*********************************************************************************************");
		System.out.println("Load Training!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Training = LoadDatasetCSV(trainPath);
		
		if(function!=2){
		crossValidation();
		try {
			output();
		} catch (IOException e) {
			e.printStackTrace();
			System.out.println("Output error!!");
			}
		}
	}
	
	public void createClassifier2(){
		System.out.println("\n*********************************************************************************************");
		System.out.println("Load Test!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Test = LoadTest(testPath);
		crossValidation();		
			try {
				output();
			} catch (IOException e) {
				e.printStackTrace();
				System.out.println("Output error!!");
			}
	}
}
