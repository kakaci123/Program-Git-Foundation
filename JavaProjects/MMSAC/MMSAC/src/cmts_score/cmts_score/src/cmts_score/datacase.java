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

public class datacase {
  private short classlabel=-1;
  private ArrayList dlist=new ArrayList();

  public datacase(short label) {
    classlabel = label;
  }

  public datacase(){

  }

  public void setClassLabel(short label){
    classlabel = label;
  }

  public void addEventToDatacase(event e){
    dlist.add(e);
  }

  public void addEventToDatacase(int index, event e){
    dlist.add(index, e);
  }

  public event getEvent(int i){
    return (event)(dlist.get(i));
  }
/*//�ĤG�ب��oitem����k
  public item getItem2(int index){
    int tmp=0;
    for (int i=0;i<dlist.size();i++){
      for (int j=0;j<((event)dlist.get(j)).getItemSize();j++){
        if (tmp==index){
          return ((event)dlist.get(j)).getItem(j);
        }
        tmp++;
      }
    }
    return null;
  }
*/
//�ھ�index���oitem
  public item getItem(int index){
    ArrayList tmplist = new ArrayList();
    for (int i=0;i<dlist.size();i++){
      for (int j=0;j<((event)dlist.get(i)).getItemSize();j++){
        tmplist.add(((event)dlist.get(i)).getItem(j));
      }
    }
    return (item)tmplist.get(index);
  }
//�ƻs�@�ӻP��datacase�ۦP��datacase�^�Ǧ^�h
  public datacase cloneDatacase(){
    datacase cdc = new datacase(this.getClassLabel());
    for (int i=0;i<this.getEventSize();i++){
      event ne = this.getEvent(i).cloneEvent();
      cdc.addEventToDatacase(ne);
    }
    return cdc;
  }

//����event by index
  public void removeEvent(int index){
    dlist.remove(index);
  }

  public short getClassLabel(){
    return classlabel;
  }
//����event���ƥ�
  public int getEventSize(){
    return dlist.size();
  }
//��item���ƥ�
  public int getItemSize(){
    int si=0;
    for (int i=0;i<dlist.size();i++){
      si+=((event)dlist.get(i)).getItemSize();
    }
    return si;
  }
//��ܲ�i��event
  public void showevent(int i){
    event tempevent = (event)(dlist.get(i));
    System.out.print("[");
    for (int ii=0;ii<tempevent.getItemSize();ii++){
      tempevent.showitem(ii);
    }
    System.out.print("]");
  }

}