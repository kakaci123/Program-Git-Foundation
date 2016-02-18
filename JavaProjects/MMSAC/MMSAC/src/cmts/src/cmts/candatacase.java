package cmts.src.cmts;
import java.util.*;
/**
 * <p>Title:Building associative classifier with multiple minimum supports</p>
 * <p>Description: Use the consept of multiple minimum supports to build up classifier for association rule </p>
 * <p>Copyright: Copyright (c) 2012</p>
 * <p>Company: National Chung Cheng University, Taiwan, R.O.C.</p>
 * @author Jian-Shian Wang
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