package maxlike.maxlike.src.maxlike;



public class freqdatacase {
	  private candatacase freqdc;
	  private double conf;

	  public freqdatacase(candatacase dc) {
	    freqdc = dc;
	  }

	  public void setConfidence(double conf){
	    this.conf = conf;
	  }

	  public double getConfidence(){
	    return conf;
	  }
	//�^�Ǧ�datacase�@���X��item
	  public int getLength(){
	    int len=0;
	    for (int i=0;i<freqdc.candlist.getEventSize();i++){
	      len+=((event)(freqdc.candlist.getEvent(i))).getItemSize();
	    }
	    return len;
	  }
	//���windex,�^��item(�|����������)
	  public item getItem(int index){
	    int tmpindex=index;
	    for (int i=0;i<freqdc.candlist.getEventSize();i++){
	      event tmpe = (event)(freqdc.candlist.getEvent(i));
	      if (tmpindex>=tmpe.getItemSize()){
	        tmpindex-=tmpe.getItemSize();
	      }
	      else{
	        return tmpe.getItem(tmpindex);
	      }
	    }
	    return null;
	  }

	  public int getSup(){
	    return freqdc.getCount();
	  }

	  public double getMinsup(){
	    return freqdc.getMinsup();
	  }

	  public datacase getDatacase(){
	    return freqdc.getDatacase();
	  }

	}
