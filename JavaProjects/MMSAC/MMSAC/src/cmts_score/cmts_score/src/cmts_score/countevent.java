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

public class countevent {
  private ArrayList eventlist = new ArrayList();

  public countevent() {

  }
//�Nitem�[�ievent
  public void addItemToEvent(item i){
    if (!checkSameItem(i.getItemAttrName())){
      eventlist.add(i);
    }
  }

//check�b�P�@event���O�_���ۦP�ݩ�
  private boolean checkSameItem(short attrname){
    for(int i=0;i<eventlist.size();i++){
      if (attrname == ((item)(eventlist.get(i))).getItemAttrName()){
        return true;
      }
    }
    return false;
  }

//���oitem
  public item getItem(int i){
    return (item)(eventlist.get(i));//�j���૬,�ݬO�_����L��k
  }

//���oitem�Ӽ�
  public int getItemNum(){
    return eventlist.size();
  }
//��ܲ�i��item
  public void showitem(int i){
    item tempitem = (item)(eventlist.get(i));
    System.out.print("(" + tempitem.getItemAttrName() + ", " + tempitem.getItemAttrValue() + ")");
  }

}