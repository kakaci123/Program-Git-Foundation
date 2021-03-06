package cmts_score.cmts_score.src.cmts_score;

/**
 * <p>Title: Classification of Multivriate Time Series Using Sequential Pattern</p>
 * <p>Description: Use the consept of sequential patterns to build up classifier for multivariate time series</p>
 * <p>Copyright: Copyright (c) 2007</p>
 * <p>Company: National Central University, Taiwan, R.O.C.</p>
 * @author Ya-Han Hu
 * @version 1.0
 */
//frequent datacase = datacase + sup + conf
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
//回傳此datacase共有幾個item
  public int getLength(){
    int len=0;
    for (int i=0;i<freqdc.candlist.getEventSize();i++){
      len+=((event)(freqdc.candlist.getEvent(i))).getItemSize();
    }
    return len;
  }
//給定index,回傳item(尚未完成測試)
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