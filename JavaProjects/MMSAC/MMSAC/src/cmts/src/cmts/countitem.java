package cmts.src.cmts;

/**
 * <p>Title:Building associative classifier with multiple minimum supports</p>
 * <p>Description: Use the consept of multiple minimum supports to build up classifier for association rule </p>
 * <p>Copyright: Copyright (c) 2012</p>
 * <p>Company: National Chung Cheng University, Taiwan, R.O.C.</p>
 * @author Jian-Shian Wang
 * @version 1.0
 */

public class countitem {
  private short attr=-1; //attribute
  private short attrValue=-1; //attribute value
  private int count=0;
  private int currentpat=-1;
  private double minsup=-1;

  public countitem(short attr, short attrValue) {
    this.attr = attr;
    this.attrValue = attrValue;
  }

  public void addCount(){
    count++;
  }

  public int getCurrentPat(){
    return currentpat;
  }

  public int getSupportCount(){
    return count;
  }

  public double getMinsup(){
    return minsup;
  }

  public void setCurrentPat(int i){
    currentpat=i;
  }

  public void setMinsup(double minsup){
    this.minsup = minsup;
  }

  public short getItemAttrName(){
    return attr;
  }

  public short getItemAttrValue(){
    return attrValue;
  }
}
