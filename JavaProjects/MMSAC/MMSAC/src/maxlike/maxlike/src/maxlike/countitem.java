package maxlike.maxlike.src.maxlike;


public class countitem {
	  private short attr=-1; //attribute
	  private short attrValue=-1; //attribute value
	  private int count=0;
	  private int currentseq=-1;
	  private double minsup=-1;

	  public countitem(short attr, short attrValue) {
	    this.attr = attr;
	    this.attrValue = attrValue;
	  }

	  public void addCount(){
	    count++;
	  }

	  public int getCurrentSeq(){
	    return currentseq;
	  }

	  public int getSupportCount(){
	    return count;
	  }

	  public double getMinsup(){
	    return minsup;
	  }

	  public void setCurrentSeq(int i){
	    currentseq=i;
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