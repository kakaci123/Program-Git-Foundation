package weight.src.weight;

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

