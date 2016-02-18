package cmts.src.cmts;

/**
 * <p>Title:Building associative classifier with multiple minimum supports</p>
 * <p>Description: Use the consept of multiple minimum supports to build up classifier for association rule </p>
 * <p>Copyright: Copyright (c) 2012</p>
 * <p>Company: National Chung Cheng University, Taiwan, R.O.C.</p>
 * @author Jian-Shian Wang
 * @version 1.0
 */

public class item {
  private short attr=-1; //attribute
  private short attrValue=-1; //attribute value
  private countitem freqit;

  public item(short attr, short attrValue) {
    this.attr = attr;
    this.attrValue = attrValue;
  }

  public short getItemAttrName(){
    return attr;
  }

  public short getItemAttrValue(){
    return attrValue;
	  }
  
  public double getMinsup(){
	    return freqit.getMinsup();
	 
	  }
 
 
}

