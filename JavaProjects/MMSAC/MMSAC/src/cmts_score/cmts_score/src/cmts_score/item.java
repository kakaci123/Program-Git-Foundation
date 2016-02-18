package cmts_score.cmts_score.src.cmts_score;

/**
 * <p>Title: Classification of Multivriate Time Series Using Sequential Pattern</p>
 * <p>Description: Use the consept of sequential patterns to build up classifier for multivariate time series</p>
 * <p>Copyright: Copyright (c) 2007</p>
 * <p>Company: National Central University, Taiwan, R.O.C.</p>
 * @author Ya-Han Hu
 * @version 1.0
 */

public class item {
  private short attr=-1; //attribute
  private short attrValue=-1; //attribute value

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
}