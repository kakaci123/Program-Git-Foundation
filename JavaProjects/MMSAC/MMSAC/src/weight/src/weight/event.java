package weight.src.weight;

import java.util.*;


public class event {
	  private ArrayList eventlist = new ArrayList();

	  public event() {

	  }
	//�Nitem�[�ievent
	  public void addItemToEvent(item i){
	    if (!checkSameItem(i.getItemAttrName())){
	      eventlist.add(i);
	    }
	  }

	//�Nitem�̾�index�[�i�hevent
	  public void addItemToEvent(int index, item i){
	    if (!checkSameItem(i.getItemAttrName())){
	      eventlist.add(index,i);
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
	//����item
	  public void removeItem(int itemindex){
	    eventlist.remove(itemindex);
	  }
	//���oitem
	  public item getItem(int i){
	    return (item)(eventlist.get(i));//�j���૬,�ݬO�_����L��k
	  }

	//���oitem�Ӽ�
	  public int getItemSize(){
	    return eventlist.size();
	  }
	//��ܲ�i��item
	  public void showitem(int i){
	    item tempitem = (item)(eventlist.get(i));
	    System.out.print("(" + tempitem.getItemAttrName() + ", " + tempitem.getItemAttrValue() + ")");
	  }
	//�ƻsevent
	  public event cloneEvent(){
	    event ne = new event();
	    for (int i=0;i<this.getItemSize();i++){
	      ne.addItemToEvent(this.getItem(i));
	    }
	    return ne;
	  }
}



