import java.io.IOException;
import java.util.Random;

import weka.classifiers.Evaluation;
import weka.classifiers.trees.J48;

public class WekaJ48 extends Classifiers{
	
	public WekaJ48(String[] args) {
		super(args);
		createClassifier();
		if (function==2){
			createClassifier2();
		}
	}

	public void crossValidation(){
		if (function==1) //Use training set
		{
			J48 WekaJ48 = new J48();
			try{
				WekaJ48.setOptions(options);
		    	WekaJ48.buildClassifier(Training);
				//Instances test;    // from somewhere
				// train classifier
				// evaluate classifier and print some statistics
				Evaluation evalf1 = new Evaluation(Training);
				evalf1.evaluateModel(WekaJ48, Training);	
				System.out.println(evalf1.toSummaryString("\nResults\n======\n", false));
				
				Accuracy	 = evalf1.correct()/evalf1.numInstances();
//		    	Precision 	 = evalf1.weightedPrecision();
//		    	Recall	 	 = evalf1.weightedRecall();
		    	TruePositive = evalf1.truePositiveRate(0);
		    	TrueNegative = evalf1.trueNegativeRate(0);
		    	Fmeasure	 = evalf1.weightedFMeasure();
		    	areaUnderROC = evalf1.areaUnderROC(0);
//		    	System.out.println(evaluate.toSummaryString());
//		    	System.out.println(evaluate.toMatrixString());
//		    	System.out.println(evaluate.predictions());
		    	
			}catch (Exception e) {
		    	System.out.println("Cross validation error!!");
			}	
		}
		
		else if (function==2) //Supplied test set
		{
			J48 WekaJ48 = new J48();
			try{
				//System.out.println("11111111111111111111111111111111111111111111111");
				WekaJ48.setOptions(options);
								
		    	WekaJ48.buildClassifier(Training);
		    	
		    	//System.out.println("2222222222222222222222222222222222222222222222");
				Evaluation evalf2 = new Evaluation(Training);
							
				//System.out.println("3333333333333333333333333333333333333333333");
				
				evalf2.evaluateModel(WekaJ48, Test);
				
				//System.out.println("44444444444444444444444444444444444444444444");
				
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
			J48 WekaJ48 = new J48();
			try{
		    	WekaJ48.setOptions(options);
		    	WekaJ48.buildClassifier(Training);
		    	Evaluation evaluate = new Evaluation(Training);
		    	evaluate.crossValidateModel(WekaJ48, Training, cross, new Random(1));
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
