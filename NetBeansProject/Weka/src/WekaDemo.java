public class WekaDemo {

	public static void main(String[] args) {
		
		setClassifier(args);
	}
	
	public static void setClassifier(String[] options){
		
		String classifier = options[3];
		
		if(classifier.equals("J48")){
			WekaJ48 j48 = new WekaJ48(options);
		}
		else if(classifier.equals("SMO")){
			WekaSVM SMO = new WekaSVM(options);
		}
		else if(classifier.equals("SimpleCart")){
			WekaCART CART = new WekaCART(options);
		}
		else if(classifier.equals("MultilayerPerceptron")){
			WekaANN MLP = new WekaANN(options);
		}
		else if(classifier.equals("RandomForest")){
			WekaRandomForest RandomForest = new WekaRandomForest(options);
		}
		else if(classifier.equals("AdaBoostM1")){
			WekaAdaboost Adaboost = new WekaAdaboost(options);
		}
		else if(classifier.equals("IBk")){
			WekaKNN IBK = new WekaKNN(options);
		}
		else if(classifier.equals("Logistic")){
			WekaLGR LGR = new WekaLGR(options);
		}
		else if(classifier.equals("SimpleLogistic")){
			WekaSL SL = new WekaSL(options);
		}
		
		else{
			System.out.println("No " +classifier +" classifier.\n");
		}
	}
}
