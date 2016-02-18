package cmts_score.cmts_score.src.cmts_score;
import java.util.*;
/**
 * <p>Title: Classification of Multivriate Time Series Using Sequential Pattern</p>
 * <p>Description: Use the consept of sequential patterns to build up classifier for multivariate time series</p>
 * <p>Copyright: Copyright (c) 2007</p>
 * <p>Company: National Central University, Taiwan, R.O.C.</p>
 * @author Ya-Han Hu
 * @version 1.0
 */

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