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
//將item加進event
  public void addItemToEvent(item i){
    if (!checkSameItem(i.getItemAttrName())){
      eventlist.add(i);
    }
  }

//check在同一event中是否有相同屬性
  private boolean checkSameItem(short attrname){
    for(int i=0;i<eventlist.size();i++){
      if (attrname == ((item)(eventlist.get(i))).getItemAttrName()){
        return true;
      }
    }
    return false;
  }

//取得item
  public item getItem(int i){
    return (item)(eventlist.get(i));//強制轉型,看是否有其他方法
  }

//取得item個數
  public int getItemNum(){
    return eventlist.size();
  }
//顯示第i個item
  public void showitem(int i){
    item tempitem = (item)(eventlist.get(i));
    System.out.print("(" + tempitem.getItemAttrName() + ", " + tempitem.getItemAttrValue() + ")");
  }

}