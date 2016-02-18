package weight.src.weight;
import java.util.*;




public class candatacase {
	  private int sup;
	  private double minsup;
	  datacase candlist;
	 

	  public candatacase(datacase dc) {
	    candlist = dc;
	  }
	  

	  public void addCount(){
	    sup++;
	  }

	  public void setCount(int c){
	    sup = c;
	  }

	  public int getCount(){
	    return sup;
	  }

	  public double getMinsup(){
	    return minsup;
	  }

	  public void setMinsup(double ms){
	    minsup = ms;
	  }

	  public datacase getDatacase(){
	    return candlist;
	  }
	  
	 
	}
