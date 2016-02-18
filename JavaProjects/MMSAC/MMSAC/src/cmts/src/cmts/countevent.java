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